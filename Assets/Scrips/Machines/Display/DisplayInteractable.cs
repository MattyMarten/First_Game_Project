using UnityEngine;

public class DisplayInteractable : PanelInteractable
{
    [Header("Display References")]
    [SerializeField] private DisplayStand displayStand;
    [SerializeField] private DisplayMenuUI displayMenuUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (displayMenuUI != null)
            displayMenuUI.OpenForDisplay(displayStand);
    }
}