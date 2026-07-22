using System;
using System.Collections.Generic;
using UnityEngine;

public class RecruitGenerator : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;
    [SerializeField] private ServiceDeskManager serviceDeskManager;

    // PLACEHOLDER (Known_Temporary_Systems.md candidate): capacity is unified now
    // (Stage 4/5 — RecruitType no longer splits capacity), so Free vs Paid is no
    // longer decided by "which type still has room." This ratio is an arbitrary
    // stand-in until there's a real design rule for how often a recruit visitor
    // should be Free vs Paid.
    [Header("Free vs Paid Ratio (placeholder, tune freely)")]
    [SerializeField, Range(0f, 1f)] private float freeRecruitChance = 0.6f;

    [Header("Model Prefabs")]
    [SerializeField] private List<GameObject> freeRecruitModelPrefabs = new();
    [SerializeField] private List<GameObject> paidRecruitModelPrefabs = new();

    [Header("Name Pool")]
    [SerializeField] private List<string> possibleNames = new()
    {
        "Mira",
        "Doran",
        "Vale",
        "Lina",
        "Bran",
        "Tessa",
        "Corin",
        "Nyra",
        "Edda",
        "Rook"
    };

    [Header("Free Recruit Motivations")]
    [TextArea]
    [SerializeField] private List<string> freeRecruitMotivations = new()
    {
        "I just need a place to sleep and I will do my best to help.",
        "I do not have much, but I can work if you let me stay.",
        "I am looking for shelter and a chance to belong somewhere.",
        "Give me a bed and I will earn my keep."
    };

    [Header("Paid Recruit Motivations")]
    [TextArea]
    [SerializeField] private List<string> paidRecruitMotivations = new()
    {
        "I heard you need capable hands. My skills are not free.",
        "I am looking for solid work and I expect fair pay.",
        "If you want someone dependable, hire me.",
        "You pay, I work. Simple as that."
    };

    [Header("Stat Totals")]
    [SerializeField] private int freeRecruitStatTotal = 18;
    [SerializeField] private int paidRecruitStatTotal = 25;
    [SerializeField] private int minimumStatValue = 1;

    [Header("Starting Levels")]
    [SerializeField] private int freeRecruitLevel = 0;
    [SerializeField] private int paidRecruitLevel = 1;

    [Header("Paid Recruit Cost")]
    [SerializeField] private int minPaidRecruitCost = 20;
    [SerializeField] private int maxPaidRecruitCost = 50;

    private void Awake()
    {
        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();

        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();
    }

    public RecruitData GenerateRecruit()
    {
        if (recruitRosterManager == null || serviceDeskManager == null)
        {
            Debug.LogWarning("RecruitGenerator: Missing required manager reference.", this);
            return null;
        }

        if (!serviceDeskManager.HasRecruitCapacityRemaining())
            return null;

        RecruitType chosenType = UnityEngine.Random.value < freeRecruitChance
            ? RecruitType.Free
            : RecruitType.Paid;

        return CreateRecruit(chosenType);
    }

    public bool CanGenerateRecruit()
    {
        if (recruitRosterManager == null || serviceDeskManager == null)
            return false;

        return serviceDeskManager.HasRecruitCapacityRemaining();
    }

    private RecruitData CreateRecruit(RecruitType recruitType)
    {
        RecruitData recruit = new RecruitData();

        recruit.recruitId = Guid.NewGuid().ToString();
        recruit.recruitType = recruitType;
        recruit.recruitName = GetRandomName();
        recruit.level = recruitType == RecruitType.Free ? freeRecruitLevel : paidRecruitLevel;
        recruit.canLevelUp = recruitType == RecruitType.Paid;
        recruit.hireCost = recruitType == RecruitType.Free ? 0 : UnityEngine.Random.Range(minPaidRecruitCost, maxPaidRecruitCost + 1);
        recruit.motivationText = GetRandomMotivation(recruitType);
        recruit.modelPrefab = GetRandomModelPrefab(recruitType);
        recruit.stats = GenerateStatsForType(recruitType);
        recruit.recruitClass = DetermineRecruitClass(recruit.stats);

        return recruit;
    }

    private RecruitStats GenerateStatsForType(RecruitType recruitType)
    {
        int targetTotal = recruitType == RecruitType.Free ? freeRecruitStatTotal : paidRecruitStatTotal;

        RecruitStats stats = new RecruitStats
        {
            health = minimumStatValue,
            strength = minimumStatValue,
            endurance = minimumStatValue,
            sense = minimumStatValue,
            stealth = minimumStatValue
        };

        int remainingPoints = targetTotal - (minimumStatValue * 5);

        for (int i = 0; i < remainingPoints; i++)
        {
            int statIndex = UnityEngine.Random.Range(0, 5);

            switch (statIndex)
            {
                case 0:
                    stats.health++;
                    break;
                case 1:
                    stats.strength++;
                    break;
                case 2:
                    stats.endurance++;
                    break;
                case 3:
                    stats.sense++;
                    break;
                case 4:
                    stats.stealth++;
                    break;
            }
        }

        return stats;
    }

    private RecruitClass DetermineRecruitClass(RecruitStats stats)
    {
        if (stats == null)
            return RecruitClass.Bruiser;

        int highestValue = Mathf.Max(
            stats.health,
            stats.strength,
            stats.endurance,
            stats.sense,
            stats.stealth
        );

        if (stats.health == highestValue)
            return RecruitClass.Bruiser;

        if (stats.strength == highestValue)
            return RecruitClass.Fighter;

        if (stats.endurance == highestValue)
            return RecruitClass.Guardian;

        if (stats.sense == highestValue)
            return RecruitClass.Scout;

        return RecruitClass.Rogue;
    }

    private string GetRandomName()
    {
        if (possibleNames == null || possibleNames.Count == 0)
            return "Unknown Recruit";

        int randomIndex = UnityEngine.Random.Range(0, possibleNames.Count);
        return possibleNames[randomIndex];
    }

    private string GetRandomMotivation(RecruitType recruitType)
    {
        List<string> sourceList = recruitType == RecruitType.Free
            ? freeRecruitMotivations
            : paidRecruitMotivations;

        if (sourceList == null || sourceList.Count == 0)
            return "I am looking for a place to stay and work.";

        int randomIndex = UnityEngine.Random.Range(0, sourceList.Count);
        return sourceList[randomIndex];
    }

    private GameObject GetRandomModelPrefab(RecruitType recruitType)
    {
        List<GameObject> sourceList = recruitType == RecruitType.Free
            ? freeRecruitModelPrefabs
            : paidRecruitModelPrefabs;

        if (sourceList == null || sourceList.Count == 0)
            return null;

        int randomIndex = UnityEngine.Random.Range(0, sourceList.Count);
        return sourceList[randomIndex];
    }
}