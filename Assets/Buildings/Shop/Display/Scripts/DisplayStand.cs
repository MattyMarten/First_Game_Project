using UnityEngine;

public class DisplayStand : MonoBehaviour
{
    [Header("Placement Slots")]
    [SerializeField] private Transform[] slotPoints;

    [Header("Shop Registration")]
    [SerializeField] private ShopManager shopManager;

    [Header("Buyer")]
    [SerializeField] private Transform buyerApproachPoint;

    private CraftingGood[] placedGoods;
    private GameObject[] spawnedVisuals;
    private ShopBuyerNPC[] reservedByBuyers;

    private void Awake()
    {
        placedGoods = new CraftingGood[slotPoints.Length];
        spawnedVisuals = new GameObject[slotPoints.Length];
        reservedByBuyers = new ShopBuyerNPC[slotPoints.Length];
    }

    private void Start()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (shopManager != null)
            shopManager.RegisterDisplay(this);
    }

    public int SlotCount => slotPoints != null ? slotPoints.Length : 0;

    public CraftingGood GetGoodInSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
            return null;

        return placedGoods[slotIndex];
    }

    public bool PlaceGoodInSlot(int slotIndex, CraftingGood good)
    {
        if (!IsValidSlot(slotIndex) || good == null)
            return false;

        placedGoods[slotIndex] = good;
        RefreshSlotVisual(slotIndex);
        return true;
    }

    public CraftingGood RemoveGoodFromSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
            return null;

        CraftingGood removed = placedGoods[slotIndex];
        placedGoods[slotIndex] = null;
        ClearSlotVisual(slotIndex);
        return removed;
    }

    public Transform GetSlotPoint(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
            return null;

        return slotPoints[slotIndex];
    }

    private void RefreshSlotVisual(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
            return;

        ClearSlotVisual(slotIndex);

        CraftingGood good = placedGoods[slotIndex];
        Transform slotPoint = slotPoints[slotIndex];

        if (good == null || slotPoint == null)
            return;

        if (good.goodsPrefab == null)
        {
            Debug.LogWarning($"DisplayStand: {good.goodName} has no goodsPrefab assigned.", this);
            return;
        }

        GameObject spawned = Instantiate(good.goodsPrefab, slotPoint.position, slotPoint.rotation, slotPoint);
        spawnedVisuals[slotIndex] = spawned;
    }

    private void ClearSlotVisual(int slotIndex)
    {
        if (!IsValidSlot(slotIndex))
            return;

        if (spawnedVisuals[slotIndex] != null)
        {
            Destroy(spawnedVisuals[slotIndex]);
            spawnedVisuals[slotIndex] = null;
        }
    }

    private bool IsValidSlot(int slotIndex)
    {
        return slotPoints != null &&
               placedGoods != null &&
               spawnedVisuals != null &&
               slotIndex >= 0 &&
               slotIndex < slotPoints.Length;
    }

    private void OnDestroy()
    {
        if (shopManager != null)
            shopManager.UnregisterDisplay(this);
    }

    public bool HasAnyItemsForSale()
    {
        if (placedGoods == null || placedGoods.Length == 0)
            return false;

        for (int i = 0; i < placedGoods.Length; i++)
        {
            if (placedGoods[i] != null)
                return true;
        }

        return false;
    }

    public CraftingGood TakeFirstAvailableGood()
    {
        if (placedGoods == null || placedGoods.Length == 0)
            return null;

        for (int i = 0; i < placedGoods.Length; i++)
        {
            if (placedGoods[i] != null)
                return RemoveGoodFromSlot(i);
        }

        return null;
    }

    public Transform GetBuyerApproachPoint()
    {
        if (buyerApproachPoint != null)
            return buyerApproachPoint;

        return transform;
    }

    public int GetItemsForSaleCount()
    {
        if (placedGoods == null || placedGoods.Length == 0)
            return 0;

        int count = 0;

        for (int i = 0; i < placedGoods.Length; i++)
        {
            if (placedGoods[i] != null)
                count++;
        }

        return count;
    }

    public bool HasAnyUnreservedItemsForSale()
    {
        if (placedGoods == null || placedGoods.Length == 0)
            return false;

        for (int i = 0; i < placedGoods.Length; i++)
        {
            if (placedGoods[i] != null && reservedByBuyers[i] == null)
                return true;
        }

        return false;
    }

    public bool TryReserveFirstAvailableSlot(ShopBuyerNPC buyer, out int reservedSlotIndex)
    {
        reservedSlotIndex = -1;

        if (buyer == null || placedGoods == null || reservedByBuyers == null)
            return false;

        for (int i = 0; i < placedGoods.Length; i++)
        {
            if (placedGoods[i] == null)
                continue;

            if (reservedByBuyers[i] != null)
                continue;

            reservedByBuyers[i] = buyer;
            reservedSlotIndex = i;
            return true;
        }

        return false;
    }

    public void ReleaseReservedSlot(int slotIndex, ShopBuyerNPC buyer)
    {
        if (!IsValidSlot(slotIndex) || reservedByBuyers == null)
            return;

        if (reservedByBuyers[slotIndex] == buyer)
            reservedByBuyers[slotIndex] = null;
    }

    public CraftingGood TakeReservedGood(int slotIndex, ShopBuyerNPC buyer)
    {
        if (!IsValidSlot(slotIndex) || reservedByBuyers == null)
            return null;

        if (reservedByBuyers[slotIndex] != buyer)
            return null;

        CraftingGood good = placedGoods[slotIndex];
        reservedByBuyers[slotIndex] = null;

        if (good == null)
            return null;

        placedGoods[slotIndex] = null;
        ClearSlotVisual(slotIndex);
        return good;
    }

    public bool HasUnreservedGoodInSlot(int slotIndex)
    {
        if (!IsValidSlot(slotIndex) || reservedByBuyers == null)
            return false;

        return placedGoods[slotIndex] != null && reservedByBuyers[slotIndex] == null;
    }

    public bool TryReserveSlot(int slotIndex, ShopBuyerNPC buyer)
    {
        if (!IsValidSlot(slotIndex) || reservedByBuyers == null || buyer == null)
            return false;

        if (placedGoods[slotIndex] == null)
            return false;

        if (reservedByBuyers[slotIndex] != null)
            return false;

        reservedByBuyers[slotIndex] = buyer;
        return true;
    }
}