using System;
using UnityEngine;

/// <summary>
/// TEMPORARY debug-only tool for testing Recruit Quarters UI, bed assignment, capacity,
/// and level upgrades — before the Dwarf (Stage 8) seeds the four real starting recruits
/// (Mara, Brok, Pip, Vael) and before Shop's hire flow (Stage 5) is the real source of
/// new recruits. Log this in Known_Temporary_Systems.md. Safe to delete once Stage 8 lands.
///
/// Usage: attach to any GameObject in the Recruit Quarters scene, assign
/// RecruitRosterManager (or let it auto-find), then right-click the component header
/// in Play mode (or the ⋮ menu) to run these from the context menu.
/// </summary>
public class RecruitQuartersDebugSeeder : MonoBehaviour
{
    [SerializeField] private RecruitRosterManager recruitRosterManager;

    [Header("Test Recruit Names")]
    [SerializeField]
    private string[] testNames = new string[]
    {
        "Mara", "Brok", "Pip", "Vael", "Sela", "Doran", "Kesh", "Orin"
    };

    private int nextNameIndex = 0;

    private void Awake()
    {
        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();
    }

    [ContextMenu("Debug: Add One Test Recruit")]
    public void AddOneTestRecruit()
    {
        if (recruitRosterManager == null)
        {
            Debug.LogWarning("[RecruitQuartersDebugSeeder] No RecruitRosterManager found.");
            return;
        }

        RecruitData recruit = CreateTestRecruit();

        if (!recruitRosterManager.TryAddRecruit(recruit))
        {
            Debug.Log("[RecruitQuartersDebugSeeder] Roster is full at current capacity — can't add another test recruit.");
            return;
        }

        Debug.Log($"[RecruitQuartersDebugSeeder] Added test recruit: {recruit.recruitName}");
    }

    [ContextMenu("Debug: Fill Roster To Capacity")]
    public void FillRosterToCapacity()
    {
        if (recruitRosterManager == null)
            return;

        int safetyLimit = 20; // guards against an infinite loop if something's misconfigured

        while (!recruitRosterManager.IsRosterFull() && safetyLimit-- > 0)
            AddOneTestRecruit();
    }

    [ContextMenu("Debug: Clear All Test Recruits")]
    public void ClearAllRecruits()
    {
        if (recruitRosterManager == null)
            return;

        foreach (RecruitData recruit in recruitRosterManager.GetAllHiredRecruits())
            recruitRosterManager.RemoveRecruit(recruit);

        nextNameIndex = 0;
        Debug.Log("[RecruitQuartersDebugSeeder] Cleared all recruits from roster.");
    }

    private RecruitData CreateTestRecruit()
    {
        string name = testNames.Length > 0 ? testNames[nextNameIndex % testNames.Length] : "Test Recruit";
        nextNameIndex++;

        return new RecruitData
        {
            recruitId = Guid.NewGuid().ToString(),
            recruitName = name,
            recruitType = RecruitType.Free,
            recruitClass = RecruitClass.Bruiser,
            level = 1,
            hireCost = 0,
            canLevelUp = false,
            motivationText = "Debug test recruit — not from a real hire flow.",
            assignedBedIndex = -1,
            stats = new RecruitStats
            {
                health = 5,
                strength = 5,
                endurance = 5,
                sense = 5,
                stealth = 5
            }
        };
    }
}
