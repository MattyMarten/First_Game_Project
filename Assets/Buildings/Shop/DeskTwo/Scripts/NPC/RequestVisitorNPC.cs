using UnityEngine;

public class RequestVisitorNPC : ServiceVisitorNPC
{
    [Header("Request")]
    [SerializeField] private ShopRequestData requestData;

    protected override void OnReachedDesk()
    {
        if (serviceDeskManager == null)
        {
            Leave();
            return;
        }

        if (requestData == null)
        {
            Debug.LogWarning("RequestVisitorNPC has no request data.", this);
            Leave();
            return;
        }

        bool created = serviceDeskManager.TryCreatePendingRequest(this, requestData);

        if (!created)
        {
            Leave();
            return;
        }
    }
}