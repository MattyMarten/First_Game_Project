# Master Plan v2 — Systems Rework

This document supersedes `Base_Master_Plan.md` for every section it covers. It captures the simplification pass done to make the game buildable solo, plus the first expedition/threat notes. Sections not mentioned here (setting/story premise, sector unlock graph, global resource categories not touched) still stand as written in the original doc.

**How to keep this current:** this file is meant to be edited directly as decisions change — treat it as the single source of truth, not a snapshot. When something here changes, update the section in place rather than appending a correction elsewhere, and use the Open Questions list at the bottom for anything not yet decided.

---

## 1. Core Loop (unchanged)

Morning (08:00, process/craft/prep) → Day (12:00, shop open) → Evening (19:00, prep expedition, deposit Core coins, launch) → Night/Expedition → next Morning. See original doc Section 3 for full phase rules.

Change: recruit visitors and request (quest) visitors are no longer part of the Day-phase shop visitor pool. Both now happen in their own dedicated spaces — see Sections 4 (Recruit Machine) and 3 (Quest Board).

---

## 2. Shop

**Desk (single):** the only checkout point. A customer picks an item off a display, carries it to the desk, and speaks in an untranslated language; a display screen translates it. Player chooses **Accept** or **Decline**.
- Accept: sale completes.
- Decline: item returns to **Storage** (not back onto the display) — must be manually re-placed to sell again.
- Currently pure UI flavor with no mechanical weight; may gain stakes later (kept intentionally simple for the demo).

**Displays:** player places crafted Goods here. Fully self-service — customers walk up and take stock directly, no desk interaction needed to browse.
- **Pricing:** fixed/determined. The crafting station shows the price when the item is made — no separate pricing system, no haggling, no Moonlighter-style reaction layer for the demo (explicitly cut for scope given the 30-day budget).

**Buyer spawning:** throttled by stock — while a display has stock, buyers keep spawning, roughly one every X seconds (exact interval TBD/tuning), capped at **5 customers in the shop at once**. No stock on any display = no buyers spawn.

**Talking customers:** independent of stock — a flat **1–3 chance to appear per day**. Handled at the same desk (dialogue via the translated display, Accept/Decline to respond).

**Water Dispenser:** separate passive station. NPCs refill and pay automatically, no player interaction needed per-visit. Requires a crafted **filter** to keep the water fresh; a red glowing button lights up when the filter needs replacing.

**No shop upgrades in the demo.** No decor changes, no display upgrades — this whole room is static scope-wise.

---

## 3. Quest Board

- **6 slots.**
- **Request NPCs** walk up and place a quest directly on an open slot. Spawn rate: **1–2 NPCs/day**, but **0% if the board is at 6/6** (same throttle pattern as shop buyers/stock).
- Quests pay **money or materials only** — no gear, no Data Sticks.
- **3-day expiry** — an unclaimed/incomplete quest silently disappears, **no penalty**.
- **No rank/XP progression system** (fully removed from the old doc).
- **No Appeal link** — quest accept/decline/complete/expire does **not** affect Appeal. Appeal stays a shop-only stat (buyer count/price, Core failure, recruit retirement — per original doc).

---

## 4. Recruit Machine

Fully replaces the old chance-based shop-visitor recruit system.

- Physical machine with a line of NPCs waiting next to it ("work for home" sign). Player interacts to call the next one in.
- Machine flow: player sets **Name** (editable), **Suit Color**, **Class** (see below). **Trait is randomly assigned**, not chosen.
- Accept → recruit sent to Recruit Quarters.
- **Spawn chance** for a new person joining the line, based on current recruit count: **100% at 1 recruit, 75% at 2, 50% at 3**.
  - Hard rule: **0% regardless of the above if there is no free bed.**
  - Formula beyond 3 recruits not yet defined — see Open Questions (only matters if Recruit Quarters capacity upgrades are in scope for the demo).
- **Game starts with 2 recruits** (fixed, pre-set traits and classes — not randomized), leaving 2 of the 4 starting beds open so the machine is usable from minute one.

### Classes (proposed, 4 total — confirm/adjust)
All effects are flat numeric modifiers, no unique active abilities (kept simple for scope):
- **Scout** — reduced noise/light footprint, higher move speed.
- **Hauler** — increased carry capacity.
- **Medic** — reduces debuff severity/duration, extra value from healing items, improves away-team survival odds.
- **Engineer** — faster/safer at technical interactions (e.g. powering sector connection points).

---

## 5. Workshop

**Grinder** — unchanged. Each Morning, loot collected on expedition appears as a "backbag"; grind it into flat, named materials (no tiers).

**Workbench (single, merged)** — crafts three categories:
| Category | Contents | Notes |
|---|---|---|
| **Goods** | Shop-sellable consumer items | 13 defined so far — see item list below |
| **Gear** | Flashbang, noisemaker, glowstone, backbag, etc. | Locker gear-grid items; occupy 2×1 or 2×2 cells depending on the item (spatial/shaped inventory, not uniform slots) |
| **Accessories** | Equip-slot buffs — more HP, more damage, etc. | 3 equip slots in the locker; specific catalog not designed yet |

Utility items (shovel, axe, etc.) are **cut entirely** — replaced by the multi-tool.

**Goods list (13, demo scope):**
- Metal — Cookpot, Scrap Toolkit, Repair Kit, Tin Lantern
- Electronics — Battery Pack, Handheld Radio, Glow Lamp
- Plants — Healing Poultice, Dried Ration Herbs, Fungal Brew
- Gems — Gem Charm, Polished Gemstone, Ward Trinket
- Found/no-recipe (sold as-is) — Old-World Artifact, SAM Fragment

**Materials:** named/specific (e.g. Steel, Copper) rather than generic "Metal," but kept small — target roughly **2–3 named materials per category (~8–12 total)** for the demo.

**Multi-tool** — one shared tool, available from the start, performs **dig / attack / scan / cut**.

**Upgrade Station (Suit + Multi-tool)** — **cut from the demo.** Suit and multi-tool stay at base/starting stats for the whole demo. Suit stats referenced for the full game: Battery (duration), Antenna (range), Defence, "other stuff" — Antenna's overlap with the Core's existing range-ownership is an open full-game question, not urgent since the station itself isn't in the demo (see Open Questions).

---

## 6. Core & Power

- Core requires **cobalt coins** deposited (Evening phase) to keep running.
- **Blackout** (insufficient coins): shop and crafting (Workbench) go offline. Only **expedition launch** and **expedition prep** (locker loadouts) remain available. The Grinder still works via a manual **hand-cranked fallback** (no power needed).
- **Emergency restart:** feeding **10 raw materials** directly into the Core relaunches it for **one cycle only** — it drops back into blackout the next day unless real coins are deposited. The coin count stays at 0 through this — materials buy time, not a fix.
- **Known open gap (explicitly deferred):** the shop is the primary coin source and it's offline during blackout, so the actual path back to real coins isn't fully resolved. Decided to punt on this until it's actually hit in testing.

---

## 7. Recruit Quarters

- **4 beds, 4 lockers, 4 wall signs** (name/class/traits) — fixed capacity for the demo.
- **Locker, 3 sections:**
  - **Top:** 3 accessory slots — select a slot, scroll available accessories from Storage.
  - **Middle:** gear grid, base 2×4 — spatial/shaped, gear items take 2×1 or 2×2 depending on item.
  - **Bottom:** backpack slot, always present, swappable for a better backpack.
- On successful return: all equipped items go back to the locker automatically.
- On death: locker empties, items are **lost** — unless the player recovers the dead recruit's dropped backpack **within that same expedition run** (sector instances don't persist, so recovery is impossible after leaving or on a future visit).
- Interacting with a recruit **switches player control** to them.
- Beds are **decorative only**, no gameplay function.

---

## 8. Storage

**Virtual/abstract for the demo** — a data container holding all items, no physical room or placement. Physical storage is an intentional future upgrade once the core game exists; nothing about how other systems *use* Storage changes when that happens.

---

## 9. Undead Capsule

- **One capsule** for the demo, holding the most recently died recruit.
- On a further recruit death, player chooses: **keep current undead or replace it**.
- Each time the undead itself dies (it's a usable, controllable unit), it gains a bad trait, **up to 3 max**.
- Cannot be assigned to the away/NPC team. **Can** accompany the player's personal expedition team — doing so **takes one of the 4 personal team slots**.

---

## 10. Expedition — Personal Team

- Player selects **up to 4 units** (recruits and/or the Undead) for their own expedition.
- All start at a **safehouse** inside the expedition site (distinct from the underground Base).
- **Suit Battery only counts down while a unit is outside the safehouse.** Return to the safehouse before Battery runs out to switch control to a different one of the up to 4 brought units, who gets their own fresh Battery timer. Effectively gives up to 4 staggered chunks of exploration time per expedition instead of one shared clock.
- **On death, control auto-switches to the next living brought unit** — the expedition continues, it doesn't end.
- Expedition ends when **all brought units are down**, or the player chooses to return to base.
- **Rescue mechanic:** a unit left in the field (e.g. stuck in a trap) can be actively rescued by another controlled unit and told to run back to the safehouse.

---

## 11. Expedition — Away/NPC Team

- Up to **4 recruits** (not the Undead) dispatched without the player present.
- Player selects a single **material-category focus** per dispatch — increases the odds of that category in the loot, but the result stays **weighted-random across categories**, not an exclusive guarantee.
- **Survival:** reuses the existing per-recruit independent roll against the destination sector's risk % (from the original doc) — no new formula needed.
- **Loot amount** scales with recruit count and class bonuses (e.g. Hauler's carry bonus applies here too).
- Results shown on a dedicated display, alongside the personal-expedition results (two displays, side by side, on return).

---

## 12. Expedition Launch (demo scope)

**Map is skipped for the demo.** Instead: **two simple select stations** — one to configure/launch the personal team, one to configure/dispatch the away team. Only **one sector "type" exists for the demo** (no multi-category unlock graph yet), but built large and loot-dense enough to compensate.

---

## 13. Expedition Site — Map & Threats

**Generation approach:** a handful of hand-built "main locations," each with **5 variants** that rotate, connected by **procedurally generated corridors** — modular chunk-based, chosen specifically to be easier to build than full procedural generation.

**Enemies:**

*Passive* — ambient roaming creatures, make noise when disturbed. Atmosphere/detection-risk, not directly dangerous.

*Aggressive* (3 planned):
1. **Chaser** — patrols, actively searches for the player, periods of disengagement.
2. **The Doll** — Weeping-Angel-style. Doesn't move while directly observed. Spawns periodically, rushes the player when active. Looking at it stops its approach (or it damages you up close). Sustained eye contact for **20 seconds** flips its expression (happy → sad) and it flees/despawns.
3. Undecided — TBD.

*Territorial* (3 planned):
1. Static traps: lasers, mines, pits/holes.
2. Ambush creatures occupying hiding spots — risk when the player tries to use the same spot for stealth.
3. Web-spinning creatures ("spiders") that immobilize the player at close range.

**World interactions:** dig spots (may or may not have loot), gems broken open with the multi-tool, locked doors with a wire-based unlock minigame. **Vents undecided** — possibly cut, flagged as hard to implement.

**Shared infrastructure note:** almost every enemy type (passive noise reaction, chaser search, the Doll's look-mechanic, hiding-spot ambush) depends on one underlying **line-of-sight/noise-detection system** — build that once, it serves nearly the whole roster.

---

## 14. Production Notes

- **Build order:** finish the base/economy loop first (shop, quest board, recruit machine, workshop, core, lockers, away-team resolution) — it needs none of the expedition map/enemy content to function. Build the explorable expedition (map, enemies, interactions) as a separate second phase.
- **Physical/diegetic interaction UI — build FIRST**, before individual base systems. New **"Interaction"** control state (alongside existing "Game" and "Inventory" states): locks the camera into a fixed position at an interactable and allows interacting with it. Nearly every base system (desk, workbench, quest board, recruit machine, lockers, Core deposit, OLD PC) depends on this existing already — build once, everything else plugs in, rather than retrofitting it onto systems built without it.
- **Sound:** split into setup vs. content.
  - Setup (early, in parallel with base systems): integrate **Steam Audio** (Valve's free, open-source, actively maintained spatial audio SDK, official Unity plugin), wire basic audio-event hooks into each system as it's built.
  - Content (dedicated later pass, matches the "atmosphere pass" already in the original 8-week plan): final SFX, ambiance, mixing. Don't compress this to the last few days.
- **Animation:** same split — basic trigger/state-machine architecture and placeholder motion early, especially for anything whose *feel* depends on timing (the Doll's 20-second stare mechanic needs to be testable early). Final polished animation in the later dedicated pass.
- **OLD PC** — command-line terminal, fills the existing "Info/Computer" station slot from the original doc, retro flavor.
- Claude explains any code/scripts written going forward, not just delivers them.

---

## 15. Open Questions

- **Blackout coin-recovery loop** — no confirmed path back to real coins once the shop is closed. Deferred until it's actually hit in testing.
- **Third aggressive monster** — not yet designed.
- **Vents** — undecided, possibly cut.
- **Recruit Quarters capacity progression** (LV2/LV3, 6/8 beds) — in or out of scope for the demo? If in scope, the recruit-machine spawn-chance formula needs values beyond 3 recruits.
- **Suit Antenna vs. Core range** — possible overlap in "how far you can reach." Full-game question, not demo-blocking since the Upgrade Station is cut from the demo.
- **Accessory catalog** — buff *type* defined (HP, damage, etc.) but no specific item list yet, unlike Goods.
- **Prototype repo decision** — reuse existing project, clean it up, or start fresh. Pending review of the actual codebase.
