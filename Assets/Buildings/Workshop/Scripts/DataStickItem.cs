// Target path in your project: Assets/Buildings/Workshop/Scripts/DataStickItem.cs

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

    [Header("Duplicate Conversion (placeholder value, TBD balancing — Room_Workshop.md Section 21)")]
    public int duplicateCoinValue = 10;

    public IUnlockableRecipe GetTargetRecipe()
    {
        if (goodsRecipe != null) return goodsRecipe;
        if (gearRecipe != null) return gearRecipe;
        return null;
    }
}
