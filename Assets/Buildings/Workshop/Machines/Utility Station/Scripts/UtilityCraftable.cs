// Target path in your project: Assets/Buildings/Workshop/Machines/Utility Station/Scripts/UtilityCraftable.cs
// (this REPLACES your existing file of the same name — changes are marked with "// TIER:")

using UnityEngine;
using System.Collections.Generic;

public enum UtilityCategory
{
    Utility,
    Backpack,
    Charm
}

// TIER: Tier1/2/3 for the Gear Upgrade Station (Room_Workshop.md Section 12).
// Declared in this order on purpose — comparisons like "nextTier > station's max reachable tier"
// rely on Tier1 < Tier2 < Tier3 as ints.
public enum ItemTier
{
    Tier1,
    Tier2,
    Tier3
}

[CreateAssetMenu(fileName = "UtilityCraftable", menuName = "Scriptable Objects/Utility Craftable")]
public class UtilityCraftable : ScriptableObject, IUnlockableRecipe
{
    [Header("Display")]
    public string itemName;

    [TextArea]
    public string description;

    public Sprite icon;

    [Header("Category")]
    public UtilityCategory category;

    [Header("Merchant")]
    public int baseMerchantPrice = 20;

    [System.Serializable]
    public struct MaterialRequirement
    {
        public RawMaterial material;
        public int amount;
    }

    [Header("Materials Required")]
    public List<MaterialRequirement> requiredMaterials = new();

    [Header("Unlock (Room_Workshop.md Section 9)")]
    [Tooltip("If true, this recipe is available from the start and never needs a Data Stick.")]
    public bool isUnlockedByDefault = true;

    public string RecipeId => name;
    public bool IsUnlockedByDefault => isUnlockedByDefault;

    // TIER: name stays the same conceptually ("Basic Axe"), tier is just a label + a
    // separate linked asset (e.g. "Basic Axe T2") per your naming convention.
    [Header("Tier (Room_Workshop.md Section 12, Gear Upgrade Station)")]
    [Tooltip("This item's own tier. T1 items are the base craftable version.")]
    public ItemTier tier = ItemTier.Tier1;

    [Tooltip("The asset this upgrades INTO at the Gear Upgrade Station. Leave empty if this is already the max tier (T3).")]
    public UtilityCraftable nextTierItem;

    [Tooltip("Materials the UPGRADE consumes (separate from requiredMaterials, which is only what crafting the base T1 item costs). Not used for T3 items with no nextTierItem.")]
    public List<MaterialRequirement> upgradeMaterials = new();
}
