# Shop Refactor / Implementation Plan — Phase 1 Draft

## Goal
Refactor the current Shop implementation so it matches the final Shop design document.

The design document is the source of truth.
Existing scripts are only reused if they help reach that final design.

---

## Core rule for the whole refactor
- Do not preserve current behavior just because it already exists
- Preserve code only when it supports the final Shop design
- If current logic conflicts with the final list, the list wins

---

## Phase 1 — Lock system ownership before coding changes

### Purpose
Before changing scripts, confirm which system owns what.

### Must be true
- Day system is outside Shop
- Storage/economy owns goods, utilities, money
- Guild owns requests and info persistence
- Recruit Quarters owns finalized recruits after machine confirmation
- Shop owns daily visitor generation, interactions, Appeal, display usage, and daily report

### Result
This prevents Shop logic from permanently absorbing other building systems.

---

## Phase 2 — Identify the real Shop core

### Problem
Current `ShopManager` is mostly a Desk 1 manager, not a true whole-Shop core.

### Goal
Split the concept of:
- Shop Core
- Desk 1 manager

### Future intended structure
- `ShopCoreManager` (or `ShopDayManager`)
  - owns daily visitor list
  - owns spawn cycle
  - owns Shop Appeal
  - owns report tracking
  - owns shop open/close state
- `Desk1Manager`
  - owns buyers, browse points, queue, pending sale
- `ServiceDeskManager`
  - owns Desk 2 active visitors/interactions
- `HireDeskManager`
  - owns Desk 3 active candidates/interactions

### Result
One central authority for the Shop, while each desk keeps local responsibilities.

---

## Phase 3 — Replace fragmented spawning with one spawn authority

### Problem
Current spawning is split between:
- local autospawn in buyer spawner
- local autospawn in service spawner
- local autospawn in hire spawner
- traffic manager random spawning

This conflicts with the final design.

### Goal
The Shop must have exactly one visitor scheduling authority.

### Final intended rule
- Shop Core generates the daily visitor list when the shop opens
- Shop Core runs the spawn cycle
- desk spawners only execute spawn requests

### Desk spawner role after refactor
- `ShopBuyerSpawner` = spawn executor for Desk 1 buy customer entries
- `ServiceVisitorSpawner` = spawn executor for Desk 2 entries
- `HireVisitorSpawner` = spawn executor for Desk 3 entries

### Result
No duplicate timing systems and no conflicting visitor spawn logic.

---

## Phase 4 — Implement the final daily visitor list model

### Goal
Replace ad-hoc/random spawning with the final list system.

### Final intended behavior
- visitor list generated on shop open
- list includes:
  - buying customers
  - talking visitors
  - request visitors
  - merchant visitor if eligible
  - hire visitors
- order randomized
- counts determined from:
  - shop level
  - appeal
  - day-based rules
- spawn cycle:
  - starts every 8 seconds
  - if front entry cannot spawn, move it to end
  - retry next after 1 second
  - continue until one spawns or all fail

### Result
Shop traffic matches the final design document.

---

## Phase 5 — Refactor Desk 1 to match the final customer flow

### Goal
Desk 1 must behave exactly like the final list design.

### Final Desk 1 flow
- customer spawns only if a valid displayed good exists
- Desk 1 cap = 4 total active buyers
- customer reserves browse point before moving
- customer browses for 4-8 seconds
- customer chooses a random valid displayed item
- if no valid displayed item exists at selection time, customer leaves
- item is reserved and taken
- customer joins Desk 1 flow
- player accepts/rejects sale or negotiation
- price uses:
  - base value
  - appeal modifier
  - negotiation modifier

### Existing code likely reusable
- `ShopBuyerNPC`
- browse point concept
- queue logic
- display reservation logic

### Result
Desk 1 becomes aligned to the final design rather than prototype timing/values.

---

## Phase 6 — Move long-term storage ownership out of Shop

### Problem
Current Shop-side storage/money references do not match final architecture.

### Goal
Shop should not permanently own:
- money total
- long-term goods storage
- utility storage

### Final intended structure
- Storage/economy owns persistent values
- Shop requests operations like:
  - add money
  - spend money
  - remove displayed good from storage
  - return unsold good to storage

### Result
Shop becomes an interaction system, not the owner of long-term economy state.

---

## Phase 7 — Refactor Desk 2 to match final Desk 2 rules

### Goal
Desk 2 should support:
- Talking
- Request
- Merchant

with the final design counts and ownership split.

### Final Desk 2 ownership
Desk 2 owns:
- active Desk 2 visitors
- interaction flow
- result handling during the Shop day

Desk 2 does not own long-term persistence:
- accepted requests go to Guild
- gained info goes to Guild
- purchased merchant goods go to Storage
- spent money goes through Storage/economy

### Existing code likely reusable
- `ServiceVisitorSpawner`
- visitor subtype idea
- service desk manager logic
- request/talking/merchant-specific scripts

### Result
Desk 2 becomes interaction-focused and delegates persistence correctly.

---

## Phase 8 — Refactor Desk 3 to match final machine flow

### Goal
Desk 3 should follow the final 3-spot flow.

### Final ownership flow
- Spot 1: Desk 3 visitor = Shop-owned candidate
- Spot 2: Machine interaction = still Shop-owned temporary candidate
- Spot 3: Bed confirmation = Recruit Quarters takes ownership

### Final Desk 3 rule
Recruit Quarters takes over only when:
- machine result is confirmed
- bed is chosen/confirmed

### Existing code likely reusable
- `HireVisitorSpawner`
- recruit generation concept
- `RecruitQuartersManager`
- hire desk manager logic

### Result
Desk 3 and Recruit Quarters are cleanly separated.

---

## Phase 9 — Add Shop Appeal as real Shop core logic

### Goal
Shop Appeal must become a real core system, not just scattered effects.

### Final Appeal responsibilities
- clamp 0–100
- affect buy customer count
- affect sale price modifier
- change from:
  - sale accept/reject
  - merchant outcomes
  - request outcomes
  - talking outcomes
  - hire accept/reject
  - sending recruit away

### Result
Appeal becomes a central driver of Shop behavior.

---

## Phase 10 — Add daily report tracking as a core service

### Goal
Track all report values during the day in one place.

### Report tracks
- Day
- Money made
- Money spent
- Requests accepted
- Info gained
- Hires accepted
- Shop Appeal change
- Total customers served
- Total customers rejected

### Result
Closing the Shop becomes a clean summary step.

---

## Phase 11 — Add early close rules from final design

### Goal
Close warning must match final logic.

### Final rule
Show close confirmation only if meaningful unresolved visitors remain:
- active visitor exists
- Desk 2 unspawned entries remain
- Desk 3 unspawned entries remain
- buy entry remains and valid displayed good exists

If only blocked buy customers remain and no valid displayed goods exist:
- no warning

### Result
Close flow matches intended player experience.

---

## Phase 12 — Final cleanup / naming pass

### Goal
After the system works, rename and reorganize scripts to match real responsibilities.

### Likely naming changes
- `ShopManager` → `Desk1Manager` or `ShopDesk1Manager`
- `ShopNpcTrafficManager` → removed or replaced by `ShopCoreManager` visitor cycle logic
- move Shop-owned scripts under clearer core/desk folders
- move storage ownership scripts out of Shop if needed

### Result
Project structure reflects actual architecture and is easier to maintain.