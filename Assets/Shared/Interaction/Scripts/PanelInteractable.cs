using UnityEngine;

public abstract class PanelInteractable : Interactable
{
    [Header("Panel References")]
    [SerializeField] protected GameObject panel;
    [SerializeField] protected InputModeManager inputModeManager;

    protected override void Awake()
    {
        base.Awake();

        if (panel != null)
            panel.SetActive(false);

        if (inputModeManager == null)
            inputModeManager = FindAnyObjectByType<InputModeManager>();
    }

    public override void Interact(PlayerInteraction player)
    {
        if (panel != null)
            panel.SetActive(true);

        OnPanelOpened(player);

        if (inputModeManager != null)
            inputModeManager.SetInventoryMode();
    }

    protected abstract void OnPanelOpened(PlayerInteraction player);
}