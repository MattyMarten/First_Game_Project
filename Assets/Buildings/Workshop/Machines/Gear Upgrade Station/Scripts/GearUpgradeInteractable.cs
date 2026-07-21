// Target path in your project: Assets/Buildings/Workshop/Machines/Gear Upgrade Station/Scripts/GearUpgradeInteractable.cs

using UnityEngine;

public class GearUpgradeInteractable : PanelInteractable
{
    [Header("Gear Upgrade References")]
    [SerializeField] private GearUpgradeUI gearUpgradeUI;

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (gearUpgradeUI != null)
            gearUpgradeUI.RefreshUI();
    }
}
