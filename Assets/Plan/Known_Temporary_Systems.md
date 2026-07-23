# Known Temporary Systems

Referenced from `Project_Status.md`. This is a running log of every debug/placeholder shortcut added to the codebase — things that stand in for a real system that doesn't exist yet. Each entry should say what it fakes, why, and which future stage replaces it. Add to this file the same session you add the shortcut; don't let it fall behind.

---

## Core upgrade slot — debug-unlocked
**Where:** `CoreRoomManager.UnlockUpgradeSlotDebug()`
**What it fakes:** the Dwarf's Upgrade Board granting Core an upgrade slot at a real milestone.
**Why it's temporary:** the Dwarf and the Upgrade Board don't exist yet (Stage 8).
**Replaced by:** Stage 8 — Dwarf's Upgrade Board calls this same unlock instead of a debug trigger.

---

## Data Stick acquisition — debug-triggered
**Where:** `DataStickConsumer.debugStickToAcquire` + the `Debug: Acquire Assigned Stick` context-menu action.
**What it fakes:** a Data Stick actually reaching the base through one of its real sources.
**Why it's temporary:** none of the real acquisition sources exist yet — sector loot (Stage 11), the Merchant's Data Stick pedestal (Stage 6), or request/talking visitors (Stage 7 / Stage 5).
**Replaced by:** Stage 6 (Merchant) now calls `DataStickConsumer.Acquire()` for real via its Data Stick pedestal — this shortcut is resolved for that source. Sector loot (Stage 11) and request/talking visitors (Stage 7/5) still don't exist, so the debug button remains useful for testing those paths until then. Safe to leave in the code as a manual test tool either way.

---

## Gear Upgrade Station level — debug-upgraded
**Where:** `GearUpgradeStationManager.TryUpgradeStationLevel()` + the `Debug: Upgrade Station Level` context-menu action.
**What it fakes:** the Dwarf's Upgrade Board raising the station's own level (LV1→LV2→LV3), same concept as Core's upgrade slot above.
**Why it's temporary:** same reason as Core's entry — the Dwarf/Upgrade Board doesn't exist yet.
**Replaced by:** Stage 8, same as Core's.

---

## Data Stick duplicate-conversion value — placeholder number
**Where:** `DataStickItem.duplicateCoinValue` (default 10).
**What it fakes:** a real, balanced payout for acquiring a Data Stick whose recipe is already unlocked.
**Why it's temporary:** exact value is explicitly TBD pending balancing (`Room_Workshop.md` Section 21, `Open_Architecture_Questions.md`).
**Replaced by:** no stage owns this specifically — revisit during the Stage 12 polish/balancing pass, or earlier if a specific stage's testing makes the placeholder value obviously wrong.

---

## Decor spot options — hand-assigned list, no ownership pool
**Where:** `DecorSpot.availableOptions`
**What it fakes:** a real "owned decor" inventory that a spot's options should draw from.
**Why it's temporary:** there's no way to acquire decor yet — that needs the Merchant (Stage 6) to sell it.
**Replaced by:** Half-resolved by Stage 6 — the Merchant can now sell decor (routes into the new `DecorStorage`), so a real ownership pool exists. `DecorSpot.availableOptions` itself hasn't been updated yet to actually read from `DecorStorage` filtered by `slotType` — that's the remaining piece, not blocked on anything now.

---

## Display Types and Effects — entirely unbuilt
**Where:** `ShopManager.GetDisplayPriceMultiplier()` (hardcoded to return `1.00f`)
**What it fakes:** Room_Shop.md Section 15's gem/wood/metal display bonuses and 1-slot/4-slot modifiers.
**Why it's temporary:** never built — no display-type field exists anywhere on `DisplayStand`. Deferred deliberately (see Project_Status.md decision log), not an oversight.
**Replaced by:** whichever future stage actually needs display bonuses to matter — no stage currently owns this.

---

## Buyer-type personalities — entirely unbuilt
**Where:** `ShopBuyerNPC` (only has a continuous random negotiation multiplier, no discrete buyer types)
**What it fakes:** Room_Shop.md Section 13's Normal/Haggle/Generous/Non-buyer categories, and by extension Decor's `GenerousBuyer`/`HaggleReduction`/`NonBuyerReduction` effect categories (`DecorManager` has the getters, nothing calls them).
**Why it's temporary:** deliberately deferred, pre-sanctioned by Section 30 ("simplified buyer logic before full personality behavior").
**Replaced by:** no stage currently owns this — build it whenever buyer personality variety actually gets prioritized, then wire the three existing `DecorManager` getters into the new roll.



Where: RecruitGenerator.freeRecruitChance (60/40 default)
What it fakes: a real design rule for how often a generated recruit visitor is Free vs Paid, now that capacity no longer splits by type.
Why it's temporary: no such rule exists in the docs yet — this was an arbitrary stand-in to unblock the Desk Three merge.
Replaced by: whenever a real Free/Paid generation rule gets designed.

- RecruitQuartersDebugSeeder (Assets/Buildings/Recruit Quarters/Scripts) — debug-only recruit seeding via context menu, for testing capacity/levels before Dwarf (Stage 8) seeds real starting recruits. Safe to delete once Stage 8 lands.
- ShopCoreManager.ModifyAppeal(int delta) — flat clamped-delta stub. Stage 5 needs to confirm this is sufficient or replace it with real Appeal rules (Room_Shop.md Section 18).
- RecruitData.activeStatusEffects — data slot only, no effect/lockout logic yet. Druid (Stage 9) owns applying/treating these.
