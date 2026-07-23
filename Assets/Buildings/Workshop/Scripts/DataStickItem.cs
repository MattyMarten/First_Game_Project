// Target path in your project: Assets/Buildings/Workshop/Scripts/DataStickItem.cs
// (this REPLACES your existing file — only change is the new baseMerchantPrice field
// under a new "Merchant" header, needed so Merchant's Data Stick pedestal has a price
// to roll variance against, same as UtilityCraftable.baseMerchantPrice / RawMaterial's.)

using UnityEngine;

/// <summary>
/// A physical, auto-consumed unlock item (Room_Workshop.md Section 9).
/// Fill in exactly ONE of goodsRecipe / gearRecipe depending on which
/// Workbench the target recipe belongs to.
///
/// Data Sticks never sit in Storage as an inventory item — acquiring one
/// immediately unlocks its recipe (or, if already unlocked, auto-converts
/// into coins — see DataStickConsumer).
/// </summary>
[CreateAssetMenu(fileName = "DataStick", menuName = "Scriptable Objects/Data Stick")]
public class DataStickItem : ScriptableObject
{
    [Header("Display")]
    public string stickName;
    [TextArea]
    public string description;
    public Sprite icon;

    [Header("Target Recipe — fill exactly one of these two")]
    public CraftingGood goodsRecipe;
    public UtilityCraftable gearRecipe;

    [Header("Merchant (Room_Merchant.md Section 10 — Data Stick pedestal)")]
    [Tooltip("Base price when sold from the Merchant's Data Stick pedestal, before the daily ± variance roll.")]
    public int baseMerchantPrice = 50;

    [Header("Duplicate Conversion (placeholder value, TBD balancing — Room_Workshop.md Section 21)")]
    public int duplicateCoinValue = 10;

    public IUnlockableRecipe GetTargetRecipe()
    {
        if (goodsRecipe != null) return goodsRecipe;
        if (gearRecipe != null) return gearRecipe;
        return null;
    }
}
