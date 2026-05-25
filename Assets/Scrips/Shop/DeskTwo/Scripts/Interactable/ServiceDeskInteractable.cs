using UnityEngine;

public class ServiceDeskInteractable : PanelInteractable
{
    [Header("Desk References")]
    [SerializeField] private ServiceDeskUI serviceDeskUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (serviceDeskUI != null)
            serviceDeskUI.OpenDesk();
    }
}