// Target path in your project: Assets/Buildings/Merchant/Scripts/Core/MerchantRoomManager.cs

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Owns all 13 physical pedestals in the Merchant Room and rolls them fresh
/// every morning (Room_Merchant.md Sections 10-11). Replaces the old NPC-visit
/// model (MerchantDayManager + MerchantVisitorNPC + ServiceDeskManager's
/// pending-merchant-visit path) with a static pedestal-room model, same
/// shape as Storage/Display.
///
/// Data Stick variant (project-specific simplification, agreed in chat —
/// deviates from Room_Merchant.md's literal "14th dedicated pedestal"):
/// there is no separate physical Data Stick pedestal. Instead, after the 13
/// regular pedestals roll normally, there's an independent 10% roll for
/// "does a Data Stick appear today at all." If it hits, ONE of the 13
/// pedestals is picked at random and its rolled item is replaced by the
/// Data Stick for that day — the item that would have been there simply
/// doesn't spawn. This means every pedestal prefab needs to be able to
/// display a DataStickItem generically (icon/name/price), not just its
/// "home" category's item type.
///
/// Pedestal counts (fixed, per Section 10 minus the dedicated 14th slot):
///   Utility 3, Backpack 1, Charm 2, Material 3, ShopDecor 4  =  13 total
/// </summary>
public class MerchantRoomManager : MonoBehaviour
{
    private const int UtilityCount = 3;
    private const int BackpackCount = 1;
    private const int CharmCount = 2;
    private const int MaterialCount = 3;
    private const int ShopDecorCount = 4;

    [Header("Item Pools — assign every candidate item here")]
    [Tooltip("All UtilityCraftable assets with category == UtilityCategory.Utility that the Merchant is allowed to sell.")]
    [SerializeField] private List<UtilityCraftable> utilityPool = new();

    [Tooltip("All UtilityCraftable assets with category == UtilityCategory.Backpack.")]
    [SerializeField] private List<UtilityCraftable> backpackPool = new();

    [Tooltip("All UtilityCraftable assets with category == UtilityCategory.Charm.")]
    [SerializeField] private List<UtilityCraftable> charmPool = new();

    [Tooltip("All RawMaterial assets the Merchant is allowed to sell (flat, no tiers).")]
    [SerializeField] private List<RawMaterial> materialPool = new();

    [Tooltip("All DecorItemData assets, every tier. Gating (tierIndex prerequisite) is applied at roll time, not here.")]
    [SerializeField] private List<DecorItemData> decorPool = new();

    [Tooltip("The Merchant's OWN curated Data Stick pool (Room_Workshop.md Section 9) — not every Data Stick in the game, only the ones the Merchant is allowed to offer. Section 23 open question: exact contents still TBD.")]
    [SerializeField] private List<DataStickItem> curatedDataStickPool = new();

    [Header("Pricing")]
    [Tooltip("Doc-stated standard is -10% to +10% (Section 10). Old MerchantDayManager code used ±20% — confirm which is correct before relying on this in a real build.")]
    [Range(0f, 0.5f)]
    [SerializeField] private float priceVariance = 0.10f;

    [Tooltip("Daily chance the Data Stick pedestal rolls anything at all (Section 10: flat 10%).")]
    [Range(0f, 1f)]
    [SerializeField] private float dataStickRollChance = 0.10f;

    [Header("Ownership Sources (for Shop Decor progression gating)")]
    [SerializeField] private DecorStorage decorStorage;
    [SerializeField] private DecorManager decorManager;

    [Header("Room Flavor (speech-cloud sign)")]
    [Tooltip("Seeded from the old MerchantProfileData's default lines. The room has no single NPC anymore, so this is now a room-wide flavor pool rather than a per-merchant one.")]
    [SerializeField] private List<string> flavorDialogueLines = new()
    {
        "I've got some new wares. Take a look.",
        "You should see what I brought today.",
        "I've come with a fresh stock of goods.",
        "Take a look at my wares.",
        "I've brought a few things you may want."
    };

    private string currentFlavorLine;

    /// <summary>Fired whenever the sign's line changes (on purchase, and once at startup).</summary>
    public event System.Action<string> OnFlavorLineChanged;

    public string CurrentFlavorLine => currentFlavorLine;

    [Header("Purchase Routing")]
    [SerializeField] private CobaltCoinStorage coinStorage;
    [SerializeField] private CraftedUtilityStorage utilityStorage;
    [SerializeField] private RawMaterialStorage rawMaterialStorage;
    [SerializeField] private DataStickConsumer dataStickConsumer;

    [Header("Runtime State")]
    [SerializeField] private List<MerchantPedestalSlot> pedestals = new();

    public IReadOnlyList<MerchantPedestalSlot> Pedestals => pedestals;

    public event System.Action OnPedestalsRolled;

    private void Awake()
    {
        if (decorStorage == null)
            decorStorage = FindAnyObjectByType<DecorStorage>();

        if (decorManager == null)
            decorManager = FindAnyObjectByType<DecorManager>();

        if (coinStorage == null)
            coinStorage = FindAnyObjectByType<CobaltCoinStorage>();

        if (utilityStorage == null)
            utilityStorage = FindAnyObjectByType<CraftedUtilityStorage>();

        if (rawMaterialStorage == null)
            rawMaterialStorage = FindAnyObjectByType<RawMaterialStorage>();

        if (dataStickConsumer == null)
            dataStickConsumer = FindAnyObjectByType<DataStickConsumer>();
    }

    private void OnEnable()
    {
        if (DayPhaseSystem.Instance != null)
            DayPhaseSystem.Instance.OnDayAdvanced += HandleDayAdvanced;
    }

    private void OnDisable()
    {
        if (DayPhaseSystem.Instance != null)
            DayPhaseSystem.Instance.OnDayAdvanced -= HandleDayAdvanced;
    }

    private void Start()
    {
        // Roll once on scene start so the room isn't empty before the first
        // real day-advance fires (mirrors MerchantDayManager's old Start() call).
        RollAllPedestals();
        RollNewFlavorLine();
    }

    private void HandleDayAdvanced(int newDay)
    {
        RollAllPedestals();
    }

    /// <summary>
    /// Rolls every pedestal fresh. Does NOT touch pedestals mid-day
    /// (Section 11: pedestals don't refresh mid-day) — only call this
    /// on a real morning transition.
    /// </summary>
    public void RollAllPedestals()
    {
        pedestals.Clear();

        RollUtilityCategory(MerchantPedestalCategory.Utility, utilityPool, UtilityCount);
        RollUtilityCategory(MerchantPedestalCategory.Backpack, backpackPool, BackpackCount);
        RollUtilityCategory(MerchantPedestalCategory.Charm, charmPool, CharmCount);
        RollMaterialCategory(MaterialCount);
        RollDecorCategory(ShopDecorCount);

        TryOverlayDataStick();

        OnPedestalsRolled?.Invoke();
    }

    private void RollUtilityCategory(MerchantPedestalCategory category, List<UtilityCraftable> pool, int count)
    {
        List<UtilityCraftable> valid = new();
        foreach (UtilityCraftable item in pool)
            if (item != null)
                valid.Add(item);

        Shuffle(valid);
        int rollCount = Mathf.Min(count, valid.Count);

        for (int i = 0; i < count; i++)
        {
            if (i < rollCount)
            {
                UtilityCraftable item = valid[i];
                pedestals.Add(new MerchantPedestalSlot
                {
                    category = category,
                    utilityItem = item,
                    finalPrice = GeneratePrice(item.baseMerchantPrice),
                    quantity = Random.Range(1, 5),
                    isEmpty = false
                });
            }
            else
            {
                // Not enough valid items in the pool to fill every pedestal —
                // leave the remainder empty rather than repeating an item.
                pedestals.Add(new MerchantPedestalSlot { category = category, isEmpty = true });
            }
        }
    }

    private void RollMaterialCategory(int count)
    {
        List<RawMaterial> valid = new();
        foreach (RawMaterial item in materialPool)
            if (item != null)
                valid.Add(item);

        Shuffle(valid);
        int rollCount = Mathf.Min(count, valid.Count);

        for (int i = 0; i < count; i++)
        {
            if (i < rollCount)
            {
                RawMaterial item = valid[i];
                pedestals.Add(new MerchantPedestalSlot
                {
                    category = MerchantPedestalCategory.Material,
                    materialItem = item,
                    finalPrice = GeneratePrice(item.baseMerchantPrice),
                    quantity = Random.Range(5, 11),
                    isEmpty = false
                });
            }
            else
            {
                pedestals.Add(new MerchantPedestalSlot { category = MerchantPedestalCategory.Material, isEmpty = true });
            }
        }
    }

    private void RollDecorCategory(int count)
    {
        List<DecorItemData> valid = new();
        foreach (DecorItemData item in decorPool)
            if (item != null && IsDecorUnlocked(item))
                valid.Add(item);

        Shuffle(valid);
        int rollCount = Mathf.Min(count, valid.Count);

        for (int i = 0; i < count; i++)
        {
            if (i < rollCount)
            {
                DecorItemData item = valid[i];
                pedestals.Add(new MerchantPedestalSlot
                {
                    category = MerchantPedestalCategory.ShopDecor,
                    decorItem = item,
                    finalPrice = GeneratePrice(item.price),
                    quantity = 1,
                    isEmpty = false
                });
            }
            else
            {
                pedestals.Add(new MerchantPedestalSlot { category = MerchantPedestalCategory.ShopDecor, isEmpty = true });
            }
        }
    }

    /// <summary>
    /// Section 10 gating rule: a decor item unlocks for Merchant stock once the
    /// player owns the previous tier of the SAME effect category. Base tier
    /// (tierIndex == 0) is always eligible. "Owns" checks both DecorStorage
    /// (owned, not placed) and DecorManager (currently placed) — either counts.
    /// </summary>
    private bool IsDecorUnlocked(DecorItemData candidate)
    {
        if (candidate.tierIndex <= 0)
            return true;

        int requiredPriorTier = candidate.tierIndex - 1;

        if (decorStorage != null)
        {
            foreach (var kv in decorStorage.GetAll())
            {
                if (kv.Key != null && kv.Key.effectType == candidate.effectType && kv.Key.tierIndex == requiredPriorTier && kv.Value > 0)
                    return true;
            }
        }

        if (decorManager != null)
        {
            foreach (DecorItemData placed in decorManager.PlacedDecor.Values)
            {
                if (placed != null && placed.effectType == candidate.effectType && placed.tierIndex == requiredPriorTier)
                    return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Independent 10% roll for "does a Data Stick appear anywhere today."
    /// If it hits, overwrites ONE randomly-chosen already-rolled pedestal
    /// (from the full 13) with the Data Stick instead — whatever item that
    /// pedestal rolled a moment ago is discarded, matching "the item that
    /// should be there won't spawn."
    /// </summary>
    private void TryOverlayDataStick()
    {
        if (pedestals.Count == 0)
            return;

        bool hit = Random.value < dataStickRollChance;
        if (!hit)
            return;

        List<DataStickItem> valid = new();
        foreach (DataStickItem item in curatedDataStickPool)
            if (item != null)
                valid.Add(item);

        if (valid.Count == 0)
            return; // curated pool empty — nothing to overlay, regular roll stands

        DataStickItem chosen = valid[Random.Range(0, valid.Count)];
        int targetIndex = Random.Range(0, pedestals.Count);

        pedestals[targetIndex] = new MerchantPedestalSlot
        {
            category = MerchantPedestalCategory.DataStick,
            dataStickItem = chosen,
            finalPrice = GeneratePrice(chosen.baseMerchantPrice),
            quantity = 1,
            isEmpty = false
        };
    }

    /// <summary>
    /// Buys exactly ONE unit at `finalPrice` (the per-unit price). For
    /// stackable pedestals (Utility/Backpack/Charm/Material), this reduces
    /// quantity by 1 and leaves the pedestal stocked until quantity hits 0.
    /// For single-item pedestals (Shop Decor, Data Stick) this is identical
    /// to TryPurchaseAll.
    /// </summary>
    public bool TryPurchaseOne(int index) => TryPurchaseInternal(index, 1);

    /// <summary>
    /// Buys the pedestal's ENTIRE remaining quantity in one purchase, at
    /// finalPrice-per-unit * quantity total.
    /// </summary>
    public bool TryPurchaseAll(int index)
    {
        if (index < 0 || index >= pedestals.Count)
            return false;

        MerchantPedestalSlot slot = pedestals[index];

        if (slot == null || slot.isEmpty || !slot.HasAnyItem)
            return false;

        int amount = IsStackable(slot) ? Mathf.Max(1, slot.quantity) : 1;
        return TryPurchaseInternal(index, amount);
    }

    private bool IsStackable(MerchantPedestalSlot slot)
    {
        // Decor and Data Stick pedestals are always single-item, never stacked.
        return slot.utilityItem != null || slot.materialItem != null;
    }

    private bool TryPurchaseInternal(int index, int requestedAmount)
    {
        if (index < 0 || index >= pedestals.Count)
            return false;

        MerchantPedestalSlot slot = pedestals[index];

        if (slot == null || slot.isEmpty || !slot.HasAnyItem)
            return false;

        bool stackable = IsStackable(slot);
        int buyAmount = stackable ? Mathf.Clamp(requestedAmount, 1, slot.quantity) : 1;

        if (buyAmount <= 0)
            return false;

        int totalPrice = slot.finalPrice * buyAmount; // finalPrice is PER UNIT

        if (coinStorage == null || !coinStorage.TrySpend(totalPrice))
            return false; // can't afford it, or no CobaltCoinStorage wired in

        if (slot.utilityItem != null && utilityStorage != null)
        {
            utilityStorage.Add(slot.utilityItem, buyAmount);
        }
        else if (slot.materialItem != null && rawMaterialStorage != null)
        {
            rawMaterialStorage.Add(slot.materialItem, buyAmount);
        }
        else if (slot.decorItem != null && decorStorage != null)
        {
            decorStorage.Add(slot.decorItem, buyAmount);
        }
        else if (slot.dataStickItem != null && dataStickConsumer != null)
        {
            // Data Sticks never sit in storage — acquiring one immediately
            // unlocks its recipe or converts to coins if already unlocked
            // (see DataStickConsumer.Acquire).
            dataStickConsumer.Acquire(slot.dataStickItem);
        }
        else
        {
            // Coins were already spent above but nothing could receive the
            // item — refund rather than silently destroying it.
            coinStorage.Add(totalPrice);
            return false;
        }

        if (stackable)
        {
            slot.quantity -= buyAmount;

            if (slot.quantity <= 0)
                pedestals[index] = new MerchantPedestalSlot { category = slot.category, isEmpty = true };
            // else: slot stays in place with reduced quantity, same price/item.
        }
        else
        {
            pedestals[index] = new MerchantPedestalSlot { category = slot.category, isEmpty = true };
        }

        RollNewFlavorLine();

        return true;
    }

    /// <summary>
    /// Picks a new random flavor line for the sign and fires OnFlavorLineChanged.
    /// Called once at startup and again after every successful purchase.
    /// </summary>
    private void RollNewFlavorLine()
    {
        if (flavorDialogueLines == null || flavorDialogueLines.Count == 0)
            return;

        List<string> validLines = new();
        foreach (string line in flavorDialogueLines)
            if (!string.IsNullOrWhiteSpace(line))
                validLines.Add(line);

        if (validLines.Count == 0)
            return;

        // Avoid repeating the exact same line twice in a row when there's
        // more than one option to pick from.
        string next;
        if (validLines.Count == 1)
        {
            next = validLines[0];
        }
        else
        {
            do
            {
                next = validLines[Random.Range(0, validLines.Count)];
            } while (next == currentFlavorLine);
        }

        currentFlavorLine = next;
        OnFlavorLineChanged?.Invoke(currentFlavorLine);
    }

    private int GeneratePrice(int basePrice)
    {
        int safeBasePrice = Mathf.Max(1, basePrice);
        float multiplier = Random.Range(1f - priceVariance, 1f + priceVariance);
        return Mathf.Max(1, Mathf.RoundToInt(safeBasePrice * multiplier));
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}
