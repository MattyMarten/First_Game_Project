using UnityEngine;

// Room_Shop.md Section 16 — each decor item is a separate purchasable piece with its
// own name and effect. Owning a weaker version does NOT upgrade into a stronger one —
// they are separate assets. `tierIndex` exists only for the Merchant's progression-gating
// rule (weaker must be purchased before stronger appears in stock) — that gating is
// implemented in Stage 6 (Merchant), this field just carries the data now so it doesn't
// need to be retrofitted later.
[CreateAssetMenu(fileName = "NewDecorItem", menuName = "Shop/Decor/Decor Item")]
public class DecorItemData : ScriptableObject
{
    [Header("Info")]
    public string decorName;

    [TextArea(2, 4)]
    public string description;

    [Header("Placement")]
    public DecorSlotType slotType;

    [Header("Effect")]
    public DecorEffectType effectType;

    // BuyerAmount / Appeal: flat value (e.g. 1, 2, 3).
    // SaleValue / DirtReduction / GenerousBuyer / HaggleReduction / NonBuyerReduction:
    // decimal percent (e.g. 0.05 = 5%).
    // RecruitChance: decimal chance add (e.g. 0.0 placeholder — single-piece effect only).
    public float effectValue;

    [Header("Merchant Progression Gating (Stage 6)")]
    [Tooltip("0 = base tier, no prerequisite. Higher tiers require the prior tier in the same effect category to be owned before the Merchant will stock them.")]
    public int tierIndex;

    [Header("Cost")]
    public int price = 25;

    // Category label shown separately from the value so the UI can show
    // "Category: Sale Value" / "Effect: +10%" as two distinct fields rather than
    // one blended sentence.
    public string GetCategoryLabel()
    {
        return effectType switch
        {
            DecorEffectType.BuyerAmount => "Buyer Amount",
            DecorEffectType.SaleValue => "Sale Value",
            DecorEffectType.Appeal => "Appeal",
            DecorEffectType.RecruitChance => "Recruit Chance",
            DecorEffectType.DirtReduction => "Dirt Reduction",
            DecorEffectType.GenerousBuyer => "Generous Buyer Chance",
            DecorEffectType.HaggleReduction => "Haggle Buyer Reduction",
            DecorEffectType.NonBuyerReduction => "Non-Buyer Reduction",
            _ => effectType.ToString()
        };
    }

    // The exact signed value, formatted per Section 16's own units for that category
    // (flat count vs. percent), so a player reading the panel sees precisely what a
    // purchase changes rather than a vague "improves X" statement.
    public string GetEffectValueLabel()
    {
        return effectType switch
        {
            DecorEffectType.BuyerAmount => $"+{effectValue:0} buyers per day",
            DecorEffectType.Appeal => $"+{effectValue:0} appeal",
            DecorEffectType.SaleValue => $"+{effectValue:P0} to item sale price",
            DecorEffectType.RecruitChance => $"+{effectValue:P0} recruit-visitor spawn chance",
            DecorEffectType.DirtReduction => $"-{effectValue:P0} dirt spawn chance",
            DecorEffectType.GenerousBuyer => $"+{effectValue:P0} generous-buyer chance",
            DecorEffectType.HaggleReduction => $"-{effectValue:P0} haggle-buyer chance",
            DecorEffectType.NonBuyerReduction => $"-{effectValue:P0} non-buyer chance",
            _ => effectValue.ToString("0.##")
        };
    }

    // Full multi-line readout for the decor panel: name, slot, category, precise
    // effect, tier, price, and the free-text description last.
    public string GetReadout()
    {
        string tierText = tierIndex > 0 ? $"Tier {tierIndex + 1}" : "Base tier";

        return
            $"{decorName}\n" +
            $"Slot: {slotType}\n" +
            $"Category: {GetCategoryLabel()}\n" +
            $"Effect: {GetEffectValueLabel()}\n" +
            $"{tierText} — {price} coins" +
            (string.IsNullOrEmpty(description) ? string.Empty : $"\n{description}");
    }
}
