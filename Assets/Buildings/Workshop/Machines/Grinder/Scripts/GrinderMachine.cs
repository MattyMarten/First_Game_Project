using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using StarterAssets;

public class GrinderMachine : MonoBehaviour
{
    [Header("References")]
    public PlayerInventory playerInventory;
    public GridInventory gridInventory;
    public StarterAssetsInputs input;
    public RawMaterialStorage storage;

    [Header("UI")]
    public GameObject summaryPanel;
    public TMP_Text summaryText;
    public RectTransform layoutRoot;

    [Header("Timer")]
    public float messageTimer = 20f;

    private float messageTimerEnd = -1f;

    void Awake()
    {
        if (summaryPanel != null)
            summaryPanel.SetActive(false);

        if (input == null)
            input = FindAnyObjectByType<StarterAssetsInputs>();
    }

    void Update()
    {
        if (summaryPanel != null && summaryPanel.activeSelf)
        {
            if (messageTimerEnd > 0f && Time.time >= messageTimerEnd)
                HideSummary();

            if (input != null && input.ConsumeSkipMessage())
                HideSummary();
        }
    }

    public void Grind()
    {
        if (playerInventory == null || storage == null)
            return;

        Dictionary<RawMaterial, int> resultMaterials = new();
        HashSet<InventoryLoot> removedLoot = new();

        int width = playerInventory.gridItems.GetLength(0);
        int height = playerInventory.gridItems.GetLength(1);

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var loot = playerInventory.gridItems[x, y];
                if (loot != null && loot.item != null && !removedLoot.Contains(loot))
                {
                    foreach (var pair in loot.item.MaterialValue)
                    {
                        if (!resultMaterials.ContainsKey(pair.Key))
                            resultMaterials[pair.Key] = 0;

                        resultMaterials[pair.Key] += pair.Value;
                    }

                    playerInventory.RemoveMultiCellItem(loot);
                    removedLoot.Add(loot);
                }
            }
        }

        foreach (var kvp in resultMaterials)
            storage.Add(kvp.Key, kvp.Value);

        var storageUI = FindAnyObjectByType<MaterialStorageUI>();
        if (storageUI != null && storageUI.gameObject.activeInHierarchy)
            storageUI.RefreshUI();

        ShowSummary(resultMaterials);

        if (gridInventory != null)
            gridInventory.RefreshGridUI();
    }

    public void ShowSummary(Dictionary<RawMaterial, int> materials)
    {
        if (summaryPanel != null)
            summaryPanel.SetActive(true);

        if (summaryText != null)
        {
            if (materials == null || materials.Count == 0)
            {
                summaryText.text = "No materials received!";
            }
            else
            {
                string summary = "You received:\n";
                foreach (var kvp in materials)
                    summary += $"{kvp.Value}x {kvp.Key.displayName}\n";

                summaryText.text = summary;
            }

            summaryText.ForceMeshUpdate();
        }

        Canvas.ForceUpdateCanvases();

        if (layoutRoot != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(layoutRoot);
        else if (summaryPanel != null)
            LayoutRebuilder.ForceRebuildLayoutImmediate(summaryPanel.GetComponent<RectTransform>());

        messageTimerEnd = Time.time + messageTimer;
    }

    public void HideSummary()
    {
        if (summaryPanel != null)
        {
            summaryPanel.SetActive(false);
            messageTimerEnd = -1f;
        }
    }
}