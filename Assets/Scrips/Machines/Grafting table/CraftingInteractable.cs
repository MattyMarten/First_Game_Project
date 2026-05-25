using UnityEngine;

public class CraftingInteractable : PanelInteractable
{
    [Header("Crafting References")]
    [SerializeField] private CraftingStationUI craftingUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (craftingUI != null)
            craftingUI.RefreshUI();
    }
}
