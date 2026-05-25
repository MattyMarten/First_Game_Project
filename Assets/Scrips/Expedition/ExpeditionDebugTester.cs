using System.Collections.Generic;
using UnityEngine;

public class ExpeditionDebugTester : MonoBehaviour
{
    [SerializeField] private ExpeditionManager expeditionManager;

    [Header("Test Selection")]
    [SerializeField] private int destinationIndex;
    [SerializeField] private int entryPointIndex;
    [SerializeField] private int recruitIndex;

    private void Awake()
    {
        if (expeditionManager == null)
            expeditionManager = FindAnyObjectByType<ExpeditionManager>();
    }

    [ContextMenu("Test Select Destination And Entry")]
    public void TestSelectDestinationAndEntry()
    {
        if (expeditionManager == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: No ExpeditionManager found.", this);
            return;
        }

        if (destinationIndex < 0 || destinationIndex >= expeditionManager.AvailableDestinations.Count)
        {
            Debug.LogWarning("ExpeditionDebugTester: Destination index is out of range.", this);
            return;
        }

        ExpeditionDestinationData destination = expeditionManager.AvailableDestinations[destinationIndex];

        if (destination == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: Destination is null.", this);
            return;
        }

        expeditionManager.SetSelectedDestination(destination);

        if (destination.entryPoints == null || destination.entryPoints.Count == 0)
        {
            Debug.LogWarning("ExpeditionDebugTester: Selected destination has no entry points.", this);
            return;
        }

        if (entryPointIndex < 0 || entryPointIndex >= destination.entryPoints.Count)
        {
            Debug.LogWarning("ExpeditionDebugTester: Entry point index is out of range.", this);
            return;
        }

        ExpeditionEntryPointData entryPoint = destination.entryPoints[entryPointIndex];
        expeditionManager.SetSelectedEntryPoint(entryPoint);

        Debug.Log($"Selected destination: {destination.destinationName}, entry: {entryPoint.entryPointName}", this);
    }

    [ContextMenu("Test Toggle Recruit Selection")]
    public void TestToggleRecruitSelection()
    {
        if (expeditionManager == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: No ExpeditionManager found.", this);
            return;
        }

        List<RecruitData> recruits = expeditionManager.GetAvailableRosterRecruits();

        if (recruits == null || recruits.Count == 0)
        {
            Debug.LogWarning("ExpeditionDebugTester: No roster recruits available.", this);
            return;
        }

        if (recruitIndex < 0 || recruitIndex >= recruits.Count)
        {
            Debug.LogWarning("ExpeditionDebugTester: Recruit index is out of range.", this);
            return;
        }

        RecruitData recruit = recruits[recruitIndex];

        if (recruit == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: Recruit is null.", this);
            return;
        }

        bool changed = expeditionManager.ToggleRecruitSelection(recruit);

        if (!changed)
        {
            Debug.LogWarning($"ExpeditionDebugTester: Could not toggle recruit '{recruit.recruitName}'. Party may be full.", this);
            return;
        }

        bool isSelected = expeditionManager.IsRecruitSelected(recruit);
        string state = isSelected ? "selected" : "removed";

        Debug.Log($"Recruit {state}: {recruit.recruitName}", this);
        LogSelectedRecruits();
    }

    [ContextMenu("Test Log Available Recruits")]
    public void TestLogAvailableRecruits()
    {
        if (expeditionManager == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: No ExpeditionManager found.", this);
            return;
        }

        List<RecruitData> recruits = expeditionManager.GetAvailableRosterRecruits();

        if (recruits == null || recruits.Count == 0)
        {
            Debug.LogWarning("ExpeditionDebugTester: No roster recruits available.", this);
            return;
        }

        Debug.Log($"Available recruits: {recruits.Count}", this);

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null)
                continue;

            Debug.Log($"[{i}] {recruit.recruitName} ({recruit.recruitClass}) Lv.{recruit.level}", this);
        }
    }

    [ContextMenu("Test Start Expedition")]
    public void TestStartExpedition()
    {
        if (expeditionManager == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: No ExpeditionManager found.", this);
            return;
        }

        bool started = expeditionManager.StartExpedition();

        if (!started)
        {
            Debug.LogWarning("ExpeditionDebugTester: Expedition failed to start.", this);
            return;
        }

        Debug.Log("ExpeditionDebugTester: Expedition started successfully.", this);
    }

    [ContextMenu("Test Clear Selection")]
    public void TestClearSelection()
    {
        if (expeditionManager == null)
        {
            Debug.LogWarning("ExpeditionDebugTester: No ExpeditionManager found.", this);
            return;
        }

        expeditionManager.ClearSelection();
        Debug.Log("ExpeditionDebugTester: Selection cleared.", this);
    }

    private void LogSelectedRecruits()
    {
        IReadOnlyList<RecruitData> selectedRecruits = expeditionManager.SelectedRecruits;

        Debug.Log($"Currently selected recruits: {selectedRecruits.Count}", this);

        for (int i = 0; i < selectedRecruits.Count; i++)
        {
            RecruitData recruit = selectedRecruits[i];

            if (recruit == null)
                continue;

            Debug.Log($" - {recruit.recruitName} ({recruit.recruitClass}) Lv.{recruit.level}", this);
        }
    }
}