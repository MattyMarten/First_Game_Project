// Data slot only — no effect logic yet. Druid (Stage 9) owns applying/treating these.
// Per Room_Druid.md Section 11: every effect here is a harsh penalty only.
// No debuff, or combination of debuffs, may ever fully block a recruit from being
// controlled or sent on expedition — that rule lives with whatever reads this list.
public enum RecruitStatusEffect
{
    Exhaustion,
    Poison,
    Injury,
    DeadManWalking,
    Cursed,
    BrokenBone,
    ShellShocked,
    Weakened
}
