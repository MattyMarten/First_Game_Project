using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HireDeskUI : MonoBehaviour
{
    [Header("Main Panel")]
    [SerializeField] private GameObject panel;

    [Header("References")]
    [SerializeField] private HireDeskManager hireDeskManager;

    [Header("Main Recruit Info")]
    [SerializeField] private TMP_Text recruitNameText;
    [SerializeField] private TMP_Text costText;
    [SerializeField] private TMP_Text motivationText;

    [Header("Stats Window")]
    [SerializeField] private GameObject statsWindow;
    [SerializeField] private TMP_Text recruitClassText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text enduranceText;
    [SerializeField] private TMP_Text senseText;
    [SerializeField] private TMP_Text stealthText;

    [Header("Buttons")]
    [SerializeField] private Button viewStatsButton;
    [SerializeField] private TMP_Text viewStatsButtonText;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    private bool isShowingStats;

    private void Awake()
    {
        if (hireDeskManager == null)
            hireDeskManager = FindAnyObjectByType<HireDeskManager>();

        if (panel != null)
            panel.SetActive(false);

        if (statsWindow != null)
            statsWindow.SetActive(false);

        if (viewStatsButton != null)
            viewStatsButton.onClick.AddListener(ToggleStatsWindow);

        if (acceptButton != null)
            acceptButton.onClick.AddListener(AcceptRecruit);

        if (declineButton != null)
            declineButton.onClick.AddListener(DeclineRecruit);
    }

    private void OnEnable()
    {
        if (hireDeskManager == null)
            hireDeskManager = FindAnyObjectByType<HireDeskManager>();

        if (hireDeskManager != null)
            hireDeskManager.OnPendingRecruitChanged += RefreshUI;
    }

    private void OnDisable()
    {
        if (hireDeskManager != null)
            hireDeskManager.OnPendingRecruitChanged -= RefreshUI;
    }

    public void OpenDesk()
    {
        if (panel != null)
            panel.SetActive(true);

        RefreshUI();
    }

    public void CloseDesk()
    {
        if (panel != null)
            panel.SetActive(false);
    }

    public void RefreshUI()
    {
        if (hireDeskManager == null)
            return;

        RecruitData recruit = hireDeskManager.PendingRecruit;

        if (!hireDeskManager.HasPendingRecruit || recruit == null)
        {
            isShowingStats = false;
            ShowEmptyState();
            RefreshStatsWindowState();
            return;
        }

        ShowRecruitInfo(recruit);
        RefreshStatsWindowState();
    }

    private void ShowRecruitInfo(RecruitData recruit)
    {
        if (recruitNameText != null)
            recruitNameText.text = recruit.recruitName;

        if (costText != null)
            costText.text = recruit.IsFreeRecruit
                ? "Cost: Free/Night"
                : $"Cost: {recruit.hireCost}/Night";

        if (motivationText != null)
            motivationText.text = recruit.motivationText;

        if (recruitClassText != null)
            recruitClassText.text = recruit.recruitClass.ToString();

        if (levelText != null)
            levelText.text = $"Level: {recruit.level}";

        if (healthText != null)
            healthText.text = $"Health: {recruit.stats.health}";

        if (strengthText != null)
            strengthText.text = $"Strength: {recruit.stats.strength}";

        if (enduranceText != null)
            enduranceText.text = $"Endurance: {recruit.stats.endurance}";

        if (senseText != null)
            senseText.text = $"Sense: {recruit.stats.sense}";

        if (stealthText != null)
            stealthText.text = $"Stealth: {recruit.stats.stealth}";

        SetButtonState(
            canViewStats: true,
            canAccept: true,
            canDecline: hireDeskManager.CanDeclinePendingRecruit());
    }

    private void ShowEmptyState()
    {
        if (recruitNameText != null)
            recruitNameText.text = "No recruit waiting";

        if (costText != null)
            costText.text = "Cost: -";

        if (motivationText != null)
            motivationText.text = "No recruit is currently waiting at the desk.";

        if (recruitClassText != null)
            recruitClassText.text = "-";

        if (levelText != null)
            levelText.text = "Level: -";

        if (healthText != null)
            healthText.text = "Health: -";

        if (strengthText != null)
            strengthText.text = "Strength: -";

        if (enduranceText != null)
            enduranceText.text = "Endurance: -";

        if (senseText != null)
            senseText.text = "Sense: -";

        if (stealthText != null)
            stealthText.text = "Stealth: -";

        SetButtonState(false, false, false);
    }

    private void SetButtonState(bool canViewStats, bool canAccept, bool canDecline)
    {
        if (viewStatsButton != null)
            viewStatsButton.interactable = canViewStats;

        if (acceptButton != null)
            acceptButton.interactable = canAccept;

        if (declineButton != null)
            declineButton.interactable = canDecline;
    }

    private void ToggleStatsWindow()
    {
        if (hireDeskManager == null || !hireDeskManager.HasPendingRecruit)
            return;

        isShowingStats = !isShowingStats;
        RefreshStatsWindowState();
    }

    private void RefreshStatsWindowState()
    {
        bool canShowStats = hireDeskManager != null && hireDeskManager.HasPendingRecruit;
        bool showStats = canShowStats && isShowingStats;

        if (statsWindow != null)
            statsWindow.SetActive(showStats);

        if (viewStatsButtonText != null)
            viewStatsButtonText.text = showStats ? "Close Stats" : "View Stats";
    }

    public void AcceptRecruit()
    {
        if (hireDeskManager == null)
            return;

        bool accepted = hireDeskManager.AcceptPendingRecruit();

        if (accepted)
        {
            isShowingStats = false;
            RefreshUI();
        }
    }

    public void DeclineRecruit()
    {
        if (hireDeskManager == null)
            return;

        bool declined = hireDeskManager.DeclinePendingRecruit();

        if (declined)
        {
            isShowingStats = false;
            RefreshUI();
        }
    }
}