using System;
using UnityEngine;

/// <summary>
/// Tracks the Quest Board's rank and rank XP (Room_QuestBoard.md Section 10:
/// "Every 250 XP causes the Quest Board to rank up"). Sub-system manager owned
/// by QuestBoardRoomManager (Step 7.2) — not a standalone room manager itself,
/// per Room_Naming_Conventions.md's "<FeatureName>Manager" rule for in-room sub-systems.
/// </summary>
public class QuestRankManager : MonoBehaviour
{
    private const int XPPerRankUp = 250;

    [SerializeField] private QuestRank currentRank = QuestRank.E;
    [SerializeField] private int currentXP;

    /// <summary>Fired whenever XP changes, for UI (rank XP progress bar).</summary>
    public event Action OnXPChanged;

    /// <summary>Fired when the board ranks up. Params: old rank, new rank.</summary>
    public event Action<QuestRank, QuestRank> OnRankUp;

    public QuestRank CurrentRank => currentRank;
    public int CurrentXP => currentXP;
    public int XPToNextRank => XPPerRankUp;

    /// <summary>
    /// Adds rank XP and handles rank-up. Call this when a quest completes
    /// (Section 10: "Complete a quest: ... quest rank XP accumulates").
    /// A single AddXP call can only trigger one rank-up even if the amount is
    /// large enough for two — this matches "quests give a fixed amount" design;
    /// revisit only if a real gap (e.g. bulk XP grants) turns out to need it.
    /// </summary>
    public void AddXP(int amount)
    {
        if (amount <= 0)
            return;

        // Already at the top of the normal ladder — no further rank to reach.
        if (currentRank == QuestRank.S)
        {
            OnXPChanged?.Invoke();
            return;
        }

        currentXP += amount;
        OnXPChanged?.Invoke();

        if (currentXP >= XPPerRankUp)
        {
            currentXP -= XPPerRankUp;
            QuestRank oldRank = currentRank;
            currentRank++;

            Debug.Log($"Quest Board — +{amount} XP — Rank Up! {oldRank} → {currentRank}");
            OnRankUp?.Invoke(oldRank, currentRank);
        }
        else
        {
            Debug.Log($"Quest Board — +{amount} XP");
        }
    }

    /// <summary>Debug/test hook — bypasses normal XP flow to force a specific rank.</summary>
    public void DEBUG_SetRank(QuestRank rank)
    {
        currentRank = rank;
        currentXP = 0;
        OnXPChanged?.Invoke();
    }
}
