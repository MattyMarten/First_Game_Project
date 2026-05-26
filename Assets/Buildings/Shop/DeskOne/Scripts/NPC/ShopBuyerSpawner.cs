using UnityEngine;

public class ShopBuyerSpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopBuyerNPC buyerPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform deskWaitPoint;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 15f;
    [SerializeField] private bool useLocalAutoSpawn = true;

    private float spawnTimer;

    private void Awake()
    {
        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        if (!useLocalAutoSpawn)
            return;

        if (shopManager == null || buyerPrefab == null || spawnPoint == null || deskWaitPoint == null)
            return;

        if (!shopManager.IsShopOpen)
            return;

        spawnTimer -= Time.deltaTime;

        if (spawnTimer > 0f)
            return;

        spawnTimer = spawnInterval;

        TrySpawnBuyer();
    }

    public bool CanSpawnBuyer()
    {
        if (shopManager == null || buyerPrefab == null || spawnPoint == null || deskWaitPoint == null)
            return false;

        if (!shopManager.IsShopOpen)
            return false;

        if (!shopManager.CanAcceptAnotherBuyer())
            return false;

        if (!shopManager.HasAnyItemsForSale())
            return false;

        return true;
    }

    public bool TrySpawnFromTrafficManager(Transform sharedSpawnPoint, Transform sharedExitPoint)
    {
        if (!CanSpawnBuyer())
            return false;

        Transform spawnToUse = sharedSpawnPoint != null ? sharedSpawnPoint : spawnPoint;
        Transform exitToUse = sharedExitPoint != null ? sharedExitPoint : spawnPoint;

        ShopBuyerNPC spawnedBuyer = Instantiate(buyerPrefab, spawnToUse.position, spawnToUse.rotation);

        if (spawnedBuyer == null)
            return false;

        bool registered = shopManager.TryRegisterBuyer(spawnedBuyer);
        if (!registered)
        {
            Destroy(spawnedBuyer.gameObject);
            return false;
        }

        spawnedBuyer.Initialize(shopManager, deskWaitPoint, exitToUse);
        return true;
    }

    private void TrySpawnBuyer()
    {
        TrySpawnFromTrafficManager(spawnPoint, spawnPoint);
    }
}