using UnityEngine;

public class RecruitManagementBoardInteractable : PanelInteractable
{
    [Header("Board References")]
    [SerializeField] private RecruitManagementBoardUI recruitManagementBoardUI;

    protected override void Awake()
    {
        base.Awake();

        if (recruitManagementBoardUI == null)
            recruitManagementBoardUI = FindAnyObjectByType<RecruitManagementBoardUI>();

        if (panel == null && recruitManagementBoardUI != null)
            panel = recruitManagementBoardUI.gameObject;
    }

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (recruitManagementBoardUI == null)
            recruitManagementBoardUI = FindAnyObjectByType<RecruitManagementBoardUI>();

        if (recruitManagementBoardUI == null)
            return;

        recruitManagementBoardUI.Open();
    }
}