using UnityEngine;

public class HireDeskInteractable : PanelInteractable
{
    [Header("Desk References")]
    [SerializeField] private HireDeskUI hireDeskUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (hireDeskUI != null)
            hireDeskUI.OpenDesk();
    }
}