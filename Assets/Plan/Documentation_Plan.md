# Documentation Plan

This file defines the plan for creating, reviewing, and improving the project documentation.

The goal is to build a documentation set strong enough to support a fully working systems-driven game version, even before final polish.

This is **not** the game design itself.
This is the plan for **how the documents should be built**.

---

## 1. Purpose

The documentation set should help:
- define the base and all major systems clearly
- define room ownership clearly
- define resource flow clearly
- define room-to-room connections clearly
- define global systems separately from room-local systems
- define save/load ownership clearly
- support implementation without guessing core architecture later
- reduce major refactors caused by unclear ownership

The end goal is to have documents good enough that the project can be built room by room and system by system into a working systems game.

---

## 2. Documentation Philosophy

The documentation should be built in layers.

### Layer 1 — Whole base understanding
These files define the entire base and the major connected systems.

### Layer 2 — Global systems understanding
These files define systems that are not owned by one room.

### Layer 3 — Room understanding
These files define how each room works inside the base.

### Layer 4 — Implementation guidance
These files define checklists, status, review points, temporary systems, and architecture questions.

### Layer 5 — Technical guidance
These files define naming, hierarchy, and script architecture rules.

The documents should move from:
- broad
- to structural
- to specific
- to implementable

---

## 3. Document Creation Order

Documents should be created and refined in this order:

### Step 1 — Base-level foundation
Create and refine:
- `Base_Master_Plan.md`
- `Open_Architecture_Questions.md`

Purpose:
- define the whole base
- define major global systems
- define resource categories
- define ownership direction
- define what is still undecided

---

### Step 2 — Global systems docs
Create and refine:
- a global systems overview file
- day/phase system planning
- save/load ownership planning
- economy / cobalt coin ownership planning if needed
- progression / unlock ownership planning if needed
- expedition result routing planning if needed

Possible file examples:
- `Base_Global_Systems.md`
- `Global_Systems_Plan.md`
- `Save_State_Plan.md`

Purpose:
- define what is not owned by a room
- reduce ownership drift
- make save/load architecture clearer before implementation starts
- define how cross-room systems work

---

### Step 3 — Core room plans
Create and refine:
- `Room_Storage.md`
- `Room_Shop.md`
- `Room_Workshop.md`
- `Room_RecruitQuarters.md`
- `Room_Core.md`

Purpose:
- define the backbone rooms first
- define inventory, economy interaction, production, recruit, and power ownership
- establish the most important room connections
- keep room-local ownership separate from global systems

---

### Step 4 — Secondary room plans
Create and refine:
- `Room_QuestBoard.md`
- `Room_Info.md`
- `Room_Map.md`
- `Room_RecruitMachine.md`
- `Room_Graveyard.md`
- `Room_Merchant.md`
- `Room_Druid.md`
- `Room_Dwarf.md`

Purpose:
- define support progression rooms
- define connected systems that depend on the base rooms and global systems
- finish the base structure

---

### Step 5 — Implementation support files
Create and refine:
- `Base_Implementation_Checklist.md`
- `Implementation_Status.md`
- `Known_Temporary_Systems.md`
- `Review_Checkpoints.md`

Purpose:
- guide actual build order
- track what is done
- track what is temporary
- define when project reviews should happen

---

### Step 6 — Technical guidance files
Create and refine:
- `Room_Naming_Conventions.md`
- `Hierarchy_Guidelines.md`
- `Script_Architecture_Guidelines.md`

Purpose:
- support clean implementation
- reduce confusion during long-term development
- keep scene and script structure understandable

---

## 4. Review Order

Documents should be reviewed in this order:

### First review group — master structure
1. `Base_Master_Plan.md`
2. `Open_Architecture_Questions.md`

Reason:
These define the whole structure and influence all later files.

---

### Second review group — global systems structure
1. global systems overview file
2. save/load planning file
3. economy/progression/global ownership files if separated

Reason:
These define what should not be owned by rooms.

---

### Third review group — core ownership rooms
1. `Room_Storage.md`
2. `Room_Shop.md`
3. `Room_Workshop.md`
4. `Room_RecruitQuarters.md`
5. `Room_Core.md`

Reason:
These define most of the base’s room-local ownership and flow.

---

### Fourth review group — support rooms
1. `Room_QuestBoard.md`
2. `Room_Info.md`
3. `Room_Map.md`
4. `Room_RecruitMachine.md`
5. `Room_Graveyard.md`
6. `Room_Merchant.md`
7. `Room_Druid.md`
8. `Room_Dwarf.md`

Reason:
These depend on the earlier base/global/core room groups being clear enough.

---

### Fifth review group — support/meta files
1. `Implementation_Status.md`
2. `Known_Temporary_Systems.md`
3. `Review_Checkpoints.md`
4. `Room_Naming_Conventions.md`
5. `Hierarchy_Guidelines.md`
6. `Script_Architecture_Guidelines.md`

Reason:
These should support the real architecture, not decide it first.

---

## 5. What Each File Must Contain

### Base-level files must define
- what the base is
- what the main rooms are
- what the global systems are
- what the daily structure is
- what global resources exist
- what room ownership direction exists
- how expedition connects to the base
- what major open questions still exist

---

### Global systems files must define
- what the system is
- why it is global instead of room-owned
- what state it owns
- what it does not own
- what rooms/systems depend on it
- what events/inputs it reacts to
- what outputs it provides
- how it should save/load
- what open questions remain

Examples:
- day/phase system
- save/load coordination
- progression/unlock system
- economy system
- expedition result routing
- global research/progression ownership if needed

---

### Room files must define
- room purpose
- what the room does
- what the room holds
- what the room takes
- what the room gives
- connected rooms/systems
- player actions
- important rules and limits
- upgrade levels if relevant
- data ownership
- UI/readability needs
- interaction/animation needs
- temporary implementation notes
- done condition
- open questions

---

### Implementation support files must define
- what order the project should be built in
- what the current project state is
- what is temporary
- what needs review
- what major questions remain open

---

### Technical guidance files must define
- naming rules
- hierarchy rules
- script responsibility rules
- separation of room systems and global systems
- how to mark temporary/debug content

---

## 6. How Each File Should Be Reviewed

When reviewing a file, check:

### 1. Purpose clarity
Can the file clearly explain what the room/system/document is for?

### 2. Ownership clarity
Is it clear what it owns and does not own?

### 3. Input/output clarity
Does it correctly describe what it takes and what it gives?

### 4. Missing information
Are there important rules, states, or flows not described yet?

### 5. Wrong assumptions
Does the file claim something as settled that is actually undecided?

### 6. Build usefulness
Would this file actually help build a working system, or is it still too vague?

### 7. Global vs local correctness
Is something being treated as room-owned that should really be global, or vice versa?

### 8. Save/load clarity
If this state must persist, is it clear how it fits into save ownership?

---

## 7. File Quality Standard

A file is considered strong enough when:
- it is understandable
- it has clear purpose
- it has clear ownership
- it has useful implementation value
- it does not strongly conflict with other files
- it leaves true unknowns in open questions instead of hiding them

A file does **not** need to be final-polish perfect.
It needs to be strong enough to support implementation and future review.

---

## 8. Revision Rules

As the project changes:
- files can be rewritten
- ownership can be adjusted
- systems can move between rooms
- names can change
- open questions can become resolved decisions
- a room-owned system can later become global if needed
- a global plan can later be split into multiple smaller files if needed

The important rule is:
**when something changes, the docs should be updated so the documentation stays more correct than memory.**

---

## 9. Documentation Workflow

Recommended workflow:

### Step A
Read one file.

### Step B
Identify:
- what feels correct
- what feels wrong
- what feels missing
- what feels too early or too final

### Step C
Discuss and revise the file.

### Step D
If the file exposes a cross-room ownership problem, check whether the issue really belongs in a global systems file instead.

### Step E
Move to the next file.

### Step F
After a group of files is reviewed, do a bigger consistency check.

This should be done gradually.
The goal is not speed.
The goal is strong structure.

---

## 10. Review Milestones

### Milestone 1
Base-level files are reviewed and stable enough.

### Milestone 2
Global systems direction is reviewed and stable enough.

### Milestone 3
Core room files are reviewed and ownership is mostly stable.

### Milestone 4
Support room files are reviewed and connected correctly.

### Milestone 5
Implementation support files are updated to match the reviewed design.

### Milestone 6
Technical guidance files are updated to match the final working documentation direction.

---

## 11. End Goal

The end goal of the documentation process is:

- a complete base plan
- a clear global systems plan
- a clear room-by-room plan
- clear ownership
- clear system connections
- clear implementation direction
- enough structure to build a working systems-driven version of the game without constantly guessing where things belong

This means the documentation should become strong enough that:
- the base can be built room by room
- rooms can communicate correctly
- global systems can support rooms cleanly
- temporary systems can be tracked safely
- large refactors can be reduced
- implementation decisions can follow documents instead of only memory

---

## 12. Notes

This file should be used as the guide for the documentation process itself.

It is expected that many other files will change during review.
That is normal.

The goal is not to lock the design too early.
The goal is to improve the documentation until it is strong enough to guide real implementation.