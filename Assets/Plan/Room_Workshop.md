# Workshop Room Plan

## 1. Room Purpose

Workshop is the base's main production and item progression room.

Its main purpose is to:
- process expedition-returned material representations into materials
- craft sellable goods
- craft recruit/expedition gear
- upgrade crafted gear to higher tiers
- **permanently upgrade the shared Suit (new this session)**

**Removed this session:** the Research Station, research points (RP), and material knowledge tiers no longer exist anywhere in Workshop. See Section 9.

Workshop is one of the base's main active gameplay rooms and supports the game's broader design direction of physical readability, strong room identity, and minimal large-menu UI.

---

## 2. Room Identity

Workshop is a production room, not a storage room and not a planning room. It is the main place where the player turns expedition outcomes into useful progression.

It connects: expedition intake, Storage, shop production, recruit gear support, and now Suit progression.

---

## 3. Workshop Machines

Workshop currently contains or owns the following production machines/systems:

- Grinder
- Goods Workbench
- Gear Workbench
- Gear Upgrade Station
- **Suit Station (new this session)**

The Grinder belongs logically to Workshop/production systems, even if it is physically placed near expedition return flow for convenience.

---

## 4. What Workshop Owns

Workshop owns the rules and local interaction state for:
- Grinder processing behavior
- Goods Workbench crafting behavior
- Gear Workbench crafting behavior
- Gear Upgrade Station upgrade behavior
- **Suit Station upgrade behavior and current Suit upgrade levels**
- station upgrade levels inside the Workshop
- active local machine interaction state
- production validation logic for crafting/upgrading

---

## 5. What Workshop Does Not Own

Workshop does not own: Storage inventory totals, placed Shop display/decor state, recruit roster/state, quest data, sector/planning data, day/time/phase ownership, base power ownership, Core loaded coin ownership, or Core range.

---

## 6. Physical / Interaction Direction

Unchanged — physical-first, machine-distinct, camera-locked focused views, notes/monitors/signage over large menus.

---

## 7. Workshop Resource Display

Should show current stored materials and cobalt coin amount.

**Removed this session:** RP is no longer a tracked value anywhere, including this display.

### Material display rule
Materials should be shown using icon, name, amount. Only materials the player currently has should be shown (0-amount materials hidden).

**Materials are now flat — no tiers, no type variants.** Wood is simply Wood. There is no Wood T1/T2/T3, no "Refined Wood" distinct from "Wood." Any future distinct material must be its own fully separate named material, not a tier of an existing one.

---

## 8. Grinder

### Purpose
The Grinder is the Workshop-owned intake processor used to convert expedition-returned processing representations into materials.

**Removed this session:** the Grinder no longer produces RP. It produces materials only.

### Core Rule
The Grinder processes returned expedition representations and produces materials only. It is not literal destruction of a recruit's real assigned case — the returned object near the Grinder is a processing representation used for material conversion flow.

### Use Flow
1. expedition-returned processing representation appears near the Grinder
2. the player interacts with the Grinder
3. the returned representation is processed
4. resulting materials are added to Storage
5. the processing representation is cleared

### Upgrade Levels
- **LV1**: processes the true amount of materials, no bonus output
- **LV2**: 5% chance to generate bonus output on each processed item
- **LV3**: 10% chance to generate bonus output on each processed item

### Ownership Notes
The Grinder owns local processing interaction state and output generation rules for materials. It does not own Storage totals or the recruit's real persistent equipment state.

---

## 9. Recipe Unlocking — Data Sticks (replaces Research Station entirely)

### What changed
The Research Station, RP, and material knowledge tiers are removed completely. Recipes for the Goods Workbench and Gear Workbench are no longer gated by researching material tiers. Instead, each recipe is simply **locked** or **unlocked**, and unlocking happens via **Data Sticks** — physical items, not a menu-driven research investment.

### How Data Sticks work
- A Data Stick unlocks one specific recipe automatically the moment it reaches the base (found during expedition, purchased from the Merchant, or received from a request/talking visitor).
- Data Sticks are **auto-consumed** on acquisition. The player never manually "uses" one, and it never sits in Storage as an inventory item — Storage does not need a Data Stick category at all.
- If a Data Stick is acquired for an already-unlocked recipe, it **auto-converts into materials or coins** instead of doing nothing (exact conversion value/type: placeholder, TBD).
- Each source draws from its own **curated pool** — a specific sector category, the Merchant, request visitors, and talking visitors can each only yield sticks from their own fixed list. This keeps unlocks random but still learnable ("the Merchant sometimes sells the Beer Mug stick").
- The Merchant has a dedicated Data Stick pedestal with a flat **10% chance per day** of a stick appearing there (see Room_Merchant.md for the pedestal detail).

### Ownership note
Data Stick delivery/consumption logic and the "which recipes are unlocked" flag list belongs to Workshop (since it governs what the Workbenches can craft). The Merchant, Info Room (documents), and Shop (visitors) are only delivery vectors — they do not own the unlocked-recipe state itself.

---

## 10. Goods Workbench

### Purpose
Crafts sellable goods only, using flat (non-tiered) materials.

### Current Goods Item List
(Recipes below are updated to remove all knowledge-tier requirements — material costs are now flat quantities of flat materials. Exact quantities are placeholders pending balancing.)

#### Weapons
- Sword
- Spear
- Bow
- Hammer

#### Defense
- Basic Shield (Wood, Metal)
- Fortified Shield (more Wood, more Metal)
- Metal Shield (Metal)
- Electric Shield (Battery, Metal)

#### Household
- Beer Mug
- Lantern

#### Decor / Misc
- Lucky Stone Statue
- Stop Sign
- Guitar
- Clothing

### Recipe Requirement Rule
A recipe can only be crafted if:
- the recipe is unlocked (via Data Stick — see Section 9)
- the required materials exist in Storage
- the Workbench level allows that recipe tier

**Removed this session:** no material knowledge tier check exists anymore.

### Crafting Speed / Output / Multi-Craft
Unchanged — instant crafting, result goes directly to Storage, multi-craft supported with auto-capped amount.

### Upgrade Levels
- LV1: Tier 1 recipes only
- LV2: Tier 1 and Tier 2 recipes
- LV3: Tier 1, Tier 2, and Tier 3 recipes

(Recipe "tier" here refers to recipe difficulty/rarity tier, unrelated to the now-removed material knowledge tiers.)

### Interaction / UI Direction
Unchanged from previous version — category browsing (Q/E), item switching (A/D), amount (W/S or scroll), craft (Space/Enter/click), exit (Esc/right click/Tab). Readability rule: lacking materials turns entries red; **lacking-knowledge red state is removed** since knowledge no longer exists — a recipe is either locked (not shown at all, or shown as locked/unrevealed) or unlocked and only gated by materials.

### Recipe Visibility States
- **Available** — unlocked, can craft if materials are sufficient
- **Known but locked** — unlocked but missing materials, or blocked by Workbench level
- **Unknown / unrevealed** — recipe not yet unlocked via Data Stick; not visible

### Power Behavior
Unchanged — works during power loss.

---

## 11. Gear Workbench

### Purpose
Crafts utility items, cases, belts, accessories, and (new this session) **Suit components**.

### Current Gear Item List

#### Utility Items
Shovel, Pickaxe, Axe, Flashlight, Pro Flashlight, UV Light, Noise Maker, Flashbang, Mega Flashbang, Medkit, Scanner, Radio, Strong Radio (unchanged)

#### Cases
Case, Heavy Case, Light Case, Stealth Case, Snail Case (unchanged)

#### Belts
Utility Belt, Bigger Belt, Even Bigger Belt (unchanged)

#### Accessories
Night Vision Goggles, Gas Mask, Sense Mask, Stat accessories (unchanged)

#### Suit Components (new this session)
Craftable components consumed by the Suit Station to permanently upgrade the shared Suit:
- **Battery** — increases Battery capacity when installed (extends how long an expedition can last before a forced return)
- **Shoes** — increases movement speed when installed
- **Mask** — increases breathing-hazard resistance when installed (toxic air, radiation via breathing)
- **Suit Material** — increases physical-hazard resistance when installed (heat, cold, corrosive fluid contact)

These are crafted here exactly like any other gear item, then carried to and installed at the Suit Station (Section 13). They are not permanently stored as reusable inventory once installed — installing one is a one-way permanent upgrade, similar in spirit to a Data Stick being consumed.

### Recipe Requirement Rule
Same as Goods Workbench — recipe must be unlocked (via Data Stick, where applicable) and materials must be available. No knowledge-tier check.

### Everything else in this section
Unchanged from previous version — crafting is instant, multi-craft supported, upgrade levels LV1/2/3 gate recipe tiers, same UI/interaction pattern, works during power loss.

---

## 12. Gear Upgrade Station

Unchanged from previous version in full. This system was never gated by research/knowledge, so nothing here changes: T1→T2→T3 tier upgrades for utility items, cases, belts, and accessories, gated by owning the source-tier item, materials, and station level. Does not upgrade the Suit — the Suit has its own separate, permanent, non-tiered upgrade path via the Suit Station (Section 13).

---

## 13. Suit Station (new this session)

### Purpose
The Suit Station is the Workshop-owned machine used to permanently upgrade the base's single shared Suit.

Its role is to:
- give the player the actual reason to return from an expedition (Battery)
- gate which sectors can be personally entered or have an away team sent to them (Mask / Suit Material resistance)
- provide a physical, readable upgrade interaction

### Core Rule
There is **one Suit**, shared by every recruit. Upgrading a component benefits every recruit immediately — there is no per-recruit suit ownership, and no need to re-upgrade for each recruit.

### Physical presentation
The Suit Station is presented as a suit mounted on a stand. The player interacts with it and selects which component to upgrade.

### Components
- **Battery** — governs total expedition duration. This is the *only* limit on how long an expedition can run; there is no other fixed timer, in-game clock, or real-time cap. When Battery runs out, the expedition forces a return.
- **Shoes** — governs movement speed.
- **Mask** — governs breathing-hazard resistance (toxic air, radiation via breathing).
- **Suit Material** — governs physical-hazard resistance (heat, cold, corrosive fluid contact).

### Upgrade Rule
- Each component upgrade consumes its corresponding crafted item (Battery, Shoes, Mask, or Suit Material — see Section 11) from Storage.
- Each upgrade is **permanent** — no tiers to track, no possibility of losing progress, no per-recruit repetition needed.
- Upgrades take effect immediately.

### Hard Travel Gate
If current Mask or Suit Material resistance is insufficient for a sector category's hazard rating, that sector cannot be selected for personal expedition or for away-team dispatch — this is a hard block, not a risk modifier (see Base_Master_Plan.md Section 7).

### Power Behavior
TBD — likely does not require power, consistent with Goods/Gear Workbench, since it's a manual mechanical upgrade rather than an active-process machine. Confirm during implementation.

### Upgrade Levels / Values
Placeholder — exact Battery/Shoes/Mask/Suit Material numeric progression is TBD pending balancing and sector hazard design.

---

## 14. Relationship With Storage

Unchanged in shape — Workshop pulls materials and owned gear from Storage, and returns processed materials, crafted goods, crafted gear, upgraded gear, and crafted Suit components to Storage. Once a Suit component is installed at the Suit Station, it leaves Storage permanently (consumed).

---

## 15. Relationship With Shop

Unchanged — Goods Workbench → Storage → Shop displays. Workshop does not sell directly.

---

## 16. Relationship With Expedition Return Flow

Unchanged in shape, minus RP: Grinder converts returned representations into materials only (RP output removed).

---

## 17. Relationship With Core / Power

Unchanged for Grinder/Goods Workbench/Gear Workbench/Gear Upgrade Station. **Research Station power dependency is removed** since the room no longer exists. Suit Station power dependency: TBD (see Section 13).

---

## 18. Upgrade Summary

### Grinder
- LV1: processes true amount, no bonus
- LV2: 5% chance for bonus output
- LV3: 10% chance for bonus output

### Goods Workbench
- LV1: Tier 1 recipes only
- LV2: Tier 1 and Tier 2 recipes
- LV3: Tier 1, Tier 2, and Tier 3 recipes

### Gear Workbench
- LV1: Tier 1 items only
- LV2: Tier 1 and Tier 2 items
- LV3: Tier 1, Tier 2, and Tier 3 items

### Gear Upgrade Station
- LV1: upgrades to Tier 2, normal material cost
- LV2: upgrades to Tier 2, reduced material cost
- LV3: upgrades to Tier 3, reduced material cost

### Suit Station
No levels — each component upgrades independently and permanently as components are installed. No LV1/2/3 structure; TBD whether a soft cap exists per component.

---

## 19. First-Pass Implementation Direction

Start with the Grinder (materials only now) since it feeds everything else. Add Goods Workbench next, then Gear Workbench, then Gear Upgrade Station, then the Suit Station last since it depends on Gear Workbench being able to craft Suit components first.

Data Stick delivery/consumption logic should be implemented alongside whichever Workbench needs its first locked recipe — it doesn't need to be a large separate system, just a flag-unlock event handler.

---

## 20. Done Condition

Workshop is good enough for implementation when:
- Grinder correctly processes expedition-returned representations into materials only
- Grinder bonus output chance works correctly at LV2 and LV3
- Goods Workbench and Gear Workbench correctly craft using flat materials with no knowledge-tier check
- Data Sticks correctly auto-unlock their recipe on acquisition, and correctly auto-convert to materials/coins on duplicate acquisition
- Gear Upgrade Station correctly upgrades owned gear by tier (unaffected by this session's changes)
- Suit Station correctly consumes crafted components and permanently increases the correct stat
- Suit Battery correctly has no cap other than its own upgraded value (no separate fixed expedition timer exists anywhere)
- Suit Mask/Suit Material correctly block sector selection when insufficient
- Workshop correctly pulls from and returns items to Storage
- the room is readable through machine identity, resource displays, and local interaction views

---

## 21. Open Questions
- Exact Data Stick duplicate-conversion value (coins vs. materials, and how much)
- Exact Suit component costs and stat values per upgrade (placeholder until balancing)
- Whether the Suit Station requires Core power
- Whether Suit components have a soft upgrade cap or scale indefinitely
