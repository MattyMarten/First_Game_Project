using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Master quest data (Room_QuestBoard.md Section 11: "Quest definitions (game data,
/// not room state)"). This is the full list of every quest that exists in the game.
/// QuestBoardRoomManager (built in Step 7.2) reads these to decide what's valid to
/// offer each day — it never mutates this asset at runtime.
///
/// NOT the same thing as ShopRequestData: that's the Shop Service Desk's small
/// base-level fetch/delivery task system, which is explicitly separate (Section 10:
/// "Smaller base-level tasks from request visitors... do not go onto the Quest Board").
/// </summary>
[CreateAssetMenu(fileName = "NewQuestDefinition", menuName = "Quest Board/Quest Definition")]
public class QuestDefinition : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("Stable unique ID, used for unlock-condition references and duplicate-record checks. " +
             "Not the asset name — renaming the asset later should not break references.")]
    public string questId;

    public string questName;

    [TextArea(3, 6)]
    public string missionDescription;

    [Header("Rank")]
    [Tooltip("Ignored if isMainline is true — main-line quests use their own giver-based slot system, not rank gating.")]
    public QuestRank rankRequirement = QuestRank.E;

    [Header("Main-line")]
    [Tooltip("Main-line (Q) quests come from special NPCs (primarily Dwarf, Professor) and compete " +
             "for the board's 3 main-line slots (Section 11) instead of the daily normal-quest pool.")]
    public bool isMainline;

    [Tooltip("Only relevant if isMainline is true. Name of the NPC who can give this quest.")]
    public string mainlineGiverName;

    [Header("Objective")]
    public QuestObjectiveType objectiveType;

    [Tooltip("Target of the objective, meaning depends on objectiveType " +
             "(e.g. material name for GatherMaterial, enemy name for DefeatEnemy/ScanEnemy, sector ID for ReachSector).")]
    public string objectiveTarget;

    [Tooltip("How much of the target is required (ignored for objective types where it doesn't apply, e.g. ReachSector).")]
    public int objectiveAmount = 1;

    [Header("Reward")]
    public QuestReward reward;

    [Header("Board Rank XP")]
    [Tooltip("XP granted toward the Quest Board's own rank on completion (Section 10: rank ups at 250 XP). " +
             "The doc doesn't specify per-quest amounts yet — this is a placeholder you should tune per quest. " +
             "Assumption: main-line (Q) quests don't feed this since Q is a separate track; set to 0 for those.")]
    public int rankXPReward = 25;

    [Header("Appearance (normal quests only)")]
    [Range(0f, 1f)]
    [Tooltip("Chance this quest appears on a given day once valid (Section 11). Ignored for main-line quests.")]
    public float baseAppearanceChance = 0.25f;

    [Header("Unlock Conditions")]
    [Tooltip("questId values of quests that must be completed before this one can appear at all. " +
             "Empty = always unlockable once rank allows it (Section 11 example: completing A adds B and C to the valid pool).")]
    public List<string> prerequisiteQuestIds = new();

    /// <summary>
    /// Rank check only — does not check prerequisites or "already active/held" state,
    /// since those require room state the definition itself doesn't have. QuestBoardRoomManager
    /// combines this with prerequisite + already-held checks to determine full validity (Section 11).
    /// </summary>
    public bool MeetsRankRequirement(QuestRank currentBoardRank)
    {
        return isMainline || rankRequirement <= currentBoardRank;
    }
}
