// Target path in your project: Assets/Buildings/Workshop/Machines/Gear Upgrade Station/Scripts/GearUpgradeStationManager.cs
// (new machine — this folder doesn't exist yet in your project, create it: Assets/Buildings/Workshop/Machines/Gear Upgrade Station/Scripts/)

using System;
using UnityEngine;

/// <summary>
/// One row of the STATION's own upgrade table (Room_Workshop.md Section 18).
/// This gates which item TIER the station can upgrade items INTO, and at what
/// material cost multiplier — separate from an individual item's own tier
/// (UtilityCraftable.tier). Mirrors CoreRoomManager's upgrade-level pattern
/// (Stage 2) since both are stand-ins for the Dwarf's real Upgrade Board (Stage 8).
/// </summary>
[Serializable]
public class GearUpgradeStationLevelData
{
    public ItemTier maxReachableTier = ItemTier.Tier2;

    [Tooltip("Multiplies upgradeMaterials cost. 1 = normal cost, less than 1 = reduced cost.")]
    [Range(0f, 1f)]
    public float materialCostMultiplier = 1f;
}

public class GearUpgradeStationManager : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private RawMaterialStorage materialStorage;
    [SerializeField] private CraftedUtilityStorage utilityStorage;

    [Header("Station Upgrade Levels (index 0 = LV1)")]
    [SerializeField]
    private GearUpgradeStationLevelData[] stationLevels = new GearUpgradeStationLevelData[]
    {
        new GearUpgradeStationLevelData { maxReachableTier = ItemTier.Tier2, materialCostMultiplier = 1f },
        new GearUpgradeStationLevelData { maxReachableTier = ItemTier.Tier2, materialCostMultiplier = 0.75f },
        new GearUpgradeStationLevelData { maxReachableTier = ItemTier.Tier3, materialCostMultiplier = 0.75f },
    };

    [Header("Current State (visible for debugging)")]
    [SerializeField] private int stationLevelIndex = 0;

    public int StationLevel => stationLevelIndex + 1; // displayed as LV1/LV2/LV3
    public bool IsMaxLevel => stationLevelIndex >= stationLevels.Length - 1;

    private GearUpgradeStationLevelData CurrentLevelData =>
        stationLevels[Mathf.Clamp(stationLevelIndex, 0, stationLevels.Length - 1)];

    public event Action<int> OnStationLevelChanged;

    private void Awake()
    {
        if (materialStorage == null)
            materialStorage = FindAnyObjectByType<RawMaterialStorage>();

        if (utilityStorage == null)
            utilityStorage = FindAnyObjectByType<CraftedUtilityStorage>();
    }

    /// <summary>
    /// Stand-in for the Dwarf's real Upgrade Board (Stage 8) — same pattern as
    /// CoreRoomManager.TryUpgrade(). Call from this context-menu action, a debug
    /// button, or the Inspector until the real milestone-trigger exists.
    /// </summary>
    [ContextMenu("Debug: Upgrade Station Level")]
    public void TryUpgradeStationLevel()
    {
        if (IsMaxLevel)
        {
            Debug.Log("[GearUpgradeStationManager] Station is already at max level.");
            return;
        }

        stationLevelIndex++;
        Debug.Log($"[GearUpgradeStationManager] Station upgraded to LV{StationLevel}.");
        OnStationLevelChanged?.Invoke(StationLevel);
    }

    /// <summary>Multiplies a base upgrade-material amount by the station's current cost multiplier.</summary>
    public int GetScaledCost(int baseAmount)
    {
        return Mathf.Max(1, Mathf.CeilToInt(baseAmount * CurrentLevelData.materialCostMultiplier));
    }

    public bool CanUpgrade(UtilityCraftable item) => GetBlockReason(item) == null;

    /// <summary>Returns null if the item can be upgraded right now, otherwise a short reason for UI display.</summary>
    public string GetBlockReason(UtilityCraftable item)
    {
        if (item == null)
            return "No item selected.";

        if (item.nextTierItem == null)
            return "Already at max tier.";

        if (item.nextTierItem.tier > CurrentLevelData.maxReachableTier)
            return $"Station level too low for {item.nextTierItem.tier}.";

        if (utilityStorage == null || !utilityStorage.HasAny(item))
            return "You don't own this item.";

        if (materialStorage == null)
            return "No material storage found.";

        var owned = materialStorage.GetAll();
        foreach (var req in item.upgradeMaterials)
        {
            int needed = GetScaledCost(req.amount);
            int have = owned.TryGetValue(req.material, out int amt) ? amt : 0;
            if (have < needed)
                return $"Not enough {req.material.displayName} ({have}/{needed}).";
        }

        return null;
    }

    /// <summary>Consumes the source item + scaled upgrade materials, grants one of the next-tier item.</summary>
    public bool TryUpgrade(UtilityCraftable item)
    {
        if (!CanUpgrade(item))
            return false;

        foreach (var req in item.upgradeMaterials)
            materialStorage.TrySpend(req.material, GetScaledCost(req.amount));

        utilityStorage.TrySpend(item, 1);
        utilityStorage.Add(item.nextTierItem, 1);

        Debug.Log($"[GearUpgradeStationManager] Upgraded {item.itemName} -> {item.nextTierItem.itemName}");
        return true;
    }
}
