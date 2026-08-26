/// <summary>
/// Starter set of expedition objective types a quest can require (Room_QuestBoard.md
/// Section 11: "objective type and target"). The doc doesn't lock this list down yet,
/// so treat this as extendable — add new values as expedition systems (Stage 11) define
/// more concrete objective kinds. Every quest is expedition-based (Section 10), so there
/// is intentionally no "base task" type here.
/// </summary>
public enum QuestObjectiveType
{
    GatherMaterial,
    DefeatEnemy,
    ReachSector,
    ScanEnemy,
    RetrieveDocument,
    SurviveDuration
}
