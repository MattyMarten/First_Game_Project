using UnityEngine;

public class ServiceQueueSpot : MonoBehaviour
{
    [SerializeField] private int queueIndex;

    private ServiceDeskManager serviceDeskManager;

    public int QueueIndex => queueIndex;

    private void Awake()
    {
        serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();
    }

    private void OnEnable()
    {
        if (serviceDeskManager == null)
            serviceDeskManager = FindAnyObjectByType<ServiceDeskManager>();

        if (serviceDeskManager != null)
            serviceDeskManager.RegisterQueueSpot(this);
    }

    private void OnDisable()
    {
        if (serviceDeskManager != null)
            serviceDeskManager.UnregisterQueueSpot(this);
    }
}