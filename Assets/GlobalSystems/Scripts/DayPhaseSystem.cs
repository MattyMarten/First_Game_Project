using System;
using UnityEngine;

/// <summary>
/// The four base phases, per Base_Master_Plan.md Section 3.
/// </summary>
public enum DayPhase
{
    Morning,
    Day,
    Evening,
    Night
}

/// <summary>
/// Stage 0 — Global Systems Foundation.
///
/// Single source of truth for what day it is and what phase the base is in.
/// Every other room should ask THIS for the phase, and subscribe to
/// OnPhaseChanged instead of polling a value every frame.
///
/// Phase rules (Base_Master_Plan.md Section 3):
///   - phases are manual at the base, driven by player actions
///   - opening the Shop:            Morning -> Day
///   - closing the Shop:            Day     -> Evening
///   - launching expedition OR
///     confirming a dispatch-only
///     night (no personal team):   Evening -> Night
///   - expedition/dispatch resolves: Night  -> next Morning (day count +1)
///
/// This is a singleton that survives scene loads (DontDestroyOnLoad),
/// since it needs to be reachable from every room's scene.
/// </summary>
public class DayPhaseSystem : MonoBehaviour
{
    public static DayPhaseSystem Instance { get; private set; }

    [Header("Current State")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private DayPhase currentPhase = DayPhase.Morning;

    public int CurrentDay => currentDay;
    public DayPhase CurrentPhase => currentPhase;

    /// <summary>Fired every time the phase changes. Subscribe, don't poll.</summary>
    public event Action<DayPhase> OnPhaseChanged;

    /// <summary>Fired specifically when the day number ticks over (Night -> Morning).</summary>
    public event Action<int> OnDayAdvanced;

    private void Awake()
    {
        // Standard singleton guard: if one already exists (e.g. we came back
        // from a scene that also had one), destroy the duplicate.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // ---- Phase transitions -------------------------------------------------
    // Each method only succeeds if called from the correct phase. This
    // matches Section 3's "hard phase locks" note. If something calls these
    // out of order (a bug elsewhere), we log a warning and do nothing rather
    // than silently corrupting state.

    /// <summary>Call this when the player opens the Shop.</summary>
    public void OpenShop()
    {
        if (currentPhase != DayPhase.Morning)
        {
            Debug.LogWarning($"[DayPhaseSystem] OpenShop() called during {currentPhase}, expected Morning. Ignored.");
            return;
        }

        SetPhase(DayPhase.Day);
    }

    /// <summary>Call this when the player closes the Shop.</summary>
    public void CloseShop()
    {
        if (currentPhase != DayPhase.Day)
        {
            Debug.LogWarning($"[DayPhaseSystem] CloseShop() called during {currentPhase}, expected Day. Ignored.");
            return;
        }

        SetPhase(DayPhase.Evening);
    }

    /// <summary>
    /// Call this when the player launches a personal expedition, OR confirms
    /// an away-team-only dispatch with no personal expedition.
    /// </summary>
    public void LaunchNight()
    {
        if (currentPhase != DayPhase.Evening)
        {
            Debug.LogWarning($"[DayPhaseSystem] LaunchNight() called during {currentPhase}, expected Evening. Ignored.");
            return;
        }

        SetPhase(DayPhase.Night);
    }

    /// <summary>
    /// Call this when expedition/dispatch results are done resolving and the
    /// base should move on to the next Morning. Advances the day counter.
    /// </summary>
    public void ResolveNight()
    {
        if (currentPhase != DayPhase.Night)
        {
            Debug.LogWarning($"[DayPhaseSystem] ResolveNight() called during {currentPhase}, expected Night. Ignored.");
            return;
        }

        currentDay++;
        OnDayAdvanced?.Invoke(currentDay);
        SetPhase(DayPhase.Morning);
    }

    private void SetPhase(DayPhase newPhase)
    {
        currentPhase = newPhase;
        Debug.Log($"[DayPhaseSystem] Phase changed to {newPhase} (Day {currentDay})");
        OnPhaseChanged?.Invoke(newPhase);
    }
}
