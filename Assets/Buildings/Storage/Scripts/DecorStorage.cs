// Target path in your project: Assets/Buildings/Storage/Scripts/DecorStorage.cs

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Owned-but-not-placed Shop Decor (Room_Storage.md: "shop decor not currently
/// placed should also be represented physically in Storage"). Purchasing decor
/// from the Merchant adds here; placing a piece via DecorSpot moves it from
/// here into DecorManager's placedDecor, and removing a placed piece moves it
/// back. Mirrors GoodStorage's shape exactly.
/// </summary>
public class DecorStorage : MonoBehaviour
{
    private Dictionary<DecorItemData, int> storedDecor = new();

    public void Add(DecorItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return;

        if (!storedDecor.ContainsKey(item))
            storedDecor[item] = 0;

        storedDecor[item] += amount;
    }

    public bool TrySpend(DecorItemData item, int amount)
    {
        if (item == null || amount <= 0)
            return false;

        if (!storedDecor.TryGetValue(item, out int current) || current < amount)
            return false;

        storedDecor[item] -= amount;

        if (storedDecor[item] <= 0)
            storedDecor.Remove(item);

        return true;
    }

    public int GetAmount(DecorItemData item)
    {
        if (item == null)
            return 0;

        return storedDecor.TryGetValue(item, out int amount) ? amount : 0;
    }

    public Dictionary<DecorItemData, int> GetAll()
    {
        return new Dictionary<DecorItemData, int>(storedDecor);
    }
}
