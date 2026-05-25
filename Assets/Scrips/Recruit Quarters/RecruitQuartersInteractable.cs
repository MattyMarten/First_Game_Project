using UnityEngine;

public class RecruitQuartersInteractable : PanelInteractable
{
    [Header("Recruit References")]
    [SerializeField] private RecruitQuartersActor recruitActor;
    [SerializeField] private RecruitQuartersUI recruitQuartersUI;

    protected override void Awake()
    {
        base.Awake();

        if (recruitActor == null)
            recruitActor = GetComponentInParent<RecruitQuartersActor>();

        if (recruitQuartersUI == null)
            recruitQuartersUI = FindAnyObjectByType<RecruitQuartersUI>();

        if (panel == null && recruitQuartersUI != null)
            panel = recruitQuartersUI.gameObject;
    }

    protected override void OnPanelOpened(PlayerInteraction player)
    {
        if (recruitActor == null)
            recruitActor = GetComponentInParent<RecruitQuartersActor>();

        if (recruitQuartersUI == null)
            recruitQuartersUI = FindAnyObjectByType<RecruitQuartersUI>();

        if (recruitActor == null || recruitQuartersUI == null)
            return;

        recruitQuartersUI.OpenForRecruit(recruitActor.RecruitData);
    }
}