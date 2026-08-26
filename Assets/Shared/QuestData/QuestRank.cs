/// <summary>
/// Quest Board rank track (Room_QuestBoard.md, Section 10).
/// Main-line quests are NOT part of this ladder — they're tracked separately
/// via QuestDefinition.isMainline, since the board can rank up through E-S
/// independently of main-line progression.
/// </summary>
public enum QuestRank
{
    E,
    D,
    C,
    B,
    A,
    S
}
