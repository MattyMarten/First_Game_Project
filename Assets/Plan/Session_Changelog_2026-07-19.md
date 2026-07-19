# Session Changelog — 2026-07-19 (Design Discussion Pass)

This session covered recruit scarcity, sectors/exploration, suits, and the removal of the Research Station. This file summarizes exactly what changed, which files were updated, and what's still genuinely open.

---

## Files updated/replaced this session
Drop these into your project folder in place of the old versions:

- `Base_Master_Plan.md` — major update (recruits, sectors, suits, removed systems, timing rules)
- `Base_Global_Systems.md` — Research/Knowledge section removed, sector graph note added
- `Room_Core.md` — now upgradeable, narrative role added
- `Room_Workshop.md` — Research Station removed, materials flattened, Suit Station added (replaces `Room_Workshop_Version2.md`)
- `Room_Map.md` — rewritten around sector categories instead of Forsaken City (replaces `Room_Map_Version2.md`)
- `Room_RecruitQuarters.md` — capacity 4/6/8 (2-per-room), retire action, debuffs never lock
- `Room_Druid.md` — debuff effect list updated so nothing is a hard lockout (replaces `Room_Druid_Version1.md`)
- `Room_Graveyard.md` — replacement decision is now a queue, not a single pending state
- `Room_Merchant.md` — added Data Stick pedestal (14th pedestal, 10%/day)
- `Room_Shop_PATCH_NOTES.md` — **not a full replacement** — apply this one small edit manually to your existing `Room_Shop_Version7_Version3_Version3.md` (recruit visitor chance formula)
- `Open_Architecture_Questions.md` — old resolved questions closed out, new ones added
- `Dispatch_Board.md` — **brand new file**, no previous version existed

### Files NOT touched this session (still accurate as-is)
- `Room_Info_Version2.md`, `Room_QuestBoard.md`, `Room_Storage_Version4_Version2.md`, `Room_Professor_Version1.md`, `Room_Dwarf_Version2.md`, `Room_Naming_Conventions.md`, `Documentation_Plan.md`, `Base_Implementation_Checklist.md`, `Review_Checkpoints.md`, `Notification_Display.md`, `Recruit_Selection_System.md`, `Entry_Base_Bonus_Loadout_System.md`, `Shop_RecruitMachine_NPC_Animation_Plan...md`, `Implementation_Status_Version5.md` (though this one should get a fresh pass noting today's changes before your next work session — see below).

---

## Everything decided this session, in order

1. **Recruit visitor chance** = free slots / total slots, 25% minimum floor.
2. **Recruit capacity**: LV1 4 / LV2 6 / LV3 8, at 2 beds per room. Base starts completely full (the 4 fixed starting recruits fill LV1 exactly).
3. **Retiring a recruit**: allowed at the locker, -3 appeal, no undead created.
4. **Debuffs**: never a hard lockout — always playable, always just a harsh penalty. This removed the need for a "what if the whole roster is unusable" safety net entirely.
5. **Away teams / Dispatch Board**: new Floor 2 station, separate from Recruit Selection and separate from the Sector Map. Any team size (including 1). Destination limited to already-unlocked sectors. Each recruit on the team rolls independently against the sector's success %. Can be used without a personal expedition — the day just skips to next Morning.
6. **Sectors replace Forsaken City entirely**: named categories (starting with N — Nature, L — Labs), each generating a fresh random instance every visit. Categories are unlocked permanently via in-expedition puzzle-solving + generator-powering.
7. **Core becomes upgradeable** — an intentional reversal of its previous "never upgradeable" rule, justified narratively: the Core is the machine rebuilding a path toward the long-dead SAM. Core upgrades go through the Dwarf's Upgrade Board, but the upgrade slot itself is milestone-locked until unlocked. Each Core upgrade level increases range, daily consumption, and capacity together, permanently (no way to depower a sector to save on drain).
8. **Research Station removed entirely** — no RP, no material knowledge tiers. Grinder now produces materials only.
9. **Materials are now flat** — no tiers, no type variants (Wood is just Wood).
10. **Recipes unlock via Data Sticks** — physical items, auto-consumed on acquisition, auto-convert to materials/coins if the recipe's already unlocked. Sourced from expedition finds, the Merchant (new dedicated pedestal, 10%/day), and request/talking visitors — each source drawing from its own curated pool (random but learnable, not a global grab-bag).
11. **Suits**: one shared, permanent, base-wide upgrade path via a new Suit Station in Workshop. Components: Battery (duration), Shoes (speed), Mask (breathing hazards), Suit Material (physical environmental hazards). Crafted at the Gear Workbench, installed permanently at the Suit Station.
12. **No fixed expedition time limit anymore** — the old "25 real minutes / 05:00 in-game" rule is gone. Suit Battery is the only thing that ends an expedition.
13. **Suits hard-gate travel** — insufficient Mask/Suit Material resistance blocks a sector from being selected at all, for both personal expeditions and away-team dispatch. Not a risk modifier — a wall.
14. **Graveyard needs a real queue** — since away-team dispatches can produce multiple deaths in one report, the previous single-pending-decision Accept/Decline flow is now a queue resolved one candidate at a time.

---

## Genuinely open / not yet decided (carry into next session)

- Exact milestone that unlocks the Core's upgrade slot
- Exact Core range/consumption/capacity numbers per level
- Full sector category list beyond N and L
- Exact away-team success % formula and loot scaling formula
- Exact Suit Station component values, and whether components have a soft upgrade cap
- Whether the camp tier system (from the old Map design) still applies to sector categories at all
- Exact Data Stick duplicate-conversion value (coins vs. materials, how much)
- Exact contents of the Merchant's curated Data Stick pool
- Whether the Suit Station requires Core power
- Exact penalty values for Exhaustion and Dead Man Walking under the new non-lockout model

None of these block moving forward with further design discussion or with the Unity prototype comparison — they're balancing/content details, not structural questions.

---

## Recommended next steps
1. Drop these files into your project folder, replacing the old versions (except `Room_Shop_PATCH_NOTES.md`, which is a manual edit to your existing Shop file, not a replacement).
2. Whenever you're ready, do a fresh pass on `Implementation_Status_Version5.md` to mark today's changed rooms as "needs re-review" rather than "stable" — Core, Workshop, Map, Recruit Quarters, Druid, Graveyard, Merchant, and Shop all had real content changes and shouldn't keep their old "Stable for implementation" checkmarks until reviewed again.
3. Bring the Unity prototype whenever you're ready — we can compare it against the *updated* docs directly, which should actually make that comparison more useful than doing it before this session, since several things it might already do differently (recruit counts, expedition timing) just became intentional design rather than things to "fix toward" the old docs.

Good session — sleep well.
