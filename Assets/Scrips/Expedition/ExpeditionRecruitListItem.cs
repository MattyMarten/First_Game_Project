using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionRecruitListItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text recruitNameText;
    [SerializeField] private TMP_Text recruitClassText;
    [SerializeField] private TMP_Text recruitLevelText;
    [SerializeField] private GameObject highlightObject;

    private RecruitData recruitData;
    private ExpeditionPrepUI expeditionPrepUI;

    public void Setup(RecruitData recruit, ExpeditionPrepUI prepUI, bool isSelected)
    {
        recruitData = recruit;
        expeditionPrepUI = prepUI;

        if (recruitNameText != null)
            recruitNameText.text = recruitData != null ? recruitData.recruitName : "Missing Recruit";

        if (recruitClassText != null)
            recruitClassText.text = recruitData != null ? recruitData.recruitClass.ToString() : "-";

        if (recruitLevelText != null)
            recruitLevelText.text = recruitData != null ? $"Lv.{recruitData.level}" : "-";

        if (highlightObject != null)
            highlightObject.SetActive(isSelected);

        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(OnClicked);
        }
    }

    private void OnClicked()
    {
        if (expeditionPrepUI == null || recruitData == null)
            return;

        expeditionPrepUI.ToggleRecruitSelection(recruitData);
    }
}