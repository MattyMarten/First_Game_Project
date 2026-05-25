using UnityEngine;
using System.Collections.Generic;

public class GoodStorage : MonoBehaviour
{
    private Dictionary<CraftingGood, int> storedGoods = new();

    public void Add(CraftingGood good, int amount)
    {
        if (good == null || amount <= 0)
            return;

        if (!storedGoods.ContainsKey(good))
            storedGoods[good] = 0;

        storedGoods[good] += amount;
    }

    public bool TrySpend(CraftingGood good, int amount)
    {
        if (good == null || amount <= 0)
            return false;

        if (!storedGoods.TryGetValue(good, out int current) || current < amount)
            return false;

        storedGoods[good] -= amount;

        if (storedGoods[good] <= 0)
            storedGoods.Remove(good);

        return true;
    }

    public int GetAmount(CraftingGood good)
    {
        if (good == null)
            return 0;

        return storedGoods.TryGetValue(good, out int amount) ? amount : 0;
    }

    public Dictionary<CraftingGood, int> GetAll()
    {
        return new Dictionary<CraftingGood, int>(storedGoods);
    }
}
