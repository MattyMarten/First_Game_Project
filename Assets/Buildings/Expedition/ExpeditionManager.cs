using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;

    [Header("Destinations")]
    [SerializeField] private List<ExpeditionDestinationData> availableDestinations = new();

    [Header("Selection Rules")]
    [SerializeField] private int maxSelectedRecruits = 3;

    private ExpeditionDestinationData selectedDestination;
    private ExpeditionEntryPointData selectedEntryPoint;
    private ExpeditionSessionData currentSession;

    private readonly List<RecruitData> selectedRecruits = new();

    public IReadOnlyList<ExpeditionDestinationData> AvailableDestinations => availableDestinations;
    public IReadOnlyList<RecruitData> SelectedRecruits => selectedRecruits;

    public ExpeditionDestinationData SelectedDestination => selectedDestination;
    public ExpeditionEntryPointData SelectedEntryPoint => selectedEntryPoint;
    public ExpeditionSessionData CurrentSession => currentSession;
    public int MaxSelectedRecruits => maxSelectedRecruits;

    private void Awake()
    {
        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();
    }

    public void SetSelectedDestination(ExpeditionDestinationData destination)
    {
        selectedDestination = destination;

        if (selectedDestination == null || selectedDestination.entryPoints == null || selectedDestination.entryPoints.Count == 0)
        {
            selectedEntryPoint = null;
            return;
        }

        selectedEntryPoint = selectedDestination.entryPoints[0];
    }

    public void SetSelectedEntryPoint(ExpeditionEntryPointData entryPoint)
    {
        if (selectedDestination == null)
            return;

        if (entryPoint == null)
        {
            selectedEntryPoint = null;
            return;
        }

        if (selectedDestination.entryPoints == null || !selectedDestination.entryPoints.Contains(entryPoint))
            return;

        selectedEntryPoint = entryPoint;
    }

    public List<RecruitData> GetAvailableRosterRecruits()
    {
        if (recruitRosterManager == null)
            return new List<RecruitData>();

        return recruitRosterManager.GetAllHiredRecruits();
    }

    public bool IsRecruitSelected(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        return selectedRecruits.Contains(recruit);
    }

    public bool ToggleRecruitSelection(RecruitData recruit)
    {
        if (recruit == null)
            return false;

        if (selectedRecruits.Contains(recruit))
        {
            selectedRecruits.Remove(recruit);
            return true;
        }

        if (selectedRecruits.Count >= maxSelectedRecruits)
            return false;

        selectedRecruits.Add(recruit);
        return true;
    }

    public bool CanStartExpedition()
    {
        return selectedDestination != null
            && selectedEntryPoint != null
            && selectedRecruits.Count > 0;
    }

    public ExpeditionSessionData BuildCurrentSession()
    {
        ExpeditionSessionData session = new ExpeditionSessionData
        {
            selectedDestination = selectedDestination,
            selectedEntryPoint = selectedEntryPoint,
            selectedMembers = new List<ExpeditionMemberData>()
        };

        for (int i = 0; i < selectedRecruits.Count; i++)
        {
            ExpeditionMemberData memberData = ExpeditionMemberData.FromRecruit(selectedRecruits[i]);

            if (memberData != null)
                session.selectedMembers.Add(memberData);
        }

        return session;
    }

    public bool StartExpedition()
    {
        if (!CanStartExpedition())
            return false;

        currentSession = BuildCurrentSession();

        string destinationName = currentSession.selectedDestination != null
            ? currentSession.selectedDestination.destinationName
            : "None";

        string entryPointName = currentSession.selectedEntryPoint != null
            ? currentSession.selectedEntryPoint.entryPointName
            : "None";

        Debug.Log($"Expedition started. Destination: {destinationName}, Entry: {entryPointName}, Members: {currentSession.selectedMembers.Count}", this);

        for (int i = 0; i < currentSession.selectedMembers.Count; i++)
        {
            ExpeditionMemberData member = currentSession.selectedMembers[i];

            if (member == null)
                continue;

            Debug.Log($" - {member.recruitName} ({member.recruitClass}) Lv.{member.level}", this);
        }

        return true;
    }

    public void ClearSelection()
    {
        selectedDestination = null;
        selectedEntryPoint = null;
        selectedRecruits.Clear();
    }
}