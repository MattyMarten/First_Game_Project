using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simulates an expedition result without loading a scene.
/// Attach to the same GameObject as DayPhaseController and assign
/// the possible material/good pools in the Inspector.
/// </summary>
public class ExpeditionResolver : MonoBehaviour
{
    [Header("Reward Pools")]
    [SerializeField] private List<RawMaterial> possibleMaterials = new();
    [SerializeField] private List<CraftingGood> possibleGoods = new();

    [Header("Resolution Tuning")]
    [Range(0f, 1f)]
    [SerializeField] private float baseSuccessChance = 0.7f;
    [SerializeField] private int baseGoldMin = 50;
    [SerializeField] private int baseGoldMax = 150;
    [SerializeField] private int failureGoldMin = 10;
    [SerializeField] private int failureGoldMax = 30;
    [Range(0f, 1f)]
    [SerializeField] private float injuryChanceOnFailure = 0.5f;

    public ExpeditionResultData Resolve(ExpeditionSessionData session)
    {
        ExpeditionResultData result = new ExpeditionResultData();

        if (session == null)
        {
            result.wasSuccessful = false;
            return result;
        }

        float successChance = CalculateSuccessChance(session);
        result.wasSuccessful = Random.value <= successChance;

        if (result.wasSuccessful)
        {
            result.goldEarned = Random.Range(baseGoldMin, baseGoldMax + 1);
            result.materialRewards = GenerateMaterialRewards();
            result.goodRewards = GenerateGoodRewards();
        }
        else
        {
            result.goldEarned = Random.Range(failureGoldMin, failureGoldMax + 1);
            result.injuredRecruitIds = GenerateInjuries(session);
        }

        string outcome = result.wasSuccessful ? "SUCCESS" : "FAILURE";
        Debug.Log($"[ExpeditionResolver] {outcome} — Gold: {result.goldEarned}, Materials: {result.materialRewards.Count}, Injuries: {result.injuredRecruitIds.Count}");

        return result;
    }

    private float CalculateSuccessChance(ExpeditionSessionData session)
    {
        float chance = baseSuccessChance;

        if (session.selectedMembers != null && session.selectedMembers.Count > 0)
        {
            float partyBonus = (session.selectedMembers.Count - 1) * 0.05f;
            chance = Mathf.Clamp01(chance + partyBonus);
        }

        return chance;
    }

    private List<ExpeditionResultData.MaterialReward> GenerateMaterialRewards()
    {
        List<ExpeditionResultData.MaterialReward> rewards = new();

        if (possibleMaterials == null || possibleMaterials.Count == 0)
            return rewards;

        int rewardCount = Random.Range(1, Mathf.Min(3, possibleMaterials.Count) + 1);
        List<RawMaterial> shuffled = new List<RawMaterial>(possibleMaterials);
        Shuffle(shuffled);

        for (int i = 0; i < rewardCount; i++)
        {
            if (shuffled[i] == null)
                continue;

            rewards.Add(new ExpeditionResultData.MaterialReward
            {
                material = shuffled[i],
                amount = Random.Range(2, 6)
            });
        }

        return rewards;
    }

    private List<ExpeditionResultData.GoodReward> GenerateGoodRewards()
    {
        List<ExpeditionResultData.GoodReward> rewards = new();

        if (possibleGoods == null || possibleGoods.Count == 0)
            return rewards;

        if (Random.value < 0.4f)
            return rewards;

        int index = Random.Range(0, possibleGoods.Count);

        if (possibleGoods[index] != null)
        {
            rewards.Add(new ExpeditionResultData.GoodReward
            {
                good = possibleGoods[index],
                amount = 1
            });
        }

        return rewards;
    }

    private List<string> GenerateInjuries(ExpeditionSessionData session)
    {
        List<string> injured = new();

        if (session.selectedMembers == null)
            return injured;

        for (int i = 0; i < session.selectedMembers.Count; i++)
        {
            ExpeditionMemberData member = session.selectedMembers[i];

            if (member == null)
                continue;

            if (Random.value <= injuryChanceOnFailure)
                injured.Add(member.recruitId);
        }

        return injured;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
