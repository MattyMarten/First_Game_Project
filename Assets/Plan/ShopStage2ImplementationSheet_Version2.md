# Shop Stage 2 Implementation Sheet

## Goal
Introduce the real Shop-wide core manager and stop treating current `ShopManager` as the whole Shop manager.

This step is about structure and authority, not final visitor behavior.

---

## Main outcome of Stage 2
After this step:
- a real `ShopCoreManager` exists
- current `ShopManager` is treated as Desk 1 manager
- Desk 1 / Desk 2 / Desk 3 are all clearly subordinate to Shop core
- old traffic authority is no longer considered the final architecture

---

# Step 2.1 — Create `ShopCoreManager`

## Purpose
Create the real top-level Shop authority.

## Script
Create a new script:
- `ShopCoreManager.cs`

## Initial responsibilities only
At this stage it only needs to:
- know whether the Shop is open
- hold references to:
  - Desk 1 manager
  - `ServiceDeskManager`
  - `HireDeskManager`
- hold references to:
  - shared spawn point
  - shared exit point

## Important
Do **not** try to put the full visitor list / Appeal / report logic in it yet.
This is just the first anchor.

## Why
You need the whole-Shop authority object to exist before deeper refactors.

---

# Step 2.2 — Add a `ShopCore` object in hierarchy

## Purpose
Give the Shop core a real scene/home object.

## Hierarchy suggestion
Under `ShopRoot`, add:
- `ShopCore`

Attach:
- `ShopCoreManager`

## Assign references
Assign:
- Desk 1 manager (`ShopManager` for now)
- `ServiceDeskManager`
- `HireDeskManager`
- shared spawn point
- shared exit point

## Why
This makes the architecture visible in the scene, not just in code.

---

# Step 2.3 — Reclassify current `ShopManager`

## Purpose
Stop treating current `ShopManager` like the whole Shop manager.

## What to do
- keep the script for now
- do not rename file yet unless you want to
- mentally/documentationally treat it as:
  - `Desk1Manager`
- ensure its responsibilities stay Desk-1-only

## What it should own for now
- active buyers
- browse points
- queue spots
- registered displays
- pending sale
- Desk 1 capacity

## What it should stop being thought of as
- the owner of the whole Shop
- the owner of final Shop-wide state

## Why
This is necessary so later Shop-core logic has a place to live.

---

# Step 2.4 — Confirm Desk 2 and Desk 3 manager references

## Purpose
Make sure the Shop core points to real desk subsystems.

## What to do
- verify `ServiceDeskManager` exists in scene
- verify `HireDeskManager` exists in scene
- connect them to `ShopCoreManager`

## Why
The Shop core must coordinate desks later.

---

# Step 2.5 — Mark old traffic logic as temporary

## Purpose
Stop designing around old traffic authority.

## What to do
- keep `ShopNpcTrafficManager` only if you still need temporary functionality
- but treat it as prototype-only
- do not build new logic into it
- if possible, disable it in scene for pure architecture testing

## Why
It will eventually be replaced by Shop core spawn-cycle logic.

---

# Step 2.6 — Decide temporary spawn mode for transition

## Purpose
Prevent confusion while refactoring in stages.

## Choice to make
During transition, either:
- keep local spawner timers temporarily for testing
or
- disable them and use only manual testing

## Recommendation
Keep temporary local spawn if needed **only for testing**, but clearly mark it as temporary.

## Why
You do not need to solve final spawn logic in this step.

---

# Step 2.7 — Do not move money/storage yet, only note it

## Purpose
Avoid mixing too many refactors into one step.

## What to do now
- do **not** fully move money/storage ownership yet
- just note clearly:
  - current `ShopManager` money is temporary
  - final ownership will move to Storage/economy later

## Why
This step is about authority structure first.

---

# Step 2.8 — Stage 2 validation checklist

## After setup, confirm:
- [ ] `ShopCoreManager` exists
- [ ] `ShopCoreManager` is in hierarchy
- [ ] `ShopCoreManager` references Desk 1 / Desk 2 / Desk 3 managers
- [ ] current `ShopManager` is now treated as Desk 1 manager only
- [ ] `ShopNpcTrafficManager` is no longer treated as final architecture
- [ ] old local spawn logic is understood as temporary if still enabled
- [ ] no attempt was made yet to force final visitor list logic into this step

---

# What to send me after Stage 2
When you finish this step, send:

## 1. Progress summary
Example:
- “Finished Stage 2”
- “Added `ShopCoreManager`”
- “Connected Desk 1 / Desk 2 / Desk 3 references”
- “Traffic manager still exists but is temporary only”

## 2. What currently works
Example:
- Desk 1 still works
- Desk 2 still works
- Desk 3 still works
- ShopCore exists

## 3. What feels uncertain
Example:
- not sure if `ShopManager` still owns too much
- not sure where shared spawn should live
- not sure if traffic manager should be disabled already

## 4. Updated files
At minimum likely:
- `ShopCoreManager.cs`
- any changed `ShopManager.cs`
- any changed scene/setup notes if relevant

Then the next step will be:
- unify spawn authority planning
- or begin splitting Shop-wide state away from Desk 1