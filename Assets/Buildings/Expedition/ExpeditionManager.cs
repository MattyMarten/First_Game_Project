using System.Collections.Generic;
using UnityEngine;

public class ExpeditionManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;

    [Header("Destinations")]
    [SerializeField] private List<ExpeditionDestinationData> availableDestinations = new();

    [Header("Selection Rules")]
    [SerializeField] private int maxPartySize = 3;

    private ExpeditionDestinationData selectedDestination;
    private ExpeditionEntryPointData selectedEntryPoint;
    private ExpeditionSessionData currentSession;

    public IReadOnlyList<ExpeditionDestinationData> AvailableDestinations => availableDestinations;

    public ExpeditionDestinationData SelectedDestination => selectedDestination;
    public ExpeditionEntryPointData SelectedEntryPoint => selectedEntryPoint;
    public ExpeditionSessionData CurrentSession => currentSession;
    public int MaxPartySize => maxPartySize;

    public IReadOnlyList<RecruitData> GetExpeditionParty()
    {
        if (recruitRosterManager == null)
            return new List<RecruitData>();

        return recruitRosterManager.ExpeditionParty;
    }

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

    public bool CanStartExpedition()
    {
        return selectedDestination != null
            && selectedEntryPoint != null
            && recruitRosterManager != null
            && recruitRosterManager.ExpeditionParty.Count > 0;
    }

    public ExpeditionSessionData BuildCurrentSession()
    {
        IReadOnlyList<RecruitData> party = GetExpeditionParty();

        ExpeditionSessionData session = new ExpeditionSessionData
        {
            selectedDestination = selectedDestination,
            selectedEntryPoint = selectedEntryPoint,
            selectedMembers = new List<ExpeditionMemberData>()
        };

        for (int i = 0; i < party.Count; i++)
        {
            ExpeditionMemberData memberData = ExpeditionMemberData.FromRecruit(party[i]);

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

        recruitRosterManager.MarkPartyOnExpedition();

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

        if (recruitRosterManager != null)
            recruitRosterManager.ClearParty();
    }
}