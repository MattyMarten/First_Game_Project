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

    private readonly List<RecruitData> expeditionParty = new();

    public event Action OnRosterChanged;
    public event Action OnPartyChanged;

    public int MaxFreeRecruitSlots => maxFreeRecruitSlots;
    public int MaxPaidRecruitSlots => maxPaidRecruitSlots;
    public int MaxTotalRecruitSlots => maxFreeRecruitSlots + maxPaidRecruitSlots;

    public int FreeRecruitCount => GetRecruitCountByType(RecruitType.Free);
    public int PaidRecruitCount => GetRecruitCountByType(RecruitType.Paid);
    public int TotalRecruitCount => hiredRecruits.Count;

    public IReadOnlyList<RecruitData> ExpeditionParty => expeditionParty;

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

        RemoveFromParty(recruit);

        if (hiredRecruits.Remove(recruit))
        {
            OnRosterChanged?.Invoke();
            Debug.Log($"Recruit removed from roster: {recruit.recruitName}");
        }
    }

    // ── Expedition party management ──────────────────────────────────────────

    public bool IsInParty(RecruitData recruit)
    {
        return recruit != null && expeditionParty.Contains(recruit);
    }

    public bool TryAddToParty(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        if (!recruit.IsAvailable)
            return false;

        if (expeditionParty.Contains(recruit))
            return false;

        recruit.status = RecruitStatus.AssignedToParty;
        expeditionParty.Add(recruit);
        OnPartyChanged?.Invoke();

        Debug.Log($"Recruit added to expedition party: {recruit.recruitName}");
        return true;
    }

    public void RemoveFromParty(RecruitData recruit)
    {
        if (recruit == null)
            return;

        if (!expeditionParty.Remove(recruit))
            return;

        if (recruit.status == RecruitStatus.AssignedToParty)
            recruit.status = RecruitStatus.Idle;

        OnPartyChanged?.Invoke();

        Debug.Log($"Recruit removed from expedition party: {recruit.recruitName}");
    }

    public void ClearParty()
    {
        for (int i = 0; i < expeditionParty.Count; i++)
        {
            RecruitData recruit = expeditionParty[i];

            if (recruit != null && recruit.status == RecruitStatus.AssignedToParty)
                recruit.status = RecruitStatus.Idle;
        }

        expeditionParty.Clear();
        OnPartyChanged?.Invoke();
    }

    public void MarkPartyOnExpedition()
    {
        for (int i = 0; i < expeditionParty.Count; i++)
        {
            RecruitData recruit = expeditionParty[i];

            if (recruit != null)
                recruit.status = RecruitStatus.OnExpedition;
        }
    }
}