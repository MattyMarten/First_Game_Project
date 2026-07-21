using System;
using System.Collections.Generic;
using UnityEngine;

public class HireDeskManager : MonoBehaviour
{
    [Header("Core Manager")]
    [SerializeField] private ShopCoreManager shopCoreManager;

    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;
    [SerializeField] private RecruitQuartersManager recruitQuartersManager;
    [SerializeField] private ShopManager shopManager;

    [Header("Queue Spots")]
    [SerializeField] private List<Transform> queueSpots = new();

    private readonly List<HireVisitorNPC> waitingQueue = new();

    private HireVisitorNPC deskAssignedVisitor;
    private HireVisitorNPC pendingVisitor;
    private RecruitData pendingRecruit;

    public event Action OnPendingRecruitChanged;

    public bool HasPendingRecruit => pendingVisitor != null && pendingRecruit != null;
    public bool IsInteractionActive => HasPendingRecruit;
    public RecruitData PendingRecruit => pendingRecruit;

    public int QueueCapacity => queueSpots.Count;
    public int QueuedVisitorCount => waitingQueue.Count;
    public bool IsDeskOccupied => deskAssignedVisitor != null;
    public bool IsShopOpen => shopCoreManager != null ? shopCoreManager.IsShopOpen : false;

    private void Awake()
    {
        if (shopCoreManager == null)
        shopCoreManager = FindAnyObjectByType<ShopCoreManager>();

        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();

        if (recruitQuartersManager == null)
            recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();

        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();
    }

    public bool TryCreatePendingRecruitOffer(HireVisitorNPC visitor, RecruitData recruit)
    {
        if (visitor == null || recruit == null)
            return false;

        if (HasPendingRecruit)
            return false;

        pendingVisitor = visitor;
        pendingRecruit = recruit;

        OnPendingRecruitChanged?.Invoke();
        return true;
    }

    public bool CanDeclinePendingRecruit()
    {
        if (!HasPendingRecruit)
            return false;

        return pendingRecruit.IsPaidRecruit;
    }

    public bool AcceptPendingRecruit()
    {
        if (!HasPendingRecruit)
            return false;

        if (recruitRosterManager == null || recruitQuartersManager == null)
            return false;

        HireVisitorNPC visitor = pendingVisitor;
        RecruitData recruit = pendingRecruit;

        if (visitor == null || recruit == null)
            return false;

        if (!CanAcceptRecruitTypeNow(recruit.recruitType))
            return false;

        if (!recruitQuartersManager.TryPrepareBedForRecruit(recruit, out RecruitBedSlot bedSlot))
            return false;

        ClearPendingRecruit();
        ReleaseDeskAccess(visitor);
        visitor.OnRecruitAcceptedAndSendToBed(bedSlot);

        return true;
    }

    public bool FinalizeAcceptedRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        if (recruitRosterManager == null)
            return false;

        if (!recruitRosterManager.CanAddRecruit(recruit))
            return false;

        return recruitRosterManager.TryAddRecruit(recruit);
    }

    public bool DeclinePendingRecruit()
    {
        if (!HasPendingRecruit)
            return false;

        if (!CanDeclinePendingRecruit())
            return false;

        HireVisitorNPC visitor = pendingVisitor;

        ClearPendingRecruit();

        if (visitor != null)
        {
            ReleaseDeskAccess(visitor);
            visitor.OnRecruitDeclined();
        }

        return true;
    }

    public bool TryRequestDeskAccess(HireVisitorNPC visitor, out int queueIndex)
    {
        queueIndex = -1;

        if (visitor == null)
            return false;

        if (deskAssignedVisitor == visitor)
            return true;

        if (waitingQueue.Contains(visitor))
        {
            queueIndex = waitingQueue.IndexOf(visitor);
            return false;
        }

        if (deskAssignedVisitor == null)
        {
            deskAssignedVisitor = visitor;
            return true;
        }

        if (waitingQueue.Count >= queueSpots.Count)
            return false;

        waitingQueue.Add(visitor);
        queueIndex = waitingQueue.Count - 1;
        NotifyQueuedVisitorsToRefreshSpots();
        return false;
    }

    public void ReleaseDeskAccess(HireVisitorNPC visitor)
    {
        if (visitor == null)
            return;

        if (deskAssignedVisitor == visitor)
        {
            deskAssignedVisitor = null;
            PromoteNextQueuedVisitor();
            return;
        }

        RemoveFromQueue(visitor);
    }

    public void RemoveFromQueue(HireVisitorNPC visitor)
    {
        if (visitor == null)
            return;

        bool removed = waitingQueue.Remove(visitor);

        if (removed)
            NotifyQueuedVisitorsToRefreshSpots();
    }

    private void PromoteNextQueuedVisitor()
    {
        while (waitingQueue.Count > 0)
        {
            HireVisitorNPC nextVisitor = waitingQueue[0];
            waitingQueue.RemoveAt(0);

            NotifyQueuedVisitorsToRefreshSpots();

            if (nextVisitor == null)
                continue;

            deskAssignedVisitor = nextVisitor;
            nextVisitor.OnDeskAccessGranted();
            break;
        }
    }

    private void NotifyQueuedVisitorsToRefreshSpots()
    {
        for (int i = 0; i < waitingQueue.Count; i++)
        {
            HireVisitorNPC queuedVisitor = waitingQueue[i];

            if (queuedVisitor != null)
                queuedVisitor.OnQueuePositionChanged(i);
        }
    }

    public Transform GetQueueSpotTransform(int queueIndex)
    {
        if (queueIndex < 0 || queueIndex >= queueSpots.Count)
            return null;

        return queueSpots[queueIndex];
    }

    public bool HasSpaceForAnotherVisitor()
    {
        if (deskAssignedVisitor == null)
            return true;

        return waitingQueue.Count < queueSpots.Count;
    }

    public int GetReservedCountByType(RecruitType recruitType)
    {
        int count = 0;

        if (deskAssignedVisitor != null)
        {
            RecruitData deskRecruit = deskAssignedVisitor.GetRecruitData();

            if (deskRecruit != null && deskRecruit.recruitType == recruitType)
                count++;
        }

        for (int i = 0; i < waitingQueue.Count; i++)
        {
            HireVisitorNPC visitor = waitingQueue[i];

            if (visitor == null)
                continue;

            RecruitData recruit = visitor.GetRecruitData();

            if (recruit != null && recruit.recruitType == recruitType)
                count++;
        }

        return count;
    }

    public bool CanAcceptRecruitTypeInPipeline(RecruitType recruitType)
    {
        if (recruitRosterManager == null || recruitQuartersManager == null)
            return false;

        if (!HasSpaceForAnotherVisitor())
            return false;

        return HasTypeCapacityRemaining(recruitType);
    }

    private bool CanAcceptRecruitTypeNow(RecruitType recruitType)
    {
        if (recruitRosterManager == null || recruitQuartersManager == null)
            return false;

        return HasTypeCapacityRemaining(recruitType);
    }

    // STOPGAP (Stage 4): Recruit Quarters capacity is now a single 4/6/8 total, not a
    // Free/Paid split, so this checks total roster capacity regardless of type. Stage 5
    // replaces this whole method with the real free-slot-ratio recruit-visitor spawn
    // formula from Room_Shop.md (25% floor).
    private bool HasTypeCapacityRemaining(RecruitType recruitType)
    {
        int totalQueuedOrDesk = GetReservedCountByType(RecruitType.Free) + GetReservedCountByType(RecruitType.Paid);
        int totalAcceptedWalking = recruitQuartersManager.GetPendingAcceptedRecruitCount(RecruitType.Free)
            + recruitQuartersManager.GetPendingAcceptedRecruitCount(RecruitType.Paid);

        if (pendingRecruit != null)
            totalQueuedOrDesk = Mathf.Max(0, totalQueuedOrDesk - 1);

        return recruitRosterManager.TotalRecruitCount + totalQueuedOrDesk + totalAcceptedWalking < recruitRosterManager.MaxTotalRecruitSlots;
    }

    private void ClearPendingRecruit()
    {
        pendingVisitor = null;
        pendingRecruit = null;
        OnPendingRecruitChanged?.Invoke();
    }
}