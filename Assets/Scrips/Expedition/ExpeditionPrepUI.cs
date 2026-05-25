using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionPrepUI : MonoBehaviour
{
    [System.Serializable]
    private class MapLocationButtonUI
    {
        public Button button;
        public TMP_Text buttonText;
        public GameObject highlightObject;
        public ExpeditionDestinationData destination;
    }

    [Header("References")]
    [SerializeField] private ExpeditionManager expeditionManager;

    [Header("Map Locations")]
    [SerializeField] private List<MapLocationButtonUI> mapLocationButtons = new();

    [Header("Selection Display")]
    [SerializeField] private GameObject mapSelectionPanel;
    [SerializeField] private TMP_Text selectedLocationNameText;
    [SerializeField] private TMP_Text selectedEntryNameText;

    [Header("Entry Point List")]
    [SerializeField] private Transform entryPointContainer;
    [SerializeField] private ExpeditionEntryPointUIItem entryPointItemPrefab;

    [Header("Recruit List")]
    [SerializeField] private Transform recruitListContainer;
    [SerializeField] private ExpeditionRecruitListItem recruitListItemPrefab;

    [Header("Selected Crew List")]
    [SerializeField] private Transform selectedCrewContainer;
    [SerializeField] private ExpeditionSelectedCrewItem selectedCrewItemPrefab;

    [Header("Actions")]
    [SerializeField] private Button startExpeditionButton;

    private readonly List<ExpeditionEntryPointUIItem> spawnedEntryPointItems = new();
    private readonly List<ExpeditionRecruitListItem> spawnedRecruitListItems = new();
    private readonly List<ExpeditionSelectedCrewItem> spawnedSelectedCrewItems = new();

    private void Awake()
    {
        if (expeditionManager == null)
            expeditionManager = FindAnyObjectByType<ExpeditionManager>();
    }

    private void Start()
    {
        SetupMapLocationButtons();
        RefreshAll();
    }

    private void SetupMapLocationButtons()
    {
        for (int i = 0; i < mapLocationButtons.Count; i++)
        {
            MapLocationButtonUI mapButton = mapLocationButtons[i];

            if (mapButton == null)
                continue;

            if (mapButton.buttonText != null)
            {
                mapButton.buttonText.text = mapButton.destination != null
                    ? mapButton.destination.destinationName
                    : "Missing Destination";
            }

            if (mapButton.button == null)
                continue;

            mapButton.button.onClick.RemoveAllListeners();

            ExpeditionDestinationData destination = mapButton.destination;
            mapButton.button.onClick.AddListener(() => SelectDestination(destination));
        }
    }

    public void RefreshAll()
    {
        RefreshMapLocationHighlights();
        RefreshSelectionSummary();
        RefreshEntryPointList();
        RefreshRecruitList();
        RefreshSelectedCrewList();
        RefreshStartButtonState();
    }

    private void RefreshMapLocationHighlights()
    {
        if (expeditionManager == null)
            return;

        for (int i = 0; i < mapLocationButtons.Count; i++)
        {
            MapLocationButtonUI mapButton = mapLocationButtons[i];

            if (mapButton == null || mapButton.highlightObject == null)
                continue;

            bool isSelected = mapButton.destination != null && mapButton.destination == expeditionManager.SelectedDestination;
            mapButton.highlightObject.SetActive(isSelected);
        }
    }

    private void RefreshSelectionSummary()
    {
        if (mapSelectionPanel != null)
            mapSelectionPanel.SetActive(expeditionManager != null && expeditionManager.SelectedDestination != null);

        if (selectedLocationNameText != null)
        {
            selectedLocationNameText.text = expeditionManager != null && expeditionManager.SelectedDestination != null
                ? expeditionManager.SelectedDestination.destinationName
                : "None";
        }

        if (selectedEntryNameText != null)
        {
            selectedEntryNameText.text = expeditionManager != null && expeditionManager.SelectedEntryPoint != null
                ? expeditionManager.SelectedEntryPoint.entryPointName
                : "None";
        }
    }

    private void RefreshEntryPointList()
    {
        ClearEntryPointList();

        if (expeditionManager == null || expeditionManager.SelectedDestination == null)
            return;

        if (entryPointContainer == null || entryPointItemPrefab == null)
            return;

        List<ExpeditionEntryPointData> entryPoints = expeditionManager.SelectedDestination.entryPoints;

        if (entryPoints == null)
            return;

        for (int i = 0; i < entryPoints.Count; i++)
        {
            ExpeditionEntryPointData entryPoint = entryPoints[i];

            if (entryPoint == null)
                continue;

            ExpeditionEntryPointUIItem item = Instantiate(entryPointItemPrefab, entryPointContainer);

            bool isSelected = entryPoint == expeditionManager.SelectedEntryPoint;
            item.Setup(entryPoint, this, isSelected);

            spawnedEntryPointItems.Add(item);
        }
    }

    private void ClearEntryPointList()
    {
        for (int i = 0; i < spawnedEntryPointItems.Count; i++)
        {
            if (spawnedEntryPointItems[i] != null)
                Destroy(spawnedEntryPointItems[i].gameObject);
        }

        spawnedEntryPointItems.Clear();

        if (entryPointContainer == null)
            return;

        for (int i = entryPointContainer.childCount - 1; i >= 0; i--)
            Destroy(entryPointContainer.GetChild(i).gameObject);
    }

    private void RefreshRecruitList()
    {
        ClearRecruitList();

        if (expeditionManager == null || recruitListContainer == null || recruitListItemPrefab == null)
            return;

        List<RecruitData> recruits = expeditionManager.GetAvailableRosterRecruits();

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null)
                continue;

            ExpeditionRecruitListItem item = Instantiate(recruitListItemPrefab, recruitListContainer);

            bool isSelected = expeditionManager.IsRecruitSelected(recruit);
            item.Setup(recruit, this, isSelected);

            spawnedRecruitListItems.Add(item);
        }
    }

    private void ClearRecruitList()
    {
        for (int i = 0; i < spawnedRecruitListItems.Count; i++)
        {
            if (spawnedRecruitListItems[i] != null)
                Destroy(spawnedRecruitListItems[i].gameObject);
        }

        spawnedRecruitListItems.Clear();

        if (recruitListContainer == null)
            return;

        for (int i = recruitListContainer.childCount - 1; i >= 0; i--)
            Destroy(recruitListContainer.GetChild(i).gameObject);
    }

    private void RefreshSelectedCrewList()
    {
        ClearSelectedCrewList();

        if (expeditionManager == null || selectedCrewContainer == null || selectedCrewItemPrefab == null)
            return;

        IReadOnlyList<RecruitData> selectedRecruits = expeditionManager.SelectedRecruits;

        for (int i = 0; i < selectedRecruits.Count; i++)
        {
            RecruitData recruit = selectedRecruits[i];

            if (recruit == null)
                continue;

            ExpeditionSelectedCrewItem item = Instantiate(selectedCrewItemPrefab, selectedCrewContainer);
            item.Setup(recruit);

            spawnedSelectedCrewItems.Add(item);
        }
    }

    private void ClearSelectedCrewList()
    {
        for (int i = 0; i < spawnedSelectedCrewItems.Count; i++)
        {
            if (spawnedSelectedCrewItems[i] != null)
                Destroy(spawnedSelectedCrewItems[i].gameObject);
        }

        spawnedSelectedCrewItems.Clear();

        if (selectedCrewContainer == null)
            return;

        for (int i = selectedCrewContainer.childCount - 1; i >= 0; i--)
            Destroy(selectedCrewContainer.GetChild(i).gameObject);
    }

    private void RefreshStartButtonState()
    {
        if (startExpeditionButton == null || expeditionManager == null)
            return;

        startExpeditionButton.interactable = expeditionManager.CanStartExpedition();
    }

    public void SelectDestination(ExpeditionDestinationData destination)
    {
        if (expeditionManager == null)
        {
            Debug.LogWarning("ExpeditionPrepUI: No ExpeditionManager assigned or found.", this);
            return;
        }

        if (destination == null)
        {
            Debug.LogWarning("ExpeditionPrepUI: Destination is null.", this);
            return;
        }

        expeditionManager.SetSelectedDestination(destination);
        RefreshAll();
    }

    public void SelectEntryPoint(ExpeditionEntryPointData entryPoint)
    {
        if (expeditionManager == null)
            return;

        if (entryPoint == null)
        {
            Debug.LogWarning("ExpeditionPrepUI: Entry point is null.", this);
            return;
        }

        expeditionManager.SetSelectedEntryPoint(entryPoint);
        RefreshAll();
    }

    public void ToggleRecruitSelection(RecruitData recruit)
    {
        if (expeditionManager == null)
            return;

        if (recruit == null)
        {
            Debug.LogWarning("ExpeditionPrepUI: Recruit is null.", this);
            return;
        }

        bool changed = expeditionManager.ToggleRecruitSelection(recruit);

        if (!changed)
        {
            Debug.LogWarning("ExpeditionPrepUI: Could not change recruit selection. Maybe max crew size reached.", this);
        }

        RefreshAll();
    }

    public void OnStartExpeditionPressed()
    {
        if (expeditionManager == null)
            return;

        bool started = expeditionManager.StartExpedition();

        if (!started)
        {
            Debug.LogWarning("ExpeditionPrepUI: Cannot start expedition yet.", this);
            RefreshStartButtonState();
            return;
        }

        Debug.Log("ExpeditionPrepUI: Expedition started.", this);
        RefreshStartButtonState();
    }
}