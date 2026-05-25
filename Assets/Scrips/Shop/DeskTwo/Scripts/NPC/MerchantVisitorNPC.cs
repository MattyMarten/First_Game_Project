using UnityEngine;

public class MerchantVisitorNPC : ServiceVisitorNPC
{
    [Header("References")]
    [SerializeField] private MerchantDayManager merchantDayManager;

    protected override void Awake()
    {
        base.Awake();

        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        if (merchantDayManager == null)
            merchantDayManager = FindAnyObjectByType<MerchantDayManager>();
    }

    protected override void OnReachedDesk()
    {
        if (serviceDeskManager == null)
        {
            Leave();
            return;
        }

        if (merchantDayManager == null || !merchantDayManager.HasMerchantToday)
        {
            Leave();
            return;
        }

        GeneratedMerchantVisit visit = merchantDayManager.TodaysMerchantVisit;

        if (visit == null)
        {
            Leave();
            return;
        }

        bool created = serviceDeskManager.TryCreatePendingMerchantVisit(this, visit);

        if (!created)
            Leave();
    }
}