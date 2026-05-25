using System.Collections.Generic;

[System.Serializable]
public class GeneratedMerchantVisit
{
    public MerchantProfileData merchantProfile;
    public string merchantName;
    public string openingLine;

    public List<GeneratedMerchantUtilityItem> utilityItems = new();
    public List<GeneratedMerchantUtilityItem> miscItems = new();
    public List<GeneratedMerchantMaterialItem> materialItems = new();
}
