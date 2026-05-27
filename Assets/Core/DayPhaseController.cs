using System;
using System.Collections.Generic;
using UnityEngine;

public enum DayPhase
{
    Morning,
    Day,
    Evening,
    Night,
    Dawn
}

/// <summary>
/// Global owner of the day/phase cycle.
/// Wire this up in the scene inspector alongside references to all
/// systems that need to respond to phase changes.
/// </summary>
public class DayPhaseController : MonoBehaviour
{
    [Header("State")]
    [SerializeField] private int currentDay = 1;
    [SerializeField] private DayPhase currentPhase = DayPhase.Morning;

    [Header("References — Time")]
    [SerializeField] private MerchantDayManager merchantDayManager;

    [Header("References — Traffic")]
    [SerializeField] private ShopNpcTrafficManager shopNpcTrafficManager;

    [Header("References — Expedition")]
    [SerializeField] private ExpeditionManager expeditionManager;
    [SerializeField] private ExpeditionResolver expeditionResolver;

    [Header("References — Reward Distribution")]
    [SerializeField] private ShopManager shopManager;
    [SerializeField] private RawMaterialStorage rawMaterialStorage;
    [SerializeField] private GoodStorage goodStorage;
    [SerializeField] private RecruitRosterManager recruitRosterManager;

    [Header("Injury Recovery")]
    [SerializeField] private int injuryRecoveryDays = 2;

    private ExpeditionResultData pendingResult;

    public int CurrentDay => currentDay;
    public DayPhase CurrentPhase => currentPhase;

    public event Action<DayPhase> OnPhaseChanged;

    private void Awake()
    {
        if (merchantDayManager == null)
            merchantDayManager = FindAnyObjectByType<MerchantDayManager>();

        if (shopNpcTrafficManager == null)
            shopNpcTrafficManager = FindAnyObjectByType<ShopNpcTrafficManager>();

        if (expeditionManager == null)
            expeditionManager = FindAnyObjectByType<ExpeditionManager>();

        if (expeditionResolver == null)
            expeditionResolver = FindAnyObjectByType<ExpeditionResolver>();

        if (shopManager == null)
            shopManager = FindAnyObjectByType<ShopManager>();

        if (rawMaterialStorage == null)
            rawMaterialStorage = FindAnyObjectByType<RawMaterialStorage>();

        if (goodStorage == null)
            goodStorage = FindAnyObjectByType<GoodStorage>();

        if (recruitRosterManager == null)
            recruitRosterManager = FindAnyObjectByType<RecruitRosterManager>();
    }

    private void Start()
    {
        ApplyPhase(currentPhase);
    }

    // ── Public API ──────────────────────────────────────────────────────────

    /// <summary>
    /// Advances to the next phase in sequence: Morning → Day → Evening → Night → Dawn → Morning.
    /// </summary>
    public void AdvancePhase()
    {
        DayPhase next = currentPhase switch
        {
            DayPhase.Morning  => DayPhase.Day,
            DayPhase.Day      => DayPhase.Evening,
            DayPhase.Evening  => DayPhase.Night,
            DayPhase.Night    => DayPhase.Dawn,
            DayPhase.Dawn     => DayPhase.Morning,
            _                 => DayPhase.Morning
        };

        SetPhase(next);
    }

    /// <summary>
    /// Jumps directly to a specific phase. Use AdvancePhase() for normal flow.
    /// </summary>
    public void SetPhase(DayPhase phase)
    {
        currentPhase = phase;
        ApplyPhase(currentPhase);
        OnPhaseChanged?.Invoke(currentPhase);

        Debug.Log($"[DayPhaseController] Day {currentDay} — Phase: {currentPhase}");
    }

    // ── Phase application ───────────────────────────────────────────────────

    private void ApplyPhase(DayPhase phase)
    {
        switch (phase)
        {
            case DayPhase.Morning:
                OnMorning();
                break;

            case DayPhase.Day:
                OnDay();
                break;

            case DayPhase.Evening:
                OnEvening();
                break;

            case DayPhase.Night:
                OnNight();
                break;

            case DayPhase.Dawn:
                OnDawn();
                break;
        }
    }

    private void OnMorning()
    {
        TickRecovery();

        if (shopNpcTrafficManager != null)
            shopNpcTrafficManager.SetTrafficEnabled(false);
    }

    private void OnDay()
    {
        if (shopNpcTrafficManager != null)
            shopNpcTrafficManager.SetTrafficEnabled(true);
    }

    private void OnEvening()
    {
        if (shopNpcTrafficManager != null)
            shopNpcTrafficManager.SetTrafficEnabled(false);
    }

    private void OnNight()
    {
        if (shopNpcTrafficManager != null)
            shopNpcTrafficManager.SetTrafficEnabled(false);

        RunExpeditionResolution();
    }

    private void OnDawn()
    {
        if (shopNpcTrafficManager != null)
            shopNpcTrafficManager.SetTrafficEnabled(false);

        DistributeExpeditionResults();
        AdvanceDay();
    }

    // ── Expedition resolution ───────────────────────────────────────────────

    private void RunExpeditionResolution()
    {
        if (expeditionManager == null || expeditionResolver == null)
            return;

        ExpeditionSessionData session = expeditionManager.CurrentSession;

        if (session == null)
            return;

        pendingResult = expeditionResolver.Resolve(session);
    }

    private void DistributeExpeditionResults()
    {
        if (pendingResult == null)
            return;

        if (shopManager != null && pendingResult.goldEarned > 0)
            shopManager.AddMoney(pendingResult.goldEarned);

        if (rawMaterialStorage != null)
        {
            for (int i = 0; i < pendingResult.materialRewards.Count; i++)
            {
                ExpeditionResultData.MaterialReward reward = pendingResult.materialRewards[i];

                if (reward?.material != null)
                    rawMaterialStorage.Add(reward.material, reward.amount);
            }
        }

        if (goodStorage != null)
        {
            for (int i = 0; i < pendingResult.goodRewards.Count; i++)
            {
                ExpeditionResultData.GoodReward reward = pendingResult.goodRewards[i];

                if (reward?.good != null)
                    goodStorage.Add(reward.good, reward.amount);
            }
        }

        ApplyInjuries(pendingResult.injuredRecruitIds);

        if (recruitRosterManager != null)
            recruitRosterManager.ClearParty();

        if (expeditionManager != null)
            expeditionManager.ClearSelection();

        pendingResult = null;
    }

    private void ApplyInjuries(List<string> injuredIds)
    {
        if (injuredIds == null || injuredIds.Count == 0 || recruitRosterManager == null)
            return;

        List<RecruitData> roster = recruitRosterManager.GetAllHiredRecruits();

        for (int i = 0; i < roster.Count; i++)
        {
            RecruitData recruit = roster[i];

            if (recruit == null)
                continue;

            for (int j = 0; j < injuredIds.Count; j++)
            {
                if (recruit.recruitId == injuredIds[j])
                {
                    recruit.status = RecruitStatus.Unavailable;
                    recruit.unavailableForDays = injuryRecoveryDays;

                    Debug.Log($"[DayPhaseController] {recruit.recruitName} was injured and will recover in {injuryRecoveryDays} day(s).");
                    break;
                }
            }
        }
    }

    // ── Day advancement ─────────────────────────────────────────────────────

    private void AdvanceDay()
    {
        currentDay++;

        if (merchantDayManager != null)
            merchantDayManager.SetCurrentDay(currentDay);

        Debug.Log($"[DayPhaseController] Advanced to Day {currentDay}.");
    }

    private void TickRecovery()
    {
        if (recruitRosterManager == null)
            return;

        List<RecruitData> roster = recruitRosterManager.GetAllHiredRecruits();

        for (int i = 0; i < roster.Count; i++)
        {
            RecruitData recruit = roster[i];

            if (recruit == null || recruit.status != RecruitStatus.Unavailable)
                continue;

            recruit.unavailableForDays = Mathf.Max(0, recruit.unavailableForDays - 1);

            if (recruit.unavailableForDays <= 0)
            {
                recruit.status = RecruitStatus.Idle;
                Debug.Log($"[DayPhaseController] {recruit.recruitName} has recovered and is available again.");
            }
        }
    }
}
