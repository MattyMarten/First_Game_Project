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

    [Header("Spawn Cycle Settings")]
    [SerializeField] private float sharedSpawnInterval = 8f;
    [SerializeField] private float failedSpawnRetryDelay = 1f;
    [SerializeField] private float currentSpawnTimer;
    [SerializeField] private float currentRetryTimer;
    [SerializeField] private bool spawnAttemptCycleActive;
    [SerializeField] private int remainingChecksThisCycle;

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
    public System.Collections.Generic.IReadOnlyList<ShopSpawnType> DailyVisitorSpawnList => dailyVisitorSpawnList;
    public event System.Action OnDailyVisitorListChanged;

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

        if (spawnAttemptCycleActive)
        {
            if (!TickRetryTimer(Time.deltaTime))
                return;

            TryAdvanceSpawnAttemptCycle();
            return;
        }

        if (!TryConsumeSpawnTick(Time.deltaTime))
            return;

        BeginSpawnAttemptCycle();
        TryAdvanceSpawnAttemptCycle();
    }

    private void NotifyDailyVisitorListChanged()
    {
        OnDailyVisitorListChanged?.Invoke();
    }

    public void OpenShop()
    {
        shopOpen = true;
        EndSpawnAttemptCycle();
        BuildDailyVisitorSpawnList();
        ResetSpawnTimer();

        if (desk1Manager != null)
            desk1Manager.OpenShop();
    }

    public void CloseShop()
    {
        shopOpen = false;
        EndSpawnAttemptCycle();
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
        NotifyDailyVisitorListChanged();
    }

    private int GetPlannedDesk1BuyerCount()
    {
        int baseCount = GetBaseBuyerCountForShopLevel();
        int appealModifier = GetBuyerCountAppealModifier();
        return Mathf.Max(3, baseCount + appealModifier);
    }

    private int GetPlannedDesk2TalkingVisitorCount()
    {
        return Random.Range(1, 4); // 1-3
    }

    private int GetPlannedDesk2RequestVisitorCount()
    {
        return Random.Range(0, 3); // 0-2
    }

    private int GetPlannedDesk2MerchantVisitorCount()
    {
        return IsMerchantVisitDay() ? 1 : 0;
    }

    private int GetPlannedDesk3HireVisitorCount()
    {
        return GetEffectiveShopLevel() switch
        {
            1 => Random.Range(0, 2), // 0-1
            2 => 1,                  // exactly 1
            3 => Random.Range(1, 3), // 1-2
            _ => Random.Range(1, 3)
        };
    }

    private bool IsMerchantVisitDay()
    {
        int currentDay = GetCurrentDay();
        int level = GetEffectiveShopLevel();

        if (level >= 3)
            return currentDay % 2 == 0;

        return currentDay % 3 == 0;
    }

    private int GetBuyerCountAppealModifier()
    {
        if (shopAppeal >= 80)
            return 2;

        if (shopAppeal >= 60)
            return 1;

        if (shopAppeal >= 40)
            return 0;

        if (shopAppeal >= 20)
            return -1;

        return -2;
    }

    private int GetBaseBuyerCountForShopLevel()
    {
        return GetEffectiveShopLevel() switch
        {
            1 => Random.Range(6, 10),   // 6-9
            2 => Random.Range(8, 12),   // 8-11
            3 => Random.Range(10, 13),  // 10-12
            _ => Random.Range(10, 13)
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

    private void TryAdvanceSpawnAttemptCycle()
    {
        if (!spawnAttemptCycleActive)
            return;

        if (!PeekNextDailyVisitor(out ShopSpawnType plannedSpawnType))
        {
            EndSpawnAttemptCycle();
            spawnCycleRunning = false;
            return;
        }

        if (CanSpawnType(plannedSpawnType) && TrySpawnSpecificType(plannedSpawnType))
        {
            RemoveNextDailyVisitor();
            EndSpawnAttemptCycle();
            spawnCycleRunning = dailyVisitorSpawnList.Count > 0;
            return;
        }

        remainingChecksThisCycle--;

        if (remainingChecksThisCycle <= 0)
        {
            EndSpawnAttemptCycle();
            spawnCycleRunning = dailyVisitorSpawnList.Count > 0;
            return;
        }

        RotateNextDailyVisitorToBack();
        currentRetryTimer = failedSpawnRetryDelay;
    }

    private void ClearDailyVisitorSpawnList()
    {
        dailyVisitorSpawnList.Clear();
        remainingDailyVisitors = 0;
        spawnCycleRunning = false;
        NotifyDailyVisitorListChanged();
    }

    private int GetEffectiveShopLevel()
    {
        return Mathf.Max(1, shopLevel);
    }

    private bool PeekNextDailyVisitor(out ShopSpawnType spawnType)
    {
        spawnType = default;

        if (dailyVisitorSpawnList.Count == 0)
            return false;

        spawnType = dailyVisitorSpawnList[0];
        return true;
    }

    private void RemoveNextDailyVisitor()
    {
        if (dailyVisitorSpawnList.Count == 0)
            return;

        dailyVisitorSpawnList.RemoveAt(0);
        remainingDailyVisitors = dailyVisitorSpawnList.Count;
        spawnCycleRunning = dailyVisitorSpawnList.Count > 0;
        NotifyDailyVisitorListChanged();
    }

    private bool CanSpawnType(ShopSpawnType spawnType)
    {
        switch (spawnType)
        {
            case ShopSpawnType.Desk1Buyer:
                return shopBuyerSpawner != null && shopBuyerSpawner.CanSpawnBuyer();

            case ShopSpawnType.Desk2TalkingVisitor:
            case ShopSpawnType.Desk2RequestVisitor:
            case ShopSpawnType.Desk2MerchantVisitor:
                return serviceVisitorSpawner != null && serviceVisitorSpawner.CanSpawnServiceVisitor();

            case ShopSpawnType.Desk3HireVisitor:
                return hireVisitorSpawner != null && hireVisitorSpawner.CanSpawnHireVisitor();

            default:
                return false;
        }
    }

    private void RotateNextDailyVisitorToBack()
    {
        if (dailyVisitorSpawnList.Count <= 1)
            return;

        ShopSpawnType firstVisitor = dailyVisitorSpawnList[0];
        dailyVisitorSpawnList.RemoveAt(0);
        dailyVisitorSpawnList.Add(firstVisitor);
        remainingDailyVisitors = dailyVisitorSpawnList.Count;
        NotifyDailyVisitorListChanged();
    }

    private void BeginSpawnAttemptCycle()
    {
        if (dailyVisitorSpawnList.Count == 0)
            return;

        spawnAttemptCycleActive = true;
        remainingChecksThisCycle = dailyVisitorSpawnList.Count;
        currentRetryTimer = 0f;
    }

    private void EndSpawnAttemptCycle()
    {
        spawnAttemptCycleActive = false;
        remainingChecksThisCycle = 0;
        currentRetryTimer = 0f;
    }

    private bool TickRetryTimer(float deltaTime)
    {
        currentRetryTimer -= deltaTime;
        return currentRetryTimer <= 0f;
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