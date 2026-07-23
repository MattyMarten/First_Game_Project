using UnityEngine;

public class ShopCoreManager : MonoBehaviour
{
    // Whole-Shop coordinator. Owns Shop-wide state and future shared systems.
    // Desk-specific behavior remains in ShopManager (Desk 1) and ServiceDeskManager
    // (Desk 2 — now also handles recruit visitors, merged in from the old Desk Three).
    // Appeal (buyer count + sale price) and daily-report tracking live here for real.
    // Decor and Dirt are still not built anywhere in the project — separate future step.

    public enum ShopSpawnType
    {
        Desk1Buyer,
        Desk2TalkingVisitor,
        Desk2RequestVisitor,
        Desk3HireVisitor
    }

    [Header("Shop State")]
    [SerializeField] private bool shopOpen;

    [Header("Desk Managers")]
    [SerializeField] private ShopManager desk1Manager;
    [SerializeField] private ServiceDeskManager desk2Manager;

    [Header("Recruit Spawn Chance")]
    [SerializeField] private RecruitRosterManager recruitRosterManager;
    [SerializeField, Range(0f, 1f)] private float recruitSpawnChanceFloor = 0.25f;

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

    [Header("Future Shop Systems")]
    [SerializeField] private DayCounter dayCounter;
    [SerializeField] private int shopLevel = 1;
    [SerializeField] private int shopAppeal = 50;
    [SerializeField] private bool spawnCycleRunning;
    [SerializeField] private int remainingDailyVisitors;

    [Header("Shared Spawn / Exit")]
    [SerializeField] private Transform sharedSpawnPoint;
    [SerializeField] private Transform sharedExitPoint;

    [Header("Cobalt Coin Bank")]
    [SerializeField] private CobaltCoinStorage cobaltCoinStorage;

    [Header("Daily Report (session-only, resets each shop opening)")]
    [SerializeField] private int itemsSoldToday;
    [SerializeField] private int visitorsSeenToday;
    [SerializeField] private int coinsEarnedToday;

    [Header("Decor")]
    [SerializeField] private DecorManager decorManager;


    public bool IsShopOpen => shopOpen;

    public ShopManager Desk1Manager => desk1Manager;
    public ServiceDeskManager Desk2Manager => desk2Manager;

    public float SharedSpawnInterval => sharedSpawnInterval;
    public float CurrentSpawnTimer => currentSpawnTimer;

    public ShopBuyerSpawner ShopBuyerSpawner => shopBuyerSpawner;
    public ServiceVisitorSpawner ServiceVisitorSpawner => serviceVisitorSpawner;

    public int ShopLevel => shopLevel;
    public int ShopAppeal => shopAppeal;

    /// <summary>
    /// Applies a clamped Appeal delta. Buyer-count and sale-price effects of the new
    /// Appeal value are now real (see GetBuyerCountAppealModifier / GetAppealSaleMultiplier).
    /// Decor's own Appeal contributions are still a separate, not-yet-built system —
    /// this method only handles the delta itself, decor will call into it once it exists.
    /// </summary>
    public void ModifyAppeal(int delta)
    {
        shopAppeal = Mathf.Clamp(shopAppeal + delta, 0, 100);
    }
    public bool IsSpawnCycleRunning => spawnCycleRunning;
    public int RemainingDailyVisitors => remainingDailyVisitors;

    public Transform SharedSpawnPoint => sharedSpawnPoint;
    public Transform SharedExitPoint => sharedExitPoint;

    // Shop Monitor read surface (Room_Shop.md Section 6/28).
    public int ItemsSoldToday => itemsSoldToday;
    public int VisitorsSeenToday => visitorsSeenToday;
    public int CoinsEarnedToday => coinsEarnedToday;
    public int TotalBaseCobaltCoins => cobaltCoinStorage != null ? cobaltCoinStorage.CoinCount : 0;
    public event System.Action OnDailyReportChanged;

    /// <summary>
    /// Call this whenever a sold good's daily-report counters need updating. Does NOT
    /// deposit into CobaltCoinStorage itself — the caller (ShopManager.AddMoney, etc.)
    /// is responsible for the actual deposit, since Shop-local money paths already route
    /// there. This just keeps the Shop Monitor's "items sold / coins earned today" numbers
    /// correct without depositing twice.
    /// </summary>
    public void RecordItemSold(int coinAmount)
    {
        if (coinAmount <= 0)
            return;

        itemsSoldToday++;
        coinsEarnedToday += coinAmount;
        OnDailyReportChanged?.Invoke();
    }

    /// <summary>
    /// Same as RecordItemSold but for non-item income (e.g. talking-visitor dialogue
    /// rewards) that shouldn't count toward "items sold today."
    /// </summary>
    public void RecordCoinsEarned(int coinAmount)
    {
        if (coinAmount <= 0)
            return;

        coinsEarnedToday += coinAmount;
        OnDailyReportChanged?.Invoke();
    }

    private void RecordVisitorSpawned()
    {
        visitorsSeenToday++;
        OnDailyReportChanged?.Invoke();
    }

    private void ResetDailyReport()
    {
        itemsSoldToday = 0;
        visitorsSeenToday = 0;
        coinsEarnedToday = 0;
        OnDailyReportChanged?.Invoke();
    }

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

        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();

        if (cobaltCoinStorage == null)
            cobaltCoinStorage = FindAnyObjectByType<CobaltCoinStorage>();

        if (decorManager == null)
            decorManager = FindAnyObjectByType<DecorManager>();
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
        ResetDailyReport();
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
            case ShopSpawnType.Desk3HireVisitor:
                return serviceVisitorSpawner != null &&
                       serviceVisitorSpawner.TrySpawnSpecificVisitor(spawnType, sharedSpawnPoint, sharedExitPoint);
        }

        return false;
    }

    public void BuildDailyVisitorSpawnList()
    {
        dailyVisitorSpawnList.Clear();

        AddDailyVisitors(ShopSpawnType.Desk1Buyer, GetPlannedDesk1BuyerCount());
        AddDailyVisitors(ShopSpawnType.Desk2TalkingVisitor, GetPlannedDesk2TalkingVisitorCount());
        AddDailyVisitors(ShopSpawnType.Desk2RequestVisitor, GetPlannedDesk2RequestVisitorCount());
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
        int decorBonus = decorManager != null ? decorManager.GetBuyerCountBonus() : 0;
        return Mathf.Max(3, baseCount + appealModifier + decorBonus);
    }

    // Appeal used for buyer-count/sale tables now includes decor's conditional bonus —
    // stored shopAppeal itself stays untouched by decor, since decor's contribution
    // should vanish the moment the piece is removed (see DecorManager.GetAppealBonus()).
    private int GetEffectiveAppeal()
    {
        int decorBonus = decorManager != null ? decorManager.GetAppealBonus() : 0;
        return Mathf.Clamp(shopAppeal + decorBonus, 0, 100);
    }

    private int GetPlannedDesk2TalkingVisitorCount()
    {
        return Random.Range(1, 4); // 1-3
    }

    private int GetPlannedDesk2RequestVisitorCount()
    {
        return Random.Range(0, 3); // 0-2
    }

    // Recruit visitor generation rule (Room_Shop.md Section 13): not a flat level-based
    // range anymore. Chance = free recruit slots / total recruit slots, floored at 25%.
    // 0 free slots -> 0% chance, no recruit visitor at all that day. Rolled once here,
    // during daily pool generation at shop opening. Max 1 recruit visitor per day.
    private int GetPlannedDesk3HireVisitorCount()
    {
        if (recruitRosterManager == null)
            return 0;

        int totalSlots = recruitRosterManager.MaxTotalRecruitSlots;

        if (totalSlots <= 0)
            return 0;

        int freeSlots = Mathf.Max(0, totalSlots - recruitRosterManager.TotalRecruitCount);

        if (freeSlots <= 0)
            return 0;

        float decorRecruitBonus = decorManager != null ? decorManager.GetRecruitChanceBonus() : 0f;
        float spawnChance = Mathf.Clamp01(Mathf.Max((float)freeSlots / totalSlots, recruitSpawnChanceFloor) + decorRecruitBonus);

        return Random.value < spawnChance ? 1 : 0;
    }

    // Appeal Rules (Room_Shop.md Section 18). Single source of truth for both the
    // buyer-count modifier and the sale-price multiplier — ShopManager reads the
    // price multiplier from here instead of keeping its own copy of the table.
    private int GetBuyerCountAppealModifier()
    {
        int appeal = GetEffectiveAppeal();
        if (appeal <= 10) return -3;
        if (appeal <= 20) return -2;
        if (appeal <= 40) return -1;
        if (appeal <= 60) return 0;
        if (appeal <= 80) return 1;
        if (appeal <= 90) return 2;
        return 3;
    }

    public float GetAppealSaleMultiplier()
    {
        int appeal = GetEffectiveAppeal();
        if (appeal <= 10) return 0.80f;
        if (appeal <= 20) return 0.85f;
        if (appeal <= 40) return 0.90f;
        if (appeal <= 60) return 1.00f;
        if (appeal <= 80) return 1.10f;
        if (appeal <= 90) return 1.15f;
        return 1.20f;
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
            RecordVisitorSpawned();
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
            case ShopSpawnType.Desk3HireVisitor:
                return serviceVisitorSpawner != null && serviceVisitorSpawner.CanSpawnServiceVisitor(spawnType);

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