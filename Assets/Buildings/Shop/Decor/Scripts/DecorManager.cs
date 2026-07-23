using System.Collections.Generic;
using UnityEngine;

// Room_Shop.md Section 16 — Decor System.
// Shop-wide aggregator, same role for decor that ShopCoreManager plays for Appeal/spawn.
// Owns no physical placement logic itself — DecorSpot (Stage 5 Step 2) calls into this
// whenever a spot's selection changes. Everything downstream (ShopManager price calc,
// ShopCoreManager spawn counts, ServiceVisitorSpawner buyer-type rolls) reads its totals
// through the getters below rather than walking spots directly.
public class DecorManager : MonoBehaviour
{
    // Keyed by spot ID (DecorSpot will supply a stable per-spot identifier in Step 2)
    // so re-selecting a spot's decor cleanly replaces its previous contribution instead
    // of stacking duplicates.
    private readonly Dictionary<string, DecorItemData> placedDecor = new();

    public event System.Action OnDecorChanged;

    public IReadOnlyDictionary<string, DecorItemData> PlacedDecor => placedDecor;

    public void SetDecorAtSpot(string spotId, DecorItemData item)
    {
        if (string.IsNullOrEmpty(spotId))
            return;

        if (item == null)
        {
            ClearSpot(spotId);
            return;
        }

        placedDecor[spotId] = item;
        OnDecorChanged?.Invoke();
    }

    public void ClearSpot(string spotId)
    {
        if (string.IsNullOrEmpty(spotId))
            return;

        if (placedDecor.Remove(spotId))
            OnDecorChanged?.Invoke();
    }

    public DecorItemData GetDecorAtSpot(string spotId)
    {
        return !string.IsNullOrEmpty(spotId) && placedDecor.TryGetValue(spotId, out DecorItemData item) ? item : null;
    }

    private float SumEffect(DecorEffectType type)
    {
        float total = 0f;

        foreach (DecorItemData item in placedDecor.Values)
        {
            if (item != null && item.effectType == type)
                total += item.effectValue;
        }

        return total;
    }

    // --- Aggregate getters — one per Section 16 effect category ---
    // Wire these in during Step 4/5:

    // ShopCoreManager.GetPlannedDesk1BuyerCount() — add to baseCount.
    public int GetBuyerCountBonus() => Mathf.RoundToInt(SumEffect(DecorEffectType.BuyerAmount));

    // ShopManager.CalculateBuyerSalePrice() — multiply in alongside display/appeal (Section 19).
    public float GetSaleValueMultiplier() => 1f + SumEffect(DecorEffectType.SaleValue);

    // ShopCoreManager — add to shopAppeal before reading the buyer/sale Appeal tables
    // (an "effective appeal" read, NOT a permanent ModifyAppeal delta — decor is
    // conditional on the piece staying placed).
    public int GetAppealBonus() => Mathf.RoundToInt(SumEffect(DecorEffectType.Appeal));

    // ShopCoreManager.GetPlannedDesk3HireVisitorCount() — add to spawnChance before the floor clamp.
    public float GetRecruitChanceBonus() => SumEffect(DecorEffectType.RecruitChance);

    // Dirt system (Step 3) — subtract from the 15% base spawn-on-purchase roll.
    public float GetDirtChanceReduction() => SumEffect(DecorEffectType.DirtReduction);

    // Buyer-type roll (wherever generous/haggle/non-buyer chance is currently rolled,
    // likely ShopBuyerNPC/ShopBuyerSpawner) — add to generous chance.
    public float GetGenerousBuyerBonus() => SumEffect(DecorEffectType.GenerousBuyer);

    // Same roll — subtract from haggle chance.
    public float GetHaggleReductionBonus() => SumEffect(DecorEffectType.HaggleReduction);

    // Same roll — subtract from non-buyer chance.
    public float GetNonBuyerReductionBonus() => SumEffect(DecorEffectType.NonBuyerReduction);
}
