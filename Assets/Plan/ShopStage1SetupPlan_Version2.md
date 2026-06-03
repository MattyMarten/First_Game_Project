# Shop Stage 1 Setup Plan

## Goal
Set up the correct Shop architecture before deeper refactors.

This stage does **not** aim to finish Shop behavior.
It creates the correct top-level structure so later implementation steps have clear ownership.

---

# Stage 1 purpose

## What this stage should accomplish
- define the real Shop-wide core
- separate Shop core from desk-local managers
- stop treating current `ShopManager` as the whole Shop manager
- prepare current systems to be refactored under the correct structure
- clarify which systems stay active and which should stop being final authorities

## What this stage should NOT try to do yet
- do not fully implement the final daily visitor list yet
- do not fully rewrite Desk 1/2/3 behavior yet
- do not fully move all storage systems yet
- do not fully rebuild machine flow yet

This stage is about structure first.

---

# Final manager structure for Stage 1

## 1. Shop Core Manager
This is the future whole-Shop authority.

### Purpose
Own Shop-wide systems that affect all desks.

### Intended responsibilities
- shop open/close state
- future daily visitor list ownership
- future spawn cycle ownership
- future Shop Appeal ownership
- future daily report ownership
- high-level coordination between desks

### Important note
At Stage 1, this manager may begin mostly as a shell/coordination object.
That is okay.
It still needs to exist first.

### Suggested script name
- `ShopCoreManager`
or
- `ShopDayManager`

Recommended:
- `ShopCoreManager`

---

## 2. Desk 1 Manager
This is what current `ShopManager` really is.

### Purpose
Own Desk 1 local systems only.

### Intended responsibilities
- active buyers
- buyer capacity
- browse points
- queue spots
- pending sale
- registered displays
- Desk 1 sale flow

### Current script mapping
- current `ShopManager` should be treated as this role

### Important note
At Stage 1, you do not need to rename the file immediately if that creates extra work.
But mentally and architecturally, it must stop being treated as whole-Shop authority.

---

## 3. Desk 2 Manager
### Purpose
Own Desk 2 local interaction state.

### Intended responsibilities
- active Desk 2 visitors
- Desk 2 queue
- pending interaction
- request/dialogue/merchant interaction flow

### Current script mapping
- `ServiceDeskManager`

---

## 4. Desk 3 Manager
### Purpose
Own Desk 3 local interaction state.

### Intended responsibilities
- active hire visitor
- pending hire candidate
- Desk 3 queue
- machine pre-confirmation state
- hire interaction flow before Recruit Quarters takes ownership

### Current script mapping
- `HireDeskManager`

---

## 5. Recruit Quarters Manager
### Purpose
Own finalized recruits after machine confirmation.

### Intended responsibilities
- recruit persistence
- bed assignment
- recruit actor spawning
- recruit visibility/state after joining

### Current script mapping
- `RecruitQuartersManager`

---

# Stage 1 hierarchy structure

## Goal
Create a clear hierarchy that reflects the manager split.

### Recommended Shop hierarchy
- `ShopRoot`
  - `ShopCore`
    - `ShopCoreManager`
  - `Desk1`
    - Desk 1 manager object
    - browse points parent
    - queue spots parent
    - Desk 1 visuals / interaction objects
  - `Desk2`
    - Desk 2 manager object
    - queue spots parent
    - service desk visuals / interaction objects
  - `Desk3`
    - Desk 3 manager object
    - queue spots parent
    - machine entry point
    - machine hidden processing point
    - machine UI/display interaction point
  - `DisplayRoot`
    - display stands
  - `SpawnRoot`
    - shared spawn point
    - shared exit point
  - `NpcRuntimeRoot`
    - active shop NPCs parent
  - `ShopUIRoot`
    - Desk 1 UI
    - Desk 2 UI
    - Desk 3 UI
    - future report UI
    - future Appeal UI

### Important note
This does not need to be pixel-perfect immediately.
The main goal is manager clarity and reference clarity.

---

# Script mapping for Stage 1

## Shop-wide
### New / needed
- `ShopCoreManager`
  - new script or placeholder shell script

### Old system status
- `ShopNpcTrafficManager`
  - no longer considered final authority
  - may remain temporarily for reference/testing
  - should be marked as temporary/prototype

---

## Desk 1
### Current scripts to keep active
- current `ShopManager` (but treated as Desk 1 manager)
- `ShopBuyerSpawner`
- `ShopBuyerNPC`
- `ShopBrowsePoint`
- `ShopQueueSpot`
- `DisplayStand`

### Stage 1 rule
Current `ShopManager` must now be thought of as:
- `Desk1Manager`
not
- whole Shop manager

---

## Desk 2
### Current scripts to keep active
- `ServiceDeskManager`
- `ServiceVisitorSpawner`
- Desk 2 visitor scripts

### Stage 1 rule
Desk 2 is a local subsystem under Shop core.

---

## Desk 3
### Current scripts to keep active
- `HireDeskManager`
- `HireVisitorSpawner`
- `RecruitGenerator`

### Stage 1 rule
Desk 3 is a local subsystem under Shop core.
It does not own finalized recruit persistence.

---

## Recruit side
### Current scripts to keep active
- `RecruitQuartersManager`

### Stage 1 rule
Recruit Quarters remains separate from the Shop and should not be absorbed into Shop logic.

---

# What should be disabled as final authority

## Goal
Stop old systems from remaining “accidentally in charge.”

### Checklist
- [ ] `ShopNpcTrafficManager` should stop being treated as the future final traffic authority
- [ ] local autospawn in desk spawners should stop being considered the final design
- [ ] `ShopManager` should stop being considered the whole Shop manager
- [ ] money in `ShopManager` should stop being treated as final architecture

### Important note
This does not necessarily mean delete immediately.
It means:
- do not design around them as final truth anymore

---

# Stage 1 setup tasks

## Task 1 — create real manager ownership map in project notes
### Goal
Before changing code, write down:
- Shop Core
- Desk 1
- Desk 2
- Desk 3
- Storage
- Guild
- Recruit Quarters

and what each owns.

### Why
Prevents confusion during refactor.

---

## Task 2 — create `ShopCoreManager`
### Goal
Introduce the real whole-Shop authority object/script.

### Minimum required at Stage 1
- knows whether the Shop is open
- holds references to:
  - Desk 1 manager
  - Desk 2 manager
  - Desk 3 manager
- can later become the owner of:
  - daily visitor list
  - spawn cycle
  - Appeal
  - report

### Why
Without this, all later refactors stay fragmented.

---

## Task 3 — reclassify current `ShopManager`
### Goal
Treat current `ShopManager` as Desk 1 manager.

### At Stage 1 this means
- stop describing it as whole-shop authority
- keep it responsible only for Desk 1 things
- prepare for later rename/refactor

### Why
Its current responsibilities are mostly Desk 1 only.

---

## Task 4 — confirm Desk 2 and Desk 3 are local subsystems
### Goal
Keep `ServiceDeskManager` and `HireDeskManager` as desk-local managers, not global shop authorities.

### Why
This matches the final design cleanly.

---

## Task 5 — set a shared spawn reference area
### Goal
Centralize the future shared Shop spawn setup.

### Recommended objects
- `SharedSpawnPoint`
- `SharedExitPoint`
- optional future `NpcRuntimeRoot`

### Why
The final visitor cycle will need one clear place to spawn and exit visitors.

---

## Task 6 — separate “prototype working” from “final authority”
### Goal
Be explicit about which current systems are temporary.

### Example
- if local autospawn remains enabled for testing, that is okay
- but it must be marked as temporary and not part of final architecture

### Why
This avoids accidental permanent prototype logic.

---

# Stage 1 object reference plan

## `ShopCoreManager` should reference
- Desk 1 manager
- `ServiceDeskManager`
- `HireDeskManager`
- future report tracker
- future Appeal tracker
- shared spawn point
- shared exit point

## Desk 1 manager should reference
- display stands
- browse points
- queue spots
- Desk 1 UI
- possibly Desk 1 sale visuals

## Desk 2 manager should reference
- Desk 2 queue spots
- request/info/guild-facing systems
- merchant UI / service UI

## Desk 3 manager should reference
- Desk 3 queue spots
- machine-related points
- machine UI
- recruit generator
- Recruit Quarters manager

---

# What not to refactor yet in Stage 1

## Do not fully change yet
- buyer behavior timing/details
- daily visitor count math
- Appeal rules
- final report logic
- merchant day rules
- Desk 3 final machine flow
- storage migration

### Why
Those belong to later stages after the structure is correct.

---

# Stage 1 definition of done

Stage 1 is done when:

- [ ] A real `ShopCoreManager` exists
- [ ] current `ShopManager` is no longer thought of as whole-Shop manager
- [ ] Desk 1 / Desk 2 / Desk 3 are clearly identified as separate subsystems
- [ ] shared spawn references are defined
- [ ] Recruit Quarters remains separate and external
- [ ] Storage/economy is understood as external ownership
- [ ] old prototype traffic logic is no longer considered final architecture

Once this is true, deeper behavior refactors can be done safely.