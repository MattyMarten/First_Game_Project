using UnityEngine;

/// <summary>
/// Reward composition for a completed quest (Room_QuestBoard.md Section 10:
/// "Rewards may include: cobalt coins, progression XP for recruits, other items
/// or progression hooks as defined per quest"). Rewards are distributed
/// automatically on completion — no manual claim step.
/// </summary>
[System.Serializable]
public class QuestReward
{
    [Header("Coins")]
    public int cobaltCoins;

    [Header("Recruit XP")]
    [Tooltip("Progression XP granted to each recruit that took part in the expedition this quest was tied to.")]
    public int recruitXP;

    [Header("Extension Hook")]
    [Tooltip("Placeholder for item/progression rewards not yet modeled (e.g. Data Sticks, gear). " +
             "Leave empty until a concrete reward type needs wiring in — do not build a generic " +
             "'reward item' system speculatively.")]
    public string notes;
}
