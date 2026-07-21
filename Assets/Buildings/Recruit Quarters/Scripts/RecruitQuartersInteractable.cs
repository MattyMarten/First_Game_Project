using UnityEngine;

public class RecruitQuartersInteractable : PanelInteractable
{
    [Header("Recruit References")]
    [SerializeField] private RecruitLocker recruitLocker;
    [SerializeField] private RecruitQuartersUI recruitQuartersUI;

    protected override void Awake()
    {
        base.Awake();

        if (recruitLocker == null)
            recruitLocker = GetComponentInParent<RecruitLocker>();

        if (recruitQuartersUI == null)
            recruitQuartersUI = FindAnyObjectByType<RecruitQuartersUI>();

        if (panel == null && recruitQuartersUI != null)
            panel = recruitQuartersUI.gameObject;
    }

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (recruitLocker == null)
            recruitLocker = GetComponentInParent<RecruitLocker>();

        if (recruitQuartersUI == null)
            recruitQuartersUI = FindAnyObjectByType<RecruitQuartersUI>();

        if (recruitLocker == null || recruitQuartersUI == null)
            return;

        // Looked up live — works whether the bed is occupied, empty, or the recruit
        // was just retired/reassigned. The locker itself never gets destroyed.
        RecruitData recruit = recruitLocker.GetOccupyingRecruit();

        recruitQuartersUI.OpenForRecruit(recruit);
    }
}
