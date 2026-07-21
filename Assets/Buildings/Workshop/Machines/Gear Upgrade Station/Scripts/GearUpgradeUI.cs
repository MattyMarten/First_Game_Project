// Target path in your project: Assets/Buildings/Workshop/Machines/Gear Upgrade Station/Scripts/GearUpgradeUI.cs

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class GearUpgradeUI : MonoBehaviour
{
    [Header("Item List")]
    [SerializeField] private Transform contentParent;
    [SerializeField] private GameObject upgradeRowPrefab;

    [Header("Detail Panel")]
    [SerializeField] private GameObject detailPanel;
    [SerializeField] private TMP_Text detailArrowText; // e.g. "Basic Axe T1 -> Basic Axe T2"
    [SerializeField] private TMP_Text detailRequirementsText;
    [SerializeField] private TMP_Text detailBlockedReasonText;
    [SerializeField] private Button detailUpgradeButton;

    [Header("Data")]
    [SerializeField] private CraftedUtilityStorage utilityStorage;
    [SerializeField] private RawMaterialStorage materialStorage;
    [SerializeField] private GearUpgradeStationManager stationManager;

    private UtilityCraftable selectedItem;
    private List<GameObject> spawnedRows = new();

    private void Awake()
    {
        if (utilityStorage == null)
            utilityStorage = FindAnyObjectByType<CraftedUtilityStorage>();

        if (materialStorage == null)
            materialStorage = FindAnyObjectByType<RawMaterialStorage>();

        if (stationManager == null)
            stationManager = FindAnyObjectByType<GearUpgradeStationManager>();
    }

    private void OnEnable()
    {
        if (stationManager != null)
            stationManager.OnStationLevelChanged += HandleStationLevelChanged;
    }

    private void OnDisable()
    {
        if (stationManager != null)
            stationManager.OnStationLevelChanged -= HandleStationLevelChanged;
    }

    private void HandleStationLevelChanged(int newLevel) => RefreshUI();

    public void RefreshUI()
    {
        foreach (var row in spawnedRows)
        {
            if (row != null)
                Destroy(row);
        }
        spawnedRows.Clear();

        if (utilityStorage == null)
        {
            RefreshDetailPanel();
            return;
        }

        // Only items you currently own that have somewhere left to upgrade to.
        IEnumerable<UtilityCraftable> upgradeableOwned = utilityStorage.GetAll()
            .Where(kvp => kvp.Value > 0 && kvp.Key != null && kvp.Key.nextTierItem != null)
            .Select(kvp => kvp.Key);

        foreach (var item in upgradeableOwned)
        {
            GameObject row = Instantiate(upgradeRowPrefab, contentParent);

            Transform nameT = row.transform.Find("ItemName");
            if (nameT != null)
            {
                TMP_Text t = nameT.GetComponent<TMP_Text>();
                if (t != null)
                    t.text = item.itemName;
            }

            Transform imageT = row.transform.Find("ItemImage");
            if (imageT != null)
            {
                Image img = imageT.GetComponent<Image>();
                if (img != null)
                    img.sprite = item.icon;
            }

            Button btn = row.GetComponent<Button>();
            if (btn != null)
            {
                UtilityCraftable captured = item;
                btn.onClick.AddListener(() => SelectItem(captured));
            }

            spawnedRows.Add(row);
        }

        RefreshDetailPanel();
    }

    private void SelectItem(UtilityCraftable item)
    {
        selectedItem = item;
        RefreshDetailPanel();
    }

    private void RefreshDetailPanel()
    {
        if (detailPanel == null)
            return;

        if (selectedItem == null)
        {
            detailPanel.SetActive(false);
            return;
        }

        detailPanel.SetActive(true);

        if (detailArrowText != null)
            detailArrowText.text = $"{selectedItem.itemName} -> {selectedItem.nextTierItem.itemName}";

        if (detailRequirementsText != null)
            detailRequirementsText.text = BuildRequirementsText(selectedItem);

        string blockReason = stationManager != null ? stationManager.GetBlockReason(selectedItem) : "No station found.";

        if (detailBlockedReasonText != null)
            detailBlockedReasonText.text = blockReason ?? "";

        if (detailUpgradeButton != null)
        {
            detailUpgradeButton.interactable = blockReason == null;
            detailUpgradeButton.onClick.RemoveAllListeners();
            detailUpgradeButton.onClick.AddListener(TryUpgradeSelected);
        }
    }

    private string BuildRequirementsText(UtilityCraftable item)
    {
        if (item == null || materialStorage == null)
            return "";

        Dictionary<RawMaterial, int> owned = materialStorage.GetAll();
        System.Text.StringBuilder sb = new();

        foreach (var req in item.upgradeMaterials)
        {
            int needed = stationManager != null ? stationManager.GetScaledCost(req.amount) : req.amount;
            int have = owned.TryGetValue(req.material, out int amt) ? amt : 0;
            string color = have >= needed ? "#FFFFFF" : "#FF5555";
            string materialName = req.material != null ? req.material.displayName : "Missing Material";

            sb.AppendLine($"<color={color}>{materialName} {needed}/{have}</color>");
        }

        return sb.ToString();
    }

    private void TryUpgradeSelected()
    {
        if (selectedItem == null || stationManager == null)
            return;

        if (stationManager.TryUpgrade(selectedItem))
        {
            selectedItem = null;
            RefreshUI();
        }
        else
        {
            RefreshDetailPanel();
        }
    }
}
