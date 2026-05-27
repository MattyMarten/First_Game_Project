using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitManagementBoardUI : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;
    [SerializeField] private GameObject rootPanel;

    [Header("List")]
    [SerializeField] private Transform listContentRoot;
    [SerializeField] private RecruitManagementListItemUI listItemPrefab;

    [Header("Stats Panel")]
    [SerializeField] private GameObject detailsPanel;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text enduranceText;
    [SerializeField] private TMP_Text stealthText;
    [SerializeField] private TMP_Text senseText;

    [Header("Party Assignment")]
    [SerializeField] private Button partyToggleButton;
    [SerializeField] private TMP_Text partyToggleButtonText;
    [SerializeField] private TMP_Text recruitStatusText;

    private readonly List<RecruitManagementListItemUI> spawnedListItems = new();

    private RecruitData selectedRecruit;

    public bool IsOpen => rootPanel != null && rootPanel.activeSelf;

    private void Awake()
    {
        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();

        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    private void OnEnable()
    {
        if (recruitRosterManager != null)
        {
            recruitRosterManager.OnRosterChanged += HandleRosterChanged;
            recruitRosterManager.OnPartyChanged += HandlePartyChanged;
        }
    }

    private void OnDisable()
    {
        if (recruitRosterManager != null)
        {
            recruitRosterManager.OnRosterChanged -= HandleRosterChanged;
            recruitRosterManager.OnPartyChanged -= HandlePartyChanged;
        }
    }

    public void Open()
    {
        if (rootPanel != null)
            rootPanel.SetActive(true);

        RefreshAll();
    }

    public void Close()
    {
        if (rootPanel != null)
            rootPanel.SetActive(false);
    }

    public void Toggle()
    {
        if (IsOpen)
            Close();
        else
            Open();
    }

    public void RefreshAll()
    {
        List<RecruitData> recruits = recruitRosterManager != null
            ? recruitRosterManager.GetAllHiredRecruits()
            : new List<RecruitData>();

        RefreshList(recruits);
        ValidateSelection(recruits);
        RefreshDetails();
        RefreshSelectionVisuals();
    }

    private void HandleRosterChanged()
    {
        if (!IsOpen)
            return;

        RefreshAll();
    }

    private void HandlePartyChanged()
    {
        if (!IsOpen)
            return;

        RefreshDetails();
        RefreshPartyIndicators();
    }

    private void RefreshList(List<RecruitData> recruits)
    {
        ClearListItems();

        if (listItemPrefab == null || listContentRoot == null)
            return;

        for (int i = 0; i < recruits.Count; i++)
        {
            RecruitData recruit = recruits[i];

            if (recruit == null)
                continue;

            RecruitManagementListItemUI item = Instantiate(listItemPrefab, listContentRoot);
            bool selected = selectedRecruit != null && recruit.recruitId == selectedRecruit.recruitId;

            item.Bind(recruit, OnRecruitClicked, selected);
            spawnedListItems.Add(item);
        }
    }

    private void ValidateSelection(List<RecruitData> recruits)
    {
        if (recruits == null || recruits.Count == 0)
        {
            selectedRecruit = null;
            return;
        }

        if (selectedRecruit != null)
        {
            for (int i = 0; i < recruits.Count; i++)
            {
                RecruitData recruit = recruits[i];

                if (recruit != null && recruit.recruitId == selectedRecruit.recruitId)
                {
                    selectedRecruit = recruit;
                    return;
                }
            }
        }

        selectedRecruit = recruits[0];
    }

    private void RefreshDetails()
    {
        bool hasSelection = selectedRecruit != null;

        if (detailsPanel != null)
            detailsPanel.SetActive(hasSelection);

        if (!hasSelection)
        {
            SetText(healthText, "Health: -");
            SetText(strengthText, "Strength: -");
            SetText(enduranceText, "Endurance: -");
            SetText(stealthText, "Stealth: -");
            SetText(senseText, "Sense: -");
            SetText(recruitStatusText, string.Empty);

            if (partyToggleButton != null)
                partyToggleButton.gameObject.SetActive(false);

            return;
        }

        RecruitStats stats = selectedRecruit.stats;

        SetText(healthText, $"Health: {stats.health}");
        SetText(strengthText, $"Strength: {stats.strength}");
        SetText(enduranceText, $"Endurance: {stats.endurance}");
        SetText(stealthText, $"Stealth: {stats.stealth}");
        SetText(senseText, $"Sense: {stats.sense}");
        SetText(recruitStatusText, $"Status: {selectedRecruit.status}");

        RefreshPartyButton();
    }

    private void RefreshPartyButton()
    {
        if (partyToggleButton == null || recruitRosterManager == null)
            return;

        bool inParty = recruitRosterManager.IsInParty(selectedRecruit);
        bool canAdd = selectedRecruit != null && selectedRecruit.IsAvailable;

        partyToggleButton.gameObject.SetActive(selectedRecruit != null);
        partyToggleButton.interactable = inParty || canAdd;

        if (partyToggleButtonText != null)
            partyToggleButtonText.text = inParty ? "Remove from Party" : "Add to Party";

        partyToggleButton.onClick.RemoveAllListeners();
        partyToggleButton.onClick.AddListener(OnPartyTogglePressed);
    }

    private void OnPartyTogglePressed()
    {
        if (selectedRecruit == null || recruitRosterManager == null)
            return;

        if (recruitRosterManager.IsInParty(selectedRecruit))
            recruitRosterManager.RemoveFromParty(selectedRecruit);
        else
            recruitRosterManager.TryAddToParty(selectedRecruit);
    }

    private void RefreshPartyIndicators()
    {
        for (int i = 0; i < spawnedListItems.Count; i++)
        {
            if (spawnedListItems[i] != null)
                spawnedListItems[i].RefreshPartyIndicator();
        }

        RefreshPartyButton();
    }

    private void RefreshSelectionVisuals()
    {
        for (int i = 0; i < spawnedListItems.Count; i++)
        {
            RecruitManagementListItemUI item = spawnedListItems[i];

            if (item == null)
                continue;

            bool selected =
                selectedRecruit != null &&
                item.BoundRecruit != null &&
                item.BoundRecruit.recruitId == selectedRecruit.recruitId;

            item.SetSelected(selected);
        }
    }

    private void OnRecruitClicked(RecruitData recruit)
    {
        selectedRecruit = recruit;
        RefreshDetails();
        RefreshSelectionVisuals();
    }

    private void ClearListItems()
    {
        for (int i = 0; i < spawnedListItems.Count; i++)
        {
            if (spawnedListItems[i] != null)
                Destroy(spawnedListItems[i].gameObject);
        }

        spawnedListItems.Clear();
    }

    private void SetText(TMP_Text textField, string value)
    {
        if (textField != null)
            textField.text = value;
    }
}
