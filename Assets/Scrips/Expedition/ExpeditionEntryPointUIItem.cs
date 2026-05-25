using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpeditionEntryPointUIItem : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text entryNameText;
    [SerializeField] private GameObject highlightObject;

    private ExpeditionEntryPointData entryPointData;
    private ExpeditionPrepUI expeditionPrepUI;

    public void Setup(ExpeditionEntryPointData entryPoint, ExpeditionPrepUI prepUI, bool isSelected)
    {
        entryPointData = entryPoint;
        expeditionPrepUI = prepUI;

        if (entryNameText != null)
            entryNameText.text = entryPointData != null ? entryPointData.entryPointName : "Missing Entry";

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
        if (expeditionPrepUI == null || entryPointData == null)
            return;

        expeditionPrepUI.SelectEntryPoint(entryPointData);
    }
}