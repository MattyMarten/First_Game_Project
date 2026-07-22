using UnityEngine;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    // Handles Desk 1 buyer flow, queue management, pending sales, browse points, and display registration
    [Header("Core Manager")]
    [SerializeField] private ShopCoreManager shopCoreManager;

    [Header("Shop State")]
    [SerializeField] private bool shopOpen;
    [SerializeField] private int currentMoney;

    [Header("Registered Displays")]
    [SerializeField] private List<DisplayStand> registeredDisplays = new();

    [Header("Stored Goods")]
    [SerializeField] private GoodStorage goodStorage;

    [Header("Cobalt Coin Bank")]
    [SerializeField] private CobaltCoinStorage cobaltCoinStorage;

    [Header("Desk")]
    [SerializeField] private ShopDeskVisuals shopDeskVisuals;

    [Header("Browse Points")]
    [SerializeField] private List<ShopBrowsePoint> browsePoints = new();

    [Header("Shop Capacity")]
    [SerializeField] private int shopCapacity = 4;

    [Header("Queue Spots")]
    [SerializeField] private List<ShopQueueSpot> queueSpots = new();
    
    private readonly List<ShopBuyerNPC> activeBuyers = new();

    private ShopBuyerNPC pendingBuyer;
    private CraftingGood pendingGood;
    private int pendingPrice;

    private string pendingBuyerName;
    private string pendingBuyerDialogue;

    public bool IsShopOpen => shopCoreManager != null ? shopCoreManager.IsShopOpen : shopOpen;
    // NOTE: this used to be a private local int, fully disconnected from the base's
    // real currency (CobaltCoinStorage, built in Stage 1). Per Room_Shop.md Section 8,
    // a sold item must add cobalt coins directly to the base's actual bank — so this
    // now proxies through CobaltCoinStorage. currentMoney is kept only as an offline
    // fallback for scenes where no CobaltCoinStorage has been wired up yet.
    public int CurrentMoney => cobaltCoinStorage != null ? cobaltCoinStorage.CoinCount : currentMoney;

    public bool HasPendingSale => pendingBuyer != null && pendingGood != null;
    public bool IsInteractionActive => HasPendingSale;
    public CraftingGood PendingGood => pendingGood;
    public int PendingPrice => pendingPrice;
    public string PendingBuyerName => pendingBuyerName;
    public string PendingBuyerDialogue => pendingBuyerDialogue;
    public event System.Action OnPendingSaleChanged;
    public event System.Action OnMoneyChanged;

    public int ShopCapacity => shopCapacity;
    public int ActiveBuyerCount => activeBuyers.Count;

    private readonly List<ShopBuyerNPC> checkoutQueue = new();
    private ShopBuyerNPC deskAssignedBuyer;

    public bool IsDeskAssigned => deskAssignedBuyer != null;
    public int CheckoutQueueCount => checkoutQueue.Count;



    private void Awake()
    {
        if (shopCoreManager == null)
        shopCoreManager = FindAnyObjectByType<ShopCoreManager>();
        
        if (goodStorage == null)
            goodStorage = FindAnyObjectByType<GoodStorage>();

        if (shopDeskVisuals == null)
            shopDeskVisuals = FindAnyObjectByType<ShopDeskVisuals>();

        if (cobaltCoinStorage == null)
            cobaltCoinStorage = FindAnyObjectByType<CobaltCoinStorage>();
    }

    public void OpenShop()
    {
        shopOpen = true;
        Debug.Log("Shop opened.");
    }

    public void CloseShop()
    {
        shopOpen = false;
        Debug.Log("Shop closed.");
    }

    public void AddMoney(int amount)
    {
        if (amount <= 0)
            return;

        if (cobaltCoinStorage != null)
            cobaltCoinStorage.Add(amount);
        else
            currentMoney += amount;

        OnMoneyChanged?.Invoke();

        Debug.Log($"Shop earned {amount}. Total money: {CurrentMoney}");
    }

    public bool SpendMoney(int amount)
    {
        if (amount <= 0)
            return false;

        if (cobaltCoinStorage != null)
        {
            if (!cobaltCoinStorage.TrySpend(amount))
                return false;

            OnMoneyChanged?.Invoke();
            return true;
        }

        if (currentMoney < amount)
            return false;

        currentMoney -= amount;
        OnMoneyChanged?.Invoke();
        return true;
    }

    public void RegisterDisplay(DisplayStand display)
    {
        if (display == null)
            return;

        if (!registeredDisplays.Contains(display))
            registeredDisplays.Add(display);
    }

    public void UnregisterDisplay(DisplayStand display)
    {
        if (display == null)
            return;

        if (registeredDisplays.Contains(display))
            registeredDisplays.Remove(display);
    }

    public List<DisplayStand> GetRegisteredDisplays()
    {
        return new List<DisplayStand>(registeredDisplays);
    }

    public bool HasAnyItemsForSale()
    {
        for (int i = 0; i < registeredDisplays.Count; i++)
        {
            DisplayStand display = registeredDisplays[i];

            if (display != null && display.HasAnyItemsForSale())
                return true;
        }

        return false;
    }

    public DisplayStand GetRandomDisplayWithItemsForSale()
    {
        List<DisplayStand> validDisplays = new();

        for (int i = 0; i < registeredDisplays.Count; i++)
        {
            DisplayStand display = registeredDisplays[i];

            if (display != null && display.HasAnyUnreservedItemsForSale())
                validDisplays.Add(display);
        }

        if (validDisplays.Count == 0)
            return null;

        int randomIndex = Random.Range(0, validDisplays.Count);
        return validDisplays[randomIndex];
    }

    public bool TryCreatePendingSale(ShopBuyerNPC buyer, CraftingGood good, int price)
    {
        if (buyer == null || good == null)
            return false;

        if (HasPendingSale)
            return false;

        pendingBuyer = buyer;
        pendingGood = good;
        pendingPrice = price;
        pendingBuyerName = buyer.GetBuyerName();
        pendingBuyerDialogue = buyer.GetBuyerDialogue();    

        if (shopDeskVisuals != null)
            shopDeskVisuals.ShowPendingItem(good);

        OnPendingSaleChanged?.Invoke();

        Debug.Log($"Pending sale created: {good.goodName} for {price}.");
        return true;
    }

    public void AcceptPendingSale()
    {
        if (!HasPendingSale)
            return;

        AddMoney(pendingPrice);

        if (shopCoreManager != null)
            shopCoreManager.RecordItemSold(pendingPrice);

        Debug.Log($"Sale accepted: {pendingGood.goodName} sold for {pendingPrice}.");

        ShopBuyerNPC buyer = pendingBuyer;

        ClearPendingSale();

        if (buyer != null)
            buyer.OnSaleAccepted();
    }

    public void DeclinePendingSale()
    {
        if (!HasPendingSale)
            return;

        if (goodStorage != null)
        {
            goodStorage.Add(pendingGood, 1);
            Debug.Log($"{pendingGood.goodName} has been put in storage.");
        }
        else
        {
            Debug.LogWarning($"ShopManager: No GoodStorage assigned. {pendingGood.goodName} could not be stored.", this);
        }

        ShopBuyerNPC buyer = pendingBuyer;

        ClearPendingSale();

        if (buyer != null)
            buyer.OnSaleDeclined();
    }

    private void ClearPendingSale()
    {
        if (shopDeskVisuals != null)
            shopDeskVisuals.ClearPendingItem();

        pendingBuyer = null;
        pendingGood = null;
        pendingPrice = 0;
        pendingBuyerName = string.Empty;
        pendingBuyerDialogue = string.Empty;

        OnPendingSaleChanged?.Invoke();
    }

    public void RegisterBrowsePoint(ShopBrowsePoint browsePoint)
    {
        if (browsePoint == null)
            return;

        if (!browsePoints.Contains(browsePoint))
            browsePoints.Add(browsePoint);
    }

    public void UnregisterBrowsePoint(ShopBrowsePoint browsePoint)
    {
        if (browsePoint == null)
            return;

        if (browsePoints.Contains(browsePoint))
            browsePoints.Remove(browsePoint);
    }

    public List<ShopBrowsePoint> GetBrowsePoints()
    {
        return new List<ShopBrowsePoint>(browsePoints);
    }

    public int GetTotalItemsForSale()
    {
        int total = 0;

        for (int i = 0; i < registeredDisplays.Count; i++)
        {
            DisplayStand display = registeredDisplays[i];

            if (display == null)
                continue;

            total += display.GetItemsForSaleCount();
        }

        return total;
    }

    public int GetMaxAllowedBuyers()
    {
        int itemsForSale = GetTotalItemsForSale();
        return Mathf.Min(shopCapacity, itemsForSale);
    }

    public bool CanAcceptAnotherBuyer()
    {
        return activeBuyers.Count < GetMaxAllowedBuyers();
    }

    public bool TryRegisterBuyer(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return false;

        if (activeBuyers.Contains(buyer))
            return false;

        if (!CanAcceptAnotherBuyer())
            return false;

        activeBuyers.Add(buyer);
        return true;
    }

    public void UnregisterBuyer(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return;

        activeBuyers.Remove(buyer);
        RemoveFromCheckoutQueue(buyer);
    }

    public void RegisterQueueSpot(ShopQueueSpot queueSpot)
    {
        if (queueSpot == null)
            return;

        if (!queueSpots.Contains(queueSpot))
        {
            queueSpots.Add(queueSpot);
            queueSpots.Sort((a, b) => a.QueueIndex.CompareTo(b.QueueIndex));
        }
    }

    public void UnregisterQueueSpot(ShopQueueSpot queueSpot)
    {
        if (queueSpot == null)
            return;

        if (queueSpots.Contains(queueSpot))
            queueSpots.Remove(queueSpot);
    }

    public List<ShopQueueSpot> GetQueueSpots()
    {
        return new List<ShopQueueSpot>(queueSpots);
    }

    public bool TryRequestDeskAccess(ShopBuyerNPC buyer, out int queueIndex)
    {
        queueIndex = -1;

        if (buyer == null)
            return false;

        if (deskAssignedBuyer == buyer)
            return true;

        if (checkoutQueue.Contains(buyer))
        {
            queueIndex = checkoutQueue.IndexOf(buyer);
            return false;
        }

        if (deskAssignedBuyer == null)
        {
            deskAssignedBuyer = buyer;
            return true;
        }

        checkoutQueue.Add(buyer);
        queueIndex = checkoutQueue.Count - 1;
        NotifyQueuedBuyersToRefreshSpots();
        return false;
    }

    public Transform GetQueueSpotTransform(int queueIndex)
    {
        if (queueIndex < 0 || queueIndex >= queueSpots.Count)
            return null;

        ShopQueueSpot queueSpot = queueSpots[queueIndex];
        return queueSpot != null ? queueSpot.transform : null;
    }

    public void ReleaseDeskAccess(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return;

        if (deskAssignedBuyer != buyer)
            return;

        deskAssignedBuyer = null;

        PromoteNextQueuedBuyer();
    }

    private void PromoteNextQueuedBuyer()
    {
        while (checkoutQueue.Count > 0)
        {
            ShopBuyerNPC nextBuyer = checkoutQueue[0];
            checkoutQueue.RemoveAt(0);

            NotifyQueuedBuyersToRefreshSpots();

            if (nextBuyer == null)
                continue;

            deskAssignedBuyer = nextBuyer;
            nextBuyer.OnDeskAccessGranted();
            break;
        }
    }

    public void RemoveFromCheckoutQueue(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return;

        bool removedFromQueue = checkoutQueue.Remove(buyer);

        if (removedFromQueue)
            NotifyQueuedBuyersToRefreshSpots();

        if (deskAssignedBuyer == buyer)
        {
            deskAssignedBuyer = null;
            PromoteNextQueuedBuyer();
        }
    }

    private void NotifyQueuedBuyersToRefreshSpots()
    {
        for (int i = 0; i < checkoutQueue.Count; i++)
        {
            ShopBuyerNPC queuedBuyer = checkoutQueue[i];

            if (queuedBuyer != null)
                queuedBuyer.OnQueuePositionChanged(i);
        }
    }

    public bool HasAnyValidDisplayedGoods()
    {
        for (int i = 0; i < registeredDisplays.Count; i++)
        {
            DisplayStand display = registeredDisplays[i];

            if (display != null && display.HasAnyUnreservedItemsForSale())
                return true;
        }

        return false;
    }

    public bool TryReserveRandomDisplayedGood(ShopBuyerNPC buyer, out DisplayStand chosenDisplay, out int reservedSlotIndex)
    {
        chosenDisplay = null;
        reservedSlotIndex = -1;

        if (buyer == null)
            return false;

        List<(DisplayStand display, int slotIndex)> validSlots = new();

        for (int i = 0; i < registeredDisplays.Count; i++)
        {
            DisplayStand display = registeredDisplays[i];

            if (display == null)
                continue;

            for (int slotIndex = 0; slotIndex < display.SlotCount; slotIndex++)
            {
                if (display.HasUnreservedGoodInSlot(slotIndex))
                    validSlots.Add((display, slotIndex));
            }
        }

        if (validSlots.Count == 0)
            return false;

        int randomIndex = Random.Range(0, validSlots.Count);
        (DisplayStand display, int slotIndex) selected = validSlots[randomIndex];

        if (!selected.display.TryReserveSlot(selected.slotIndex, buyer))
            return false;

        chosenDisplay = selected.display;
        reservedSlotIndex = selected.slotIndex;
        return true;
    }

    public int CalculateBuyerSalePrice(ShopBuyerNPC buyer, CraftingGood good)
    {
        if (good == null)
            return 0;

        int baseValue = Mathf.Max(0, good.valueGold);
        float appealMultiplier = GetAppealPriceMultiplier();
        float negotiationMultiplier = GetNegotiationPriceMultiplier(buyer);

        int priceAfterAppeal = Mathf.RoundToInt(baseValue * appealMultiplier);
        int finalPrice = Mathf.RoundToInt(priceAfterAppeal * negotiationMultiplier);

        return Mathf.Max(0, finalPrice);
    }

    // Delegates to ShopCoreManager.GetAppealSaleMultiplier() — the single source of
    // truth for Appeal's price effect (Room_Shop.md Section 18), instead of keeping
    // a second, separately-maintained copy of the same table here.
    private float GetAppealPriceMultiplier()
    {
        return shopCoreManager != null ? shopCoreManager.GetAppealSaleMultiplier() : 1.00f;
    }

    private float GetNegotiationPriceMultiplier(ShopBuyerNPC buyer)
    {
        if (buyer == null)
            return 1.00f;

        return buyer.GetNegotiationPriceMultiplier();
    }

}