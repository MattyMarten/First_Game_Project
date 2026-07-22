# Project Status — Anchor Doc

**Paste this file into any new chat first, along with the specific stage files `Build_Roadmap.md` tells you to bring.** This alone should be enough for a fresh chat to understand where the project stands and what to do next.

---

## What this project is
An underground-base management game (Unity). Core loop: **Craft, Sell, Prepare, Loot.** The player runs a base that crafts goods, sells them in a shop, manages recruits, and sends expeditions into procedurally-instanced sector categories to gather loot, working toward eventually reactivating a long-dead machine called SAM. Full picture: `Base_Master_Plan.md`.

## How this project is being built
Room-by-room, in dependency order, against a set of design docs that are the source of truth. Existing code is only kept where it matches the current docs — see `Code_Audit_KeepChangeCut.md` for the full room-by-room verdict. The step-by-step order is `Build_Roadmap.md`; each stage there is meant to be one focused chat session.

## Where things actually stand (as of the last full code audit)

| Stage | Room/System | Code status |
|---|---|---|
| 0 | Global Day/Phase System | DONE — DayPhaseSystem + CoreRoomManager stub + DayCounter absorbed |
| 1 | Storage | DONE — CobaltCoinStorage + StorageSummaryUI added, materials/goods/utility covered |
| 2 | Core Room (power/upkeep) | DONE — CoreRoomManager (deposit, Normal/Warning/Offline, debug upgrade path) |
| 3 | Workshop (Grinder/Goods/Gear Workbench) | DONE — CraftingRecipe confirmed clean of tier fields, Data Stick unlock-flag system added (RecipeUnlockManager, DataStickItem, DataStickConsumer) |
| 3 | Gear Upgrade Station | DONE — GearUpgradeStationManager (LV1-3, mirrors Core's upgrade pattern), tier-chain fields added to UtilityCraftable, own UI/panel built |
| 4 | Recruit Quarters | DONE — capacity 4/6/8 by level (RecruitQuartersManager), unified roster capacity (RecruitRosterManager), retire action with appeal stub, Locker/Actor split (RecruitLocker persistent per-bed fixture, decoupled from RecruitQuartersActor) |
| 5 | Shop | REFACTOR — mid-refactor already (see `Assets/Plan/ShopRefactorPlan_Phase1_Version2.md`) |
| 6 | Merchant | REFACTOR (logic exists inside Shop, needs its own room) |
| 7 | Quest Board | REFACTOR (partial logic exists inside Shop as `RequestBoardManager`) |
| 7 | Info Room | REFACTOR (partial logic exists inside Shop as `DialogueInfoManager`) |
| 8 | Dwarf | MISSING entirely |
| 9 | Druid | MISSING entirely |
| 9 | Professor | MISSING entirely |
| 10 | Graveyard | MISSING entirely |
| 11 | Sector Map / unlock graph | REBUILD (Expedition exists but on old fixed-location model) |
| 11 | Suit Station | MISSING entirely |
| 11 | Dispatch Board | MISSING entirely |
| 12 | Save/load, full integration | not started |

**Current stage: Stage 5, Shop — Desk Two/Three merged (one Service Desk handles talking/request/recruit), Quest Board + Info Room extracted, Appeal + daily-report real. Remaining: Decor system, Dirt system, full Section 19 price pipeline. Merchant still physically in Shop/DeskTwo, deferred to Stage 6.**     // [update this line each session — e.g. "Stage 6, Merchant, in progress"]

## Design decisions locked in (do not re-litigate these without a real reason)
- No fixed expedition time limit — Suit Battery is the only limit on expedition duration.
- Recruit capacity: 4/6/8 across LV1/2/3, 2 beds per room. Base starts completely full.
- Recruit visitor spawn chance = free slots / total slots, 25% minimum floor.
- No debuff/status effect ever fully locks a recruit out of being controlled.
- Retiring a recruit: -3 appeal, no undead created.
- No material knowledge tiers, no Research Station, no RP. Materials are flat.
- Recipes unlock via Data Sticks (physical, auto-consumed, curated per source).
- Core is upgradeable (range/consumption/capacity, permanent, milestone-gated slot on the Dwarf's Upgrade Board) — an intentional reversal of the original "Core never upgrades" rule, justified by the SAM/Core narrative.
- Sectors (named categories, e.g. N/Nature, L/Labs) replace the old fixed Forsaken City location entirely. Categories generate a fresh random instance every visit.
- Away teams (Dispatch Board) resolve with independent per-recruit success/death rolls, separate from the player's own party (Recruit Selection).
- Graveyard's replacement-decision system is a queue, not a single pending state (needed because away teams can produce multiple deaths at once).
- Gear tiers are separate chained ScriptableObject assets (e.g. "Basic Axe" → "Basic Axe T2" → "Basic Axe T3"), linked via `nextTierItem` — not one asset with an internal level. Name stays conceptually the same; only the tier suffix changes. No stat differences between tiers yet (deferred until base items have real gameplay behavior).
- Data Stick unlock state keyed by ScriptableObject asset `name` (used as a stable-enough recipe ID for now — revisit if asset renaming ever becomes a real problem).
- Duplicate Data Stick acquisition pays out flat coins (default 10, placeholder — exact value still TBD per Room_Workshop.md Section 21).
- Gear Upgrade Station's own upgrade level (LV1-3) is a debug-button stand-in for the Dwarf's Upgrade Board (Stage 8), same pattern as Core Room's `TryUpgrade()` — not the same thing as an individual item's tier.
- Recruit capacity is a single total (4/6/8 by Recruit Quarters level), not split into Free/Paid pools. RecruitType (Free/Paid) still exists but now only affects hire cost/generation flavor in the Shop's hire desk, not capacity. HireDeskManager's capacity check was patched to match as a Stage 5 stopgap — Stage 5 should replace it properly with the free-slot-ratio recruit-visitor spawn formula (25% floor) from Room_Shop.md, not just leave the patch in place.
- Recruit Quarters' locker is a persistent per-bed fixture (RecruitLocker), separate from the transient RecruitQuartersActor. RecruitQuartersInteractable lives on the locker and looks up its current occupant live (RecruitQuartersManager.GetRecruitAtBedIndex) — never cache a direct reference to the actor across an interaction, since retiring/reassigning destroys it.

Full detail and reasoning for all of the above: `Session_Changelog_2026-07-19.md` and `Open_Architecture_Questions.md`.

## Files that matter, and what each is for
- `Base_Master_Plan.md` — whole-base source of truth, read this first for any question about how systems connect.
- `Base_Global_Systems.md` — cross-room systems (day/phase, save/load, economy, appeal).
- `Room_<Name>.md` files — one per room, each has its own Done Condition section defining "finished."
- `Open_Architecture_Questions.md` — anything genuinely still undecided; check here before assuming a gap is an oversight.
- `Code_Audit_KeepChangeCut.md` — what to keep/refactor/rebuild/cut in the existing repo, room by room.
- `Build_Roadmap.md` — the stage-by-stage build order; each stage lists exactly which files to bring into that chat.
- `Room_Naming_Conventions.md`, `Script_Architecture_Guidelines.md`, `Hierarchy_Guidelines.md` — engineering conventions, apply to all new code regardless of stage.
- `Known_Temporary_Systems.md` — log every debug/placeholder shortcut here as you add it.

## What NOT to assume
- Don't assume a room doc marked "Stable for implementation" in old `Implementation_Status_Version5.md` is still accurate if it's one of the rooms touched last session (Core, Workshop, Map, Recruit Quarters, Druid, Graveyard, Merchant, Shop) — those need a fresh status pass.
- Don't assume the repo's `Assets/Plan` folder (old planning docs) is current — it predates this rework. It's still useful for Shop specifically (the refactor plan there is good and still being followed), but nothing else in it should be treated as current design.
- Don't reintroduce research points, material tiers, or a fixed expedition timer — all three were deliberately removed.

## How to end a session
Before closing a chat, update:
1. The "Current stage" line above.
2. The code-status table above, if a stage's status changed.
3. Add anything newly decided to the locked-decisions list, or flag it as a new open question in `Open_Architecture_Questions.md`.
