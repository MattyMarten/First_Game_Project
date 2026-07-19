# Base Master Plan

## 1. Base Overview

The underground base is the player's central hub for preparation, progression, survival, and earning money.

It is designed as a bunker / sewer / underground facility type of base with no windows and artificial lighting.

The base is where the player:
- processes loot brought back from expeditions
- stores physical items and resources
- crafts goods and utility items
- prepares and runs the shop
- manages recruits and their equipment
- unlocks and uses NPC-operated service spaces
- maintains the systems that keep the base running
- prepares and launches the next expedition, or dispatches an away team instead

The base is designed as a system-heavy management space where rooms, areas, and stations connect to each other and exchange resources, information, and progression state.

This document is the high-level source of truth for the base structure.
It defines:
- what the base is
- how the base is physically organized
- what the major rooms and areas are
- what the major systems are
- how major ownership is expected to work

It does **not** fully define the detailed mechanics of every room.
Detailed room and station behavior should be handled in separate documents.

---

## 1a. Setting / Core Story Premise

The underground world has been kept alive for over 1000 years by a machine called **SAM**.

SAM stopped functioning after a monster attack.

The player's base runs on a smaller, newer power core built to survive long enough to push outward, sector by sector, toward SAM's original location, in the hope of reactivating it.

This gives the Core Room genuine narrative weight rather than being a purely mechanical daily-upkeep chore:
- upgrading the Core is framed as part of the journey back to SAM
- reaching SAM is the game's long-term main-line direction

This premise should inform main-line quest writing (the Dwarf and Professor's quests) and the tone of the Core's own upgrade offers.

---

## 2. Core Base Purpose

The base exists to support a repeating progression cycle:

1. go on expedition (or dispatch an away team)
2. gather loot
3. return to base with loot and expedition results
4. process and store what came back
5. craft goods and useful items
6. sell goods through the shop
7. improve rooms, recruit progression, and other base systems
8. prepare for the next expedition

This is the core loop: **Craft, Sell, Prepare, Loot.**

Expedition is primarily for gathering loot.

Other important expedition outcomes can include:
- discovering special NPCs
- finding documents and Data Sticks
- discovering new sector connections
- gaining recruit XP and recruit status outcomes
- completing or progressing assigned quests

Recruits are not primarily acquired through expedition.
Recruits are mainly acquired through the shop visitor flow.

Important rule:
- the whole game is played as a recruit
- on the base, the player switches control by interacting with a recruit or undead
- expeditions are also played through recruits/undead

Even if the game later grows with more systems, this loop is the core reason the base exists.

---

## 3. Daily Base Structure

The game starts on:
- Day 1
- Morning
- 08:00

The base runs in three main base phases plus the expedition night phase.

Important phase rule:
- phases are manual at the base
- opening the Shop starts Day
- closing the Shop starts Evening
- launching the expedition, OR dispatching an away team with no personal expedition, starts Night
- expedition/dispatch resolution starts the next Morning

### Morning — 08:00
Morning is the processing and setup phase.

During morning, the player:
- takes returned cases from expedition results
- reviews expedition outcome status (including any away-team report)
- grinds cases / loot into materials
- uses materials for crafting
- prepares the shop before opening
- changes shop decor if needed
- changes displays if needed
- places goods into displays for sale

Important current Core rule:
- if the Core does not have enough loaded cobalt for the day's upkeep requirement, warning state begins in Morning
- warning remains active until resolved

### Day — 12:00
Day is the shop operation phase.

The player moves from Morning to Day by opening the shop.

During day:
- the shop is opened
- the game generates a full daily visitor/customer list at shop opening
- the list includes buyers, request visitors, talking visitors, and recruit visitors
- visitors arrive one by one over time

Visitors may:
- buy an item from a display
- give a request
- talk and provide information
- ask to join the team as a recruit

The day phase ends when:
- the player closes the shop, or
- there are no more visitors/customers left to come that day

### Evening — 19:00
Evening is the expedition preparation and upkeep phase.

The player moves from Day to Evening by closing the shop.

During evening, the player:
- crafts needed utility items
- assigns equipment to recruits
- prepares expedition loadouts
- assigns quests for the expedition
- selects which recruits will personally go on the expedition, and/or dispatches an away team to a different unlocked sector
- inserts cobalt coins into the Core deposit system to keep the base running
- reviews quests, sector, and planning data
- confirms and launches the next expedition, and/or confirms an away-team dispatch

Important current Core rule:
- the Core consumes its daily cobalt upkeep at the start of Evening (base amount 50, higher if the Core has been upgraded — see Room_Core.md)

### Night / Expedition — 00:00 onward
The player moves from Evening to Night by launching a personal expedition, or by confirming an away-team dispatch with no personal expedition.

**If the player personally expeditions:**
- time becomes 00:00
- the player enters the expedition with the selected team and loadout
- during the expedition, time advances continuously
- **there is no fixed real-time or in-game-time expedition length.** The only limit on how long the player can stay out is the equipped Suit's Battery charge (see Suit System). When Battery runs out, the player is forced to return.
- a stronger/more-upgraded Suit allows a longer expedition before Battery forces a return

**If the player only dispatches an away team (no personal expedition):**
- the base skips directly to the next Morning
- the away team's outcome is resolved and shown on the Expedition Results Display / away-team report the next Morning

### Time structure summary
Base time is mostly phase-based:
- Morning = 08:00
- Day = 12:00
- Evening = 19:00
- Night / expedition start = 00:00

At the base, time advances mainly through manual phase transitions.
During a personal expedition, time advances continuously until Battery forces a return.
During a dispatch-only night, time skips straight to the next Morning.

### Day/time display direction
Day number and time should always be visible to the player.

### Phase restriction note
Current hard phase locks:
- opening the Shop happens during Morning and starts Day
- closing the Shop happens during Day and starts Evening
- launching the expedition, or confirming a dispatch-only night, happens during Evening and starts Night

Everything else can generally be done at any time unless later changed.

---

## 4. Base Physical Structure

The base has three floors.

### Floor 0 — Storage and NPC Service Floor
This is the bottom floor.

This floor contains:
- the Storage Room in the middle
- a surrounding corridor connected to multiple NPC-operated service rooms/spaces

Current known service NPC types include:
- Dwarf (base owner, present from the start)
- Merchant (Rat Merchant)
- Druid
- Professor
- future NPCs

The base starts with the Dwarf already available.
All other special NPCs must be discovered during expeditions before their service spaces open.

### Floor 1 — Main Operations Floor
This floor contains:
- Shop Room
- Workshop Room (Grinder, Goods Workbench, Gear Workbench, Gear Upgrade Station, Suit Station)
- Recruit housing area
- Graveyard
- Core access corridor and deposit point
- expedition case return / spawn area
- recruit management board/station
- Notification Display (mounted near the stairs between floors)

Important current rule:
- recruit management happens on Floor 1
- there should only be one recruit management board on Floor 1

### Floor 2 — Planning and Expedition Control Floor
This floor contains a shared planning room / area with stations such as:
- Sector Map station (personal destination + entry selection)
- Info / Computer station
- Quest board station
- Recruit Selection station (your own personal party)
- Dispatch Board station (away-team assignment — separate from personal expedition selection)
- Entry base bonus loadout station
- Expedition Results Display
- expedition launch door / confirmation point

Important current rule:
- Floor 2 does **not** handle full recruit management
- Floor 2 handles: selecting who personally goes on expedition, and separately, dispatching an away team
- the Sector Map only handles **your own** destination selection (you explore and loot)
- the Dispatch Board only handles **away-team** destination + roster (they only loot, at a location you've already unlocked)

---

## 5. Sectors, Exploration, and the Unlock Graph

This replaces the previous single-fixed-location design. There is no longer a single starting city; the world is built from **named sector categories**, each generating a fresh random instance every time it's entered.

### Sector categories
Each sector belongs to a named category with a letter/style identity and a random numeric instance.

Example:
- **N — Nature.** A huge underground dome containing forest terrain with randomized building placement. Visiting N generates a fresh random instance each time (e.g. N45, N66) — never the same layout twice.
- **L — Labs.** A second category, not yet designed in detail.

More categories will be added later. Currently only N and L are planned; the exact final list is open.

### Discovery and the unlock graph
- Each sector instance contains hidden connection points to other sector categories.
- Reaching a connection point requires tools or puzzle-solving, while monsters actively hunt the player during the attempt.
- Finding a connection is not enough by itself — the player must also **power** it (via backup generators or similar), which is what actually unlocks that sector *category* as a new valid starting point on the Sector Map going forward.
- Once a category is unlocked this way, it stays unlocked permanently — but every future visit to it still generates a new random instance. Unlocking a category is not the same as memorizing a map.
- The Core determines how far from the base the player can reach at all (see Room_Core.md) — a sector category can only be powered/reached if it's within the Core's current range.

### Suits gate what you can survive, not just what you can reach
Independently of Core range, a sector's environmental hazards (heat, cold, corrosive fluids, toxic/thin air) require a sufficiently upgraded Suit before the player — or an away team — can enter at all. If the Suit can't handle a sector's hazards, that sector is not selectable for personal expedition or for dispatch, regardless of whether it's unlocked. See Suit System.

### Away teams and already-unlocked sectors
The player can send an away team (via the Dispatch Board) to any sector *category* already unlocked. Since instances are randomized and not persistent, an away team is not sent to a specific remembered place — they're sent to loot that category generically, with a risk % and loot preview shown per category (see Dispatch Board doc).

---

## 6. Recruits

### Acquisition
Recruit visitors appear in the Shop's daily visitor pool. The **chance of a recruit visitor being generated each day scales with free recruit housing space**:

```
chance = free slots / total slots
minimum floor = 25%
```

Example: 4 free out of 4 total → 100%. 1 free out of 12 → would mathematically be ~8%, but the 25% floor applies, so it's still 25%. 0 free out of any total → 0%, no recruit visitor is generated at all.

### Starting state and capacity
The base starts with **4 recruits already filling all starting capacity** — the Dwarf's four fixed starting recruits (Mara, Brok, Pip, Vael; see Room_Dwarf.md) exactly fill Recruit Quarters LV1.

Recruit rooms hold **2 beds each** (revised down from 4):
- **LV1**: 2 rooms × 2 beds = 4 capacity
- **LV2**: 3 rooms × 2 beds = 6 capacity
- **LV3**: 4 rooms × 2 beds = 8 capacity

This means the base starts completely full, and new recruit opportunities only open up once a recruit dies, is retired, or the room is upgraded.

### Retiring a recruit
The player can retire a living recruit at their locker in Recruit Quarters (with a confirmation prompt). Retiring:
- permanently removes the recruit from the roster
- applies a **-3 appeal** penalty
- does **not** create an undead — only actual expedition death feeds the Graveyard

### Debuffs are never a hard lockout
Recruit status effects/debuffs (Poison, Injury, Broken Bone, Exhaustion, Cursed, etc.) are always **harsh penalties**, never a full unplayability lock. A recruit can always be controlled and sent on expedition regardless of current debuffs, at reduced effectiveness. (This is a change from the previous design, where Exhaustion made a recruit fully unusable — see Room_Druid.md for the updated effect list.)

### Personal party vs. away team
- **Personal party** (Recruit Selection station, Floor 2): the player's own party, up to the current recruit roster size, pod 1 always locked to the currently controlled recruit. The player explores and loots this destination directly.
- **Away team** (Dispatch Board, Floor 2, separate station): any remaining recruits not in the personal party can instead be assigned to a second, unlocked-sector destination that they resolve without the player present. Team size is fully player-chosen — even a single recruit is a valid away team.
- **The player can dispatch an away team without personally expeditioning at all** that night. Doing so still starts Night, but skips directly to the next Morning.
- Away-team outcome uses **independent per-recruit rolls**: if the sector's success chance is 90%, each recruit on the away team individually rolls against that percentage — a bigger team means more individual rolls, and therefore a higher chance that at least one recruit doesn't come back, even on an overall-successful mission.

---

## 7. Suit System

Suits replace any fixed expedition time limit as the thing that determines how long the player can stay on an expedition, and what environments they can enter at all.

### Core properties
- There is **one shared Suit**, upgraded permanently through the **Suit Station** (a new Workshop station — see Room_Workshop.md). Upgrades are never redone per-recruit; every recruit benefits from the same upgraded Suit immediately.
- The Suit Station is presented physically as a suit mounted on a stand; the player interacts with it and chooses which component to upgrade.
- Suit components and what they govern:
  - **Battery** — governs total expedition duration. This is the *only* limit on how long an expedition can run — there is no other fixed timer. When Battery runs out, the expedition forces a return.
  - **Shoes** — governs movement speed.
  - **Mask** — governs breathing-hazard resistance (toxic air, radiation via breathing, etc.)
  - **Suit Material** — governs physical environmental resistance (heat, cold, corrosive fluid contact, etc.)
- Suit components are crafted from materials at the Gear Workbench (as a new craftable category) and then installed permanently at the Suit Station.

### Hard travel gate
If the current Suit's Mask/Suit Material resistance is insufficient for a sector category's hazard rating, that sector cannot be selected — not by the player for personal expedition, and not for an away team dispatch either. This is a hard block, not a risk modifier.

### Relationship to Core range
Core range determines which sectors are reachable at all (distance). Suit resistance determines which of those reachable sectors can actually be survived (environment). Both must be satisfied for a sector to be selectable.

---

## 8. Global Systems

These systems affect the whole base and are not limited to just one room:

- day, time, and phase system
- Core upkeep and power state
- Core upgrade progression (range, daily consumption, capacity)
- Suit upgrade progression (shared, permanent)
- sector unlock graph tracking
- expedition result / away-team result routing
- quest progression
- Quest Board rank progression
- recipe unlock tracking (via Data Sticks — see below)
- power availability from the Core
- contextual room/station HUD logic
- NPC arrival logic for shop-related visitors
- hidden progression score for unlock gating
- visible progression tracking / registry systems
- save/load coordination
- progression / unlock system

**Removed this session:** research points (RP) and material knowledge tiers no longer exist as a system. See Section 10.

### Appeal
Appeal is a shop-related value ranging from 0 to 100, owned directly by the Shop.

Appeal affects buyer count and sale price. It is also affected by:
- quest accept/decline/complete/discard (see Room_QuestBoard.md)
- Core failure penalty (-20, once per failed day)
- retiring a recruit (-3, see Section 6)

### Cobalt coins
Cobalt coins are the base's generic currency. Storage owns cobalt coin truth; the Core holds its own separate loaded amount.

### Recipe unlocks — Data Sticks
Recipes (Goods Workbench and Gear Workbench) are no longer unlocked through research. Instead:
- **Data Sticks** are physical items that unlock a specific recipe automatically the moment they reach the base (found via expedition, purchased from the Merchant, or given by request/talking visitors).
- Data Sticks are **auto-consumed** on acquisition — there is no player choice of when to use them, and they never sit in Storage as an inventory item.
- If a Data Stick is acquired for a recipe already unlocked, it **auto-converts into materials or coins** instead of doing nothing.
- Each source (a specific sector category, the Merchant, request visitors, talking visitors) draws from its **own curated pool** of possible sticks — random within a source, but knowable/predictable which source can yield which stick.
- The Merchant has a dedicated Data Stick pedestal with a flat **10% chance per day** of a stick appearing.

---

## 9. Global Resource and State Categories

### Physical / inventory-like categories
- cobalt coins
- materials (flat, no tiers, no type variants beyond distinct named materials)
- loot
- goods
- utility items
- belts
- cases
- accessories
- shop decor
- Suit components (Battery, Shoes, Mask, Suit Material — consumed on install, not stored long-term)
- cases returned from expedition

### Non-physical progression / state categories
- information
- recruits
- undead recruits
- quests
- Quest Board rank and board XP
- unlocked recipes (via Data Sticks)
- unlocked sector categories
- recruit injuries / debuffs (never a hard lockout)
- current day / time / phase
- power availability
- Core upgrade level (range / consumption / capacity)
- Suit upgrade level (Battery / Shoes / Mask / Suit Material)
- service NPC unlock state
- hidden base progression score
- save state / persistent game state

**Removed this session:** research points (RP), unlocked knowledge tiers, material tier variants.

### Room/system progression categories
- room upgrade levels
- station upgrade levels
- Core upgrade level (new — see Room_Core.md)
- Suit upgrade level (new — see Suit System, Section 7)

---

## 10. Removed Systems (this session)

### Research Station — removed entirely
The Research Station, research points (RP), and material knowledge tiers no longer exist.
- The Grinder now produces **materials only** (no RP).
- Recipes are simply locked or unlocked (via Data Sticks), with no intermediate knowledge-tier requirement.
- Gear tier upgrades (T1→T2→T3 at the Gear Upgrade Station) are unaffected — they were never gated by research and remain exactly as before.
- Materials themselves are now **flat** — no tiers, no type variants (e.g. no "Wood T1/T2/T3" or "Refined Wood"). Just Wood, Metal, etc. as single distinct items.

### Forsaken City — removed
The single fixed starting location is replaced entirely by the sector category system (Section 5).

---

## 11. High-Level Ownership Rules

(Unchanged sections from the previous version are not repeated here in full — see the room docs for full ownership detail. Only new/changed ownership is called out below.)

### Core Room now owns (in addition to prior state)
- Core upgrade level (range / consumption / capacity)
- Core upgrade unlock-gating state (upgrades are milestone-locked, then offered through the Dwarf's Upgrade Board like any other room upgrade)

### Workshop now owns
- Suit Station upgrade state (Battery / Shoes / Mask / Suit Material levels)
- (No longer owns) Research Station state, RP, or knowledge tiers — removed

### Recruit Quarters now owns
- retire action and its appeal-cost trigger
- revised capacity (4/6/8, 2-per-room)

### Graveyard now owns
- a **pending replacement queue** rather than a single pending decision — multiple deaths arriving from one away-team report must be resolved one at a time

### Map / Sector system now owns
- sector category unlock state (replacing per-location entry unlock state)
- sector instance generation (random per visit)

### Dispatch Board (new) owns
- away-team roster assignment
- away-team destination selection (restricted to already-unlocked sectors)
- away-team risk/loot preview

---

## 12. Room Connection Summary (updated)

### Expedition (personal) takes from base
- recruits (personal party)
- cases, belts, utility items, accessories
- assigned quests
- selected sector destination

### Away-team dispatch takes from base
- recruits (away-team roster only)
- selected sector destination (already-unlocked only)

### Expedition/dispatch returns to base with
- recruits (or death outcomes)
- recruit XP / status outcomes
- cases containing loot/materials
- quest progress / completion
- documents / information
- Data Sticks (recipe unlocks)
- newly discovered sector connections
- special NPC discovery opportunities

### Important flow note
The Grinder still logically belongs to Workshop, physically placed near expedition return flow, same as before.

---

## 13. Upgrade Philosophy

Most major rooms, stations, and service spaces can grow through upgrades or unlock progression, generally affecting capacity, features, tiers, speed, or output.

Current upgrade ownership split:
- room/system owns current level/state
- shared upgrade definitions hold costs/effects

**New this session:** the Core is now part of this same philosophy. It upgrades through the Dwarf's Upgrade Board like any other room — but its upgrade slot is **locked until a progression milestone unlocks it**. Once unlocked, Core upgrades proceed normally through the Upgrade Board's existing priority-group system.

---

## 14. Open Questions

- Exact trigger for unlocking Core upgrades on the Upgrade Board (which milestone specifically)
- Full list of sector categories beyond N (Nature) and L (Labs)
- Whether duplicate Data Stick conversion always yields coins, always materials, or depends on context
- Exact Suit Station component costs/values (placeholder until balancing)
- Exact Core range/consumption/capacity numbers per level (placeholder until balancing)

---

## 15. Document Rules

This file is the high-level source of truth for the base. It should define the whole base, the floor structure, the major rooms/areas/stations, the major system relationships, the ownership direction, and the overall UI/readability direction. Detailed behavior belongs in separate room, station, and system documents.
