using System.Collections.Generic;
using UnityEngine;

public class RecruitQuartersManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;

    [Header("Bed Slots")]
    [SerializeField] private List<RecruitBedSlot> bedSlots = new();

    [Header("Actor Setup")]
    [SerializeField] private RecruitQuartersActor recruitActorPrefab;
    [SerializeField] private Transform actorParent;

    private readonly Dictionary<string, RecruitQuartersActor> spawnedActorsByRecruitId = new();
    private readonly Dictionary<string, RecruitData> pendingAcceptedRecruitsById = new();
    private readonly Dictionary<string, int> pendingReservedBedsByRecruitId = new();

    private void Awake()
    {
        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();
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

    public void RefreshQuarters()
    {
        if (recruitRosterManager == null)
            return;

        List<RecruitData> recruits = recruitRosterManager.GetAllHiredRecruits();

        ValidateAssignedBeds(recruits);
        AssignBedsToUnassignedRecruits(recruits);
        RefreshSpawnedActors(recruits);
        CleanupCompletedReservations(recruits);
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

        if (recruit.assignedBedIndex >= 0 && !usedBedIndices.Contains(recruit.assignedBedIndex))
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

            if (!usedBedIndices.Contains(index))
                return index;
        }

        return -1;
    }

    private bool IsValidBedIndex(int bedIndex)
    {
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