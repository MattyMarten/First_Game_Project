using TMPro;
using UnityEngine;

/// <summary>
/// Readout for the stored categories that don't need a full per-type row
/// list (Room_Storage.md Section 6):
///   - cobalt coins: shown as an exact count
///   - goods: shown as a summed total (per-type breakdown isn't required)
///   - utility items: shown as a summed total, same reasoning
///
/// This sits ALONGSIDE MaterialStorageUI, not instead of it — materials still
/// get their own per-type row list there. This component doesn't own any of
/// the underlying data; it just reads from CobaltCoinStorage (new, lives in
/// Storage) and the existing GoodStorage / CraftedUtilityStorage (which
/// already live in Shop/Display and Workshop/Utility Station respectively).
/// Storage represents them here without taking over their ownership.
/// </summary>
public class StorageSummaryUI : MonoBehaviour
{
    [Header("Sources")]
    [SerializeField] private CobaltCoinStorage coinStorage;
    [SerializeField] private GoodStorage goodStorage;
    [SerializeField] private CraftedUtilityStorage utilityStorage;

    [Header("Readout Text (assign TMP_Text objects in the Storage panel)")]
    [SerializeField] private TMP_Text coinText;
    [SerializeField] private TMP_Text goodsTotalText;
    [SerializeField] private TMP_Text utilityTotalText;

    public void RefreshUI()
    {
        if (coinText != null)
        {
            coinText.text = coinStorage != null
                ? $"Cobalt Coins: {coinStorage.CoinCount}"
                : "Cobalt Coins: -";
        }

        if (goodsTotalText != null)
            goodsTotalText.text = $"Goods (stored): {SumGoods()}";

        if (utilityTotalText != null)
            utilityTotalText.text = $"Utility Items (stored): {SumUtility()}";
    }

    private int SumGoods()
    {
        if (goodStorage == null) return 0;

        int total = 0;
        foreach (var kv in goodStorage.GetAll())
            total += kv.Value;
        return total;
    }

    private int SumUtility()
    {
        if (utilityStorage == null) return 0;

        int total = 0;
        foreach (var kv in utilityStorage.GetAll())
            total += kv.Value;
        return total;
    }
}
