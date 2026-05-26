using UnityEngine;
using UnityEngine.AI;

public class HireVisitorNPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float arriveDistance = 0.4f;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    private HireDeskManager hireDeskManager;
    private RecruitQuartersManager recruitQuartersManager;
    private Transform deskPoint;
    private Transform exitPoint;
    private NavMeshAgent agent;
    private RecruitData recruitData;
    private RecruitBedSlot destinationBedSlot;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    private enum HireVisitorState
    {
        None,
        GoingToDesk,
        GoingToQueueSpot,
        WaitingInQueue,
        WaitingAtDesk,
        GoingToBed,
        Leaving
    }

    private HireVisitorState currentState = HireVisitorState.None;
    private int currentQueueIndex = -1;
    private bool finalizedRecruit;

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();
        recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();
    }

    private void OnDestroy()
    {
        if (!finalizedRecruit)
            ReleasePendingBedReservationIfNeeded();
    }

    public void Initialize(HireDeskManager manager, Transform targetDeskPoint, Transform targetExitPoint, RecruitData assignedRecruitData)
    {
        hireDeskManager = manager;
        deskPoint = targetDeskPoint;
        exitPoint = targetExitPoint;
        recruitData = assignedRecruitData;

        if (hireDeskManager == null || deskPoint == null || agent == null || recruitData == null)
        {
            Debug.LogWarning("HireVisitorNPC failed to initialize.", this);
            Destroy(gameObject);
            return;
        }

        RequestDeskAccess();
    }

    private void Update()
    {
        switch (currentState)
        {
            case HireVisitorState.GoingToDesk:
                UpdateGoingToDesk();
                break;

            case HireVisitorState.GoingToQueueSpot:
                UpdateGoingToQueueSpot();
                break;

            case HireVisitorState.GoingToBed:
                UpdateGoingToBed();
                break;

            case HireVisitorState.Leaving:
                UpdateLeaving();
                break;
        }
    }

    private void RequestDeskAccess()
    {
        if (hireDeskManager == null)
        {
            BeginLeaving();
            return;
        }

        bool gotDeskAccess = hireDeskManager.TryRequestDeskAccess(this, out int queueIndex);

        if (gotDeskAccess)
        {
            GoToDesk();
            return;
        }

        if (queueIndex >= 0)
        {
            MoveToQueueSpot(queueIndex);
            return;
        }

        BeginLeaving();
    }

    private void GoToDesk()
    {
        if (deskPoint == null)
        {
            BeginLeaving();
            return;
        }

        currentQueueIndex = -1;
        destinationBedSlot = null;
        agent.SetDestination(deskPoint.position);
        SetWalkingAnimation(true);
        currentState = HireVisitorState.GoingToDesk;
    }

    private void UpdateGoingToDesk()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        currentState = HireVisitorState.WaitingAtDesk;

        TryOfferRecruit();
    }

    private void MoveToQueueSpot(int queueIndex)
    {
        if (hireDeskManager == null)
        {
            BeginLeaving();
            return;
        }

        Transform queueSpot = hireDeskManager.GetQueueSpotTransform(queueIndex);

        if (queueSpot == null)
        {
            BeginLeaving();
            return;
        }

        currentQueueIndex = queueIndex;
        destinationBedSlot = null;
        agent.SetDestination(queueSpot.position);
        SetWalkingAnimation(true);
        currentState = HireVisitorState.GoingToQueueSpot;
    }

    private void UpdateGoingToQueueSpot()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        currentState = HireVisitorState.WaitingInQueue;
    }

    private void TryOfferRecruit()
    {
        if (hireDeskManager == null || recruitData == null)
        {
            BeginLeaving();
            return;
        }

        bool created = hireDeskManager.TryCreatePendingRecruitOffer(this, recruitData);

        if (!created)
            BeginLeaving();
    }

    private void BeginLeaving()
    {
        if (currentState == HireVisitorState.GoingToBed)
            ReleasePendingBedReservationIfNeeded();

        if (hireDeskManager != null)
            hireDeskManager.RemoveFromQueue(this);

        destinationBedSlot = null;

        if (exitPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        agent.SetDestination(exitPoint.position);
        SetWalkingAnimation(true);
        currentState = HireVisitorState.Leaving;
    }

    private void UpdateLeaving()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        Destroy(gameObject);
    }

    private void BeginGoingToBed(RecruitBedSlot bedSlot)
    {
        if (bedSlot == null || bedSlot.StandPoint == null)
        {
            BeginLeaving();
            return;
        }

        destinationBedSlot = bedSlot;
        currentQueueIndex = -1;

        agent.SetDestination(bedSlot.StandPoint.position);
        SetWalkingAnimation(true);
        currentState = HireVisitorState.GoingToBed;
    }

    private void UpdateGoingToBed()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);

        bool added = false;

        if (hireDeskManager != null && recruitData != null)
            added = hireDeskManager.FinalizeAcceptedRecruit(recruitData);

        if (added)
        {
            finalizedRecruit = true;
            ReleasePendingBedReservationIfNeeded();
            Destroy(gameObject);
            return;
        }

        ReleasePendingBedReservationIfNeeded();
        BeginLeaving();
    }

    private void ReleasePendingBedReservationIfNeeded()
    {
        if (recruitQuartersManager == null || recruitData == null)
            return;

        recruitQuartersManager.ReleasePendingBedReservation(recruitData);
    }

    private bool HasReachedDestination()
    {
        if (agent == null)
            return false;

        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > arriveDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude > 0.01f)
            return false;

        return true;
    }

    private void SetWalkingAnimation(bool value)
    {
        if (animator != null)
            animator.SetBool(IsWalkingHash, value);
    }

    public void OnDeskAccessGranted()
    {
        GoToDesk();
    }

    public void OnQueuePositionChanged(int newQueueIndex)
    {
        if (newQueueIndex < 0)
            return;

        if (currentQueueIndex == newQueueIndex && currentState == HireVisitorState.WaitingInQueue)
            return;

        MoveToQueueSpot(newQueueIndex);
    }

    public void OnRecruitAcceptedAndSendToBed(RecruitBedSlot bedSlot)
    {
        BeginGoingToBed(bedSlot);
    }

    public void OnRecruitAccepted()
    {
        BeginLeaving();
    }

    public void OnRecruitDeclined()
    {
        BeginLeaving();
    }

    public RecruitData GetRecruitData()
    {
        return recruitData;
    }
}