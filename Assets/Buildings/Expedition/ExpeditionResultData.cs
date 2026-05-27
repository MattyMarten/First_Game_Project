using System;
using System.Collections.Generic;

[Serializable]
public class ExpeditionResultData
{
    public bool wasSuccessful;
    public int goldEarned;
    public List<MaterialReward> materialRewards = new();
    public List<GoodReward> goodRewards = new();
    public List<string> injuredRecruitIds = new();

    [Serializable]
    public class MaterialReward
    {
        public RawMaterial material;
        public int amount;
    }

    [Serializable]
    public class GoodReward
    {
        public CraftingGood good;
        public int amount;
    }
}
