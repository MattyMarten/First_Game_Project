using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitQuartersUI : MonoBehaviour
{
    [Header("Panel")]
    [SerializeField] private GameObject panel;

    [Header("References")]
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
    [SerializeField] private InputModeManager inputModeManager;
    [SerializeField] private RecruitQuartersManager recruitQuartersManager;

    [Header("Recruit Info")]
    [SerializeField] private TMP_Text recruitNameText;
    [SerializeField] private TMP_Text recruitClassText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text strengthText;
    [SerializeField] private TMP_Text enduranceText;
    [SerializeField] private TMP_Text senseText;
    [SerializeField] private TMP_Text stealthText;

    [Header("Buttons")]
    [SerializeField] private Button playAsButton;
    [SerializeField] private Button retireButton;
    [SerializeField] private Button closeButton;

    [Header("Retire Confirmation")]
    [SerializeField] private GameObject retireConfirmPanel;
    [SerializeField] private Button retireConfirmButton;
    [SerializeField] private Button retireCancelButton;

    private RecruitData currentRecruit;

    public RecruitData CurrentRecruit => currentRecruit;

    private void Awake()
    {
        if (panel != null)
            panel.SetActive(false);

        if (playerCharacterManager == null)
            playerCharacterManager = FindAnyObjectByType<PlayerCharacterManager>();

        if (inputModeManager == null)
            inputModeManager = FindAnyObjectByType<InputModeManager>();

        if (recruitQuartersManager == null)
            recruitQuartersManager = FindAnyObjectByType<RecruitQuartersManager>();

        if (playAsButton != null)
            playAsButton.onClick.AddListener(OnPlayAsPressed);

        if (retireButton != null)
            retireButton.onClick.AddListener(OnRetirePressed);

        if (closeButton != null)
            closeButton.onClick.AddListener(OnClosePressed);

        if (retireConfirmButton != null)
            retireConfirmButton.onClick.AddListener(OnRetireConfirmed);

        if (retireCancelButton != null)
            retireCancelButton.onClick.AddListener(OnRetireCancelled);

        if (retireConfirmPanel != null)
            retireConfirmPanel.SetActive(false);
    }

    public void OpenForRecruit(RecruitData recruit)
    {
        currentRecruit = recruit;

        if (panel != null)
            panel.SetActive(true);

        if (retireConfirmPanel != null)
            retireConfirmPanel.SetActive(false);

        RefreshUI();
    }

    public void RefreshUI()
    {
        if (currentRecruit == null)
        {
            ShowEmptyState();
            RefreshButtonState();
            return;
        }

        if (recruitNameText != null)
            recruitNameText.text = currentRecruit.recruitName;

        if (recruitClassText != null)
            recruitClassText.text = currentRecruit.recruitClass.ToString();

        if (levelText != null)
            levelText.text = $"Level: {currentRecruit.level}";

        if (healthText != null)
            healthText.text = $"Health: {currentRecruit.stats.health}";

        if (strengthText != null)
            strengthText.text = $"Strength: {currentRecruit.stats.strength}";

        if (enduranceText != null)
            enduranceText.text = $"Endurance: {currentRecruit.stats.endurance}";

        if (senseText != null)
            senseText.text = $"Sense: {currentRecruit.stats.sense}";

        if (stealthText != null)
            stealthText.text = $"Stealth: {currentRecruit.stats.stealth}";

        RefreshButtonState();
    }

    private void ShowEmptyState()
    {
        if (recruitNameText != null)
            recruitNameText.text = "No recruit selected";

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
    }

    private void RefreshButtonState()
    {
        if (retireButton != null)
            retireButton.interactable = currentRecruit != null;

        if (playAsButton == null || playerCharacterManager == null)
            return;

        bool alreadyControllingThisRecruit =
            currentRecruit != null &&
            playerCharacterManager.IsControllingRecruit &&
            playerCharacterManager.ActiveRecruit != null &&
            playerCharacterManager.ActiveRecruit.recruitId == currentRecruit.recruitId;

        playAsButton.interactable = currentRecruit != null && !alreadyControllingThisRecruit;
    }

    private void OnPlayAsPressed()
    {
        if (currentRecruit == null)
            return;

        if (playerCharacterManager == null)
        {
            Debug.LogWarning("RecruitQuartersUI: No PlayerCharacterManager found.", this);
            return;
        }

        bool switched = playerCharacterManager.SwitchToRecruit(currentRecruit);

        if (!switched)
            return;

        if (panel != null)
            panel.SetActive(false);

        if (inputModeManager != null)
            inputModeManager.SetGameplayMode();
    }

    private void OnClosePressed()
    {
        if (panel != null)
            panel.SetActive(false);

        if (retireConfirmPanel != null)
            retireConfirmPanel.SetActive(false);

        currentRecruit = null;

        if (inputModeManager != null)
            inputModeManager.SetGameplayMode();
    }

    private void OnRetirePressed()
    {
        if (currentRecruit == null)
            return;

        if (retireConfirmPanel != null)
            retireConfirmPanel.SetActive(true);
    }

    private void OnRetireCancelled()
    {
        if (retireConfirmPanel != null)
            retireConfirmPanel.SetActive(false);
    }

    private void OnRetireConfirmed()
    {
        if (currentRecruit == null || recruitQuartersManager == null)
        {
            if (retireConfirmPanel != null)
                retireConfirmPanel.SetActive(false);

            return;
        }

        bool retired = recruitQuartersManager.TryRetireRecruit(currentRecruit);

        if (retireConfirmPanel != null)
            retireConfirmPanel.SetActive(false);

        if (!retired)
            return;

        currentRecruit = null;

        if (panel != null)
            panel.SetActive(false);

        // Retiring is triggered by a UI button, not the normal interact/close input path,
        // so we have to restore gameplay mode explicitly here — nothing else will.
        if (inputModeManager != null)
            inputModeManager.SetGameplayMode();
    }
}