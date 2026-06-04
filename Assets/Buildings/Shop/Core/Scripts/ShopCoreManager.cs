using UnityEngine;

public class ShopCoreManager : MonoBehaviour
{
    // Whole-Shop coordinator. Owns Shop-wide state and future shared systems.
    // Desk-specific behavior remains in ShopManager (Desk 1), ServiceDeskManager, and HireDeskManager.
    // Future whole-Shop ownership lives here: Appeal, daily visitor flow, spawn cycle, and report tracking.

    public enum ShopSpawnType
    {
        Desk1Buyer,
        Desk2TalkingVisitor,
        Desk2RequestVisitor,
        Desk2MerchantVisitor,
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
    [SerializeField] private DayCounter dayCounter;
    [SerializeField] private int shopLevel = 1;
    [SerializeField] private int shopAppeal = 50;
    [SerializeField] private bool spawnCycleRunning;
    [SerializeField] private int remainingDailyVisitors;

    [Header("Shared Spawn / Exit")]
    [SerializeField] private Transform sharedSpawnPoint;
    [SerializeField] private Transform sharedExitPoint;

    [Header("Daily Visitor Plan")]
    [SerializeField] private int dailyDesk2MerchantVisitors = 1;


    public bool IsShopOpen => shopOpen;

    public ShopManager Desk1Manager => desk1Manager;
    public ServiceDeskManager Desk2Manager => desk2Manager;
    public HireDeskManager Desk3Manager => desk3Manager;

    public float SharedSpawnInterval => sharedSpawnInterval;
    public float CurrentSpawnTimer => currentSpawnTimer;

    public ShopBuyerSpawner ShopBuyerSpawner => shopBuyerSpawner;
    public ServiceVisitorSpawner ServiceVisitorSpawner => serviceVisitorSpawner;
    public HireVisitorSpawner HireVisitorSpawner => hireVisitorSpawner;

    public int ShopLevel => shopLevel;
    public int ShopAppeal => shopAppeal;
    public bool IsSpawnCycleRunning => spawnCycleRunning;
    public int RemainingDailyVisitors => remainingDailyVisitors;

    public Transform SharedSpawnPoint => sharedSpawnPoint;
    public Transform SharedExitPoint => sharedExitPoint;

    private readonly System.Collections.Generic.List<ShopSpawnType> dailyVisitorSpawnList = new(); 
    public int DailyVisitorSpawnCount => dailyVisitorSpawnList.Count;
    public bool HasRemainingDailyVisitors => dailyVisitorSpawnList.Count > 0;

    private void Awake()
    {
        if (dayCounter == null)
            dayCounter = FindAnyObjectByType<DayCounter>();

        if (shopBuyerSpawner == null)
            shopBuyerSpawner = FindAnyObjectByType<ShopBuyerSpawner>();

        if (serviceVisitorSpawner == null)
            serviceVisitorSpawner = FindAnyObjectByType<ServiceVisitorSpawner>();

        if (hireVisitorSpawner == null)
            hireVisitorSpawner = FindAnyObjectByType<HireVisitorSpawner>();
    }

    private void Update()
    {
        if (sharedSpawnPoint == null || sharedExitPoint == null)
            return;

        if (!spawnCycleRunning)
            return;

        if (!TryConsumeSpawnTick(Time.deltaTime))
            return;

        TrySpawnNextNpc();
    }

    public void OpenShop()
    {
        shopOpen = true;

        BuildDailyVisitorSpawnList();
        ResetSpawnTimer();

        if (desk1Manager != null)
            desk1Manager.OpenShop();
    }

    public void CloseShop()
    {
        shopOpen = false;
        ClearDailyVisitorSpawnList();
        ResetSpawnTimer();

        if (desk1Manager != null)
            desk1Manager.CloseShop();
    }

    [ContextMenu("Open Shop")]
    private void DebugOpenShop()
    {
        OpenShop();
    }

    [ContextMenu("Close Shop")]
    private void DebugCloseShop()
    {
        CloseShop();
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

    private int GetCurrentDay()
    {
        return dayCounter != null ? dayCounter.CurrentDay : 1;
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
        if (TryConsumeNextDailyVisitor(out ShopSpawnType plannedSpawnType))
        {
            if (TrySpawnSpecificType(plannedSpawnType))
                return;
        }

        if (!HasRemainingDailyVisitors)
            return;

        System.Collections.Generic.List<ShopSpawnType> validSpawnTypes = new();

        if (shopBuyerSpawner != null && shopBuyerSpawner.CanSpawnBuyer())
            validSpawnTypes.Add(ShopSpawnType.Desk1Buyer);

        if (serviceVisitorSpawner != null && serviceVisitorSpawner.CanSpawnServiceVisitor())
        {
            validSpawnTypes.Add(ShopSpawnType.Desk2TalkingVisitor);
            validSpawnTypes.Add(ShopSpawnType.Desk2RequestVisitor);
            validSpawnTypes.Add(ShopSpawnType.Desk2MerchantVisitor);
        }

        if (hireVisitorSpawner != null && hireVisitorSpawner.CanSpawnHireVisitor())
            validSpawnTypes.Add(ShopSpawnType.Desk3HireVisitor);

        if (validSpawnTypes.Count == 0)
            return;

        ShopSpawnType chosenType = ChooseNextSpawnType(validSpawnTypes);
        TrySpawnSpecificType(chosenType);
    }

    private bool TrySpawnSpecificType(ShopSpawnType spawnType)
    {
        switch (spawnType)
        {
            case ShopSpawnType.Desk1Buyer:
                return shopBuyerSpawner != null &&
                       shopBuyerSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);

            case ShopSpawnType.Desk2TalkingVisitor:
            case ShopSpawnType.Desk2RequestVisitor:
            case ShopSpawnType.Desk2MerchantVisitor:
                return serviceVisitorSpawner != null &&
                       serviceVisitorSpawner.TrySpawnSpecificVisitor(spawnType, sharedSpawnPoint, sharedExitPoint);

            case ShopSpawnType.Desk3HireVisitor:
                return hireVisitorSpawner != null &&
                       hireVisitorSpawner.TrySpawnFromTrafficManager(sharedSpawnPoint, sharedExitPoint);
        }

        return false;
    }

    public void BuildDailyVisitorSpawnList()
    {
        dailyVisitorSpawnList.Clear();

        AddDailyVisitors(ShopSpawnType.Desk1Buyer, GetPlannedDesk1BuyerCount());
        AddDailyVisitors(ShopSpawnType.Desk2TalkingVisitor, GetPlannedDesk2TalkingVisitorCount());
        AddDailyVisitors(ShopSpawnType.Desk2RequestVisitor, GetPlannedDesk2RequestVisitorCount());
        AddDailyVisitors(ShopSpawnType.Desk2MerchantVisitor, GetPlannedDesk2MerchantVisitorCount());
        AddDailyVisitors(ShopSpawnType.Desk3HireVisitor, GetPlannedDesk3HireVisitorCount());

        ShuffleDailyVisitorSpawnList();
        remainingDailyVisitors = dailyVisitorSpawnList.Count;
        spawnCycleRunning = dailyVisitorSpawnList.Count > 0;
    }

    private int GetPlannedDesk1BuyerCount()
    {
        int baseCount = GetBaseBuyerCountForShopLevel();
        int appealModifier = GetBuyerCountAppealModifier();
        return Mathf.Max(0, baseCount + appealModifier);
    }

    private int GetPlannedDesk2TalkingVisitorCount()
    {
        return GetEffectiveShopLevel() switch
        {
            1 => 1,
            2 => 1,
            3 => 2,
            _ => 2
        };
    }

    private int GetPlannedDesk2RequestVisitorCount()
    {
        return GetEffectiveShopLevel() switch
        {
            1 => 1,
            2 => 2,
            3 => 2,
            _ => 2
        };
    }

    private int GetPlannedDesk2MerchantVisitorCount()
    {
        if (!IsMerchantVisitDay())
            return 0;

        return Mathf.Max(0, dailyDesk2MerchantVisitors);
    }

    private int GetPlannedDesk3HireVisitorCount()
    {
        return GetEffectiveShopLevel() switch
        {
            1 => 1,
            2 => 2,
            3 => 2,
            _ => 2
        };
    }

    private bool IsMerchantVisitDay()
    {
        return GetCurrentDay() % 3 == 0;
    }

    private int GetBuyerCountAppealModifier()
    {
        if (shopAppeal >= 70)
            return 1;

        if (shopAppeal <= 30)
            return -1;

        return 0;
    }

    private int GetBaseBuyerCountForShopLevel()
    {
        return GetEffectiveShopLevel() switch
        {
            1 => 3,
            2 => 4,
            3 => 5,
            _ => 5
        };
    }

    private void AddDailyVisitors(ShopSpawnType spawnType, int count)
    {
        int safeCount = Mathf.Max(0, count);

        for (int i = 0; i < safeCount; i++)
            dailyVisitorSpawnList.Add(spawnType);
    }

    private void ShuffleDailyVisitorSpawnList()
    {
        for (int i = 0; i < dailyVisitorSpawnList.Count; i++)
        {
            int randomIndex = Random.Range(i, dailyVisitorSpawnList.Count);
            (dailyVisitorSpawnList[i], dailyVisitorSpawnList[randomIndex]) =
                (dailyVisitorSpawnList[randomIndex], dailyVisitorSpawnList[i]);
        }
    }

    public bool TryConsumeNextDailyVisitor(out ShopSpawnType nextSpawnType)
    {
        nextSpawnType = default;

        if (dailyVisitorSpawnList.Count == 0)
            return false;

        nextSpawnType = dailyVisitorSpawnList[0];
        dailyVisitorSpawnList.RemoveAt(0);
        remainingDailyVisitors = dailyVisitorSpawnList.Count;
        spawnCycleRunning = dailyVisitorSpawnList.Count > 0;
        return true;
    }

    private void ClearDailyVisitorSpawnList()
    {
        dailyVisitorSpawnList.Clear();
        remainingDailyVisitors = 0;
        spawnCycleRunning = false;
    }

    private int GetEffectiveShopLevel()
    {
        return Mathf.Max(1, shopLevel);
    }


    [ContextMenu("Debug Print Daily Visitor Plan")]
    private void DebugPrintDailyVisitorPlan()
    {
        BuildDailyVisitorSpawnList();

        System.Text.StringBuilder builder = new();
        builder.AppendLine("=== Shop Daily Visitor Plan ===");
        builder.AppendLine($"Day: {GetCurrentDay()}");
        builder.AppendLine($"Shop Level: {GetEffectiveShopLevel()}");
        builder.AppendLine($"Shop Appeal: {shopAppeal}");
        builder.AppendLine($"Planned Count: {dailyVisitorSpawnList.Count}");

        for (int i = 0; i < dailyVisitorSpawnList.Count; i++)
            builder.AppendLine($"{i + 1}. {dailyVisitorSpawnList[i]}");

        Debug.Log(builder.ToString());
    }
}