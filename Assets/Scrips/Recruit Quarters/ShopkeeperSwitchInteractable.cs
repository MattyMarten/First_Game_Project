using UnityEngine;

public class ShopkeeperSwitchInteractable : Interactable
{
    [Header("References")]
    [SerializeField] private PlayerCharacterManager playerCharacterManager;
    [SerializeField] private InputModeManager inputModeManager;

    protected override void Awake()
    {
        base.Awake();

        if (playerCharacterManager == null)
            playerCharacterManager = FindAnyObjectByType<PlayerCharacterManager>();

        if (inputModeManager == null)
            inputModeManager = FindAnyObjectByType<InputModeManager>();
    }

    public override void Interact(PlayerInteraction player)
    {
        if (playerCharacterManager == null)
        {
            Debug.LogWarning("ShopkeeperSwitchInteractable: No PlayerCharacterManager found.", this);
            return;
        }

        playerCharacterManager.SwitchToShopkeeper();

        if (inputModeManager != null)
            inputModeManager.SetGameplayMode();
    }
}