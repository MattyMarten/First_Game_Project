using UnityEngine;

public class ShopCoreManager : MonoBehaviour
{
    // Whole-Shop coordinator. Owns Shop-wide state and future shared systems.
    // Desk-specific behavior remains in ShopManager (Desk 1), ServiceDeskManager, and HireDeskManager.
    // Future whole-Shop ownership lives here: Appeal, daily visitor flow, spawn cycle, and report tracking.

    public enum ShopSpawnType
    {
        Desk1Buyer,
        Desk2ServiceVisitor,
        Desk3HireVisitor
    }

    [Header("Shop State")]
    [SerializeField] private bool shopOpen;

    [Header("Desk Managers")]
    [SerializeField] private ShopManager desk1Manager;
    [SerializeField] private ServiceDeskManager desk2Manager;
    [SerializeField] private HireDeskManager desk3Manager;

    [Header("Spawn Settings")]
    [SerializeField] private float sharedSpawnInterval = 6f;
    [SerializeField] private float currentSpawnTimer;

    [Header("Spawn Executors")]
    [SerializeField] private ShopBuyerSpawner shopBuyerSpawner;
    [SerializeField] private ServiceVisitorSpawner serviceVisitorSpawner;
    [SerializeField] private HireVisitorSpawner hireVisitorSpawner;

    [Header("Future Shop Systems")]
    [SerializeField] private int shopAppeal = 50;
    [SerializeField] private bool spawnCycleRunning;
    [SerializeField] private int remainingDailyVisitors;

    [Header("Shared Spawn / Exit")]
    [SerializeField] private Transform sharedSpawnPoint;
    [SerializeField] private Transform sharedExitPoint;

    public bool IsShopOpen => shopOpen;

    public ShopManager Desk1Manager => desk1Manager;
    public ServiceDeskManager Desk2Manager => desk2Manager;
    public HireDeskManager Desk3Manager => desk3Manager;

    public float SharedSpawnInterval => sharedSpawnInterval;
    public float CurrentSpawnTimer => currentSpawnTimer;

    public ShopBuyerSpawner ShopBuyerSpawner => shopBuyerSpawner;
    public ServiceVisitorSpawner ServiceVisitorSpawner => serviceVisitorSpawner;
    public HireVisitorSpawner HireVisitorSpawner => hireVisitorSpawner;

    public int ShopAppeal => shopAppeal;
    public bool IsSpawnCycleRunning => spawnCycleRunning;
    public int RemainingDailyVisitors => remainingDailyVisitors;

    public Transform SharedSpawnPoint => sharedSpawnPoint;
    public Transform SharedExitPoint => sharedExitPoint;

    private void Awake()
    {
        if (shopBuyerSpawner == null)
            shopBuyerSpawner = FindAnyObjectByType<ShopBuyerSpawner>();

        if (serviceVisitorSpawner == null)
            serviceVisitorSpawner = FindAnyObjectByType<ServiceVisitorSpawner>();

        if (hireVisitorSpawner == null)
            hireVisitorSpawner = FindAnyObjectByType<HireVisitorSpawner>();

        ResetSpawnTimer();
    }

    private void Update()
    {
        if (sharedSpawnPoint == null || sharedExitPoint == null)
            return;

        if (!TryConsumeSpawnTick(Time.deltaTime))
            return;

        TrySpawnNextNpc();
    }

    public void OpenShop()
    {
        shopOpen = true;

        if (desk1Manager != null)
            desk1Manager.OpenShop();
    }

    public void CloseShop()
    {
        shopOpen = false;

        if (desk1Manager != null)
            desk1Manager.CloseShop();
    }

    public void ResetSpawnTimer()
    {
        currentSpawnTimer = sharedSpawnInterval;
    }

    public void TickSpawnTimer(float deltaTime)
    {
        currentSpawnTimer -= deltaTime;
    }

    public bool IsSpawnTimerReady()
    {
        return currentSpawnTimer <= 0f;
    }

    public bool TryConsumeSpawnTick(float deltaTime)
    {
        if (!shopOpen)
            return false;

        TickSpawnTimer(deltaTime);

        if (!IsSpawnTimerReady())
            return false;

        ResetSpawnTimer();
        return true;
    }

    public ShopSpawnType ChooseNextSpawnType(System.Collections.Generic.List<ShopSpawnType> validSpawnTypes)
    {
        if (validSpawnTypes == null || validSpawnTypes.Count == 0)
            return default;

        int randomIndex = Random.Range(0, validSpawnTypes.Count);
        return validSpawnTypes[randomIndex];
    }

    public void TrySpawnNextNpc()
    {
        System.Collections.Generic.List<ShopSpawnType> validSpawnTypes = new();

        if (shopBuyerSpawner != null && shopBuyerSpawner.CanSpawnBuyer())
            validSpawnTypes.Add(ShopSpawnType.Desk1Buyer);

        if (serviceVisitorSpawner != null && serviceVisitorSpawner.CanSpawnServiceVisitor())
            validSpawnTypes.Add(ShopSpawnType.Desk2ServiceVisitor);

        if (hireVisitorSpawner != null && hireVisitorSpawner.CanSpawnHireVisitor())
            validSpawnTypes.Add(ShopSpawnType.Desk3HireVisitor);

        if (validSpawnTypes.Count == 0)
            return;

        ShopSpawnType chosenType = ChooseNextSpawnType(validSpawnTypes);

        switch (chosenType)
        {
            case ShopSpawnType.Desk1Buyer:
                shopBuyerSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
                break;

            case ShopSpawnType.Desk2ServiceVisitor:
                serviceVisitorSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
                break;

            case ShopSpawnType.Desk3HireVisitor:
                hireVisitorSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
                break;
        }
    }
}