using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Quest Board room manager (Room_QuestBoard.md). Owns the daily quest pool,
/// held/active quest lists, main-line slot competition, and quest completion.
///
/// This replaces RequestBoardManager as the room's real manager — RequestBoardManager
/// operated on ShopRequestData (Shop Service Desk's separate base-level task system)
/// and only had add/remove/list, none of the rank/pool/limits/mainline logic this
/// room actually needs. If ShopRequestData / RequestBoardManager are still wired to
/// live NPCs elsewhere, leave them running as the Service Desk's own system — they
/// are a different concept from the QuestDefinition quests this class manages.
///
/// Naming: "<RoomName>RoomManager" per Room_Naming_Conventions.md Section 5.
/// </summary>
public class QuestBoardRoomManager : MonoBehaviour
{
    private const int MaxHeldNormal = 5;
    private const int MaxHeldMainline = 3;
    private const int MaxActiveNormal = 2;
    // Only one active main-line quest at a time — no constant needed, just a single slot.

    [Header("Quest Pool (game data)")]
    [Tooltip("Every quest that exists in the game. This list is read, never mutated at runtime.")]
    [SerializeField] private List<QuestDefinition> allQuestDefinitions = new();

    [Header("Sub-systems")]
    [SerializeField] private QuestRankManager rankManager;

    [Header("Cross-Room Refs")]
    [Tooltip("Appeal currently lives on ShopCoreManager (no dedicated AppealSystem exists yet). " +
             "Quest Board calls into it directly for appeal changes, the same way Decor does.")]
    [SerializeField] private ShopCoreManager shopCoreManager;

    // ---- Room state (not game data) ----
    private readonly List<QuestDefinition> heldNormalQuests = new();
    private readonly List<QuestDefinition> heldMainlineQuests = new();
    private readonly List<QuestDefinition> activeNormalQuests = new();
    private QuestDefinition activeMainlineQuest;

    private readonly HashSet<string> completedQuestIds = new();
    private readonly Dictionary<string, int> questProgress = new();

    /// <summary>NPCs waiting for a main-line slot to open (Section 11 slot competition).</summary>
    private readonly List<(string npcName, QuestDefinition quest)> waitingMainlineGivers = new();

    public event Action OnHeldQuestsChanged;
    public event Action OnActiveQuestsChanged;
    public event Action<QuestDefinition, QuestReward> OnQuestCompleted;

    /// <summary>Fired on completion for the Info Room to record (built in Step 7.5).</summary>
    public event Action<QuestDefinition> OnQuestRecordReady;

    /// <summary>UI feedback hook for blocked actions (Section 9/16: "clear warning appears").</summary>
    public event Action<string> OnActionBlocked;

    public IReadOnlyList<QuestDefinition> HeldNormalQuests => heldNormalQuests;
    public IReadOnlyList<QuestDefinition> HeldMainlineQuests => heldMainlineQuests;
    public IReadOnlyList<QuestDefinition> ActiveNormalQuests => activeNormalQuests;
    public QuestDefinition ActiveMainlineQuest => activeMainlineQuest;
    public QuestRank CurrentRank => rankManager != null ? rankManager.CurrentRank : QuestRank.E;

    private void OnEnable()
    {
        if (DayPhaseSystem.Instance != null)
            DayPhaseSystem.Instance.OnDayAdvanced += HandleDayAdvanced;
    }

    private void OnDisable()
    {
        if (DayPhaseSystem.Instance != null)
            DayPhaseSystem.Instance.OnDayAdvanced -= HandleDayAdvanced;
    }

    private void HandleDayAdvanced(int newDay)
    {
        RefreshDailyQuests();
    }

    // ------------------------------------------------------------------
    // Daily pool refresh (Section 11: automatic each morning)
    // ------------------------------------------------------------------

    public void RefreshDailyQuests()
    {
        int freeSlots = MaxHeldNormal - heldNormalQuests.Count;
        if (freeSlots <= 0)
            return;

        var candidates = allQuestDefinitions.Where(IsValidToOffer).ToList();

        foreach (var quest in candidates)
        {
            if (freeSlots <= 0)
                break;

            if (UnityEngine.Random.value <= quest.baseAppearanceChance)
            {
                heldNormalQuests.Add(quest);
                freeSlots--;
            }
        }

        OnHeldQuestsChanged?.Invoke();
    }

    /// <summary>
    /// Section 11's three stated validity rules, plus one assumption not spelled out
    /// in the doc: a quest that's already been completed doesn't reappear. The doc
    /// only lists rank/unlock/not-already-held-or-active as the rules — flag this if
    /// you want repeatable quests, since that would need an explicit flag on
    /// QuestDefinition instead of this blanket exclusion.
    /// </summary>
    private bool IsValidToOffer(QuestDefinition quest)
    {
        if (quest == null || quest.isMainline)
            return false;

        if (heldNormalQuests.Contains(quest) || activeNormalQuests.Contains(quest))
            return false;

        if (completedQuestIds.Contains(quest.questId))
            return false;

        if (!quest.MeetsRankRequirement(CurrentRank))
            return false;

        foreach (var prereqId in quest.prerequisiteQuestIds)
        {
            if (!completedQuestIds.Contains(prereqId))
                return false;
        }

        return true;
    }

    // ------------------------------------------------------------------
    // Quest handoff from Shop's request-visitor flow (Section 5: "accepted
    // quest handoff from the Shop request visitor flow"; Section 10 appeal
    // rules for Accept/Decline "at Service Desk"). This is the entry point
    // for quests an NPC personally offers, as opposed to ones that silently
    // appear via RefreshDailyQuests. Main-line quest givers should also come
    // through here so the appeal rule applies to them too.
    // ------------------------------------------------------------------

    public bool AcceptQuestFromVisitor(QuestDefinition quest, string sourceNpcName = null)
    {
        if (quest == null)
            return false;

        bool added = quest.isMainline
            ? RequestMainlineQuestSlot(sourceNpcName, quest)
            : TryAddHeldNormalQuest(quest);

        // Appeal gain applies to the act of accepting, even if the quest ends up
        // queued waiting for a main-line slot rather than immediately on the board.
        ApplyAppeal(+1);
        return added;
    }

    public void DeclineQuestFromVisitor(QuestDefinition quest)
    {
        // Nothing to remove — a declined quest was never added. Just the appeal hit.
        ApplyAppeal(-2);
    }

    private bool TryAddHeldNormalQuest(QuestDefinition quest)
    {
        if (heldNormalQuests.Contains(quest))
            return false;

        if (heldNormalQuests.Count >= MaxHeldNormal)
        {
            OnActionBlocked?.Invoke("Quest Board is full — discard a held quest first.");
            return false;
        }

        heldNormalQuests.Add(quest);
        OnHeldQuestsChanged?.Invoke();
        return true;
    }

    // ------------------------------------------------------------------
    // Main-line slot competition (Section 11)
    // ------------------------------------------------------------------

    public bool RequestMainlineQuestSlot(string npcName, QuestDefinition quest)
    {
        if (quest == null || !quest.isMainline)
        {
            Debug.LogWarning("[QuestBoardRoomManager] RequestMainlineQuestSlot called with a non-mainline quest.");
            return false;
        }

        if (heldMainlineQuests.Contains(quest) || waitingMainlineGivers.Any(w => w.quest == quest))
            return false;

        if (heldMainlineQuests.Count < MaxHeldMainline)
        {
            heldMainlineQuests.Add(quest);
            OnHeldQuestsChanged?.Invoke();
            return true;
        }

        // No free slot — the NPC holds their quest until one opens (Section 11).
        waitingMainlineGivers.Add((npcName, quest));
        return false;
    }

    private void TryFillMainlineSlotFromWaiting()
    {
        if (heldMainlineQuests.Count >= MaxHeldMainline || waitingMainlineGivers.Count == 0)
            return;

        // "If a main-line slot is free and more than one NPC is ready... the giver
        // is chosen randomly between them" — same random-pick rule applies here
        // when a slot opens up later (Section 11).
        int index = UnityEngine.Random.Range(0, waitingMainlineGivers.Count);
        var picked = waitingMainlineGivers[index];
        waitingMainlineGivers.RemoveAt(index);

        heldMainlineQuests.Add(picked.quest);
        Debug.Log($"[QuestBoardRoomManager] Main-line slot filled by {picked.npcName}: {picked.quest.questName}");
        OnHeldQuestsChanged?.Invoke();
    }

    // ------------------------------------------------------------------
    // Select / deselect / discard (Section 9)
    // ------------------------------------------------------------------

    public bool TrySelectQuest(QuestDefinition quest)
    {
        if (quest == null)
            return false;

        if (quest.isMainline)
        {
            if (!heldMainlineQuests.Contains(quest))
                return false;

            if (activeMainlineQuest != null)
            {
                OnActionBlocked?.Invoke("A main-line quest is already active — deselect it first.");
                return false;
            }

            activeMainlineQuest = quest;
        }
        else
        {
            if (!heldNormalQuests.Contains(quest) || activeNormalQuests.Contains(quest))
                return false;

            if (activeNormalQuests.Count >= MaxActiveNormal)
            {
                OnActionBlocked?.Invoke("2 active quests already selected — deselect one first.");
                return false;
            }

            activeNormalQuests.Add(quest);
        }

        OnActiveQuestsChanged?.Invoke();
        return true;
    }

    public bool TryDeselectQuest(QuestDefinition quest)
    {
        if (quest == null)
            return false;

        bool removed;
        if (quest.isMainline && activeMainlineQuest == quest)
        {
            activeMainlineQuest = null;
            removed = true;
        }
        else
        {
            removed = activeNormalQuests.Remove(quest);
        }

        if (removed)
            OnActiveQuestsChanged?.Invoke();

        return removed;
    }

    /// <summary>
    /// Section 11, last line: "Main-line quests still cannot be rejected or
    /// discarded by the player once delivered." Discard only ever applies to
    /// normal quests — attempting it on a main-line quest is blocked outright.
    /// </summary>
    public bool DiscardQuest(QuestDefinition quest)
    {
        if (quest == null)
            return false;

        if (quest.isMainline)
        {
            OnActionBlocked?.Invoke("Main-line quests can't be discarded.");
            return false;
        }

        if (!heldNormalQuests.Remove(quest))
            return false;

        activeNormalQuests.Remove(quest);
        questProgress.Remove(quest.questId);

        ApplyAppeal(-3);
        OnHeldQuestsChanged?.Invoke();
        OnActiveQuestsChanged?.Invoke();
        return true;
    }

    // ------------------------------------------------------------------
    // Progress + completion
    // ------------------------------------------------------------------

    /// <summary>Expedition systems call this to report progress toward an active quest's objective.</summary>
    public void PushQuestProgress(string questId, int amount)
    {
        if (string.IsNullOrEmpty(questId) || amount == 0)
            return;

        questProgress.TryGetValue(questId, out int current);
        questProgress[questId] = current + amount;
        OnActiveQuestsChanged?.Invoke();
    }

    public int GetQuestProgress(QuestDefinition quest)
    {
        if (quest == null)
            return 0;

        return questProgress.TryGetValue(quest.questId, out int value) ? value : 0;
    }

    /// <summary>
    /// Completes an active quest: distributes rewards, grants board rank XP,
    /// applies appeal, and notifies the Info Room. Coins and recruit XP are NOT
    /// applied directly here — Quest Board doesn't own physical inventory or the
    /// recruit roster (Section 14), so OnQuestCompleted just hands the reward data
    /// to whatever system does (Economy/Storage for coins, an expedition/recruit
    /// router for recruit XP).
    /// </summary>
    public bool CompleteQuest(QuestDefinition quest)
    {
        if (quest == null)
            return false;

        bool isActive = quest.isMainline
            ? activeMainlineQuest == quest
            : activeNormalQuests.Contains(quest);

        if (!isActive)
        {
            Debug.LogWarning($"[QuestBoardRoomManager] CompleteQuest called on a non-active quest: {quest.questName}");
            return false;
        }

        if (quest.isMainline)
        {
            activeMainlineQuest = null;
            heldMainlineQuests.Remove(quest);
        }
        else
        {
            activeNormalQuests.Remove(quest);
            heldNormalQuests.Remove(quest);
        }

        completedQuestIds.Add(quest.questId);
        questProgress.Remove(quest.questId);

        ApplyAppeal(+2);

        if (rankManager != null && quest.rankXPReward > 0)
            rankManager.AddXP(quest.rankXPReward);

        OnQuestCompleted?.Invoke(quest, quest.reward);
        OnQuestRecordReady?.Invoke(quest);
        OnHeldQuestsChanged?.Invoke();
        OnActiveQuestsChanged?.Invoke();

        if (quest.isMainline)
            TryFillMainlineSlotFromWaiting();

        return true;
    }

    private void ApplyAppeal(int delta)
    {
        if (shopCoreManager != null)
            shopCoreManager.ModifyAppeal(delta);
        else
            Debug.LogWarning("[QuestBoardRoomManager] No ShopCoreManager reference assigned — appeal change skipped.");
    }
}
