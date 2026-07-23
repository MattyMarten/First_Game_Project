// Target path in your project: Assets/Buildings/Merchant/Scripts/Data/MerchantPedestalCategory.cs

/// <summary>
/// One entry per physical pedestal group in the Merchant Room
/// (Room_Merchant.md Section 10). Utility/Backpack/Charm map directly
/// onto UtilityCraftable.category — same items Utility Station crafts,
/// just a different sales pool. Material/ShopDecor/DataStick pull from
/// their own respective SO types instead.
/// </summary>
public enum MerchantPedestalCategory
{
    Utility,    // 3 pedestals — UtilityCraftable where category == UtilityCategory.Utility
    Backpack,   // 1 pedestal  — UtilityCraftable where category == UtilityCategory.Backpack
    Charm,      // 2 pedestals — UtilityCraftable where category == UtilityCategory.Charm
    Material,   // 3 pedestals — RawMaterial
    ShopDecor,  // 4 pedestals — DecorItemData (progression-gated by tierIndex, see MerchantRoomManager)
    DataStick   // 1 pedestal  — DataStickItem, 10%/day roll, curated pool only
}
