using UnityEngine;

/// <summary>
/// Storage's true cobalt coin reserve (Room_Storage.md Section 11).
///
/// This is the "bank." It is deliberately separate from whatever the Core has
/// loaded into itself — Core's own loaded-coin state (e.g. "50 / 150 loaded")
/// is built in Stage 2, Room_Core.md. The Core's deposit interaction will
/// call TrySpend() here and add the same amount to its own loaded total.
///
/// Kept as a plain int rather than a Dictionary (like RawMaterialStorage) on
/// purpose — coins are a single currency, not a set of distinct item types.
/// </summary>
public class CobaltCoinStorage : MonoBehaviour
{
    [SerializeField] private int coinCount = 0;

    public int CoinCount => Mathf.Max(0, coinCount);

    public void Add(int amount)
    {
        if (amount <= 0) return;
        coinCount += amount;
    }

    public bool TrySpend(int amount)
    {
        if (amount <= 0 || coinCount < amount) return false;
        coinCount -= amount;
        return true;
    }
}
