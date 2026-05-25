using UnityEngine;

public class UtilityCraftingInteractable : PanelInteractable
{
    [Header("Utility Crafting References")]
    [SerializeField] private UtilityCraftingUI utilityCraftingUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (utilityCraftingUI != null)
            utilityCraftingUI.ShowCategory(UtilityCategory.Utility);
    }
}
