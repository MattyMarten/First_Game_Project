# Implementation Status

This file tracks what is currently planned, implemented, temporary, broken, or under review.

Update this file regularly during development.

---

## 1. Project State Summary

### Current focus
- stabilizing all room and system documentation
- confirming room ownership and room-to-room flow
- preparing documentation for implementation use
- clarifying global systems vs room-owned systems
- syncing support/tracking files with the newer stronger room/system docs

### Current room being worked on
- none as a build target yet
- documentation review pass has now covered all rooms and all special NPCs

### Current phase of project
Mark current phase:
- [x] Planning
- [x] Architecture setup
- [ ] Room implementation
- [ ] Integration
- [x] Review
- [ ] Refactor
- [ ] UI / readability pass
- [ ] Polish

---

## 2. Master Documents Status

### Base / architecture docs
- [x] Base_Master_Plan.md exists
- [x] Room_Template.md exists
- [x] Base_Implementation_Checklist.md exists
- [x] Implementation_Status.md exists
- [x] Base_Global_Systems.md exists

### Support / architecture docs
- [x] Open_Architecture_Questions.md exists
- [x] Known_Temporary_Systems.md exists
- [x] Review_Checkpoints.md exists
- [x] Documentation_Plan.md exists
- [x] Room_Naming_Conventions.md exists
- [x] Hierarchy_Guidelines.md exists
- [x] Script_Architecture_Guidelines.md exists

### System docs
- [x] Notification_Display.md exists (NEW)

### Room docs
- [x] Room_Shop.md exists
- [x] Room_Workshop.md exists
- [x] Room_Storage.md exists
- [x] Room_RecruitQuarters.md exists
- [x] Room_Core.md exists
- [x] Room_QuestBoard.md exists
- [x] Room_Info.md exists
- [x] Room_Map.md exists
- [x] Room_RecruitMachine.md exists
- [x] Room_Graveyard.md exists
- [x] Room_Merchant.md exists
- [x] Room_Druid.md exists (renamed from Room_Healer.md)
- [x] Room_Dwarf.md exists
- [x] Room_Professor.md exists (NEW)

---

## 3. Room Progress Tracking

Use this section to track room-by-room progress.

## Shop Room
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Shop plan is strong and implementation-ready.
- Decor system updated: each decor effect is now a set of separate named decor pieces, not upgradeable tiers.
- Merchant progression gating applies to shop decor (weaker version must be bought before stronger appears).
- Current important known decisions:
  - Shop uses one shared daily visitor pool
  - buyers, request, talking, and recruit visitors all generated at shop opening
  - visitors spawn one by one over time
  - invalid spawns pushed to bottom and retried
  - sold goods add cobalt coins directly
  - unsold items remain on displays after closing
  - Shop Monitor shows total coins, appeal, sold count, visitor count, gained-today count
  - Sales Desk uses exactly 2 decision buttons
  - Service Desk handles request/talking/recruit interactions
  - appeal owned directly by the Shop
  - request visitors pull from quest list, accepted ones sent to Quest Board

---

## Workshop Room
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Workshop doc fully updated with raw design data absorbed.
- Finalized machine naming: Grinder, Research Station, Goods Workbench, Gear Workbench, Gear Upgrade Station.
- Research costs RP only (materials cost removed).
- Grinder levels: LV1 true amount, LV2 5% bonus chance, LV3 10% bonus chance.
- Gear Upgrade Station levels: LV1 to T2 normal cost, LV2 to T2 reduced cost, LV3 to T3 reduced cost.
- Full goods item list and gear item list now in the doc.
- Power: Research Station and Gear Upgrade Station need power; Grinder, Goods Workbench, Gear Workbench work manually; screens off during power loss.
- RP/knowledge truth belongs to research side.

---

## Storage Room
### Plan status
- [x] Reviewed
- [x] Stable for implementation

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Storage is one of the most stabilized room docs.
- Tiered gear rule settled: physical visuals do not change by tier, note shows existing tier lines only.
- Storage does not require power.
- Storage owns cobalt coin truth separate from Core loaded coins.

---

## Recruit Quarters
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Recruit management happens on Floor 1.
- Player plays as recruits, switches control by interacting.
- Finalized recruits spawn from correct pipe into correct room/bed area.
- Each recruit has a locker as main equipment/info interaction point.
- Debuff ownership stays here; Druid processes/treats debuffs.
- Hidden stats not shown directly.
- First trait random from general pool, later traits random from class pool.
- The four starting recruits are now fixed and defined in Room_Dwarf.md (Mara, Brok, Pip, Vael).

---

## Core Room
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Core internal loaded coin storage separate from Storage reserve.
- Max capacity 200, deposit 50, consumes 50 at start of Evening.
- Morning warning if upkeep not covered.
- Normal / Warning / Offline states.
- Unresolved warning can shut down base when closing Shop.
- Offline recovery uses 5-second restoration sequence.
- Appeal penalty -20 once per failed day.
- Research Station and Gear Upgrade Station require power.
- Special NPC rooms (Merchant, Druid, Dwarf, Professor) close during offline state.
- Core does not own day/time/phase state.
- Core state now also surfaced on the Notification Display as a permanent status line.

---

## Quest Board Room
### Plan status
- [x] Reviewed
- [x] Stable for implementation

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Quest Board now fully reviewed.
- Rank up at 250 XP from completed quests.
- Ranks E D C B A S, plus Q for main line.
- Appeal: accept +1, decline -2, complete +2, discard -3.
- Quest pool system: quest definitions (game data) vs daily available quests (room state).
- Quests pulled daily by rank and unlock state, each with appearance chance.
- All quests expedition-based; no base fetch quests.
- Rewards automatic on completion.
- Completed quest records sent to Info / Computer, no duplicates.
- Board interaction: pinboard, left normal / right main line, camera lock, WASD/mouse, select to activate, Tab/Esc to exit.
- Active limits: 2 normal, 1 main line.
- NEW: hosts the Expedition Results Display on Floor 2 (refreshes each morning after expedition).

---

## Info Room
### Plan status
- [x] Reviewed
- [x] Stable for implementation

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Final direction: computer with full-typing command line.
- Partial command matching supported.
- Core commands: help, documents, enemies, npc, quests, clear, exit.
- Open entry by typing its name.
- Enemy scan system: 3 slots per enemy (Lore / Abilities+Strengths / Weaknesses+Counters), each needs a deeper scan.
- Locked slots show nothing, no hint.
- Information read-only, no mechanical unlocks.
- Requires power.
- Technical note for building the command line included in the doc.

---

## Map Room
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Square map table, camera lock, WASD/mouse, Space/M1 select, Tab/Esc exit.
- World map -> select location -> map slides down, location map rises.
- First location Forsaken City (forest, city, caves with mountains).
- Entry text format: Location - Side - Camp State (e.g. Forsaken Forest - East - Base LV1).
- Undiscovered entries do not appear at all.
- Entries unlocked only during expeditions.
- Camp tiers cost entry count AND coins/materials, defined per location.
- Camp "check on other players" flagged multiplayer-only.
- Expedition launched at exit door, not the map table.
- Works during power loss.
- Multiplayer design note added: single player first, 1-4 player multiplayer is the end goal.

---

## Recruit Machine Room
### Plan status
- [x] Reviewed
- [ ] Stable for implementation

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Stronger now due to clearer Shop -> Recruit Machine -> Recruit Quarters flow.
- Recruit cannot be accepted if no valid recruit space.
- Accepted recruits may queue for the machine if needed.

---

## Graveyard
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Undead playable, usable even when living recruits exist.
- Undead keep name, color, class, level; do not level up.
- Undead use same equipment-management style as Recruit Quarters.
- Full-capacity replacement uses Graveyard computer Accept/Decline flow.
- Surfaces "New Undead Available" on the Notification Display.

---

## Merchant Room
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Now reviewed. NPC is the Rat Merchant — quiet, businesslike, found scavenging in the city.
- 13 pedestals: 3 utility, 1 case/belt, 2 accessory, 3 material, 4 decor.
- Material pedestals roll type and amount (5-10) independently each morning.
- Price variation -10% to +10% of base.
- E buys one, Y buys all on a pedestal (partial purchase if short on coins).
- Shop decor progression gating applies.
- Items roll from the craftable/findable pool, no rare one-time items.
- Closes during power loss.

---

## Druid Room (renamed from Healer Room)
### Plan status
- [x] Reviewed
- [ ] Stable for implementation  <!-- NEEDS RE-REVIEW: touched by the sector/suit/core-upgrade/recipe rework session (2026-07-19). See Session_Changelog_2026-07-19.md and the updated room doc before assuming this is still accurate. -->

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Healer renamed to Druid (natural apothecary, found in the forest).
- Sells medical items on 5 pedestals, daily random roll, no stacking.
- Items: Bandage, Medkit, Antidote, Energy Pills, Splint, Elixir, Valerian Drops, Tonic.
- Effects: Exhaustion, Poison, Injury, Dead Man Walking, Cursed, Broken Bone, Shell Shocked, Weakened.
- Items used on recruits in Recruit Quarters via numbered popup.
- Recruit Quarters still owns debuff state; Druid provides treatment items.
- Medkit also craftable at Gear Workbench.
- Closes during power loss.

---

## Dwarf Room
### Plan status
- [x] Reviewed
- [x] Stable for implementation

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- Major redefinition: Dwarf is now the base owner and central story NPC.
- Room open from start; Upgrade Board activates Day 5.
- All upgrades go through the Dwarf via the Upgrade Board.
- Board shows max 6 papers, random from current priority group (P1, then P2, then P3).
- In-progress papers stay on board with IN PROGRESS label and days remaining.
- Upgrade timing: select day X, starts X+1 morning, finishes X+2 morning. Shop/Grinder/Goods Workbench 1 day, everything else 2 days.
- Upgrades begin/finish on morning transitions; multiple at once but one per room/station.
- Work In Progress notice closes the room/station during upgrade.
- Recruited the four fixed starting recruits (Mara, Brok, Pip, Vael).
- Gives main line quests through conversation, cannot be rejected.
- Conversations use Service-Desk-style popup.
- Uses Notification Display to call the player.
- Closes during power loss, active upgrades pause.

---

## Professor Room (NEW)
### Plan status
- [x] Reviewed
- [x] Stable for implementation

### Build status
- [ ] Hierarchy started
- [ ] Logic started
- [ ] UI started
- [ ] First working version
- [ ] Needs refactor
- [ ] Stable

### Notes
- New special NPC: a droid scientist who wants to build himself a biobody.
- Found in the caves of Forsaken City.
- Grants Scanner recipe permanently on joining.
- Scanner progresses by recipe swaps (new recipe replaces old; old scanners become useless). Does NOT use Gear Upgrade Station.
- Scanner tier maps to Info Room enemy scan slots (T1 lore, T2 abilities, T3 weaknesses).
- Gives scan-milestone main line quests (e.g. have 5 monsters scanned), rewards coins/materials.
- Scanner recipe grants on a separate track gated by quest count and day (e.g. 2 quests done and day 15+).
- Uses Notification Display to call the player.
- Closes during power loss.
- Not connected to other special NPCs.

---

## 4. Global Systems Progress

### Day phase system
- [x] Planned

### Economy / currency flow
- [x] Planned

### Appeal system
- [x] Planned (owned by Shop; quest interactions also affect appeal)

### Expedition return flow
- [x] Planned

### Power / Core dependency flow
- [x] Planned

### Research / knowledge flow
- [x] Planned

### Save/load coordination
- [x] Planned

### Global progression / unlock flow
- [x] Planned

### Notification Display (NEW)
- [x] Planned

---

## 5. Current Temporary Systems
- none implemented yet in project status tracking
- documentation examples exist, but no active tracked in-project temporary systems yet
- fill this section when actual prototype/debug systems are added

---

## 6. Current Known Problems
- save/load ownership structure not deeply implemented yet
- exact balancing values across many systems still placeholder
- expedition-side systems (scanning, camps, entry unlocks) intentionally deferred to expedition phase

---

## 7. Current Open Architecture Questions
- should RP/knowledge remain under Workshop/Research Station ownership long-term, or split into a separate research system later?
- expedition result payload format still needs to be finalized before expedition integration

---

## 8. Latest Review Summary

### Date
- secondary room + special NPC review pass

### What was reviewed
- Workshop (raw data absorbed)
- Shop (decor rework)
- Quest Board (full review + Expedition Results Display)
- Druid (renamed from Healer, full rework)
- Merchant (Rat identity, full review)
- Dwarf (major redefinition as base owner + Upgrade Board)
- Notification Display (new system)
- Map (full review + camp system)
- Info (command line + enemy scan system)
- Professor (new special NPC)

### Main conclusions
- all rooms now have reviewed, implementation-ready docs
- three new systems created: Notification Display, Expedition Results Display, Upgrade Board
- two new/renamed rooms: Druid (was Healer), Professor (new)
- scanner progression now ties Professor, Info Room, and Gear Workbench into one loop
- Dwarf is now the narrative center of the game

### What needs to change next
- sync remaining support docs (checklist, review checkpoints, naming, open questions)
- begin GitHub project audit and build-order planning
- finalize expedition result payload format before expedition phase

---

## 9. Next Work Target

### Next step
- finish syncing remaining tracking/support docs
- then move to GitHub project audit and staged build plan

### Goal of next step
- get all documentation consistent with this session's decisions
- produce a clear staged implementation checklist for actual Unity work

### Expected outcome
- documentation fully aligned
- ready to start room-by-room implementation with a clear order

---

## 10. Milestone Checks

### Milestone 1 — planning foundation
- [x] Base docs created
- [x] first room docs created
- [x] ownership pass started
- [x] all room docs reviewed

### Milestone 2 — first core room implementation
- [ ] Storage first version works
- [ ] Shop first version works
- [ ] Workshop first version works

### Milestone 3 — core integration
- [ ] Storage <-> Shop connected
- [ ] Storage <-> Workshop connected
- [ ] Workshop <-> Shop connected

### Milestone 4 — recruit systems
- [ ] Recruit Quarters first version works
- [ ] recruit loadouts work
- [ ] expedition selection can read recruit data
- [ ] undead selection/control works

### Milestone 5 — Core dependency integration
- [ ] Core payment system works
- [ ] power dependency works
- [ ] penalties work

### Milestone 6 — pre-expedition base readiness
- [ ] day structure works
- [ ] base rooms communicate correctly
- [ ] expedition prep is supported

---

## 11. Notes

This file is for practical tracking, not final design truth.

Use it to track:
- what is real right now
- what is temporary
- what needs review
- what changed from plan
