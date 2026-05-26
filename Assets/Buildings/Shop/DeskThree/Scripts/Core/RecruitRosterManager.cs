using System;
using System.Collections.Generic;
using UnityEngine;

public class RecruitRosterManager : MonoBehaviour
{
    [Header("Roster Limits")]
    [SerializeField] private int maxFreeRecruitSlots = 4;
    [SerializeField] private int maxPaidRecruitSlots = 4;

    [Header("Hired Recruits")]
    [SerializeField] private List<RecruitData> hiredRecruits = new();

    public event Action OnRosterChanged;

    public int MaxFreeRecruitSlots => maxFreeRecruitSlots;
    public int MaxPaidRecruitSlots => maxPaidRecruitSlots;
    public int MaxTotalRecruitSlots => maxFreeRecruitSlots + maxPaidRecruitSlots;

    public int FreeRecruitCount => GetRecruitCountByType(RecruitType.Free);
    public int PaidRecruitCount => GetRecruitCountByType(RecruitType.Paid);
    public int TotalRecruitCount => hiredRecruits.Count;

    public List<RecruitData> GetAllHiredRecruits()
    {
        return new List<RecruitData>(hiredRecruits);
    }

    public int GetRecruitCountByType(RecruitType recruitType)
    {
        int count = 0;

        for (int i = 0; i < hiredRecruits.Count; i++)
        {
            RecruitData recruit = hiredRecruits[i];

            if (recruit == null)
                continue;

            if (recruit.recruitType == recruitType)
                count++;
        }

        return count;
    }

    public bool HasFreeRecruitSpace()
    {
        return FreeRecruitCount < maxFreeRecruitSlots;
    }

    public bool HasPaidRecruitSpace()
    {
        return PaidRecruitCount < maxPaidRecruitSlots;
    }

    public bool NeedsMoreFreeRecruits()
    {
        return FreeRecruitCount < maxFreeRecruitSlots;
    }

    public bool CanSpawnPaidRecruits()
    {
        return FreeRecruitCount >= maxFreeRecruitSlots && HasPaidRecruitSpace();
    }

    public bool IsRosterFull()
    {
        return !HasFreeRecruitSpace() && !HasPaidRecruitSpace();
    }

    public bool CanAddRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        if (recruit.recruitType == RecruitType.Free)
            return HasFreeRecruitSpace();

        if (recruit.recruitType == RecruitType.Paid)
            return HasPaidRecruitSpace();

        return false;
    }

    public bool TryAddRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        if (!CanAddRecruit(recruit))
            return false;

        hiredRecruits.Add(recruit);
        OnRosterChanged?.Invoke();

        Debug.Log($"Recruit added to roster: {recruit.recruitName} ({recruit.recruitType})");
        return true;
    }

    public void RemoveRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return;

        if (hiredRecruits.Remove(recruit))
        {
            OnRosterChanged?.Invoke();
            Debug.Log($"Recruit removed from roster: {recruit.recruitName}");
        }
    }
}