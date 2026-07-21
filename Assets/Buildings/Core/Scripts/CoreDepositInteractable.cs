using UnityEngine;

/// <summary>
/// The Core's physical deposit point (Room_Core.md Section 8: "deposit 50
/// cobalt coins into the Core"). One interact = one fixed-amount deposit
/// attempt, pulled from Storage's coin reserve. No custom amounts, per doc.
/// </summary>
public class CoreDepositInteractable : Interactable
{
    public override void Interact(PlayerInteraction player)
    {
        if (CoreRoomManager.Instance == null)
        {
            Debug.LogWarning("[CoreDepositInteractable] No CoreRoomManager found in scene.");
            return;
        }

        CoreRoomManager.Instance.TryDeposit();
    }
}
