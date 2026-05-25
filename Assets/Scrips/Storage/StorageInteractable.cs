using UnityEngine;

public class StorageInteractable : PanelInteractable
{
    [Header("Storage References")]
    [SerializeField] private RawMaterialStorage storage;
    [SerializeField] private MaterialStorageUI storageUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (storageUI != null)
        {
            storageUI.storage = storage;
            storageUI.RefreshUI();
        }
    }
}