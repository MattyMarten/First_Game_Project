using UnityEngine;

/// <summary>
/// This file replaces: Assets/Buildings/Storage/Scripts/StorageInteractable.cs
/// (same path, same class name — drop it in over the old one).
///
/// Stage 1 change: also refreshes the new StorageSummaryUI (coins/goods/
/// utility totals) whenever the panel opens, alongside the existing
/// per-material row list.
/// </summary>
public class StorageInteractable : PanelInteractable
{
    [Header("Storage References")]
    [SerializeField] private RawMaterialStorage storage;
    [SerializeField] private MaterialStorageUI storageUI;

    [Header("Stage 1 Addition")]
    [SerializeField] private StorageSummaryUI summaryUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (storageUI != null)
        {
            storageUI.storage = storage;
            storageUI.RefreshUI();
        }

        if (summaryUI != null)
            summaryUI.RefreshUI();
    }
}