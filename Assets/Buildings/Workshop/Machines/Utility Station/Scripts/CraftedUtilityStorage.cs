using UnityEngine;
using System.Collections.Generic;

public class CraftedUtilityStorage : MonoBehaviour
{
    private Dictionary<UtilityCraftable, int> craftedCounts = new();

    public void Add(UtilityCraftable item, int amount)
    {
        if (item == null || amount <= 0)
            return;

        if (!craftedCounts.ContainsKey(item))
            craftedCounts[item] = 0;

        craftedCounts[item] += amount;
    }

    public bool TrySpend(UtilityCraftable item, int amount)
    {
        if (item == null || amount <= 0)
            return false;

        if (!craftedCounts.ContainsKey(item) || craftedCounts[item] < amount)
            return false;

        craftedCounts[item] -= amount;

        if (craftedCounts[item] <= 0)
            craftedCounts.Remove(item);

        return true;
    }

    public int GetCount(UtilityCraftable item)
    {
        if (item == null)
            return 0;

        return craftedCounts.TryGetValue(item, out int count) ? count : 0;
    }

    public Dictionary<UtilityCraftable, int> GetAll()
    {
        return new Dictionary<UtilityCraftable, int>(craftedCounts);
    }

    public bool HasAny(UtilityCraftable item)
    {
        return GetCount(item) > 0;
    }
}