using System;
using System.Collections.Generic;
using UnityEngine;

public class RecruitRosterManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitQuartersManager recruitQuartersManager;

    [Header("Hired Recruits")]
    [SerializeField] private List<RecruitData> hiredRecruits = new();

    public event Action OnRosterChanged;

    // Total roster capacity now comes from Recruit Quarters' level (4/6/8 — see
    // Room_RecruitQuarters.md Section 16/17). RecruitType (Free/Paid) no longer defines
    // separate capacity pools — it's kept only for hire-cost/visitor-flow purposes in the
    // Shop's hire desk (see Code_Audit_KeepChangeCut.md, Recruit Quarters section).
    public int MaxTotalRecruitSlots => recruitQuartersManager != null ? recruitQuartersManager.Capacity : 4;

    public int FreeRecruitCount => GetRecruitCountByType(RecruitType.Free);
    public int PaidRecruitCount => GetRecruitCountByType(RecruitType.Paid);
    public int TotalRecruitCount => hiredRecruits.Count;

    private void Awake()
    {
        if (recruitQuartersManager == null)
            recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();
    }

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

    public bool IsRosterFull()
    {
        return TotalRecruitCount >= MaxTotalRecruitSlots;
    }

    public bool CanAddRecruit(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        return TotalRecruitCount < MaxTotalRecruitSlots;
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
