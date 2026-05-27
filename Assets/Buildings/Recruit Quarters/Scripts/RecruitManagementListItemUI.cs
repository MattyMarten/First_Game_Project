using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecruitManagementListItemUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text classText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private GameObject selectedHighlight;
    [SerializeField] private GameObject inPartyIndicator;

    private RecruitData boundRecruit;
    private Action<RecruitData> onClicked;

    public RecruitData BoundRecruit => boundRecruit;

    private void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();
    }

    public void Bind(RecruitData recruit, Action<RecruitData> onClick, bool selected)
    {
        boundRecruit = recruit;
        onClicked = onClick;

        if (nameText != null)
            nameText.text = recruit != null ? recruit.recruitName : "-";

        if (classText != null)
            classText.text = recruit != null ? recruit.recruitClass.ToString() : "-";

        if (levelText != null)
            levelText.text = recruit != null ? $"Lv. {recruit.level}" : "-";

        SetSelected(selected);
        RefreshPartyIndicator();

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(HandleClicked);
        }
    }

    public void SetSelected(bool selected)
    {
        if (selectedHighlight != null)
            selectedHighlight.SetActive(selected);
    }

    public void RefreshPartyIndicator()
    {
        if (inPartyIndicator == null)
            return;

        bool inParty = boundRecruit != null && boundRecruit.status == RecruitStatus.AssignedToParty;
        inPartyIndicator.SetActive(inParty);
    }

    private void HandleClicked()
    {
        onClicked?.Invoke(boundRecruit);
    }
}