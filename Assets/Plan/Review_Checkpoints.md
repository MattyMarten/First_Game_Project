# Review Checkpoints

This file defines when to stop building and review the project structure, room logic, ownership, and implementation direction.

Use this to avoid building too far in the wrong direction.

---

## 1. Purpose

Review checkpoints exist to:
- catch ownership mistakes early
- catch duplicated systems early
- keep room docs aligned with implementation
- prevent temporary systems from becoming permanent by accident
- catch terminology drift before it spreads
- make refactors smaller and easier

There are two checkpoint types:
- small reviews
- big project checks

---

## 2. Small Reviews

Small reviews happen often and focus on the current room or current implementation step.

### Run a small review:
- after writing a room plan
- after revising a room plan in a major way
- after cleaning up terminology across related docs
- after creating a room hierarchy
- after first room logic works
- after first room UI/readability pass
- after adding a temporary system
- after changing ownership of important data

### Small review checklist
- [ ] Does the room still match its room plan?
- [ ] Does the room own only what it should own?
- [ ] Is anything duplicated with another room or system?
- [ ] Are temporary systems marked clearly?
- [ ] Is the room understandable to use?
- [ ] Does the room still fit the Base Master Plan?
- [ ] Did a new open architecture question appear?
- [ ] Did terminology drift from current standards?
- [ ] Are recently resolved decisions reflected in the room doc?

### Current terminology standards to watch
Use this list during small reviews:
- cases, not backpacks
- shop decor for placeable decor
- decor for sellable goods category only
- finalized Workshop machine names:
  - Grinder
  - Research Station
  - Goods Workbench
  - Gear Workbench
  - Gear Upgrade Station
- Sales Desk and Service Desk for Shop desk naming

---

## 3. Big Project Checks

Big project checks happen at milestone points and review the structure of the whole project.

### Big Check 1 — After core planning docs are written
#### Trigger
- Base Master Plan exists
- first core room docs exist

#### Review
- [ ] Are the room names stable enough?
- [ ] Are global systems separated from room systems?
- [ ] Are ownership directions clear enough to start building?
- [ ] Are major open questions written down?
- [ ] Does implementation order still make sense?
- [ ] Are major terminology decisions already drifting across docs?

---

### Big Check 2 — After major terminology and ownership cleanup
#### Trigger
- core room docs exist
- terminology cleanup has been done across major docs
- ownership direction is much clearer than before

#### Review
- [ ] Do Base Master Plan, Storage, Workshop, and Shop agree with each other?
- [ ] Are resolved terms standardized across the current major docs?
- [ ] Are resolved decisions moved into Open Architecture Questions history where appropriate?
- [ ] Are remaining open questions still clearly open instead of half-assumed?
- [ ] Is Implementation Status updated to match the actual document state?
- [ ] Is Base Implementation Checklist updated to match the actual document state?

---

### Big Check 3 — After Storage + Shop + Workshop are all connected
#### Trigger
- Storage first version works
- Shop first version works
- Workshop first version works
- basic room-to-room flow exists

#### Review
- [ ] Does Storage clearly own physical inventory representation and stored categories?
- [ ] Does Shop only own shop-local state?
- [ ] Does Workshop only own workshop-local process state?
- [ ] Are outputs routed correctly between rooms?
- [ ] Are fake/debug paths still doing too much?
- [ ] Is UI readable enough to understand the flow?
- [ ] Are shop decor and sellable goods kept distinct?
- [ ] Does direct cobalt coin generation from Shop sales route correctly?

---

### Big Check 4 — After recruit systems are connected
#### Trigger
- Recruit Machine first version works
- Recruit Quarters first version works
- recruit intake / assignment flow exists

#### Review
- [ ] Is recruit ownership clear?
- [ ] Is assigned gear correctly removed from free storage use?
- [ ] Is recruit intake separated from recruit long-term ownership?
- [ ] Are recruit-related open questions still acceptable?
- [ ] Is room capacity handled clearly?
- [ ] Are recruit debuffs owned and processed consistently between Recruit Quarters and Druid?
- [ ] Does roster-full / no-space flow stay in the correct room/system boundaries?
- [ ] Does playable recruit control transfer work clearly?
- [ ] Does undead control/replacement flow stay in the correct room/system boundaries?
- [ ] Is Floor 1 recruit management vs Floor 2 recruit selection still clear?

---

### Big Check 5 — Before expedition integration
#### Trigger
- major base rooms work individually
- room ownership mostly stable
- base logic mostly readable

#### Review
- [ ] Is expedition return payload format defined?
- [ ] Does every return type have a target room/system?
- [ ] Are room inputs ready to receive expedition results?
- [ ] Are temporary substitutes for expedition clearly marked?
- [ ] Are unresolved architecture questions acceptable?
- [ ] Is case terminology consistent across expedition-related systems?

---

### Big Check 6 — After expedition integration
#### Trigger
- expedition results enter the base
- reward flow is connected to rooms

#### Review
- [ ] Does case/material return go to the right place?
- [ ] Does info go to the right place?
- [ ] Does quest progress go to the right place?
- [ ] Do recruit injuries/deaths go to the right place?
- [ ] Does the base still have clean ownership?
- [ ] Did expedition integration create duplicated state?

---

### Big Check 7 — Before polish phase
#### Trigger
- core base loop works at a basic level
- rooms communicate correctly
- project is preparing for stronger visual/UI work

#### Review
- [ ] Are major temporary systems replaced?
- [ ] Are room plans updated to match reality?
- [ ] Is architecture stable enough for polish?
- [ ] Is UI structure stable enough for better presentation?
- [ ] Is hierarchy clean enough for longer-term scaling?
- [ ] Are there major refactors that should happen before polish?

---

## 4. Full Project Review Areas

During any big project check, review these areas:

### Ownership review
- who owns inventory?
- who owns cobalt coin truth?
- who owns appeal?
- who owns recruit data?
- who owns quest data?
- who owns power state?
- who owns knowledge / research state?

### Hierarchy review
- are room roots clear?
- are global managers separated from rooms?
- are debug objects isolated?
- are temporary objects easy to identify?

### Script architecture review
- are scripts too large?
- are managers owning too much?
- are shared systems placed correctly?
- are room boundaries still clear?

### UI / readability review
- can the player understand the room state?
- are important numbers visible?
- are interactions readable?
- are hidden systems causing confusion?

### Temporary systems review
- are all temporary systems documented?
- are any temporary systems still owning final data?
- should any temporary systems be replaced now?

### Terminology review
- are cases used consistently?
- is shop decor kept distinct from sellable decor goods?
- are finalized Workshop machine names used consistently?
- are resolved naming decisions reflected in docs and implementation notes?
- are Sales Desk and Service Desk used consistently?

---

## 5. Review Output Template

Use this after a review:

## Review Date
- 

## Review Type
- Small Review
- Big Project Check

## What was reviewed
- 

## What is working well
- 

## Problems found
- 

## Architecture concerns
- 

## Terminology concerns
- 

## Temporary systems that need attention
- 

## Changes needed next
- 

## Safe to continue?
- [ ] Yes
- [ ] Yes, with caution
- [ ] No, fix structure first

---

## 6. Recommended Review Habit

Suggested workflow:
- small review after every meaningful room step
- small review after every major terminology/ownership cleanup
- big project check after every milestone
- update `Implementation_Status.md` after each big review
- update `Known_Temporary_Systems.md` if new shortcuts were added
- update `Open_Architecture_Questions.md` if a new important question appeared
- update room docs if resolved decisions changed their assumptions

---

## 7. Current Best Next Review Targets

Based on the current documentation state, the next strongest review targets are:

1. `Room_QuestBoard.md`
2. `Room_Druid.md`
3. `Room_Map.md`
4. `Room_Merchant.md`

Reason:
- Storage, Workshop, Core, Shop, Recruit Quarters, and Graveyard are already more stable than the others
- Quest Board should be reviewed against request visitor flow
- Druid should be reviewed against recruit debuff ownership
- Map still needs terminology cleanup and expedition-structure alignment
- Merchant still needs terminology/economy consistency review

---

## 8. Notes

A review is not a sign that progress stopped.

A review is part of progress.

For a systems-heavy project, reviews prevent:
- ownership drift
- hidden duplication
- messy refactors later
- terminology drift
- confusion about what the current architecture really is