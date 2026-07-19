# Core Room Plan

## 1. Room Name
Core Room

---

## 2. Purpose
The Core Room is the base power room.

It exists to:
- power the underground base
- keep important systems functioning
- create a daily cobalt coin upkeep requirement
- give the player a readable maintenance responsibility
- create tension between reserve resources and daily operations
- **power the sectors the player can reach, tying the base's core narrative (rebuilding toward SAM) directly to a gameplay system**

### Narrative role (new this session)
The underground world was powered for over 1000 years by a machine called SAM, until it stopped functioning after a monster attack. The player's Core is a smaller, newer machine built to survive long enough to push outward, sector by sector, toward SAM's location. Upgrading the Core is framed as literal progress toward that goal, not just a bigger number.

The Core is one of the most important rooms in the base because its power state — and now its upgrade level — affects multiple floors, rooms, machines, service spaces, and how far the player can travel at all.

---

## 3. What It Does
The Core Room:
- stores loaded cobalt coins inside the Core
- powers the base while enough coin power supply exists
- **determines how many sectors away from the base the Core can currently supply power to (range)**
- consumes cobalt coins automatically at the start of Evening
- enters a warning state at Morning if there is not enough loaded cobalt for the current day
- keeps that warning active throughout the day until resolved
- powers down if the player closes the Shop while warning state is still unresolved
- applies power-loss penalties when shutdown happens
- allows the player to manually restore power by depositing more cobalt coins
- **can be upgraded through the Dwarf's Upgrade Board once unlocked (new this session — see Section 9)**

The Core is not just a passive requirement. It is a readable machine with visible state, visible reserve amount, and clear consequences — and now a visible growth path tied to the story.

---

## 4. What It Holds

The Core Room holds or tracks:
- current loaded cobalt coin amount inside the Core
- Core coin capacity (now scales with upgrade level)
- required daily consumption amount (now scales with upgrade level)
- **current Core upgrade level (range / consumption / capacity)**
- **whether the Core's upgrade slot has been unlocked yet on the Upgrade Board**
- current Core state (Normal / Warning / Offline)
- whether today's failed-day penalty has already been applied
- whether the base currently has power
- power dependency output state for connected rooms/systems
- power restoration countdown/transition state when recovering from offline

### Fixed base values (LV1, before any upgrade)
- Core max capacity: 200 coins
- Core deposit amount: 50 coins
- Core daily consumption: 50 coins
- Core range: reaches only the base's immediately adjacent, already-unlocked sector connections (exact starting range value TBD during sector design)

---

## 5. What It Takes

The Core Room takes:
- cobalt coins from Storage reserve through player deposit interaction
- Morning phase start signal from the global day/phase system
- Evening phase start signal from the global day/phase system
- Shop closing event from the Shop/day-phase flow
- **an upgrade unlock signal from the Progression/Unlock system, once the milestone condition is met (exact milestone TBD)**
- **upgrade purchase input via the Dwarf's Upgrade Board, once unlocked**

Important note:
- the Core does **not** own day count or phase progression
- it reacts to those systems rather than owning them

---

## 6. What It Gives

The Core Room gives:
- power to dependent systems while active
- **the maximum sector distance the player can currently reach (range)**
- warning feedback when loaded cobalt is insufficient
- emergency/offline feedback when the Core shuts down
- recovery interaction so the player can restore power
- penalty application when the player fails to resolve warning state in time

---

## 7. Connected Rooms / Systems

The Core Room connects to:
- Storage Room
- Shop Room
- Workshop Room
- Recruit Quarters
- Graveyard
- Merchant Room
- Druid Room
- Dwarf Room (Upgrade Board — new dependency this session)
- Info / Computer systems
- **Sector Map system (range gating — new dependency this session)**
- global day/phase system
- global economy / cobalt coin flow

---

## 8. Player Actions

The player can:
- inspect current Core status
- inspect loaded Core coin amount
- inspect total stored cobalt coin amount
- inspect daily consumption amount
- inspect current Core range
- deposit 50 cobalt coins into the Core
- check whether the Core is in normal, warning, or offline state
- review what happens if power is lost
- restore power after shutdown by depositing enough cobalt
- **purchase a Core upgrade through the Dwarf's Upgrade Board, once unlocked**

The player does **not** deposit custom amounts. Each press deposits one fixed 50-coin chunk (subject to the current level's deposit amount, if that also scales — TBD).

---

## 9. Core Upgrades (new this session)

### Overturning the previous rule
Earlier design intentionally made the Core the one room with **no** upgrade path, to keep it simple and fixed. This is now deliberately reversed: because the Core's growth is now tied directly to the game's core story (reaching SAM), giving it a real upgrade path adds meaning rather than just adding complexity. This reversal is intentional, not an oversight.

### How Core upgrades work
- Core upgrades appear on the Dwarf's Upgrade Board exactly like any other room/station upgrade — same priority-group system, same paper format, same timing rules (see Room_Dwarf.md).
- **The Core's upgrade slot does not exist on the board from the start.** It is locked until a progression milestone is reached (exact milestone TBD — likely tied to the hidden base progression score or a specific main-line quest beat). Once unlocked, it behaves like any other upgradeable target.

### What each Core upgrade level changes
Every Core upgrade level increases all three of the following together:
- **Range** — how many sectors away from the base the Core can supply power to. This is the mechanic that actually gates which sector categories can be unlocked/started-from at all, independent of whether the player has physically found the connection.
- **Daily consumption** — increases per level. Reaching further costs more upkeep, ongoing, permanently.
- **Capacity** — increases per level, to keep the "days of buffer before trouble" feeling from collapsing as consumption rises.

### Permanence
Once a Core upgrade is purchased, its increased range and consumption are **permanent** — there is no way to "power down" a sector to reduce daily drain. This is an intentional design choice to match the tone of the setting (SAM has been failing for 1000 years; the Core only ever pushes forward, it doesn't retreat).

### Placeholder level values (subject to balancing)
| Level | Capacity | Daily Consumption | Range |
|---|---|---|---|
| LV1 (start) | 200 | 50 | adjacent unlocked sectors only |
| LV2 | TBD, higher | TBD, higher | +1 sector reach |
| LV3 | TBD, higher | TBD, higher | +1 sector reach further |

Exact numbers should be filled in once sector distances are actually designed.

---

## 10. Rules and Limits (unchanged from previous version except where noted)

### Core storage rules
- the Core has its own internal loaded coin storage, separate from Storage reserve
- each deposit is exactly 50 coins (or the current level's deposit amount, if that scales)
- if the player has less than the deposit amount in reserve coins, deposit cannot happen

### Daily consumption rule
- the Core consumes its daily required amount at the start of Evening (50 at LV1, more at higher levels)

### Warning state rule
- checked at Morning; if the Core does not have enough loaded cobalt for the day's required amount, it enters warning state
- during warning state, the base is still powered; warning lights turn yellow/orange
- if the player deposits enough during warning state, the Core returns to safe operation for that day

### Shop close warning rule
- if the player tries to close the Shop while the Core warning state is still unresolved, a small popup warning appears
- the player can still ignore the warning and continue closing the Shop

### Shutdown / offline rule
- if the player closes the Shop while the Core is still in warning state, the Core powers down; failed-day penalty applies once for that day

### Recovery rule
- if the Core is offline, the player can restore it by depositing the required amount
- recovery uses a short ~5-second restoration sequence (AI voice line, emergency lighting off, then normal lighting on)
- the failed-day penalty for that day is **not** undone

### Failed-day penalty rule
- appeal reduced by 20, once per failed day only

### Current Core states
Normal / Warning / Offline-Emergency — unchanged from previous version.

---

## 11. Core State Readability

Main Core display should show:
- loaded Core coins / max capacity
- daily consumption amount
- **current range**
- current Core state
- **current Core upgrade level**

Secondary coin display should show total stored cobalt coin amount in Storage reserve.

---

## 12. Power State / Room Effects

Unchanged from the previous version — see prior lighting/audio/dependency behavior for Normal/Warning/Offline states. Range does not affect power-state behavior; range only affects which sectors are reachable while the Core is in Normal or Warning state (Offline blocks travel entirely, same as before, since the base has no power at all).

---

## 13. Power Dependency List

Unchanged from previous version — Shop/Workshop/Recruit Quarters/Graveyard/Storage/special NPC rooms/Info/Map/Quest Board dependency rules all remain the same. The Sector Map and Dispatch Board should be treated the same way the Map/Quest Board were previously (still function during offline state, since planning should never be hard-blocked).

---

## 14. Data Ownership

### This room owns
- loaded Core coin amount
- Core coin capacity
- daily Core consumption rule
- **Core range**
- **Core upgrade level and upgrade unlock-gating state**
- current Core state
- warning/offline transition logic
- failed-day penalty application
- base power active / inactive state
- Core recovery transition state

### This room does not own
- Storage reserve inventory totals
- global day count / phase ownership
- sector unlock graph itself (Core only supplies the range number; the Sector Map system owns which sectors exist and their connection state)
- shared upgrade cost/effect definitions (owned by the shared upgrade definition data, same as every other room)

---

## 15. UI / Readability Needs

Same as previous version, plus:
- current Core range must be visible somewhere readable (Core display and/or Sector Map should both surface it, since range gates sector selection)
- whether the Core's upgrade slot is currently locked or unlocked should be clear when viewing the Upgrade Board

---

## 16. Interaction / Animation Needs

Same as previous version, plus:
- Core upgrade purchased feedback (through the Upgrade Board, same as any other room upgrade)
- range increase feedback when a Core upgrade completes

---

## 17. Temporary Implementation Notes

Same as previous version, plus:
- placeholder Core upgrade level values until sector/range design is finalized
- debug button to force-unlock the Core's upgrade slot for testing before the real milestone trigger exists

---

## 18. Done Condition

Same as previous version's conditions, plus:
- Core upgrade correctly appears on the Upgrade Board only after its unlock milestone is met
- Core upgrade correctly increases range, consumption, and capacity together, permanently
- Sector Map correctly reads current Core range when determining selectable sectors
- ownership does not conflict with the Dwarf's Upgrade Board or the Sector Map system

---

## 19. Open Questions
- Exact milestone that unlocks the Core's upgrade slot on the Upgrade Board
- Exact capacity/consumption/range numbers per level (placeholder until sector design and balancing pass)
