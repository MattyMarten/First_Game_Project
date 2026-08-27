# Build Checklist

**New chat starting on this project?** Read in this order: `Master_Plan_v2.md` (what the game is), `Code_Audit_v2.md` (what exists vs. what's new), then this file (what's actually done). Pasting the GitHub repo link is enough — no other setup needed.

Check items off as you finish them. Add new lines as needed — this file is meant to be edited freely, not regenerated. `[x]` = done and matches the current plan. `[ ]` = still to do. Each item's "needs" note is the short version of what it takes to work — see the audit doc for the full reasoning.

---

## Global / Foundation
- [x] Day Phase System (Morning/Day/Evening/Night) — matches plan, no changes needed.
- [x] Interaction base class (`Interactable.cs`) — ready to build on.
- [ ] **Interaction control state** — needs: extend `InputModeManager` with a 3rd mode; define a per-interactable camera anchor + lock/return behavior. Build this first, everything below plugs into it.

## Shop
- [ ] Single desk (merge buyer + talker flow) — needs: adapt DeskOne's buyer/desk flow, fold DeskTwo's talker dialogue in, translation-display UI.
- [x] Displays (self-service, stock-gated) — matches plan, no changes needed.
- [ ] Water Dispenser + filter maintenance — needs: built from scratch; auto-pay on refill; red-light filter state; filter as a craftable item.
- [ ] Remove Decor system — needs: delete Decor scripts entirely.
- [ ] Decide: keep or cut "Dirt" system — needs: your call, doesn't map to current plan.

## Quest Board
- [ ] 6 slots, 1–2 NPCs/day, 0% at full, 3-day silent expiry — needs: adapt existing board manager to these numbers, drop Appeal linkage.
- [ ] Remove Quest Rank system — needs: delete `QuestRankManager`, strip rank fields elsewhere.

## Recruit Machine
- [ ] Physical queue + "next" interaction — needs: built from scratch.
- [ ] Name/Color/Class selection UI — needs: built from scratch; **blocked on finalizing the 4 classes** (proposed, not confirmed).
- [ ] Random trait assignment on accept — needs: built from scratch.
- [ ] Spawn-chance formula (100/75/50 by count, 0% if no bed) — needs: implement; confirm formula past 3 recruits if room upgrades are in scope.
- [ ] Remove old Free/Paid generation logic — needs: replace `RecruitGenerator`'s logic (roster manager may survive).

## Workshop
- [x] Grinder (loot → materials each Morning) — matches plan, no changes needed.
- [ ] Merge into single Workbench (Goods + Gear + Accessories) — needs: extend crafting recipes to cover all 3 categories.
- [ ] Multi-tool (dig/attack/scan/cut) — needs: built from scratch.
- [ ] Remove Gear Upgrade Station — needs: delete entirely (cut from demo).
- [ ] Remove Utility Station — needs: delete entirely (replaced by multi-tool).
- [ ] Finalize material list (~8–12 named materials) — needs: your input, still open.
- [ ] Finalize Accessory catalog — needs: your input, buff type known, specific items not.

## Core
- [x] Coin deposit + running state — foundation exists, no changes needed.
- [ ] Blackout state (shop + crafting offline) — needs: built from scratch.
- [ ] Hand-cranked Grinder fallback — needs: bypass power-check during blackout.
- [ ] Emergency material refuel (10 materials, one cycle) — needs: built from scratch.
- [ ] Blackout coin-recovery path — needs: still an open design question (deferred).

## Recruit Quarters
- [x] Beds, lockers, wall signs, interact-to-switch-control — foundation exists, no changes needed.
- [ ] 3-part locker (accessories / gear grid / backpack) — needs: wire existing Grid Inventory (already shape/rotation-capable) into the 3 sections.
- [ ] Death → locker empties / backpack drop-and-recover — needs: built from scratch.

## Storage
- [x] Virtual/abstract storage — matches plan, no changes needed.

## Undead Capsule
- [ ] Single capsule, keep/replace on death — needs: built from scratch.
- [ ] Bad-trait accumulation (up to 3) — needs: built from scratch.
- [ ] Slot-consuming inclusion in personal team — needs: built from scratch.

## Expedition — Personal Team
- [x] Session/roster data — foundation exists (`ExpeditionManager`/`SessionData`/`MemberData`).
- [ ] Safehouse + per-recruit staggered Battery timer — needs: built from scratch.
- [ ] Death → auto-switch to next living recruit — needs: built from scratch.
- [ ] Rescue mechanic — needs: built from scratch.

## Expedition — Away Team
- [ ] Material-focus selection — needs: built from scratch.
- [ ] Per-recruit survival roll vs. sector risk % — needs: not yet in code, implement per original doc's formula.
- [ ] Loot-amount resolution (scaled by count/class) — needs: built from scratch.
- [ ] Results display (both teams, side by side) — needs: built from scratch.

## Expedition Launch
- [ ] Two select stations (personal + away), no map — needs: rework existing entry-point UI away from map-based selection.

## Expedition Site — Map & Threats
- [ ] Chunk-based main-locations + procedural corridors — needs: built from scratch.
- [ ] Line-of-sight / noise detection system — needs: built from scratch, foundation for nearly every enemy below.
- [ ] Passive creatures — needs: built from scratch.
- [ ] Chaser (aggressive) — needs: built from scratch.
- [ ] The Doll (aggressive) — needs: built from scratch.
- [ ] Third aggressive monster — needs: design not finalized yet.
- [ ] Traps (territorial) — needs: built from scratch.
- [ ] Hiding-spot ambush creature (territorial) — needs: built from scratch.
- [ ] Spiders/webbing (territorial) — needs: built from scratch.
- [ ] Dig spots, breakable gems, wire-minigame doors — needs: built from scratch.
- [ ] Vents — needs: still undecided, possibly cut.
