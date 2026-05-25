using UnityEngine;

public class ShopDeskInteractable : PanelInteractable
{
    [Header("Desk References")]
    [SerializeField] private ShopDeskUI shopDeskUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (shopDeskUI != null)
            shopDeskUI.OpenDesk();
    }
}
