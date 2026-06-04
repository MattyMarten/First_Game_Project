using System;
using System.Collections.Generic;
using UnityEngine;

public class ServiceDeskManager : MonoBehaviour
{
    public enum ServiceInteractionType
    {
        None,
        Request,
        Dialogue,
        MerchantOffer
    }

    [Header("Core Manager")]
    [SerializeField] private ShopCoreManager shopCoreManager;

    [Header("References")]
    [SerializeField] private RequestBoardManager requestBoardManager;
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private DialogueInfoManager dialogueInfoManager;
    [SerializeField] private CraftedUtilityStorage craftedUtilityStorage;
    [SerializeField] private RawMaterialStorage rawMaterialStorage;

    [Header("Queue Spots")]
    [SerializeField] private List<ServiceQueueSpot> queueSpots = new();

    private readonly List<ServiceVisitorNPC> activeVisitors = new();
    private readonly List<ServiceVisitorNPC> waitingQueue = new();

    private ServiceVisitorNPC deskAssignedVisitor;

    private ServiceVisitorNPC pendingVisitor;
    private ShopRequestData pendingRequest;
    private DialogueEncounterData pendingDialogue;
    private GeneratedMerchantVisit pendingMerchantVisit;
    private ServiceInteractionType currentInteractionType = ServiceInteractionType.None;

    private string currentDialogueResultText;

    private bool isViewingMerchantWares;
    private GeneratedMerchantUtilityItem selectedMerchantUtilityItem;
    private GeneratedMerchantMaterialItem selectedMerchantMaterialItem;

    public event Action OnPendingInteractionChanged;

    public bool HasPendingRequest => currentInteractionType == ServiceInteractionType.Request && pendingVisitor != null && pendingRequest != null;
    public bool HasPendingDialogue => currentInteractionType == ServiceInteractionType.Dialogue && pendingVisitor != null && pendingDialogue != null;
    public bool HasPendingMerchantVisit => currentInteractionType == ServiceInteractionType.MerchantOffer && pendingVisitor != null && pendingMerchantVisit != null;

    public bool HasPendingInteraction => currentInteractionType != ServiceInteractionType.None && pendingVisitor != null;
    public bool IsInteractionActive => HasPendingInteraction;

    public ShopRequestData PendingRequest => pendingRequest;
    public DialogueEncounterData PendingDialogue => pendingDialogue;
    public GeneratedMerchantVisit PendingMerchantVisit => pendingMerchantVisit;

    public ServiceInteractionType CurrentInteractionType => currentInteractionType;

    public string CurrentDialogueResultText => currentDialogueResultText;
    public bool HasDialogueResult => !string.IsNullOrWhiteSpace(currentDialogueResultText);

    public int ActiveVisitorCount => activeVisitors.Count;
    public int MaxActiveVisitors => 1 + queueSpots.Count;
    public bool IsDeskOccupied => deskAssignedVisitor != null;
    public bool IsShopOpen => shopCoreManager != null ? shopCoreManager.IsShopOpen : false;

    public bool IsViewingMerchantWares => isViewingMerchantWares;
    public GeneratedMerchantUtilityItem SelectedMerchantUtilityItem => selectedMerchantUtilityItem;
    public GeneratedMerchantMaterialItem SelectedMerchantMaterialItem => selectedMerchantMaterialItem;
    public bool HasSelectedMerchantItem => selectedMerchantUtilityItem != null || selectedMerchantMaterialItem != null;

    private void Awake()
    {
        if (shopCoreManager == null)
        shopCoreManager = FindAnyObjectByType<ShopCoreManager>();

        if (requestBoardManager == null)
            requestBoardManager = FindAnyObjectByType<RequestBoardManager>();

        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (dialogueInfoManager == null)
            dialogueInfoManager = FindAnyObjectByType<DialogueInfoManager>();

        if (craftedUtilityStorage == null)
            craftedUtilityStorage = FindAnyObjectByType<CraftedUtilityStorage>();

        if (rawMaterialStorage == null)
            rawMaterialStorage = FindAnyObjectByType<RawMaterialStorage>();
    }

    public bool CanAcceptAnotherVisitor()
    {
        return activeVisitors.Count < MaxActiveVisitors;
    }

    public bool TryRegisterVisitor(ServiceVisitorNPC visitor)
    {
        if (visitor == null)
            return false;

        if (activeVisitors.Contains(visitor))
            return false;

        if (!CanAcceptAnotherVisitor())
            return false;

        activeVisitors.Add(visitor);
        return true;
    }

    public void UnregisterVisitor(ServiceVisitorNPC visitor)
    {
        if (visitor == null)
            return;

        activeVisitors.Remove(visitor);
        RemoveFromQueue(visitor);

        if (deskAssignedVisitor == visitor)
        {
            deskAssignedVisitor = null;
            PromoteNextQueuedVisitor();
        }

        if (pendingVisitor == visitor)
            ClearPendingInteraction();
    }

    public bool TryRequestDeskAccess(ServiceVisitorNPC visitor, out int queueIndex)
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

    public void ReleaseDeskAccess(ServiceVisitorNPC visitor)
    {
        if (visitor == null)
            return;

        if (deskAssignedVisitor != visitor)
            return;

        deskAssignedVisitor = null;
        PromoteNextQueuedVisitor();
    }

    private void PromoteNextQueuedVisitor()
    {
        while (waitingQueue.Count > 0)
        {
            ServiceVisitorNPC nextVisitor = waitingQueue[0];
            waitingQueue.RemoveAt(0);

            NotifyQueuedVisitorsToRefreshSpots();

            if (nextVisitor == null)
                continue;

            deskAssignedVisitor = nextVisitor;
            nextVisitor.OnDeskAccessGranted();
            break;
        }
    }

    public void RemoveFromQueue(ServiceVisitorNPC visitor)
    {
        if (visitor == null)
            return;

        bool removed = waitingQueue.Remove(visitor);

        if (removed)
            NotifyQueuedVisitorsToRefreshSpots();
    }

    private void NotifyQueuedVisitorsToRefreshSpots()
    {
        for (int i = 0; i < waitingQueue.Count; i++)
        {
            ServiceVisitorNPC queuedVisitor = waitingQueue[i];

            if (queuedVisitor != null)
                queuedVisitor.OnQueuePositionChanged(i);
        }
    }

    public Transform GetQueueSpotTransform(int queueIndex)
    {
        if (queueIndex < 0 || queueIndex >= queueSpots.Count)
            return null;

        ServiceQueueSpot queueSpot = queueSpots[queueIndex];
        return queueSpot != null ? queueSpot.transform : null;
    }

    public void RegisterQueueSpot(ServiceQueueSpot queueSpot)
    {
        if (queueSpot == null)
            return;

        if (!queueSpots.Contains(queueSpot))
        {
            queueSpots.Add(queueSpot);
            queueSpots.Sort((a, b) => a.QueueIndex.CompareTo(b.QueueIndex));
        }
    }

    public void UnregisterQueueSpot(ServiceQueueSpot queueSpot)
    {
        if (queueSpot == null)
            return;

        if (queueSpots.Contains(queueSpot))
            queueSpots.Remove(queueSpot);
    }

    public bool TryCreatePendingRequest(ServiceVisitorNPC visitor, ShopRequestData request)
    {
        if (visitor == null || request == null)
            return false;

        if (currentInteractionType != ServiceInteractionType.None)
            return false;

        pendingVisitor = visitor;
        pendingRequest = request;
        pendingDialogue = null;
        pendingMerchantVisit = null;
        currentInteractionType = ServiceInteractionType.Request;
        currentDialogueResultText = null;

        ResetMerchantState();

        OnPendingInteractionChanged?.Invoke();
        //Debug.Log($"Pending request created: {request.requestTitle}");
        return true;
    }

    public bool TryCreatePendingDialogue(ServiceVisitorNPC visitor, DialogueEncounterData dialogue)
    {
        if (visitor == null || dialogue == null)
            return false;

        if (currentInteractionType != ServiceInteractionType.None)
            return false;

        pendingVisitor = visitor;
        pendingRequest = null;
        pendingDialogue = dialogue;
        pendingMerchantVisit = null;
        currentInteractionType = ServiceInteractionType.Dialogue;
        currentDialogueResultText = null;

        ResetMerchantState();

        OnPendingInteractionChanged?.Invoke();
        //Debug.Log($"Pending dialogue created: {dialogue.encounterTitle}");
        return true;
    }

    public bool TryCreatePendingMerchantVisit(ServiceVisitorNPC visitor, GeneratedMerchantVisit merchantVisit)
    {
        if (visitor == null || merchantVisit == null)
            return false;

        if (currentInteractionType != ServiceInteractionType.None)
            return false;

        pendingVisitor = visitor;
        pendingRequest = null;
        pendingDialogue = null;
        pendingMerchantVisit = merchantVisit;
        currentInteractionType = ServiceInteractionType.MerchantOffer;
        currentDialogueResultText = null;

        ResetMerchantState();

        OnPendingInteractionChanged?.Invoke();
        //Debug.Log($"Pending merchant visit created: {merchantVisit.merchantName}");
        return true;
    }

    public void AcceptPendingRequest()
    {
        if (!HasPendingRequest)
            return;

        ServiceVisitorNPC visitor = pendingVisitor;
        ShopRequestData request = pendingRequest;

        if (requestBoardManager != null)
            requestBoardManager.TryAddRequest(request);

        ClearPendingInteraction();

        if (visitor != null)
            visitor.OnInteractionAccepted();
    }

    public void DeclinePendingRequest()
    {
        if (!HasPendingRequest)
            return;

        ServiceVisitorNPC visitor = pendingVisitor;

        ClearPendingInteraction();

        if (visitor != null)
            visitor.OnInteractionDeclined();
    }

    public void ChooseDialogueOption(int choiceIndex)
    {
        if (!HasPendingDialogue)
            return;

        if (HasDialogueResult)
            return;

        if (pendingDialogue.choices == null)
            return;

        if (choiceIndex < 0 || choiceIndex >= pendingDialogue.choices.Count)
            return;

        DialogueChoiceData chosenChoice = pendingDialogue.choices[choiceIndex];

        if (chosenChoice.rewardMoney > 0 && shopManager != null)
            shopManager.AddMoney(chosenChoice.rewardMoney);

        if (chosenChoice.givesInfo && !string.IsNullOrWhiteSpace(chosenChoice.infoText))
        {
            if (dialogueInfoManager != null)
                dialogueInfoManager.TryAddInfo(chosenChoice.infoText);
        }

        currentDialogueResultText = BuildDialogueResultText(chosenChoice);

        if (!string.IsNullOrWhiteSpace(chosenChoice.npcResponse))
           // Debug.Log($"NPC response: {chosenChoice.npcResponse}");

        OnPendingInteractionChanged?.Invoke();
    }

    public void FinishPendingDialogue()
    {
        if (!HasPendingDialogue)
            return;

        ServiceVisitorNPC visitor = pendingVisitor;

        ClearPendingInteraction();

        if (visitor != null)
            visitor.OnInteractionAccepted();
    }

    public void AcceptPendingMerchantVisit()
    {
        if (!HasPendingMerchantVisit)
            return;

        isViewingMerchantWares = true;
        AutoSelectFirstMerchantItem();

        OnPendingInteractionChanged?.Invoke();
    }

    public void DeclinePendingMerchantVisit()
    {
        if (!HasPendingMerchantVisit)
            return;

        ServiceVisitorNPC visitor = pendingVisitor;

        ClearPendingInteraction();

        if (visitor != null)
            visitor.OnInteractionDeclined();
    }

    public void FinishMerchantVisit()
    {
        if (!HasPendingMerchantVisit)
            return;

        ServiceVisitorNPC visitor = pendingVisitor;

        ClearPendingInteraction();

        if (visitor != null)
            visitor.OnInteractionAccepted();
    }

    public void SelectMerchantUtilityItem(GeneratedMerchantUtilityItem item)
    {
        if (!HasPendingMerchantVisit || item == null)
            return;

        selectedMerchantUtilityItem = item;
        selectedMerchantMaterialItem = null;

        OnPendingInteractionChanged?.Invoke();
    }

    public void SelectMerchantMaterialItem(GeneratedMerchantMaterialItem item)
    {
        if (!HasPendingMerchantVisit || item == null)
            return;

        selectedMerchantUtilityItem = null;
        selectedMerchantMaterialItem = item;

        OnPendingInteractionChanged?.Invoke();
    }

    public void BuySelectedMerchantItem()
    {
        if (!HasPendingMerchantVisit || !HasSelectedMerchantItem || shopManager == null)
            return;

        if (selectedMerchantUtilityItem != null)
        {
            UtilityCraftable utilityItem = selectedMerchantUtilityItem.item;
            int price = selectedMerchantUtilityItem.finalPrice;

            if (utilityItem == null || craftedUtilityStorage == null || selectedMerchantUtilityItem.quantity <= 0)
                return;

            if (!shopManager.SpendMoney(price))
            {
                //Debug.Log("Not enough money to buy this item.");
                return;
            }

            craftedUtilityStorage.Add(utilityItem, 1);
            selectedMerchantUtilityItem.quantity--;

            if (selectedMerchantUtilityItem.quantity <= 0)
            {
                RemoveUtilityItemFromMerchantStock(selectedMerchantUtilityItem);
                selectedMerchantUtilityItem = null;
            }
        }
        else if (selectedMerchantMaterialItem != null)
        {
            RawMaterial materialItem = selectedMerchantMaterialItem.item;
            int price = selectedMerchantMaterialItem.finalPrice;

            if (materialItem == null || rawMaterialStorage == null || selectedMerchantMaterialItem.quantity <= 0)
                return;

            if (!shopManager.SpendMoney(price))
            {
                //Debug.Log("Not enough money to buy this item.");
                return;
            }

            rawMaterialStorage.Add(materialItem, 1);
            selectedMerchantMaterialItem.quantity--;

            if (selectedMerchantMaterialItem.quantity <= 0)
            {
                RemoveMaterialItemFromMerchantStock(selectedMerchantMaterialItem);
                selectedMerchantMaterialItem = null;
            }
        }

        if (GetRemainingMerchantItemCount() <= 0)
        {
            //Debug.Log("Merchant is sold out and leaves.");
            FinishMerchantVisit();
            return;
        }

        if (!HasSelectedMerchantItem)
        AutoSelectFirstMerchantItem();
        
        OnPendingInteractionChanged?.Invoke();
    }

    private void AutoSelectFirstMerchantItem()
    {
        if (pendingMerchantVisit == null)
            return;

        selectedMerchantUtilityItem = null;
        selectedMerchantMaterialItem = null;

        if (pendingMerchantVisit.utilityItems != null && pendingMerchantVisit.utilityItems.Count > 0)
        {
            selectedMerchantUtilityItem = pendingMerchantVisit.utilityItems[0];
            return;
        }

        if (pendingMerchantVisit.miscItems != null && pendingMerchantVisit.miscItems.Count > 0)
        {
            selectedMerchantUtilityItem = pendingMerchantVisit.miscItems[0];
            return;
        }

        if (pendingMerchantVisit.materialItems != null && pendingMerchantVisit.materialItems.Count > 0)
        {
            selectedMerchantMaterialItem = pendingMerchantVisit.materialItems[0];
        }
    }

    public int GetRemainingMerchantItemCount()
    {
        if (pendingMerchantVisit == null)
            return 0;

        int total = 0;

        if (pendingMerchantVisit.utilityItems != null)
            total += pendingMerchantVisit.utilityItems.Count;

        if (pendingMerchantVisit.miscItems != null)
            total += pendingMerchantVisit.miscItems.Count;

        if (pendingMerchantVisit.materialItems != null)
            total += pendingMerchantVisit.materialItems.Count;

        return total;
    }

    private void RemoveUtilityItemFromMerchantStock(GeneratedMerchantUtilityItem item)
    {
        if (pendingMerchantVisit == null || item == null)
            return;

        pendingMerchantVisit.utilityItems.Remove(item);
        pendingMerchantVisit.miscItems.Remove(item);
    }

    private void RemoveMaterialItemFromMerchantStock(GeneratedMerchantMaterialItem item)
    {
        if (pendingMerchantVisit == null || item == null)
            return;

        pendingMerchantVisit.materialItems.Remove(item);
    }

    private string BuildDialogueResultText(DialogueChoiceData chosenChoice)
    {
        if (chosenChoice == null)
            return "UHHH.";

        if (chosenChoice.givesInfo && !string.IsNullOrWhiteSpace(chosenChoice.infoText))
            return "Here, take this note I found.";

        if (chosenChoice.rewardMoney > 0)
            return "Here, take some coin.";

        if (!string.IsNullOrWhiteSpace(chosenChoice.npcResponse))
            return chosenChoice.npcResponse;

        return "UHHH.";
    }

    private void ResetMerchantState()
    {
        isViewingMerchantWares = false;
        selectedMerchantUtilityItem = null;
        selectedMerchantMaterialItem = null;
    }

    private void ClearPendingInteraction()
    {
        pendingVisitor = null;
        pendingRequest = null;
        pendingDialogue = null;
        pendingMerchantVisit = null;
        currentInteractionType = ServiceInteractionType.None;
        currentDialogueResultText = null;

        ResetMerchantState();

        OnPendingInteractionChanged?.Invoke();
    }
}