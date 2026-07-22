using UnityEngine;

// Merged into the Service Desk (Room_Shop.md Section 5/11: recruit visitors use the
// Service Desk, same as request/talking visitors — there is no separate Desk Three).
// Shares queue/desk access with every other ServiceVisitorNPC. Its one unique behavior
// on top of the shared base: after acceptance it doesn't just leave, it walks to its
// assigned bed slot in Recruit Quarters before finalizing.
public class HireVisitorNPC : ServiceVisitorNPC
{
    private RecruitQuartersManager recruitQuartersManager;
    private RecruitData recruitData;
    private RecruitBedSlot destinationBedSlot;
    private bool finalizedRecruit;
    private bool isGoingToBed;

    protected override void Awake()
    {
        base.Awake();
        recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();
    }

    private void OnDestroy()
    {
        if (!finalizedRecruit)
            ReleasePendingBedReservationIfNeeded();
    }

    public void Initialize(ServiceDeskManager manager, Transform targetDeskPoint, Transform targetExitPoint, RecruitData assignedRecruitData)
    {
        recruitData = assignedRecruitData;

        if (recruitData == null)
        {
            Debug.LogWarning("HireVisitorNPC has no recruit data.", this);
            Destroy(gameObject);
            return;
        }

        base.Initialize(manager, targetDeskPoint, targetExitPoint);
    }

    protected override void Update()
    {
        if (isGoingToBed)
        {
            UpdateGoingToBed();
            return;
        }

        base.Update();
    }

    protected override void OnReachedDesk()
    {
        if (serviceDeskManager == null || recruitData == null)
        {
            Leave();
            return;
        }

        bool created = serviceDeskManager.TryCreatePendingRecruitOffer(this, recruitData);

        if (!created)
            Leave();
    }

    // Called by ServiceDeskManager.AcceptPendingRecruit() once a bed has been
    // reserved — this is why it's a bespoke method rather than the generic
    // OnInteractionAccepted() every other visitor type uses. Decline still uses
    // the inherited OnInteractionDeclined() (-> Leave()) unchanged.
    public void OnRecruitAcceptedAndSendToBed(RecruitBedSlot bedSlot)
    {
        if (bedSlot == null || bedSlot.StandPoint == null || agent == null)
        {
            Leave();
            return;
        }

        destinationBedSlot = bedSlot;
        isGoingToBed = true;

        agent.SetDestination(bedSlot.StandPoint.position);
        SetWalkingAnimation(true);
    }

    private void UpdateGoingToBed()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        isGoingToBed = false;

        bool added = false;

        if (serviceDeskManager != null && recruitData != null)
            added = serviceDeskManager.FinalizeAcceptedRecruit(recruitData);

        if (added)
        {
            finalizedRecruit = true;
            ReleasePendingBedReservationIfNeeded();
            Destroy(gameObject);
            return;
        }

        ReleasePendingBedReservationIfNeeded();
        Leave();
    }

    private void ReleasePendingBedReservationIfNeeded()
    {
        if (recruitQuartersManager == null || recruitData == null)
            return;

        recruitQuartersManager.ReleasePendingBedReservation(recruitData);
    }

    public RecruitData GetRecruitData()
    {
        return recruitData;
    }
}
