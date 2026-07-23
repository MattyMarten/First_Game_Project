// Target path in your project: Assets/Buildings/Merchant/Scripts/Core/MerchantPedestalInteractable.cs

using UnityEngine;

/// <summary>
/// Sits on one physical pedestal GameObject in the Merchant Room.
/// Same PanelInteractable flow as DisplayInteractable/DecorSpotInteractable —
/// Interact() shows `panel`, calls OnPanelOpened(), switches to inventory
/// input mode. Closing is handled by a ClosableUIPanel on the same panel
/// GameObject, no new code needed for that.
///
/// IMPORTANT — pedestalIndex assignment:
/// MerchantRoomManager.RollAllPedestals() always fills its list in this
/// fixed order: Utility(3) -> Backpack(1) -> Charm(2) -> Material(3) ->
/// ShopDecor(4). So index 0-2 = the three Utility pedestals, 3 = the
/// Backpack pedestal, 4-5 = the two Charm pedestals, 6-8 = the three
/// Material pedestals, 9-12 = the four Shop Decor pedestals. Assign each
/// physical pedestal's `pedestalIndex` in the Inspector to match its
/// intended category and position — this is NOT auto-assigned, since the
/// room's physical layout is a scene decision, not a code one. Any pedestal
/// can also end up showing a Data Stick overnight regardless of its index
/// (see MerchantRoomManager.TryOverlayDataStick).
/// </summary>
public class MerchantPedestalInteractable : PanelInteractable
{
    [Header("Pedestal References")]
    [SerializeField] private MerchantRoomManager roomManager;
    [SerializeField] private MerchantPedestalUI pedestalUI;

    [Tooltip("Which slot in MerchantRoomManager.Pedestals this physical pedestal displays. See class comment above for the fixed ordering.")]
    [SerializeField] private int pedestalIndex;

    protected override void Awake()
    {
        base.Awake();

        if (roomManager == null)
            roomManager = FindAnyObjectByType<MerchantRoomManager>();
    }

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (pedestalUI != null)
            pedestalUI.OpenForPedestal(roomManager, pedestalIndex);
    }
}
