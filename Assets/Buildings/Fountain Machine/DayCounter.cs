using UnityEngine;

/// <summary>
/// LEGACY COMPATIBILITY SHIM — replaces the old standalone DayCounter.
///
/// This used to hold its own currentDay value. Now it just forwards to
/// DayPhaseSystem, which is the single source of truth as of Stage 0.
///
/// This exists only so ShopCoreManager.cs (and anything else that already
/// references DayCounter) keeps compiling without changes today.
///
/// Stage 5 (Shop) should remove this class entirely and point ShopCoreManager
/// straight at DayPhaseSystem.Instance.CurrentDay instead.
///
/// This file replaces: Assets/Buildings/Fountain Machine/DayCounter.cs
/// (same path, same class name — drop it in over the old one).
/// </summary>
public class DayCounter : MonoBehaviour
{
    public int CurrentDay => DayPhaseSystem.Instance != null ? DayPhaseSystem.Instance.CurrentDay : 1;
}
