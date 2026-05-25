using UnityEngine;

public class ShopQueueSpot : MonoBehaviour
{
    [SerializeField] private int queueIndex;

    private ShopManager shopManager;

    public int QueueIndex => queueIndex;

    private void Awake()
    {
        shopManager = FindAnyObjectByType<ShopManager>();
    }

    private void OnEnable()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (shopManager != null)
            shopManager.RegisterQueueSpot(this);
    }

    private void OnDisable()
    {
        if (shopManager != null)
            shopManager.UnregisterQueueSpot(this);
    }
}
