// Room_Shop.md Section 16 — Decor System.
// One enum per effect category. Multiple DecorItemData assets can share a category
// at different strengths (e.g. +1/+2/+3 Buyer Decor are all BuyerAmount).
public enum DecorEffectType
{
    BuyerAmount,        // flat add to daily Desk1 buyer count
    SaleValue,          // percent add to sale price (e.g. 0.05 = +5%)
    Appeal,             // flat add to effective Shop Appeal
    RecruitChance,       // flat add to recruit-visitor spawn chance (0-1 scale)
    DirtReduction,       // percent subtracted from dirt spawn chance
    GenerousBuyer,       // percent add to generous-buyer chance
    HaggleReduction,     // percent subtracted from haggle-buyer chance
    NonBuyerReduction    // percent subtracted from non-buyer chance
}

// Room_Shop.md Section 16 — Shop decor is split into Wall Decoration and Floor Decoration.
public enum DecorSlotType
{
    Wall,
    Floor
}
