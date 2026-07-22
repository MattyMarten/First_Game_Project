using UnityEngine;
using UnityEngine.AI;

public abstract class ServiceVisitorNPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] protected float arriveDistance = 0.4f;

    [Header("Animation")]
    [SerializeField] protected Animator animator;

    protected ServiceDeskManager serviceDeskManager;
    protected Transform deskPoint;
    protected Transform exitPoint;
    protected NavMeshAgent agent;

    protected static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");

    protected int currentQueueIndex = -1;
    protected bool hasDeskAccess;

    protected enum VisitorState
    {
        None,
        GoingToDesk,
        WaitingAtDesk,
        GoingToQueueSpot,
        WaitingInQueue,
        Leaving
    }

    protected VisitorState currentState;

    protected virtual void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        agent = GetComponent<NavMeshAgent>();
        currentState = VisitorState.None;
    }

    public virtual void Initialize(ServiceDeskManager manager, Transform targetDeskPoint, Transform targetExitPoint)
    {
        serviceDeskManager = manager;
        deskPoint = targetDeskPoint;
        exitPoint = targetExitPoint;

        if (agent == null || serviceDeskManager == null || deskPoint == null)
        {
            Debug.LogWarning("ServiceVisitorNPC failed to initialize.", this);
            Destroy(gameObject);
            return;
        }

        bool registered = serviceDeskManager.TryRegisterVisitor(this);

        if (!registered)
        {
            Debug.Log("Service desk is full. Visitor will not enter.");
            Destroy(gameObject);
            return;
        }

        RequestDeskAccess();
    }

    protected virtual void Update()
    {
        switch (currentState)
        {
            case VisitorState.GoingToDesk:
                UpdateGoingToDesk();
                break;

            case VisitorState.GoingToQueueSpot:
                UpdateGoingToQueueSpot();
                break;

            case VisitorState.Leaving:
                UpdateLeaving();
                break;
        }
    }

    protected void RequestDeskAccess()
    {
        if (serviceDeskManager == null)
        {
            Leave();
            return;
        }

        bool granted = serviceDeskManager.TryRequestDeskAccess(this, out int queueIndex);

        if (granted)
        {
            hasDeskAccess = true;
            currentQueueIndex = -1;
            GoToDesk();
            return;
        }

        currentQueueIndex = queueIndex;
        hasDeskAccess = false;
        GoToQueueSpot();
    }

    protected virtual void GoToDesk()
    {
        if (deskPoint == null)
        {
            Leave();
            return;
        }

        agent.SetDestination(deskPoint.position);
        SetWalkingAnimation(true);
        currentState = VisitorState.GoingToDesk;
    }

    protected virtual void GoToQueueSpot()
    {
        if (serviceDeskManager == null)
        {
            Leave();
            return;
        }

        Transform queueSpot = serviceDeskManager.GetQueueSpotTransform(currentQueueIndex);

        if (queueSpot == null)
        {
            Leave();
            return;
        }

        agent.SetDestination(queueSpot.position);
        SetWalkingAnimation(true);
        currentState = VisitorState.GoingToQueueSpot;
    }

    protected virtual void UpdateGoingToDesk()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        currentState = VisitorState.WaitingAtDesk;
        OnReachedDesk();
    }

    protected virtual void UpdateGoingToQueueSpot()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        currentState = VisitorState.WaitingInQueue;
    }

    protected abstract void OnReachedDesk();

    protected virtual void BeginLeaving()
    {
        if (exitPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        agent.SetDestination(exitPoint.position);
        SetWalkingAnimation(true);
        currentState = VisitorState.Leaving;
    }

    protected virtual void UpdateLeaving()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);

        if (serviceDeskManager != null)
            serviceDeskManager.UnregisterVisitor(this);

        Destroy(gameObject);
    }

    protected bool HasReachedDestination()
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

    protected void SetWalkingAnimation(bool value)
    {
        if (animator != null)
            animator.SetBool(IsWalkingHash, value);
    }

    protected void Leave()
    {
        if (serviceDeskManager != null)
            serviceDeskManager.ReleaseDeskAccess(this);

        BeginLeaving();
    }

    public virtual void OnInteractionAccepted()
    {
        Leave();
    }

    public virtual void OnInteractionDeclined()
    {
        Leave();
    }

    public virtual void OnDeskAccessGranted()
    {
        hasDeskAccess = true;
        currentQueueIndex = -1;
        GoToDesk();
    }

    public virtual void OnQueuePositionChanged(int newIndex)
    {
        currentQueueIndex = newIndex;

        if (!hasDeskAccess && currentState != VisitorState.Leaving)
            GoToQueueSpot();
    }
}