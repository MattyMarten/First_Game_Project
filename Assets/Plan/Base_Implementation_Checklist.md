# Base Implementation Checklist

This checklist is for turning the base plan into a working in-project structure.
It should be updated over time as plans change.

---

## 1. Planning Foundation

- [x] Create `Base_Master_Plan.md`
- [x] Create `Room_Template.md`
- [x] Create first room plan documents
- [x] Review room names and confirm final "room" naming style
- [x] Confirm which planned areas are true rooms and which are global systems
- [x] Add open questions section to each room plan
- [x] Add temporary implementation notes to each room plan
- [x] Add done conditions to each room plan

---

## 2. Global Base Architecture

- [x] Define Morning / Day / Evening phase rules
- [x] Define what actions are allowed in each phase
- [x] Define how phase transitions happen
- [x] Define whether phases are manual or timed
- [x] Define what systems are global vs room-owned
- [x] Define shared resource categories
- [x] Define base-wide upgrade philosophy
- [x] Define Core power dependency rules
- [ ] Define expedition-to-base return payload format
- [x] Define base save-data ownership at high level

---

## 3. Data Ownership Pass

- [x] Confirm Storage owns physical inventory representation and stored item categories
- [x] Confirm Shop owns display layout and daily shop session state
- [x] Confirm Workshop owns local crafting / grinding / upgrade process state
- [x] Confirm Recruit Quarters owns recruit roster and assignments
- [x] Confirm Quest system ownership direction
- [x] Confirm Info system ownership direction
- [x] Confirm Core ownership direction
- [x] Decide final cobalt coin truth ownership at architecture level
- [x] Decide appeal ownership
- [x] Decide research / recipe / knowledge ownership
- [x] Record all current ownership decisions in docs

---

## 4. Room Planning Pass

### Core starting room docs
- [x] Shop Room plan ready
- [x] Workshop Room plan ready
- [x] Storage Room plan ready
- [x] Recruit Quarters plan ready
- [x] Core Room plan ready

### Secondary room docs
- [x] Quest Room / Quest Board plan ready
- [x] Info Room plan ready
- [x] Map / Expedition Planning Room plan ready
- [x] Recruit Machine Room plan ready
- [x] Graveyard plan ready
- [x] Merchant Room plan ready
- [x] Druid Room plan ready
- [x] Dwarf Room plan ready

### Review quality pass
- [x] Re-review Shop Room against current terminology and ownership decisions
- [x] Re-review Workshop Room against current terminology and ownership decisions
- [x] Re-review Storage Room against current terminology and ownership decisions
- [x] Re-review Recruit Quarters against current terminology and ownership decisions
- [x] Re-review Core Room against current terminology and ownership decisions
- [ ] Re-review Quest Board Room against current terminology and ownership decisions
- [ ] Re-review Info Room against current terminology and ownership decisions
- [ ] Re-review Map / Expedition Planning Room against current terminology and ownership decisions
- [x] Re-review Recruit Machine Room against current terminology and ownership decisions
- [x] Re-review Graveyard against current terminology and ownership decisions
- [ ] Re-review Merchant Room against current terminology and ownership decisions
- [ ] Re-review Druid Room against current terminology and ownership decisions
- [ ] Re-review Dwarf Room against current terminology and ownership decisions

---

## 5. Project Structure Pass

- [x] Review documentation file structure
- [ ] Review Unity folder structure
- [ ] Decide room script folder naming
- [ ] Decide shared systems folder naming
- [ ] Decide data / ScriptableObject folder naming
- [ ] Decide UI folder naming
- [ ] Decide prototype / temp folder rules
- [ ] Clean obviously outdated scene names
- [ ] Mark temporary scenes clearly
- [x] Add implementation status tracking document if needed

---

## 6. Base Scene / Hierarchy Pass

- [x] Define root base scene hierarchy direction
- [x] Define where global managers live
- [x] Define where room roots live
- [x] Define where shared interaction objects live
- [x] Define where debug / testing objects live
- [x] Define where room UI objects live
- [x] Define room naming consistency in scene
- [x] Define temporary placeholder object naming rules

---

## 7. Global Script Architecture Pass

- [ ] Decide whether there is a BaseManager
- [ ] Decide whether there is a PhaseManager / DayCycleManager
- [ ] Decide how rooms communicate
- [ ] Decide how shared events/signals are sent
- [x] Decide how global save data is structured
- [ ] Decide where item definitions live
- [ ] Decide where recruit class / trait definitions live
- [ ] Decide where quest definitions live

---

## 8. Storage Room Implementation

### Planning
- [x] Confirm stored categories
- [x] Confirm item ownership rules
- [x] Confirm assigned-vs-free item rules
- [x] Confirm tiered gear note/readout rule
- [x] Confirm Storage does not require power
- [x] Confirm shop decor terminology for Storage usage

### Scene / hierarchy
- [ ] Create Storage room root
- [ ] Create storage interaction points
- [ ] Create storage display / shelf placeholders
- [ ] Create storage UI placeholders

### Scripts / logic
- [ ] Create Storage room manager
- [ ] Create storage inventory data structure
- [ ] Create add item flow
- [ ] Create remove item flow
- [ ] Create availability check flow
- [ ] Create assigned item exclusion flow
- [ ] Create tiered gear note/readout data support

### Readability
- [ ] Show important item counts
- [ ] Show item categories clearly
- [ ] Show invalid transfer feedback
- [ ] Show tier-specific text breakdown for tiered gear
- [ ] Show shop decor separately from sellable goods where needed

### Review
- [ ] Check ownership conflicts
- [ ] Check Workshop / Shop / Recruit integration assumptions

---

## 9. Shop Room Implementation

### Planning
- [x] Confirm customer categories
- [x] Confirm shared daily visitor pool
- [x] Confirm display types
- [x] Confirm dirt logic
- [x] Confirm decor rules at reviewed level
- [x] Confirm shop open / close rules
- [x] Confirm direct cobalt coin income from sales
- [x] Confirm Shop Monitor
- [x] Confirm Sales Desk / Service Desk split
- [x] Confirm display persistence after closing
- [x] Confirm recruit intake starts from Service Desk flow

### Scene / hierarchy
- [ ] Create Shop room root
- [ ] Create display parent objects
- [ ] Create display carpet/grid placeholder
- [ ] Create customer entry / exit placeholders
- [ ] Create Sales Desk
- [ ] Create Service Desk
- [ ] Create recruit machine placement in Shop
- [ ] Create shop decor slot placeholders
- [ ] Create dirt spawn placeholders
- [ ] Create Shop Monitor placeholder
- [ ] Create shop UI placeholders

### Scripts / logic
- [ ] Create Shop room manager
- [ ] Create full daily visitor pool generation
- [ ] Create visitor spawn interval logic
- [ ] Create invalid spawn fallback logic
- [ ] Create display assignment system
- [ ] Create display value modifier logic
- [ ] Create dirt spawn and cleaning logic
- [ ] Create sale result logic
- [ ] Create haggle discount logic
- [ ] Create request / talk / recruit visitor logic
- [ ] Create Sales Desk flow
- [ ] Create Service Desk flow
- [ ] Create shop phase restrictions
- [ ] Create shop decor placement logic
- [ ] Create Shop Monitor data feed

### Readability
- [ ] Show shop open / closed state
- [ ] Show current display contents
- [ ] Show dirt penalty
- [ ] Show visitor progress for the day
- [ ] Show sale result feedback
- [ ] Show appeal range / bonus state
- [ ] Show Shop Monitor values clearly

### Review
- [ ] Check Storage integration
- [ ] Check economy / cobalt coin integration
- [ ] Check appeal ownership assumptions
- [ ] Check Quest/Info/Recruit handoff assumptions
- [ ] Check ownership conflicts

---

## 10. Workshop Room Implementation

### Planning
- [x] Confirm Workshop is one room with multiple stations/machines
- [x] Confirm final recipe ownership
- [x] Confirm final research ownership
- [x] Confirm output routing rules at room-plan level
- [x] Confirm final Workshop machine naming
- [x] Confirm Gear Upgrade Station level structure
- [x] Confirm Goods/Gear visibility state matching
- [x] Confirm Workshop resource display rule

### Scene / hierarchy
- [ ] Create Workshop room root
- [ ] Create Goods Workbench area
- [ ] Create Gear Workbench area
- [ ] Create Grinder area
- [ ] Create Research Station area
- [ ] Create Gear Upgrade Station area
- [ ] Create Workshop resource display placeholder
- [ ] Create Workshop UI placeholders

### Scripts / logic
- [ ] Create Workshop room manager or station managers
- [ ] Create recipe check logic
- [ ] Create material consumption logic
- [ ] Create goods crafting logic
- [ ] Create gear crafting logic
- [ ] Create grinder conversion logic
- [ ] Create research timer logic
- [ ] Create knowledge unlock logic
- [ ] Create gear upgrade logic
- [ ] Create Workshop resource display data feed

### Readability
- [ ] Show recipe requirements
- [ ] Show available materials
- [ ] Show available RP
- [ ] Show available cobalt coin amount if included in room display
- [ ] Show running process state
- [ ] Show unlock / locked reasons
- [ ] Show output result feedback

### Review
- [ ] Check Storage integration
- [ ] Check recruit equipment integration
- [ ] Check RP / knowledge ownership assumptions
- [ ] Check ownership conflicts

---

## 11. Recruit Systems Implementation

- [x] Create Recruit Machine room plan
- [x] Create Recruit Quarters plan
- [x] Confirm recruit ownership rules in reviewed state
- [x] Confirm recruit equipment assignment rules in reviewed state
- [ ] Confirm class / trait data structure
- [ ] Confirm level progression rules
- [x] Confirm undead / Graveyard interaction rules
- [x] Confirm recruit debuff ownership between Recruit Quarters and Druid
- [x] Confirm roster-full replacement flow in reviewed state
- [x] Confirm playable recruit / undead control transfer direction
- [x] Confirm Floor 1 recruit management vs Floor 2 expedition recruit selection split
- [x] Confirm only one recruit management board remains on Floor 1

---

## 12. Core Systems Implementation

- [x] Create Core Room plan
- [x] Confirm Core daily payment rules at draft level
- [x] Confirm Core power dependency list
- [x] Confirm what happens when Core is unpaid at draft level
- [x] Confirm final cobalt coin truth ownership at architecture level
- [x] Confirm appeal penalty behavior at draft level

---

## 13. Expedition Integration Planning

- [ ] Confirm expedition result payload structure
- [x] Confirm where case/material return goes
- [x] Confirm where info returns
- [x] Confirm where quest progress returns
- [x] Confirm where injuries / debuffs return
- [x] Confirm where recruit death is processed
- [x] Confirm where loadout selection happens
- [ ] Confirm map entry selection ownership
- [ ] Confirm terminology consistency for cases across expedition-related systems
- [x] Confirm first-pass expedition scope stays simple

---

## 14. UI / Readability Pass

- [ ] Define minimum functional UI for each core room
- [ ] Define what must be shown in-world vs in menus
- [ ] Define debug UI vs normal player UI
- [ ] Define room status display rules
- [ ] Define interaction highlight style
- [ ] Define placeholder art style rules
- [ ] Define what polish is intentionally delayed
- [x] Define room attention sign direction where needed

---

## 15. Animation / Feedback Pass

- [ ] Define minimum interaction feedback for each room
- [ ] Define busy / idle state visuals for machines
- [ ] Define item placement feedback
- [ ] Define cleaning feedback
- [ ] Define customer arrival / sale feedback
- [ ] Define recruit assignment feedback
- [ ] Define error / missing requirement feedback
- [x] Define shop visitor animation set
- [x] Define queue/wait behavior for Shop NPCs
- [ ] Define recruit control transfer feedback
- [ ] Define undead tube control / replacement feedback

---

## 16. Review Checkpoints

### Small reviews
- [ ] Review after each room plan is written
- [ ] Review after each room hierarchy is created
- [ ] Review after each room logic first works
- [ ] Review after each room gets first readable UI

### Big project checks
- [x] Big check after core room plans are complete
- [ ] Big check after Storage + Shop + Workshop all connect
- [ ] Big check after recruit systems are added
- [ ] Big check before expedition integration
- [ ] Big check after expedition integration
- [ ] Big check before polish phase

For big checks, review:
- ownership conflicts
- naming consistency
- hierarchy clarity
- temporary systems still in use
- duplicated logic
- unclear UI
- refactor needs

---

## 17. Temporary Systems Tracking

- [x] Create `Known_Temporary_Systems.md`
- [ ] Mark every temporary manager
- [ ] Mark every debug-only flow
- [ ] Mark every placeholder UI
- [ ] Mark every fake data path
- [ ] Mark every temporary scene object
- [ ] Review temporary systems regularly
- [ ] Decide when each temporary system must be replaced

---

## 18. Implementation Status Tracking

Support files currently available:
- [x] `Implementation_Status.md`
- [x] `Known_Temporary_Systems.md`
- [x] `Open_Architecture_Questions.md`
- [x] `Review_Checkpoints.md`

---

## 19. Notes

This checklist should not be treated as fixed forever.
As the project changes:
- update the room docs
- update ownership rules
- update implementation order
- mark completed steps clearly
- mark changed assumptions clearly