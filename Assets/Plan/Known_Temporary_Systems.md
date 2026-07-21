# Known Temporary Systems

Referenced from `Project_Status.md`. This is a running log of every debug/placeholder shortcut added to the codebase — things that stand in for a real system that doesn't exist yet. Each entry should say what it fakes, why, and which future stage replaces it. Add to this file the same session you add the shortcut; don't let it fall behind.

---

## Core upgrade slot — debug-unlocked
**Where:** `CoreRoomManager.UnlockUpgradeSlotDebug()`
**What it fakes:** the Dwarf's Upgrade Board granting Core an upgrade slot at a real milestone.
**Why it's temporary:** the Dwarf and the Upgrade Board don't exist yet (Stage 8).
**Replaced by:** Stage 8 — Dwarf's Upgrade Board calls this same unlock instead of a debug trigger.

---

## Data Stick acquisition — debug-triggered
**Where:** `DataStickConsumer.debugStickToAcquire` + the `Debug: Acquire Assigned Stick` context-menu action.
**What it fakes:** a Data Stick actually reaching the base through one of its real sources.
**Why it's temporary:** none of the real acquisition sources exist yet — sector loot (Stage 11), the Merchant's Data Stick pedestal (Stage 6), or request/talking visitors (Stage 7 / Stage 5).
**Replaced by:** whichever of those stages ships first should call `DataStickConsumer.Acquire()` directly instead of the debug button. The debug action can stay in the code afterward as a manual test tool — it's harmless — but it should no longer be the only way to unlock a recipe.

---

## Gear Upgrade Station level — debug-upgraded
**Where:** `GearUpgradeStationManager.TryUpgradeStationLevel()` + the `Debug: Upgrade Station Level` context-menu action.
**What it fakes:** the Dwarf's Upgrade Board raising the station's own level (LV1→LV2→LV3), same concept as Core's upgrade slot above.
**Why it's temporary:** same reason as Core's entry — the Dwarf/Upgrade Board doesn't exist yet.
**Replaced by:** Stage 8, same as Core's.

---

## Data Stick duplicate-conversion value — placeholder number
**Where:** `DataStickItem.duplicateCoinValue` (default 10).
**What it fakes:** a real, balanced payout for acquiring a Data Stick whose recipe is already unlocked.
**Why it's temporary:** exact value is explicitly TBD pending balancing (`Room_Workshop.md` Section 21, `Open_Architecture_Questions.md`).
**Replaced by:** no stage owns this specifically — revisit during the Stage 12 polish/balancing pass, or earlier if a specific stage's testing makes the placeholder value obviously wrong.



- RecruitQuartersDebugSeeder (Assets/Buildings/Recruit Quarters/Scripts) — debug-only recruit seeding via context menu, for testing capacity/levels before Dwarf (Stage 8) seeds real starting recruits. Safe to delete once Stage 8 lands.
- ShopCoreManager.ModifyAppeal(int delta) — flat clamped-delta stub. Stage 5 needs to confirm this is sufficient or replace it with real Appeal rules (Room_Shop.md Section 18).
- RecruitData.activeStatusEffects — data slot only, no effect/lockout logic yet. Druid (Stage 9) owns applying/treating these.
