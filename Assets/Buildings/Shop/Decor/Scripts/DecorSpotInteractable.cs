using UnityEngine;

// Room_Shop.md Section 16 — pressing E opens the decor panel: same PanelInteractable
// flow as DisplayInteractable (Assets/Buildings/Shop/Display/Scripts/DisplayInteractable.cs).
// PanelInteractable.Interact() already shows `panel` and calls
// InputModeManager.SetInventoryMode(), which disables the Player action map (stopping
// camera look) and enables the Inventory map — where Q/E are already bound to
// LeftPage/RightPage. DecorMenuUI reads those while the panel is open to cycle options.
// Closing is handled by attaching a ClosableUIPanel to the same panel GameObject
// (Assets/Shared/Interaction/Scripts/ClosableUIPanel.cs) — no new code needed for that.
public class DecorSpotInteractable : PanelInteractable
{
    [Header("Decor References")]
    [SerializeField] private DecorSpot decorSpot;
    [SerializeField] private DecorMenuUI decorMenuUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (decorMenuUI != null)
            decorMenuUI.OpenForSpot(decorSpot);
    }
}
