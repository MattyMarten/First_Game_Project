using System;
using UnityEngine;

/// <summary>
/// This file replaces the Stage 0 stub at:
/// Assets/GlobalSystems/Scripts/CoreRoomManager.cs (same path, same class name).
///
/// Stage 2 — real Core Room logic per Room_Core.md:
///   - loaded cobalt coins, capacity, daily consumption, range, all scaling
///     together per upgrade level (Section 9)
///   - Normal / Warning / Offline state machine (Section 10)
///   - deposit interaction pulling from Storage's CobaltCoinStorage (Stage 1)
///   - a debug-only upgrade unlock, standing in for the Dwarf's real
///     Upgrade Board (Stage 8)
///
/// Appeal is NOT touched here on purpose - Base_Global_Systems.md is explicit
/// that Shop owns Appeal directly. Core only fires OnFailedDayPenalty; Shop
/// (Stage 5) is what will actually subtract appeal when that fires.
/// </summary>
public enum CoreState
{
    Normal,
    Warning,
    Offline
}

/// <summary>
/// One row of the upgrade table. LV1 values are the doc's real fixed values;
/// LV2/LV3 are explicitly placeholders per Room_Core.md Section 9 ("subject
/// to balancing") - tweak freely once sector/range design is finalized.
/// </summary>
[Serializable]
public class CoreUpgradeLevelData
{
    public int capacity = 200;
    public int dailyConsumption = 50;
    public int depositAmount = 50;
    [Tooltip("0 = adjacent unlocked sectors only (LV1 baseline). Each level beyond that adds reach.")]
    public int range = 0;
}

public class CoreRoomManager : MonoBehaviour
{
    public static CoreRoomManager Instance { get; private set; }

    [Header("Dependencies")]
    [Tooltip("Storage's coin reserve, built in Stage 1 (CobaltCoinStorage).")]
    [SerializeField] private CobaltCoinStorage coinReserve;

    [Header("Upgrade Levels (index 0 = LV1)")]
    [SerializeField]
    private CoreUpgradeLevelData[] upgradeLevels = new CoreUpgradeLevelData[]
    {
        new CoreUpgradeLevelData { capacity = 200, dailyConsumption = 50, depositAmount = 50, range = 0 },
        // LV2/LV3 are placeholders - see Room_Core.md Section 9 & 19 (Open Questions)
        new CoreUpgradeLevelData { capacity = 300, dailyConsumption = 75, depositAmount = 50, range = 1 },
        new CoreUpgradeLevelData { capacity = 400, dailyConsumption = 100, depositAmount = 50, range = 2 },
    };

    [Header("Current State (visible for debugging)")]
    [SerializeField] private int upgradeLevelIndex = 0;
    [SerializeField] private bool upgradeSlotUnlocked = false;
    [SerializeField] private int loadedCoins = 0;
    [SerializeField] private CoreState currentState = CoreState.Normal;

    private bool failedDayPenaltyAppliedToday = false;

    public int LoadedCoins => loadedCoins;
    public int Capacity => CurrentLevelData.capacity;
    public int DailyConsumption => CurrentLevelData.dailyConsumption;
    public int DepositAmount => CurrentLevelData.depositAmount;
    public int Range => CurrentLevelData.range;
    public int UpgradeLevel => upgradeLevelIndex + 1; // displayed as LV1/LV2/LV3
    public bool UpgradeSlotUnlocked => upgradeSlotUnlocked;
    public bool IsMaxLevel => upgradeLevelIndex >= upgradeLevels.Length - 1;
    public CoreState CurrentState => currentState;

    /// <summary>
    /// Stage 0's stub hardcoded this to true. Now it reflects real state:
    /// only Offline actually means no power.
    /// </summary>
    public bool IsOnline => currentState != CoreState.Offline;

    private CoreUpgradeLevelData CurrentLevelData =>
        upgradeLevels[Mathf.Clamp(upgradeLevelIndex, 0, upgradeLevels.Length - 1)];

    public event Action<CoreState> OnStateChanged;
    public event Action<int> OnRangeChanged;
    /// <summary>Fires once per failed day. Stage 5 (Shop) subscribes to actually reduce Appeal by 20.</summary>
    public event Action OnFailedDayPenalty;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        if (DayPhaseSystem.Instance != null)
            DayPhaseSystem.Instance.OnPhaseChanged += HandlePhaseChanged;
    }

    private void OnDisable()
    {
        if (DayPhaseSystem.Instance != null)
            DayPhaseSystem.Instance.OnPhaseChanged -= HandlePhaseChanged;
    }

    private void HandlePhaseChanged(DayPhase newPhase)
    {
        switch (newPhase)
        {
            case DayPhase.Morning:
                HandleMorning();
                break;
            case DayPhase.Evening:
                HandleEvening();
                break;
        }
    }

    /// <summary>
    /// Room_Core.md "Warning state rule": checked at Morning; not enough
    /// loaded cobalt for the day -> Warning, remains until resolved.
    /// </summary>
    private void HandleMorning()
    {
        failedDayPenaltyAppliedToday = false;

        // "Recovery rule": Offline only clears when the player manually
        // deposits enough - never auto-clear it here.
        if (currentState == CoreState.Offline)
            return;

        SetState(loadedCoins < DailyConsumption ? CoreState.Warning : CoreState.Normal);
    }

    /// <summary>
    /// Room_Core.md "Daily consumption rule" + "Shutdown / offline rule".
    /// Your existing DayPhaseSystem.CloseShop() already IS the "Shop close"
    /// trigger the doc describes, so this hooks straight into it - no new
    /// day/phase logic needed.
    /// </summary>
    private void HandleEvening()
    {
        if (currentState == CoreState.Warning)
        {
            SetState(CoreState.Offline);
            ApplyFailedDayPenaltyOnce();
            return;
        }

        loadedCoins = Mathf.Max(0, loadedCoins - DailyConsumption);
    }

    private void ApplyFailedDayPenaltyOnce()
    {
        if (failedDayPenaltyAppliedToday) return;

        failedDayPenaltyAppliedToday = true;
        Debug.Log("[CoreRoomManager] Failed-day penalty triggered (Appeal -20 - Shop applies this in Stage 5).");
        OnFailedDayPenalty?.Invoke();
    }

    /// <summary>
    /// Room_Core.md Section 8: fixed-amount deposit, no custom amounts.
    /// Also handles the "Recovery rule" - depositing enough while Offline
    /// restores power.
    /// </summary>
    public bool TryDeposit()
    {
        if (coinReserve == null)
        {
            Debug.LogWarning("[CoreRoomManager] No CobaltCoinStorage reference assigned - cannot deposit.");
            return false;
        }

        int amount = DepositAmount;

        if (loadedCoins >= Capacity)
        {
            Debug.Log("[CoreRoomManager] Core is already at max capacity.");
            return false;
        }

        if (!coinReserve.TrySpend(amount))
        {
            Debug.Log("[CoreRoomManager] Not enough coins in Storage reserve to deposit.");
            return false;
        }

        loadedCoins = Mathf.Min(Capacity, loadedCoins + amount);

        if ((currentState == CoreState.Offline || currentState == CoreState.Warning)
            && loadedCoins >= DailyConsumption)
        {
            bool wasOffline = currentState == CoreState.Offline;
            SetState(CoreState.Normal);

            if (wasOffline)
                Debug.Log("[CoreRoomManager] Power restored. (Stage 2 placeholder for the ~5s restoration sequence - add the real AI voice line / lighting swap later.)");
        }

        return true;
    }

    /// <summary>
    /// Stand-in for the Dwarf's real Upgrade Board (Stage 8). Call this from
    /// a debug button/inspector context menu until the real milestone-trigger
    /// exists (Room_Core.md Section 17).
    /// </summary>
    public void UnlockUpgradeSlotDebug()
    {
        upgradeSlotUnlocked = true;
        Debug.Log("[CoreRoomManager] Upgrade slot force-unlocked (debug).");
    }

    public bool TryUpgrade()
    {
        if (!upgradeSlotUnlocked)
        {
            Debug.Log("[CoreRoomManager] Upgrade slot is still locked.");
            return false;
        }

        if (IsMaxLevel)
        {
            Debug.Log("[CoreRoomManager] Core is already at max level.");
            return false;
        }

        upgradeLevelIndex++;
        Debug.Log($"[CoreRoomManager] Core upgraded to LV{UpgradeLevel}. Range={Range}, Capacity={Capacity}, DailyConsumption={DailyConsumption}");
        OnRangeChanged?.Invoke(Range);
        return true;
    }

    private void SetState(CoreState newState)
    {
        if (currentState == newState) return;

        currentState = newState;
        Debug.Log($"[CoreRoomManager] Core state changed to {newState}");
        OnStateChanged?.Invoke(newState);
    }
}
