// Target path in your project: Assets/Buildings/Merchant/Scripts/Data/MerchantPedestalSlot.cs

using System;

/// <summary>
/// Runtime state of one physical pedestal in the Merchant Room.
/// Rolled fresh every morning by MerchantRoomManager; does not
/// refresh again until the next morning (Room_Merchant.md Section 11).
/// </summary>
[Serializable]
public class MerchantPedestalSlot
{
    public MerchantPedestalCategory category;

    // Exactly one of these is non-null, matching `category` above.
    public UtilityCraftable utilityItem;   // Utility / Backpack / Charm pedestals
    public RawMaterial materialItem;       // Material pedestals
    public DecorItemData decorItem;        // Shop Decor pedestals
    public DataStickItem dataStickItem;    // Data Stick pedestal

    public int finalPrice;
    public int quantity;   // Data Stick and Shop Decor pedestals ignore this (single item, not stackable)
    public bool isEmpty;   // true if today's roll produced nothing (Data Stick's 90% miss, or an empty pool)

    public bool HasAnyItem =>
        utilityItem != null || materialItem != null || decorItem != null || dataStickItem != null;

    /// <summary>
    /// Generic display name regardless of which item type is on this pedestal.
    /// Lets UI code (MerchantPedestalUI) stay a single generic panel instead of
    /// branching per category.
    /// </summary>
    public string GetDisplayName()
    {
        if (utilityItem != null) return utilityItem.itemName;
        if (materialItem != null) return materialItem.displayName;
        if (decorItem != null) return decorItem.decorName;
        if (dataStickItem != null) return dataStickItem.stickName;
        return "Empty";
    }

    /// <summary>
    /// NOTE: DecorItemData currently has no Sprite icon field (checked directly —
    /// Room_Shop.md's Section 16 decor readout is text-only). Returns null for
    /// decor pedestals until/unless an icon field gets added to DecorItemData;
    /// UI should hide the icon image cleanly when this returns null.
    /// </summary>
    public UnityEngine.Sprite GetIcon()
    {
        if (utilityItem != null) return utilityItem.icon;
        if (materialItem != null) return materialItem.icon;
        if (dataStickItem != null) return dataStickItem.icon;
        return null; // decorItem has no icon field yet
    }
}
