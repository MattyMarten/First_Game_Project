# Shop Refactor Checklist

## Rule for this checklist
The final Shop design document is the source of truth.
Current code is reused only when it helps reach that design.

---

# Stage 0 — Freeze the target

## Goal
Make sure the team/dev process always follows the final Shop design, not current prototype behavior.

### Checklist
- [ ] Treat the written Shop design as the final target
- [ ] Stop preserving current code behavior just because it already works
- [ ] Mark current systems as:
  - keep
  - refactor
  - replace

### Explanation
This prevents prototype logic from silently becoming permanent architecture.

### Current likely categories
- Keep/refactor:
  - `DisplayStand`
  - `ShopBuyerNPC`
  - `ShopBrowsePoint`
  - `ShopQueueSpot`
  - `ServiceDeskManager`
  - `ServiceVisitorSpawner`
  - `HireDeskManager`
  - `RecruitGenerator`
  - `RecruitQuartersManager`
- Replace/refactor heavily:
  - `ShopNpcTrafficManager`
  - `ShopManager` as the overall Shop authority
  - local autospawn as the final spawn logic
  - money ownership inside Shop

---

# Stage 1 — Define the real Shop core

## Goal
Create the concept of a true Shop-wide core manager.

### Checklist
- [ ] Decide on a final name:
  - `ShopCoreManager`
  - or `ShopDayManager`
- [ ] Define it as the single owner of:
  - shop open/close state
  - daily visitor list
  - spawn cycle
  - Shop Appeal
  - daily report tracking
  - high-level desk coordination
- [ ] Remove the idea that current `ShopManager` is the whole Shop manager

### Explanation
Right now `ShopManager` is mostly Desk 1 logic.  
The final design needs a true Shop-wide authority above all desks.

### Scripts affected
- current `ShopManager`
- `ShopNpcTrafficManager`
- all spawners

### Expected outcome
You now have:
- one Shop-wide core
- Desk 1 manager below it
- Desk 2 manager below it
- Desk 3 manager below it

---

# Stage 2 — Rename responsibilities before rewriting behavior

## Goal
Clarify what current managers really are.

### Checklist
- [ ] Reclassify current `ShopManager` as Desk 1 manager logic
- [ ] Keep `ServiceDeskManager` as Desk 2 manager
- [ ] Keep `HireDeskManager` as Desk 3 manager
- [ ] Make sure future Shop Core does not absorb desk-local logic

### Explanation
This is mostly a planning/architecture step first.
Even if script files are not renamed immediately, your mental model must change now.

### Current role map
- `ShopManager` → Desk 1 manager
- `ServiceDeskManager` → Desk 2 manager
- `HireDeskManager` → Desk 3 manager
- future `ShopCoreManager` → real whole-Shop manager

### Expected outcome
Every system has one clear responsibility layer.

---

# Stage 3 — Unify spawn authority

## Goal
Only one system decides when visitors appear.

### Checklist
- [ ] Disable/retire local autospawn as the final authority
- [ ] Disable/retire random traffic-manager-based spawning as the final authority
- [ ] Make Shop Core the only system that schedules visitor appearances
- [ ] Convert desk spawners into spawn executors only

### Explanation
Right now multiple systems can influence spawning:
- buyer spawner timer
- service spawner timer
- hire spawner timer
- traffic manager

That conflicts with the final design.

### Final intended roles
- Shop Core = decides what visitor entry to spawn
- `ShopBuyerSpawner` = executes buyer spawn
- `ServiceVisitorSpawner` = executes Desk 2 spawn
- `HireVisitorSpawner` = executes hire spawn

### Scripts affected
- `ShopNpcTrafficManager`
- `ShopBuyerSpawner`
- `ServiceVisitorSpawner`
- `HireVisitorSpawner`

### Expected outcome
One source of truth for shop traffic.

---

# Stage 4 — Build the final daily visitor list

## Goal
Match the final written visitor generation system.

### Checklist
- [ ] Generate the daily visitor list when the Shop opens
- [ ] Include:
  - buying customers
  - talking visitors
  - request visitors
  - merchant visitor if day-eligible
  - hire visitors
- [ ] Use Shop level and Appeal to determine counts
- [ ] Randomize final list order
- [ ] Store it centrally in Shop Core

### Explanation
The final design is based on a daily list, not repeated ad-hoc random spawn checks.

### Important design rules to implement
- buy count = Shop level range modified by Appeal
- merchant = day-based rule
- talking/request counts fixed by Shop level rules
- hires based on Shop level rules

### Expected outcome
The whole Shop day is preplanned at open time.

---

# Stage 5 — Build the final spawn cycle

## Goal
Implement the exact final spawn timing behavior.

### Checklist
- [ ] Start a spawn cycle every 8 seconds
- [ ] Check the front entry of the list
- [ ] If it can spawn:
  - spawn it
  - remove it from the list
- [ ] If it cannot spawn:
  - move it to the end of the list
  - wait 1 second
  - try next
- [ ] Stop the cycle when:
  - one visitor spawns
  - or all current entries have failed

### Explanation
This replaces the current random/independent timing systems.

### Expected outcome
Visitor flow matches the final written Shop rules.

---

# Stage 6 — Refactor Desk 1 to exact final behavior

## Goal
Make Desk 1 match the written design exactly.

### Checklist
- [ ] Desk 1 cap must be 4 active buyers total
- [ ] Buyer can spawn only if:
  - a valid displayed good exists
  - Desk 1 cap allows it
- [ ] Buyers reserve browse points before moving
- [ ] Browse time becomes exactly 5 seconds
- [ ] Buyer chooses a random valid displayed item after browsing
- [ ] If no valid displayed item exists then, buyer leaves
- [ ] Sale uses:
  - base value
  - Appeal price modifier
  - negotiation modifier

### Explanation
Current Desk 1 behavior is close but not exact.
This step makes it match the final design.

### Scripts likely involved
- `ShopManager` (future Desk 1 manager)
- `ShopBuyerNPC`
- `ShopBrowsePoint`
- `ShopQueueSpot`
- `DisplayStand`

### Expected outcome
Desk 1 is fully aligned to the final design.

---

# Stage 7 — Refactor display/storage boundary

## Goal
Move long-term inventory ownership out of Shop.

### Checklist
- [ ] Decide that displays only own active display-slot contents
- [ ] Move long-term goods ownership to Storage
- [ ] Move money ownership to Storage/economy
- [ ] Make Shop request inventory/money operations instead of owning them

### Explanation
Shop should not own persistent economy values.

### Keep in Shop
- displayed goods
- display reservations
- item currently reserved for a buyer

### Move outside Shop
- total stored goods
- utilities
- money
- materials

### Expected outcome
The Shop uses Storage/economy instead of duplicating it.

---

# Stage 8 — Refactor Desk 2 to final rules

## Goal
Make Desk 2 match the final written design and ownership split.

### Checklist
- [ ] Desk 2 cap must be 3
- [ ] Desk 2 supports:
  - Talking
  - Request
  - Merchant
- [ ] Desk 2 only handles interaction flow
- [ ] Accepted requests go to Guild
- [ ] Info gained goes to Guild
- [ ] Merchant purchases go to Storage/economy
- [ ] Merchant day logic comes from Shop Core visitor list, not Desk 2 local planning

### Explanation
Desk 2 should be interaction-focused, not the owner of progression/storage state.

### Scripts likely involved
- `ServiceDeskManager`
- `ServiceVisitorSpawner`
- request/merchant/dialogue scripts
- Guild-facing systems

### Expected outcome
Desk 2 becomes clean and modular.

---

# Stage 9 — Refactor Desk 3 to final 3-spot flow

## Goal
Make Desk 3 match your final machine/bed handoff design.

### Checklist
- [ ] Survivor starts as a Shop-owned hire candidate
- [ ] Desk 3 accept sends candidate to machine
- [ ] Machine process remains Shop-owned temporary state
- [ ] Machine UI handles class/setup choices
- [ ] Recruit Quarters is queried for bed options/capacity
- [ ] Recruit Quarters takes ownership only when machine result is confirmed and bed is chosen
- [ ] Recruit is then placed/spawned at chosen bed

### Explanation
Current `HireDeskManager.AcceptPendingRecruit()` transfers too early for the final design.

### Scripts likely involved
- `HireDeskManager`
- `HireVisitorSpawner`
- `RecruitGenerator`
- machine/class selection scripts
- `RecruitQuartersManager`
- recruit roster manager

### Expected outcome
Desk 3 and Recruit Quarters have a clean handoff boundary.

---

# Stage 10 — Add real Shop Appeal system

## Goal
Make Appeal a central Shop system.

### Checklist
- [ ] Appeal range 0–100
- [ ] Appeal clamp works
- [ ] Appeal changes are tracked centrally
- [ ] Appeal changes buyer count
- [ ] Appeal changes sale price modifier
- [ ] Appeal reacts to:
  - sales
  - merchant outcomes
  - requests
  - talking
  - hire accept/reject
  - sending recruit away

### Explanation
Appeal should not be scattered and inconsistent.

### Expected outcome
Appeal becomes a real gameplay driver.

---

# Stage 11 — Add daily report tracking

## Goal
Track report data during the day in one place.

### Checklist
- [ ] Track money made
- [ ] Track money spent
- [ ] Track requests accepted
- [ ] Track info gained
- [ ] Track hires accepted
- [ ] Track Appeal delta
- [ ] Track served/rejected buyers
- [ ] Show report on close

### Explanation
The Shop close step should summarize the real day state.

### Expected outcome
End-of-day report matches design.

---

# Stage 12 — Add final close logic

## Goal
Match the final early-close behavior.

### Checklist
- [ ] Shop stays open until player closes it
- [ ] Close warning appears only if meaningful unresolved visitors remain
- [ ] No warning if only blocked buy customers remain and no valid displayed goods exist
- [ ] Active visitors leave on close
- [ ] Unspawned entries are discarded on close
- [ ] Time moves to evening on close

### Expected outcome
Close behavior matches the intended player flow.

---

# Stage 13 — Cleanup / file structure / naming pass

## Goal
Make the project structure reflect actual architecture.

### Checklist
- [ ] Rename/reorganize scripts to match real roles
- [ ] Remove dead/legacy scripts
- [ ] Move storage-domain scripts out of Shop where appropriate
- [ ] Separate Shop core from Desk 1 manager cleanly

### Explanation
Do this after behavior works, not before.

### Expected outcome
The architecture is maintainable and easier to expand.