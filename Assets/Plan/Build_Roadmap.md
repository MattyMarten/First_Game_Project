# Build Roadmap

This is the step-by-step build order for the whole project. Each stage below is designed to be **one future chat session**. When you start a new chat for a stage, paste `Project_Status.md` plus the specific files listed under that stage's "Bring into the chat" — nothing more is needed for that session to pick up correctly.

Stages are ordered by real dependency (what needs to exist before the next thing can be built on top of it), not by doc order. This reflects the actual state of your repo as of the code audit — some stages are "finish what exists," others are "build from nothing."

---

## Stage 0 — Global Systems Foundation
**Why first:** every room reacts to Morning/Day/Evening/Night, and right now that's a single disconnected `DayCounter.currentDay` int. Nothing else should be built further until a real phase system exists to hook into.

**Status:** MISSING (stub only)

**Goal:**
- A real Day/Phase manager: Morning → Day → Evening → Night → next Morning, with the correct manual-transition rules (open Shop starts Day, close Shop starts Evening, launch expedition or confirm dispatch-only starts Night).
- An event/signal system other rooms can subscribe to ("phase changed to X") rather than polling.
- A minimal Core Room stub — doesn't need real power/upkeep logic yet, just enough of a presence that other systems can ask "is the Core online" and get `true` for now.

**Bring into the chat:** `Room_Shop.md` (single consolidated doc — supersedes the old Version7/PATCH_NOTES/RefactorPlan/OwnershipMap split), `Code_Audit_KeepChangeCut.md` (Shop section), this roadmap.

**Done when:** a test scene can manually step through all four phases, other scripts can subscribe to phase-change events, and `DayCounter.cs` is either replaced or absorbed into the new system.

---

## Stage 1 — Storage
**Why here:** already the closest to finished room in the repo; a fast, confidence-building first real "done" room.

**Status:** KEEP / near-final (see Code_Audit_KeepChangeCut.md)

**Goal:** confirm `RawMaterialStorage` + `MaterialStorageUI` + `StorageInteractable` cover every category `Room_Storage.md` requires (materials, cobalt coins, goods, shop decor, belts/cases/utility/accessories not currently assigned) — add what's missing, don't rebuild what exists.

**Bring into the chat:** `Room_Storage_Version4_Version2.md`, `Code_Audit_KeepChangeCut.md` (Storage section), this roadmap.

**Done when:** matches `Room_Storage.md` Section 18 (Done Condition) in full.

---

## Stage 2 — Core Room
**Why here:** small, self-contained, and now has real narrative + mechanical weight (upgradeable, gates sector range). Building it early means later stages (Sector Map, Dispatch Board) have something real to check range against instead of a stub.

**Status:** MISSING entirely

**Goal:** cobalt coin upkeep, deposit interaction, Normal/Warning/Offline states, and the new upgrade system (range/consumption/capacity, milestone-locked slot on the Upgrade Board). The Upgrade Board itself belongs to the Dwarf (Stage 8) — for now, just make Core's upgrade level a value that *can* be changed by a debug button, so later stages have something to hook the real board into.

**Bring into the chat:** `Room_Core.md` (the updated one from last session), `Base_Master_Plan.md` Section 1a and 7, this roadmap.

**Done when:** matches `Room_Core.md` Section 18 (Done Condition).

---

## Stage 3 — Workshop (Grinder, Goods Workbench, Gear Workbench, Gear Upgrade Station)
**Why here:** feeds Shop (goods) and Recruit Quarters (gear) — needs to exist before either of those can be fully wired up for real item flow.

**Status:** REFACTOR (Grinder/Goods/Gear Workbench exist and are solid) + MISSING (Gear Upgrade Station, Suit Station)

**Goal:**
- Confirm `CraftingRecipe` has no leftover tier/knowledge fields (check directly — wasn't fully read during the audit).
- Build the Gear Upgrade Station from scratch (T1→T2→T3, doesn't exist yet).
- Add the Data Stick unlock-flag system (simple: a recipe is locked/unlocked, no research spend).
- Suit Station can wait until Stage 3b below, since it depends on sector hazard design existing first — don't block this stage on it.

**Bring into the chat:** `Room_Workshop.md` (updated), `Code_Audit_KeepChangeCut.md` (Workshop section), this roadmap.

**Done when:** matches `Room_Workshop.md` Section 20 (Done Condition), excluding the Suit Station (deferred to Stage 6b).

---

## Stage 4 — Recruit Quarters
**Why here:** Shop's hire flow already hands off into this room's pending-reservation system — finishing Recruit Quarters properly unblocks finishing Shop's Desk Three cleanly.

**Status:** REFACTOR — solid foundation, needs updates

**Goal:**
- Enforce 2-beds-per-room, 4/6/8 capacity by level (currently generic/configurable, needs to actually match).
- Add the retire action (locker-side, -3 appeal call — appeal doesn't fully exist yet either, see Stage 5, so this can call a stub for now).
- Move `RecruitData`/`RecruitClass`/`RecruitStats`/`RecruitType` out of `Shop/DeskThree/` into a shared location — do this at the start of this stage, since Recruit Quarters needs to reference them constantly.
- Check whatever status-effect handling exists (or build it if it doesn't) to confirm no debuff ever disables control — this is a hard rule now.

**Bring into the chat:** `Room_RecruitQuarters.md` (updated), `Room_Druid.md` (updated, for the debuff list), `Code_Audit_KeepChangeCut.md` (Recruit Quarters section), this roadmap.

**Done when:** matches `Room_RecruitQuarters.md` Section 22 (Done Condition).

---

## Stage 5 — Shop (finish the refactor you already started)
**Why here:** most-built room already, and everything it hands off to (Storage, Workshop, Recruit Quarters) is now finished ahead of it.

**Status:** REFACTOR — pick up `ShopRefactorPlan_Phase1_Version2.md` where you left off

**Goal:**
- Fill in `ShopCoreManager`'s Appeal, decor, and daily-report logic (currently scoped but not implemented per its own code comments).
- Add the new recruit-visitor spawn formula (free-slot ratio from Recruit Quarters, 25% floor) to Desk Three's spawn logic.
- **Extract Merchant out of Desk Two** into its own room (see Stage 6) — remove `merchants/` scripts from Shop once the new room exists, don't duplicate them.
- **Extract Quest Board and Info Room** out of Desk Two (`RequestBoardManager` → Quest Board, `DialogueInfoManager` → Info Room) into their own rooms (see Stage 7) — same rule, move don't duplicate.
- Everything else (Desk One buying flow, displays, dirt, decor placement) should mostly just need Appeal/Core wiring, not rebuilding.

**Bring into the chat:** `Room_Shop_Version7_Version3_Version3.md` + `Room_Shop_PATCH_NOTES.md`, your existing `Assets/Plan/ShopRefactorPlan_Phase1_Version2.md` and `ShopOwnershipMap_Version2.md`, `Code_Audit_KeepChangeCut.md` (Shop section), this roadmap.

**Done when:** matches `Room_Shop.md` Section 31 (Done Condition), with Merchant/Quest Board/Info Room genuinely living in their own rooms rather than inside Shop.

---

## Stage 6 — Merchant (extracted into its own room)
**Status:** REFACTOR (real logic exists in `merchants/`, needs a new home) + MISSING (Data Stick pedestal)

**Goal:** move the existing merchant offer/stock logic (`GeneratedMerchantMaterialItem`, `GeneratedMerchantUtilityItem`, `GeneratedMerchantVisit`, `MerchantDayManager`, `MerchantProfileData`) into its own room structure (14 pedestals per the doc), add the Data Stick pedestal (10%/day).

**Bring into the chat:** `Room_Merchant.md` (updated), `Code_Audit_KeepChangeCut.md` (Shop section, Merchant subsection), this roadmap.

**Done when:** matches `Room_Merchant.md` Section 22 (Done Condition).

---

## Stage 7 — Quest Board + Info Room (extracted into their own rooms)
**Status:** REFACTOR (partial logic exists as `RequestBoardManager` + `DialogueInfoManager`) + a fair amount MISSING (rank system, main-line quest slots, Expedition Results Display, terminal command-line UI)

**Goal:** split the two concerns cleanly, build out rank/XP progression and the Expedition Results Display for Quest Board, build the command-line terminal for Info Room.

**Bring into the chat:** `Room_QuestBoard.md`, `Room_Info_Version2.md`, `Code_Audit_KeepChangeCut.md` (Shop section), this roadmap.

**Done when:** matches each doc's own Done Condition section.

---

## Stage 8 — Dwarf (base owner, Upgrade Board)
**Status:** MISSING entirely

**Goal:** the Dwarf NPC, conversation popup, and the Upgrade Board (priority groups, paper display, timing rules) — this is also where Core's upgrade slot finally gets wired to a real board instead of Stage 2's debug button.

**Bring into the chat:** `Room_Dwarf_Version2.md`, `Room_Core.md` (updated, Section 9), this roadmap.

**Done when:** matches `Room_Dwarf.md` Section 19 (Done Condition).

---

## Stage 9 — Druid + Professor
**Status:** MISSING entirely

**Goal:** Druid's pedestal/stock/debuff-treatment flow (with the updated non-lockout debuff list), Professor's scanner recipe progression and quest milestones.

**Bring into the chat:** `Room_Druid.md` (updated), `Room_Professor_Version1.md`, this roadmap.

**Done when:** matches each doc's Done Condition.

---

## Stage 10 — Graveyard
**Status:** MISSING entirely

**Goal:** undead tubes, the Accept/Decline replacement flow — build the **queue version directly**, since the single-pending-decision version was already superseded before any code existed for it.

**Bring into the chat:** `Room_Graveyard.md` (updated, queue version), this roadmap.

**Done when:** matches `Room_Graveyard.md` Section 22 (Done Condition).

---

## Stage 11 — Sectors, Sector Map, Suit Station, Dispatch Board (the big new-system stage)
**Why last among "core" stages:** this is the largest rework in the whole project (rebuilding Expedition's fixed-location data model into sector categories) and depends on Workshop (Stage 3, for Suit Station's craftable components) and Core (Stage 2, for range) already existing.

**Status:** REBUILD (Expedition core loop exists but on the wrong data model) + MISSING (Sector Map's category/instance system, Suit Station, Dispatch Board entirely)

**Goal, roughly in this order:**
1. Redesign `ExpeditionDestinationData`/`ExpeditionEntryPointData` around sector categories (category ID, hazard rating, instance generation) instead of fixed scene names.
2. Build the Suit Station in Workshop (Battery/Shoes/Mask/Suit Material, permanent shared upgrades).
3. Wire Core range + Suit hazard resistance as selection gates on the Sector Map.
4. Split personal-party selection (existing `ExpeditionRecruitListItem`/`ExpeditionSelectedCrewItem`, refactored into their own Recruit Selection station) from the brand-new Dispatch Board (away-team roster + destination + independent per-recruit resolution).
5. Remove the old fixed 25-minute/05:00 expedition timer if it exists in `ExpeditionManager` or `ExpeditionSessionData` — replace with Battery depletion as the only limit.

**Bring into the chat:** `Base_Master_Plan.md` Sections 5-7, `Room_Map.md` (updated), `Dispatch_Board.md` (new), `Recruit_Selection_System.md`, `Room_Workshop.md` Section 13 (Suit Station), `Code_Audit_KeepChangeCut.md` (Expedition section), this roadmap. This stage will likely need to be split across 2-3 chat sessions given its size — that's fine, split it by the numbered sub-goals above.

**Done when:** matches the Done Conditions of `Room_Map.md`, `Dispatch_Board.md`, and the Suit Station section of `Room_Workshop.md`.

---

## Stage 12 — Polish / Save-Load / Cross-Room Integration Pass
**Status:** not started, intentionally last

**Goal:** real save/load coordination, a full playthrough of the core loop (Craft, Sell, Prepare, Loot) across all rooms together, and a genuine review pass using `Review_Checkpoints.md`'s Big Project Check list.

**Bring into the chat:** `Review_Checkpoints.md`, `Base_Implementation_Checklist.md`, this roadmap.

---

## Notes on using this roadmap
- Stages are dependency-ordered, not difficulty-ordered — some "later" stages (Druid, Professor) are actually simpler than "earlier" ones (Shop).
- If you finish a stage faster or slower than expected, that's fine — just update `Project_Status.md` before ending the session so the next chat knows the real state.
- If a new design idea comes up mid-stage that changes a doc, finish the current stage's *code* against the doc as it stood at the start of the session where possible, then handle the doc update as its own small task — don't let a doc change mid-build cause a half-finished stage.
