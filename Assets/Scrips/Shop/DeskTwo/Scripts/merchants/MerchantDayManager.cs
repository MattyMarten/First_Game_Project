using System.Collections.Generic;
using UnityEngine;

public class MerchantDayManager : MonoBehaviour
{
    [Header("Day State")]
    [SerializeField] private int currentDay = 1;

    [Header("Merchant Profiles")]
    [SerializeField] private List<MerchantProfileData> merchantProfiles = new();

    [Header("Debug")]
    [SerializeField] private MerchantProfileData todaysMerchant;
    [SerializeField] private GeneratedMerchantVisit todaysMerchantVisit;

    public int CurrentDay => currentDay;
    public bool HasMerchantToday => IsMerchantDay(currentDay) && todaysMerchant != null && todaysMerchantVisit != null;
    public MerchantProfileData TodaysMerchant => todaysMerchant;
    public GeneratedMerchantVisit TodaysMerchantVisit => todaysMerchantVisit;

    private void Start()
    {
        RefreshMerchantForCurrentDay();
    }

    public void SetCurrentDay(int day)
    {
        currentDay = Mathf.Max(1, day);
        RefreshMerchantForCurrentDay();
    }

    public void AdvanceToNextDay()
    {
        currentDay++;
        RefreshMerchantForCurrentDay();
    }

    public bool IsMerchantDay(int day)
    {
        return day % 2 == 0;
    }

    public void RefreshMerchantForCurrentDay()
    {
        todaysMerchant = null;
        todaysMerchantVisit = null;

        if (!IsMerchantDay(currentDay))
        {
            //Debug.Log($"Day {currentDay}: No merchant today.");
            return;
        }

        List<MerchantProfileData> validProfiles = new();

        for (int i = 0; i < merchantProfiles.Count; i++)
        {
            MerchantProfileData profile = merchantProfiles[i];

            if (profile != null)
                validProfiles.Add(profile);
        }

        if (validProfiles.Count == 0)
        {
            //Debug.LogWarning($"Day {currentDay}: Merchant day, but no merchant profiles are assigned.", this);
            return;
        }

        int randomIndex = Random.Range(0, validProfiles.Count);
        todaysMerchant = validProfiles[randomIndex];
        todaysMerchantVisit = GenerateVisit(todaysMerchant);

        //Debug.Log($"Day {currentDay}: Today's merchant is {todaysMerchant.merchantName}.");
    }

    private GeneratedMerchantVisit GenerateVisit(MerchantProfileData profile)
    {
        if (profile == null)
            return null;

        GeneratedMerchantVisit visit = new GeneratedMerchantVisit
        {
            merchantProfile = profile,
            merchantName = profile.merchantName,
            openingLine = GetRandomDialogueLine(profile)
        };

        visit.utilityItems = GenerateUtilitySelection(profile.utilityItems, 3);
        visit.miscItems = GenerateUtilitySelection(profile.miscItems, 2);
        visit.materialItems = GenerateMaterialSelection(profile.materialItems, 2);

        return visit;
    }

    private string GetRandomDialogueLine(MerchantProfileData profile)
    {
        if (profile == null || profile.dialogueLines == null || profile.dialogueLines.Count == 0)
            return "Take a look at my wares.";

        List<string> validLines = new();

        for (int i = 0; i < profile.dialogueLines.Count; i++)
        {
            string line = profile.dialogueLines[i];

            if (!string.IsNullOrWhiteSpace(line))
                validLines.Add(line);
        }

        if (validLines.Count == 0)
            return "Take a look at my wares.";

        int randomIndex = Random.Range(0, validLines.Count);
        return validLines[randomIndex];
    }

    private List<GeneratedMerchantUtilityItem> GenerateUtilitySelection(List<UtilityCraftable> sourceItems, int count)
    {
        List<GeneratedMerchantUtilityItem> results = new();

        if (sourceItems == null || sourceItems.Count == 0 || count <= 0)
            return results;

        List<UtilityCraftable> validItems = new();

        for (int i = 0; i < sourceItems.Count; i++)
        {
            UtilityCraftable item = sourceItems[i];

            if (item != null)
                validItems.Add(item);
        }

        Shuffle(validItems);

        int itemCount = Mathf.Min(count, validItems.Count);

        for (int i = 0; i < itemCount; i++)
        {
            UtilityCraftable item = validItems[i];

            results.Add(new GeneratedMerchantUtilityItem
            {
                item = item,
                finalPrice = GeneratePrice(item.baseMerchantPrice),
                quantity = Random.Range(1, 5)
            });
        }

        return results;
    }

    private List<GeneratedMerchantMaterialItem> GenerateMaterialSelection(List<RawMaterial> sourceItems, int count)
    {
        List<GeneratedMerchantMaterialItem> results = new();

        if (sourceItems == null || sourceItems.Count == 0 || count <= 0)
            return results;

        List<RawMaterial> validItems = new();

        for (int i = 0; i < sourceItems.Count; i++)
        {
            RawMaterial item = sourceItems[i];

            if (item != null)
                validItems.Add(item);
        }

        Shuffle(validItems);

        int itemCount = Mathf.Min(count, validItems.Count);

        for (int i = 0; i < itemCount; i++)
        {
            RawMaterial item = validItems[i];

            results.Add(new GeneratedMerchantMaterialItem
            {
                item = item,
                finalPrice = GeneratePrice(item.baseMerchantPrice),
                quantity = Random.Range(5, 11)
            });
        }

        return results;
    }

    private int GeneratePrice(int basePrice)
    {
        int safeBasePrice = Mathf.Max(1, basePrice);
        float multiplier = Random.Range(0.8f, 1.2f);
        return Mathf.Max(1, Mathf.RoundToInt(safeBasePrice * multiplier));
    }

    private void Shuffle<T>(List<T> list)
    {
        if (list == null)
            return;

        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}