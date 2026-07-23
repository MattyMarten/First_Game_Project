# Open Architecture Questions

(Structure/purpose/template sections unchanged from previous version — see prior file. This version updates Section 4 "Current Open Questions" and Section 6 "Recently Resolved Questions" to reflect this session's design discussion.)

---

## 4. Current Open Questions (updated this session)

### Question
- Should RP/knowledge remain under Workshop/Research Station ownership long-term, or later split into a more separate research system?

### Status
**Resolved by removal.** The Research Station, RP, and material knowledge tiers no longer exist. Moved to Section 6 (Recently Resolved).

---

### Question
- How should map / expedition planning details be finalized during the map review pass?

### Status
**Superseded.** The fixed-location Map design (Forsaken City) has been replaced entirely by the sector category system. This question is now folded into the new open questions below rather than being a standalone cleanup item.

---

### Question (new this session)
- Exact milestone that unlocks the Core's upgrade slot on the Upgrade Board

### Why it matters
- The Core is now upgradeable (a deliberate reversal of its previous fixed-forever design), but the upgrade slot should not be available from the start — it needs a real trigger tied to progression/story.

### Affects
- Core Room, Dwarf Room / Upgrade Board, Progression/Unlock System

### Current assumption
- Likely tied to the hidden base progression score or a specific main-line quest beat, exact trigger TBD

### Blocks work?
- [ ] Yes
- [x] No — Core can launch with the upgrade slot permanently locked (debug-unlockable for testing) until this is decided

### Must be decided by
- before Core upgrade implementation

---

### Question (new this session)
- Full list of sector categories beyond N (Nature) and L (Labs)

### Why it matters
- Only two categories are currently named; the broader unlock graph, hazard ratings, and loot pools for any further categories are undesigned.

### Affects
- Sector Map, Dispatch Board, Core range values, Suit hazard-gating values

### Current assumption
- Intentionally deferred — first-pass implementation only needs N and L to prove the unlock-graph mechanic works

### Blocks work?
- [ ] Yes
- [x] No

### Must be decided by
- before content expansion beyond the first two sectors

---

### Question (new this session)
- Exact away-team success % formula and loot scaling

### Why it matters
- The Dispatch Board needs a concrete formula to resolve outcomes, and none has been finalized yet.

### Affects
- Dispatch Board, Graveyard (death volume), recruit scarcity pacing

### Current assumption
- Placeholder shape proposed: base % + recruit level factor − sector hazard factor, clamped between reasonable bounds; each recruit on the team rolls independently against this %

### Blocks work?
- [x] Yes — blocks Dispatch Board implementation specifically
- [ ] No

### Must be decided by
- before Dispatch Board implementation

---

### Question (new this session)
- Exact Suit Station component values and whether components have a soft cap

### Why it matters
- Battery, Shoes, Mask, and Suit Material all need real numbers before the Suit Station can be implemented or balanced against sector hazard ratings.

### Affects
- Workshop / Suit Station, Sector Map gating, Dispatch Board gating

### Current assumption
- Placeholder — no numbers locked yet

### Blocks work?
- [x] Yes — blocks Suit Station implementation specifically
- [ ] No

### Must be decided by
- before Suit Station implementation, and before sector hazard values are finalized (the two need to be designed together)

---

### Question (new this session)
- Does the camp tier system still apply to sector categories, or was it only meaningful for the old fixed-location Map design?

### Why it matters
- Camp Tier 1/2/3 (storage/transmission features) was designed around persistent named entries at one fixed location. Sector instances are now randomized and non-persistent, so it's unclear whether "camps" still make sense per category.

### Affects
- Sector Map Room

### Current assumption
- Undecided — flagged directly in Room_Map.md Section 12 as an open design question

### Blocks work?
- [ ] Yes
- [x] No

### Must be decided by
- before Sector Map implementation goes beyond a first pass

---

### Question (new this session)
- Merchant's price variance: should it be ±10% (as Room_Merchant.md Section 10 states for the Data Stick pedestal, presumably meaning it's the project-wide standard) or ±20% (what the old MerchantDayManager.GeneratePrice code actually implemented)?

### Why it matters
- The two disagree, and MerchantRoomManager (Stage 6) needed to pick one to ship with. Affects how much daily price fluctuation the player sees on every pedestal, not just Merchant's — if ±10% really is meant to be project-wide, ShopManager's own sale-price variance (if any) should probably match it too.

### Affects
- Merchant Room, potentially Shop's sale-price variance if the two are meant to be consistent

### Current assumption
- MerchantRoomManager.priceVariance defaults to ±10% (matches the doc's literal words), serialized and easy to flip to ±20% in the Inspector once this is confirmed

### Blocks work?
- [ ] Yes
- [x] No — either value works mechanically, this is pure balancing

### Must be decided by
- whenever Merchant pricing gets a real balancing pass, or Stage 12 polish at the latest

---

### Question (new this session)
- What's the full intended contents of the Merchant's curated Data Stick pool, beyond the one recipe that currently exists?

### Why it matters
- Room_Merchant.md Section 23 already flagged this as open. A full audit this session found only ONE recipe in the entire project is actually locked-by-default (Basic Shovel T1 — Basic Shovel T2 was also locked but that was a mistake, now fixed to unlocked-by-default). The curated pool is seeded with just that one Data Stick for now.

### Affects
- Merchant Room, Workshop (any future recipe that gets locked behind a Data Stick needs a corresponding DataStickItem asset made and added to this pool)

### Current assumption
- Pool grows organically as more recipes get locked; no rush to pad it out artificially

### Blocks work?
- [ ] Yes
- [x] No — an effectively-empty/single-entry pool is a valid state, the pedestal just stays empty most days it would've rolled a stick

### Must be decided by
- whenever more recipes get locked behind Data Sticks, or Stage 12 polish

---

## 6. Recently Resolved Questions (additions this session)

### Question
- Should RP/knowledge remain under Workshop/Research Station ownership long-term, or later split into a more separate research system?

### Final decision
- Removed entirely. Research Station no longer exists. Recipes unlock via Data Sticks instead.

---

### Question
- Should the Core ever be upgradeable?

### Final decision
- Yes. This deliberately reverses the previous "Core is intentionally not upgradeable" rule, because the Core's growth is now tied directly to the game's core story (rebuilding toward SAM). Core upgrades go through the Dwarf's Upgrade Board like any other room, but the upgrade slot is milestone-locked until unlocked.

---

### Question
- Should material knowledge tiers exist in any form?

### Final decision
- No. Materials are fully flat — no tiers, no type variants. A recipe's requirement is just a flat quantity of a flat material.

---

### Question
- Should recruit debuffs ever fully lock a recruit out of being played?

### Final decision
- No. Every debuff is a harsh penalty only. This removes the need for any softlock safety net.

---

### Question
- What replaces the fixed single starting location (Forsaken City)?

### Final decision
- A sector category system: named categories (starting with N — Nature, and L — Labs), each generating a fresh random instance per visit, unlocked permanently at the category level via in-expedition puzzle-solving + generator-powering, gated further by Core range and Suit hazard resistance.

---

### Question
- Is there a fixed expedition time limit?

### Final decision
- No. The previous fixed 25-real-minute / 05:00-in-game-time rule is removed entirely. The only limit on expedition duration is the Suit's Battery charge, which is permanently upgradeable and shared across all recruits.

---

## Notes
Unchanged from previous version.
