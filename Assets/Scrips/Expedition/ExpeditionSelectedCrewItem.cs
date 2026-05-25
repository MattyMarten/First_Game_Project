using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionSelectedCrewItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TMP_Text recruitNameText;
    [SerializeField] private TMP_Text recruitClassText;
    [SerializeField] private TMP_Text recruitLevelText;
    [SerializeField] private Button viewEquipmentButton;

    private RecruitData recruitData;

    public void Setup(RecruitData recruit)
    {
        recruitData = recruit;

        if (recruitNameText != null)
            recruitNameText.text = recruitData != null ? recruitData.recruitName : "Missing Recruit";

        if (recruitClassText != null)
            recruitClassText.text = recruitData != null ? recruitData.recruitClass.ToString() : "-";

        if (recruitLevelText != null)
            recruitLevelText.text = recruitData != null ? $"Lv.{recruitData.level}" : "-";

        if (viewEquipmentButton != null)
        {
            viewEquipmentButton.onClick.RemoveAllListeners();
            viewEquipmentButton.onClick.AddListener(OnViewEquipmentClicked);
        }
    }

    private void OnViewEquipmentClicked()
    {
        if (recruitData == null)
            return;

        Debug.Log($"View EQ clicked for {recruitData.recruitName}", this);
    }
}