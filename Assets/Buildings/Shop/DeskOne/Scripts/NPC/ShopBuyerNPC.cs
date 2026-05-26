using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class ShopBuyerNPC : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float arriveDistance = 0.4f;

    [Header("Browse Behaviour")]
    [SerializeField] private int minBrowsePointsToVisit = 1;
    [SerializeField] private int maxBrowsePointsToVisit = 3;
    [SerializeField] private float minBrowseWaitTime = 4f;
    [SerializeField] private float maxBrowseWaitTime = 8f;

    [Header("Item Interaction")]
    [SerializeField] private float takeItemDuration = 3f;

    [Header("Carry Visual")]
    [SerializeField] private Transform carryPoint;
    [SerializeField] private Vector3 carryLocalPosition = Vector3.zero;
    [SerializeField] private Vector3 carryLocalEulerAngles = Vector3.zero;
    [SerializeField] private Vector3 carryLocalScale = Vector3.one;

    [Header("Buyer Identity")]
    [SerializeField] private string buyerName;

    [TextArea]
    [SerializeField] private string buyerDialogue;

    [Header("Animation")]
    [SerializeField] private Animator animator;

    protected ShopManager shopManager;
    private Transform deskWaitPoint;
    private Transform exitPoint;
    private NavMeshAgent agent;
    private float takeItemTimer;

    private DisplayStand targetDisplay;
    private int reservedSlotIndex = -1;
    private CraftingGood carriedGood;
    private GameObject carriedVisual;
    private int currentQueueIndex = -1;
    private bool hasDeskAccess;
    private ShopBrowsePoint currentReservedBrowsePoint;

    private readonly List<ShopBrowsePoint> plannedBrowsePoints = new();
    private int currentBrowseIndex;
    private float browseWaitTimer;

    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsLookingHash = Animator.StringToHash("IsLooking");
    private static readonly int TakeItemHash = Animator.StringToHash("TakeItem");

    private BuyerState currentState;

    private static readonly string[] PossibleBuyerNames =
    {
    "Mira",
    "Doran",
    "Lina",
    "Corin",
    "Tessa",
    "Rook",
    "Nyra",
    "Vale"
    };

    private static readonly string[] PossibleBuyerDialogues =
    {
    "I think this would sell well in my village.",
    "I need something useful for the road.",
    "This looks like exactly what I was hoping to find.",
    "I have been looking for a piece like this all day.",
    "This would make a fine addition to my supplies.",
    "I need something dependable, and this looks promising.",
    "I want to take this home before someone else buys it.",
    "This item caught my eye the moment I walked in."
    };


    private enum BuyerState
    {
        None,
        GoingToBrowsePoint,
        WaitingAtBrowsePoint,
        GoingToDisplay,
        TakingItem,
        GoingToQueueSpot,
        WaitingInQueue,
        GoingToDesk,
        WaitingAtDesk,
        LeavingShop
    }

    private void Awake()
    {
        if (animator == null)
            animator = GetComponentInChildren<Animator>();

        currentState = BuyerState.None;
    }

    public virtual void Initialize(ShopManager manager, Transform deskPoint, Transform leavePoint)
    {
        shopManager = manager;
        deskWaitPoint = deskPoint;
        exitPoint = leavePoint;

        agent = GetComponent<NavMeshAgent>();

        SetWalkingAnimation(false);
        SetLookingAnimation(false);

        GenerateBuyerIdentity();

        if (shopManager == null)
        {
            Debug.LogWarning("ShopBuyerNPC: No ShopManager assigned.");
            return;
        }

        if (agent == null)
        {
            Debug.LogWarning("ShopBuyerNPC: No NavMeshAgent found.", this);
            return;
        }

        BuildBrowseRoute();

        if (plannedBrowsePoints.Count > 0)
        {
            GoToCurrentBrowsePoint();
        }
        else
        {
            StartShopping();
        }
    }

    private void Update()
    {
        if (agent == null)
            return;

        switch (currentState)
        {
            case BuyerState.GoingToBrowsePoint:
                UpdateGoingToBrowsePoint();
                break;

            case BuyerState.WaitingAtBrowsePoint:
                UpdateWaitingAtBrowsePoint();
                break;

            case BuyerState.GoingToDisplay:
                UpdateGoingToDisplay();
                break;

            case BuyerState.TakingItem:
                UpdateTakingItem();
                break;

            case BuyerState.GoingToQueueSpot:
                UpdateGoingToQueueSpot();
                break;

            case BuyerState.WaitingInQueue:
                UpdateWaitingInQueue();
                break;

            case BuyerState.GoingToDesk:
                UpdateGoingToDesk();
                break;

            case BuyerState.LeavingShop:
                UpdateLeavingShop();
                break;
        }
    }

    private void BuildBrowseRoute()
    {
        plannedBrowsePoints.Clear();
        currentBrowseIndex = 0;

        if (shopManager == null)
            return;

        List<ShopBrowsePoint> availablePoints = shopManager.GetBrowsePoints();

        if (availablePoints.Count == 0)
            return;

        int minCount = Mathf.Max(0, minBrowsePointsToVisit);
        int maxCount = Mathf.Max(minCount, maxBrowsePointsToVisit);
        int visitCount = Random.Range(minCount, maxCount + 1);
        visitCount = Mathf.Min(visitCount, availablePoints.Count);

        for (int i = 0; i < visitCount; i++)
        {
            int randomIndex = Random.Range(0, availablePoints.Count);
            ShopBrowsePoint chosenPoint = availablePoints[randomIndex];

            plannedBrowsePoints.Add(chosenPoint);
            availablePoints.RemoveAt(randomIndex);
        }
    }

    private void GoToCurrentBrowsePoint()
    {
        ReleaseCurrentBrowsePoint();

        while (currentBrowseIndex < plannedBrowsePoints.Count)
        {
            ShopBrowsePoint browsePoint = plannedBrowsePoints[currentBrowseIndex];

            if (browsePoint == null)
            {
                currentBrowseIndex++;
                continue;
            }

            if (!browsePoint.TryReserve(this))
            {
                currentBrowseIndex++;
                continue;
            }

            currentReservedBrowsePoint = browsePoint;

            agent.SetDestination(browsePoint.transform.position);

            SetLookingAnimation(false);
            SetWalkingAnimation(true);

            currentState = BuyerState.GoingToBrowsePoint;
            return;
        }

        StartShopping();
    }

    private void UpdateGoingToBrowsePoint()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        SetLookingAnimation(true);

        browseWaitTimer = Random.Range(minBrowseWaitTime, maxBrowseWaitTime);
        currentState = BuyerState.WaitingAtBrowsePoint;
    }

    private void UpdateWaitingAtBrowsePoint()
    {
        browseWaitTimer -= Time.deltaTime;

        if (browseWaitTimer > 0f)
            return;

        currentBrowseIndex++;
        GoToCurrentBrowsePoint();
    }

    private void GenerateBuyerIdentity()
    {
        buyerName = GetRandomBuyerName();
        buyerDialogue = GetRandomBuyerDialogue();
    }

    private string GetRandomBuyerName()
    {
        if (PossibleBuyerNames == null || PossibleBuyerNames.Length == 0)
            return "Customer";

        int randomIndex = Random.Range(0, PossibleBuyerNames.Length);
        return PossibleBuyerNames[randomIndex];
    }

    private string GetRandomBuyerDialogue()
    {
        if (PossibleBuyerDialogues == null || PossibleBuyerDialogues.Length == 0)
            return "I would like to buy this item.";

        int randomIndex = Random.Range(0, PossibleBuyerDialogues.Length);
        return PossibleBuyerDialogues[randomIndex];
    }

    public string GetBuyerName()
    {
        return string.IsNullOrWhiteSpace(buyerName) ? "Customer" : buyerName;
    }

    public string GetBuyerDialogue()
    {
        return string.IsNullOrWhiteSpace(buyerDialogue)
            ? "I would like to buy this item."
            : buyerDialogue;
    }

    private void StartShopping()
    {
        if (shopManager == null)
        {
            LeaveShop();
            return;
        }

        targetDisplay = null;
        reservedSlotIndex = -1;

        List<DisplayStand> displays = shopManager.GetRegisteredDisplays();
        List<DisplayStand> candidateDisplays = new();

        for (int i = 0; i < displays.Count; i++)
        {
            DisplayStand display = displays[i];

            if (display != null && display.HasAnyUnreservedItemsForSale())
                candidateDisplays.Add(display);
        }

        while (candidateDisplays.Count > 0)
        {
            int randomIndex = Random.Range(0, candidateDisplays.Count);
            DisplayStand chosenDisplay = candidateDisplays[randomIndex];
            candidateDisplays.RemoveAt(randomIndex);

            if (chosenDisplay.TryReserveFirstAvailableSlot(this, out int slotIndex))
            {
                targetDisplay = chosenDisplay;
                reservedSlotIndex = slotIndex;
                break;
            }
        }

        if (targetDisplay == null || reservedSlotIndex < 0)
        {
            Debug.Log("Buyer found no reservable item for sale.");
            LeaveShop();
            return;
        }

        Transform displayPoint = targetDisplay.GetBuyerApproachPoint();

        if (displayPoint == null)
        {
            targetDisplay.ReleaseReservedSlot(reservedSlotIndex, this);
            LeaveShop();
            return;
        }

        SetLookingAnimation(false);
        SetWalkingAnimation(true);

        agent.SetDestination(displayPoint.position);
        currentState = BuyerState.GoingToDisplay;
    }

    private void UpdateGoingToDisplay()
    {
        if (!HasReachedDestination())
            return;

        if (targetDisplay == null)
        {
            LeaveShop();
            return;
        }

        carriedGood = targetDisplay.TakeReservedGood(reservedSlotIndex, this);
        reservedSlotIndex = -1;

        if (carriedGood == null)
        {
            LeaveShop();
            return;
        }

        CreateCarriedVisual();

        Debug.Log($"Buyer took {carriedGood.goodName} from display.");

        SetWalkingAnimation(false);
        SetLookingAnimation(false);
        TriggerTakeItemAnimation();

        takeItemTimer = Mathf.Max(0f, takeItemDuration);
        currentState = BuyerState.TakingItem;
    }

    private void UpdateGoingToDesk()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        SetLookingAnimation(false);

        currentState = BuyerState.WaitingAtDesk;

        if (!hasDeskAccess)
        {
            LeaveShop();
            return;
        }

        if (shopManager == null || carriedGood == null)
        {
            LeaveShop();
            return;
        }

        int salePrice = Mathf.Max(0, carriedGood.valueGold);
        bool created = shopManager.TryCreatePendingSale(this, carriedGood, salePrice);

        if (!created)
        {
            Debug.Log("Buyer could not create pending sale.");
            LeaveShop();
            return;
        }

        ClearCarriedVisual();

        Debug.Log($"Buyer placed {carriedGood.goodName} on the desk for {salePrice} gold.");
    }

    public void OnSaleAccepted()
    {
        if (carriedGood != null)
            Debug.Log($"Sale accepted for {carriedGood.goodName}.");

        if (shopManager != null)
            shopManager.ReleaseDeskAccess(this);

        hasDeskAccess = false;
        carriedGood = null;
        ClearCarriedVisual();
        BeginLeavingShop();
    }

    public void OnSaleDeclined()
    {
        if (carriedGood != null)
            Debug.Log($"Sale declined for {carriedGood.goodName}.");

        if (shopManager != null)
            shopManager.ReleaseDeskAccess(this);

        hasDeskAccess = false;
        carriedGood = null;
        ClearCarriedVisual();
        BeginLeavingShop();
    }

    private void CreateCarriedVisual()
    {
        ClearCarriedVisual();

        if (carriedGood == null || carriedGood.goodsPrefab == null)
            return;

        Transform parent = carryPoint != null ? carryPoint : transform;

        carriedVisual = Instantiate(carriedGood.goodsPrefab, parent);
        carriedVisual.transform.localPosition = carryLocalPosition;
        carriedVisual.transform.localRotation = Quaternion.Euler(carryLocalEulerAngles);
        carriedVisual.transform.localScale = carryLocalScale;
    }

    private void ClearCarriedVisual()
    {
        if (carriedVisual != null)
        {
            Destroy(carriedVisual);
            carriedVisual = null;
        }
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

    private void LeaveShop()
    {
        ReleaseCurrentBrowsePoint();

        if (targetDisplay != null && reservedSlotIndex >= 0)
        {
            targetDisplay.ReleaseReservedSlot(reservedSlotIndex, this);
            reservedSlotIndex = -1;
        }

        if (shopManager != null)
            shopManager.UnregisterBuyer(this);

        hasDeskAccess = false;
        currentQueueIndex = -1;

        ClearCarriedVisual();

        SetWalkingAnimation(false);
        SetLookingAnimation(false);

        Destroy(gameObject);
    }

    private void RequestDeskAccess()
    {
        if (shopManager == null)
        {
            LeaveShop();
            return;
        }

        bool granted = shopManager.TryRequestDeskAccess(this, out int queueIndex);

        if (granted)
        {
            hasDeskAccess = true;
            currentQueueIndex = -1;
            GoToDesk();
            return;
        }

        hasDeskAccess = false;
        currentQueueIndex = queueIndex;
        GoToQueueSpot();
    }

    private void GoToDesk()
    {
        if (deskWaitPoint == null)
        {
            LeaveShop();
            return;
        }

        agent.SetDestination(deskWaitPoint.position);
        SetLookingAnimation(false);
        SetWalkingAnimation(true);
        currentState = BuyerState.GoingToDesk;
    }

    private void GoToQueueSpot()
    {
        if (shopManager == null)
        {
            LeaveShop();
            return;
        }

        Transform queueSpot = shopManager.GetQueueSpotTransform(currentQueueIndex);

        if (queueSpot == null)
        {
            Debug.Log("Buyer could not find a valid queue spot.");
            LeaveShop();
            return;
        }

        agent.SetDestination(queueSpot.position);

        SetLookingAnimation(false);
        SetWalkingAnimation(true);

        currentState = BuyerState.GoingToQueueSpot;
    }

    private void UpdateGoingToQueueSpot()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        SetLookingAnimation(false);
        currentState = BuyerState.WaitingInQueue;
    }

    private void UpdateWaitingInQueue()
    {
    }

    public void OnDeskAccessGranted()
    {
        hasDeskAccess = true;
        currentQueueIndex = -1;

        if (currentState == BuyerState.WaitingInQueue || currentState == BuyerState.GoingToQueueSpot)
            GoToDesk();
    }

    public void OnQueuePositionChanged(int newIndex)
    {
        currentQueueIndex = newIndex;

        if (hasDeskAccess)
            return;

        if (currentState == BuyerState.WaitingInQueue || currentState == BuyerState.GoingToQueueSpot)
            GoToQueueSpot();
    }

    private void BeginLeavingShop()
    {
        ReleaseCurrentBrowsePoint();

        if (shopManager != null)
            shopManager.UnregisterBuyer(this);

        hasDeskAccess = false;
        currentQueueIndex = -1;

        if (targetDisplay != null && reservedSlotIndex >= 0)
        {
            targetDisplay.ReleaseReservedSlot(reservedSlotIndex, this);
            reservedSlotIndex = -1;
        }

        ClearCarriedVisual();

        if (exitPoint == null)
        {
            Destroy(gameObject);
            return;
        }

        agent.SetDestination(exitPoint.position);
        SetLookingAnimation(false);
        SetWalkingAnimation(true);
        currentState = BuyerState.LeavingShop;
    }

    private void UpdateLeavingShop()
    {
        if (!HasReachedDestination())
            return;

        SetWalkingAnimation(false);
        SetLookingAnimation(false);
        Destroy(gameObject);
    }

    private void SetWalkingAnimation(bool isWalking)
    {
        if (animator == null)
            return;

        animator.SetBool(IsWalkingHash, isWalking);
    }

    private void SetLookingAnimation(bool isLooking)
    {
        if (animator == null)
            return;

        animator.SetBool(IsLookingHash, isLooking);
    }

    private void TriggerTakeItemAnimation()
    {
        if (animator == null)
            return;

        animator.SetTrigger(TakeItemHash);
    }

    private void UpdateTakingItem()
    {
        takeItemTimer -= Time.deltaTime;

        if (takeItemTimer > 0f)
            return;

        RequestDeskAccess();
    }

    private void ReleaseCurrentBrowsePoint()
    {
        if (currentReservedBrowsePoint != null)
        {
            currentReservedBrowsePoint.Release(this);
            currentReservedBrowsePoint = null;
        }
    }
}