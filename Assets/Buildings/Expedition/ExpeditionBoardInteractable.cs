using UnityEngine;

public class ExpeditionBoardInteractable : PanelInteractable
{
    [Header("Board References")]
    [SerializeField] private ExpeditionPrepUI expeditionPrepUI;

    protected override void Awake()
    {
        base.Awake();

        if (expeditionPrepUI == null)
            expeditionPrepUI = FindAnyObjectByType<ExpeditionPrepUI>();

        if (panel == null && expeditionPrepUI != null)
            panel = expeditionPrepUI.gameObject;
    }

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (expeditionPrepUI == null)
            expeditionPrepUI = FindAnyObjectByType<ExpeditionPrepUI>();

        if (expeditionPrepUI == null)
            return;

        expeditionPrepUI.RefreshAll();
    }
}