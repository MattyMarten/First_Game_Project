using System;
using System.Collections.Generic;
using UnityEngine;

public class RecruitQuartersManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;

    [Header("Appeal (temporary — Stage 5 owns real Appeal logic)")]
    [SerializeField] private ShopCoreManager shopCoreManager;
    [SerializeField] private int retireAppealPenalty = -3;

    [Header("Bed Slots")]
    [SerializeField] private List<RecruitBedSlot> bedSlots = new();

    [Header("Level / Capacity (index 0 = LV1)")]
    // LV1 = 4 (2 rooms x 2 beds), LV2 = 6 (3 rooms x 2 beds), LV3 = 8 (4 rooms x 2 beds)
    // Mirrors CoreRoomManager / GearUpgradeStationManager's upgrade-level pattern —
    // this is a debug stand-in for the Dwarf's real Upgrade Board (Stage 8).
    [SerializeField] private int[] capacityByLevel = new int[] { 4, 6, 8 };
    [SerializeField] private int quartersLevelIndex = 0;

    [Header("Actor Setup")]
    [SerializeField] private RecruitQuartersActor recruitActorPrefab;
    [SerializeField] private Transform actorParent;

    private readonly Dictionary<string, RecruitQuartersActor> spawnedActorsByRecruitId = new();
    private readonly Dictionary<string, RecruitData> pendingAcceptedRecruitsById = new();
    private readonly Dictionary<string, int> pendingReservedBedsByRecruitId = new();

    public event Action<int> OnQuartersLevelChanged;
    public event Action<RecruitData> OnRecruitRetired;

    public int QuartersLevel => quartersLevelIndex + 1; // displayed as LV1/LV2/LV3
    public bool IsMaxLevel => quartersLevelIndex >= capacityByLevel.Length - 1;

    public int Capacity => capacityByLevel[Mathf.Clamp(quartersLevelIndex, 0, capacityByLevel.Length - 1)];

    private void Awake()
    {
        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();

        if (shopCoreManager == null)
            shopCoreManager = FindAnyObjectByType<ShopCoreManager>();
    }

    private void OnEnable()
    {
        if (recruitRosterManager != null)
            recruitRosterManager.OnRosterChanged += RefreshQuarters;
    }

    private void OnDisable()
    {
        if (recruitRosterManager != null)
            recruitRosterManager.OnRosterChanged -= RefreshQuarters;
    }

    private void Start()
    {
        RefreshQuarters();
    }

    /// <summary>
    /// Stand-in for the Dwarf's real Upgrade Board (Stage 8) — same pattern as
    /// CoreRoomManager.TryUpgrade() / GearUpgradeStationManager.TryUpgradeStationLevel().
    /// Call from this context-menu action, a debug button, or wire it up properly later.
    /// </summary>
    [ContextMenu("Debug: Upgrade Recruit Quarters Level")]
    public void TryUpgradeQuartersLevel()
    {
        if (IsMaxLevel)
        {
            Debug.Log("[RecruitQuartersManager] Quarters already at max level.");
            return;
        }

        quartersLevelIndex++;
        Debug.Log($"[RecruitQuartersManager] Quarters upgraded to LV{QuartersLevel} (capacity {Capacity}).");
        OnQuartersLevelChanged?.Invoke(QuartersLevel);

        RefreshQuarters();
    }

    public void RefreshQuarters()
    {
        SetBedSlotActiveStatesForCapacity();

        if (recruitRosterManager == null)
            return;

        List<RecruitData> recruits = recruitRosterManager.GetAllHiredRecruits();

        ValidateAssignedBeds(recruits);
        AssignBedsToUnassignedRecruits(recruits);
        RefreshSpawnedActors(recruits);
        CleanupCompletedReservations(recruits);
    }

    /// <summary>
    /// Hides/shows bed slot objects beyond the current level's capacity. If your bed slots are
    /// grouped under a per-room parent object instead, toggle that parent here instead of the
    /// bed slot itself — whichever matches your actual scene hierarchy.
    /// </summary>
    private void SetBedSlotActiveStatesForCapacity()
    {
        for (int i = 0; i < bedSlots.Count; i++)
        {
            RecruitBedSlot bedSlot = bedSlots[i];

            if (bedSlot == null)
                continue;

            bedSlot.gameObject.SetActive(bedSlot.BedIndex < Capacity);
        }
    }

    public bool TryRetireRecruit(RecruitData recruit)
    {
        if (recruit == null || string.IsNullOrEmpty(recruit.recruitId) || recruitRosterManager == null)
            return false;

        List<RecruitData> recruits = recruitRosterManager.GetAllHiredRecruits();
        bool isInRoster = false;

        for (int i = 0; i < recruits.Count; i++)
        {
            if (recruits[i] != null && recruits[i].recruitId == recruit.recruitId)
            {
                isInRoster = true;
                break;
            }
        }

        if (!isInRoster)
            return false;

        recruitRosterManager.RemoveRecruit(recruit);

        if (shopCoreManager != null)
            shopCoreManager.ModifyAppeal(retireAppealPenalty);

        Debug.Log($"[RecruitQuartersManager] Retired {recruit.recruitName} ({retireAppealPenalty} appeal). No undead created — retirement is a clean removal, not a death.");
        OnRecruitRetired?.Invoke(recruit);

        return true;
    }

    public bool TryPrepareBedForRecruit(RecruitData recruit, out RecruitBedSlot bedSlot)
    {
        bedSlot = null;

        if (recruit == null || string.IsNullOrEmpty(recruit.recruitId))
            return false;

        if (pendingReservedBedsByRecruitId.TryGetValue(recruit.recruitId, out int existingReservedIndex))
        {
            recruit.assignedBedIndex = existingReservedIndex;
            bedSlot = GetBedSlotByIndex(existingReservedIndex);
            return bedSlot != null;
        }

        HashSet<int> usedBedIndices = GetUsedBedIndicesIncludingPending();

        if (recruit.assignedBedIndex >= 0 && IsValidBedIndex(recruit.assignedBedIndex) && !usedBedIndices.Contains(recruit.assignedBedIndex))
        {
            pendingReservedBedsByRecruitId[recruit.recruitId] = recruit.assignedBedIndex;
            pendingAcceptedRecruitsById[recruit.recruitId] = recruit;
            bedSlot = GetBedSlotByIndex(recruit.assignedBedIndex);
            return bedSlot != null;
        }

        int freeBedIndex = GetFirstFreeBedIndex(usedBedIndices);
        if (freeBedIndex < 0)
            return false;

        recruit.assignedBedIndex = freeBedIndex;
        pendingReservedBedsByRecruitId[recruit.recruitId] = freeBedIndex;
        pendingAcceptedRecruitsById[recruit.recruitId] = recruit;

        bedSlot = GetBedSlotByIndex(freeBedIndex);
        return bedSlot != null;
    }

    public void ReleasePendingBedReservation(RecruitData recruit)
    {
        if (recruit == null || string.IsNullOrEmpty(recruit.recruitId))
            return;

        pendingReservedBedsByRecruitId.Remove(recruit.recruitId);
        pendingAcceptedRecruitsById.Remove(recruit.recruitId);
    }

    public int GetPendingAcceptedRecruitCount(RecruitType recruitType)
    {
        int count = 0;

        foreach (KeyValuePair<string, RecruitData> pair in pendingAcceptedRecruitsById)
        {
            RecruitData recruit = pair.Value;

            if (recruit == null)
                continue;

            if (recruit.recruitType == recruitType)
                count++;
        }

        return count;
    }

    private HashSet<int> GetUsedBedIndicesIncludingPending()
    {
        HashSet<int> usedBedIndices = new();

        if (recruitRosterManager != null)
        {
            List<RecruitData> rosterRecruits = recruitRosterManager.GetAllHiredRecruits();

            for (int i = 0; i < rosterRecruits.Count; i++)
            {
                RecruitData rosterRecruit = rosterRecruits[i];

                if (rosterRecruit == null)
                    continue;

                if (rosterRecruit.assignedBedIndex >= 0)
                    usedBedIndices.Add(rosterRecruit.assignedBedIndex);
            }
        }

        foreach (KeyValuePair<string, int> pair in pendingReservedBedsByRecruitId)
            usedBedIndices.Add(pair.Value);

        return usedBedIndices;
    }

    private void CleanupCompletedReservations(List<RecruitData> recruits)
    {
        if (recruits == null || recruits.Count == 0)
            return;

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null || string.IsNullOrEmpty(recruit.recruitId))
                continue;

            pendingReservedBedsByRecruitId.Remove(recruit.recruitId);
            pendingAcceptedRecruitsById.Remove(recruit.recruitId);
        }
    }

    private void ValidateAssignedBeds(List<RecruitData> recruits)
    {
        HashSet<int> usedBedIndices = new();

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null)
                continue;

            if (recruit.assignedBedIndex < 0)
                continue;

            if (!IsValidBedIndex(recruit.assignedBedIndex))
            {
                recruit.assignedBedIndex = -1;
                continue;
            }

            if (usedBedIndices.Contains(recruit.assignedBedIndex))
            {
                recruit.assignedBedIndex = -1;
                continue;
            }

            usedBedIndices.Add(recruit.assignedBedIndex);
        }
    }

    private void AssignBedsToUnassignedRecruits(List<RecruitData> recruits)
    {
        HashSet<int> usedBedIndices = new();

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null)
                continue;

            if (recruit.assignedBedIndex >= 0)
                usedBedIndices.Add(recruit.assignedBedIndex);
        }

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null)
                continue;

            if (recruit.assignedBedIndex >= 0)
                continue;

            int freeBedIndex = GetFirstFreeBedIndex(usedBedIndices);

            if (freeBedIndex < 0)
            {
                Debug.LogWarning($"No free bed available for recruit: {recruit.recruitName}", this);
                continue;
            }

            recruit.assignedBedIndex = freeBedIndex;
            usedBedIndices.Add(freeBedIndex);
        }
    }

    private void RefreshSpawnedActors(List<RecruitData> recruits)
    {
        HashSet<string> validRecruitIds = new();

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null || string.IsNullOrEmpty(recruit.recruitId))
                continue;

            validRecruitIds.Add(recruit.recruitId);

            RecruitBedSlot bedSlot = GetBedSlotByIndex(recruit.assignedBedIndex);
            if (bedSlot == null)
                continue;

            if (!spawnedActorsByRecruitId.TryGetValue(recruit.recruitId, out RecruitQuartersActor actor) || actor == null)
            {
                actor = SpawnActor(recruit, bedSlot);

                if (actor != null)
                    spawnedActorsByRecruitId[recruit.recruitId] = actor;
            }
            else
            {
                actor.Initialize(recruit, bedSlot);
            }
        }

        RemoveInvalidActors(validRecruitIds);
    }

    private RecruitQuartersActor SpawnActor(RecruitData recruit, RecruitBedSlot bedSlot)
    {
        if (recruitActorPrefab == null)
        {
            Debug.LogWarning("RecruitQuartersManager: No recruitActorPrefab assigned.", this);
            return null;
        }

        Transform parentToUse = actorParent != null ? actorParent : transform;
        RecruitQuartersActor actor = Instantiate(recruitActorPrefab, parentToUse);

        actor.Initialize(recruit, bedSlot);
        return actor;
    }

    private void RemoveInvalidActors(HashSet<string> validRecruitIds)
    {
        List<string> idsToRemove = new();

        foreach (KeyValuePair<string, RecruitQuartersActor> pair in spawnedActorsByRecruitId)
        {
            if (!validRecruitIds.Contains(pair.Key) || pair.Value == null)
                idsToRemove.Add(pair.Key);
        }

        for (int i = 0; i < idsToRemove.Count; i++)
        {
            string recruitId = idsToRemove[i];

            if (spawnedActorsByRecruitId.TryGetValue(recruitId, out RecruitQuartersActor actor) && actor != null)
                Destroy(actor.gameObject);

            spawnedActorsByRecruitId.Remove(recruitId);
        }
    }

    private int GetFirstFreeBedIndex(HashSet<int> usedBedIndices)
    {
        for (int i = 0; i < bedSlots.Count; i++)
        {
            RecruitBedSlot bedSlot = bedSlots[i];

            if (bedSlot == null)
                continue;

            int index = bedSlot.BedIndex;

            if (index >= Capacity)
                continue;

            if (!usedBedIndices.Contains(index))
                return index;
        }

        return -1;
    }

    private bool IsValidBedIndex(int bedIndex)
    {
        if (bedIndex >= Capacity)
            return false;

        for (int i = 0; i < bedSlots.Count; i++)
        {
            RecruitBedSlot bedSlot = bedSlots[i];

            if (bedSlot == null)
                continue;

            if (bedSlot.BedIndex == bedIndex)
                return true;
        }

        return false;
    }

    public RecruitBedSlot GetBedSlotByIndex(int bedIndex)
    {
        for (int i = 0; i < bedSlots.Count; i++)
        {
            RecruitBedSlot bedSlot = bedSlots[i];

            if (bedSlot == null)
                continue;

            if (bedSlot.BedIndex == bedIndex)
                return bedSlot;
        }

        return null;
    }

    /// <summary>
    /// Looked up live (not cached) so a persistent Locker always reflects who's currently
    /// assigned, even after retirement, death, or reassignment changes who's in that bed.
    /// </summary>
    public RecruitData GetRecruitAtBedIndex(int bedIndex)
    {
        if (recruitRosterManager == null || bedIndex < 0)
            return null;

        List<RecruitData> recruits = recruitRosterManager.GetAllHiredRecruits();

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit != null && recruit.assignedBedIndex == bedIndex)
                return recruit;
        }

        return null;
    }

    public RecruitQuartersActor GetSpawnedActorByRecruitId(string recruitId)
    {
        if (string.IsNullOrEmpty(recruitId))
            return null;

        if (spawnedActorsByRecruitId.TryGetValue(recruitId, out RecruitQuartersActor actor))
            return actor;

        return null;
    }

    public void SetRecruitActorVisible(string recruitId, bool visible)
    {
        RecruitQuartersActor actor = GetSpawnedActorByRecruitId(recruitId);

        if (actor == null)
            return;

        actor.gameObject.SetActive(visible);
    }
}
