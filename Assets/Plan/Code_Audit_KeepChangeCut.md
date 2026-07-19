# Code Audit — Keep / Refactor / Rebuild / Cut

Based on a full read of `github.com/MattyMarten/First_Game_Project` against the current (post-rework) design docs. This replaces guessing from folder names — every verdict below is based on actually reading the relevant script or its class declaration.

**Legend:** KEEP (works as-is, matches current docs) · REFACTOR (real logic exists, needs changes to match current docs) · REBUILD (concept is right but code predates a design change too large to patch) · CUT (delete, superseded or throwaway) · MISSING (doesn't exist yet, needs building from scratch)

---

## Good signs, project-wide, worth knowing before the details
- No singleton abuse, no `FindObjectOfType` scattered coupling anywhere in 117 scripts. You're wiring references properly. That's genuinely above-average discipline for this stage.
- `RawMaterialStorage` already stores materials as a flat `Dictionary<RawMaterial, int>` — **no tier concept exists in the code at all.** Last session's "remove material knowledge tiers" decision requires zero code changes to Storage. Pure win.
- You already ran a real refactor-planning pass on Shop specifically (`ShopRefactorPlan_Phase1`, `ShopOwnershipMap` in `Assets/Plan`), and the code shows it — `ShopCoreManager`'s own comments explicitly document why it's split from `ShopManager` (whole-Shop coordinator vs. Desk-1-specific). That's exactly the right instinct, just not finished or extended to other rooms yet.
- The "three inventory systems" I flagged last message turned out, on actually reading them, to **not** be pure duplicates:
  - `Grid Inventory` (`GridInventory.cs`) — a Tarkov-style spatial grid with rarity highlighting, referenced directly by `GrinderMachine`. This is your **Case / expedition loot container**, not a duplicate of anything.
  - `Main Inventory` (`InventoryManager.cs`) + `UtilityInventory` (`InventorySlot.cs`) — a toolbar/hotbar pair (`InventoryManager` owns slots and an active-slot count; `InventorySlot` is just its per-slot select/deselect visual). This is your **recruit utility-belt hotbar**, and the two files work together rather than competing.
  - `DemoScript.cs` — a throwaway test harness for `InventoryManager`. **CUT.**
  - Verdict: keep both real systems, just rename folders so it's obvious they're not rivals — e.g. `Assets/NightGameplay/Inventory/CaseGrid/` and `Assets/NightGameplay/Inventory/UtilityBelt/`.

---

## Storage Room
**Verdict: KEEP, near-final.**
`RawMaterialStorage.cs`, `MaterialStorageUI.cs`, `StorageInteractable.cs` — 3 small scripts, matches the doc's own "Storage is not a deep gameplay station" intent. Flat material model already correct. Nothing here needs rework from last session's decisions.
**Action:** light pass to confirm it covers all the categories `Room_Storage.md` lists (goods, shop decor, belts/cases/utility, cobalt coins) — right now it looks materials-only. Cobalt coins and other categories may need their own small additions here, not a rebuild.

---

## Core Room (power / upkeep / cobalt coins)
**Verdict: MISSING — zero code exists.**
Important naming note: several folders are named `.../Core/Scripts/...` (e.g. `Shop/Core`, `Shop/DeskOne/Scripts/Core`) — these are **Shop's own internal "core logic" folders**, unrelated to the Core Room (power) concept. Don't confuse them. There is no cobalt coin currency, no power/upkeep state, no Core Room script anywhere in the repo — the only related file is `Fountain Machine/DayCounter.cs`, a 6-line day-counter stub that isn't connected to anything.
**Action:** build from scratch, matching the updated `Room_Core.md` (now upgradeable — range/consumption/capacity, Dwarf-gated unlock). Since this is new, build it *with* the upgrade system already in mind rather than adding upgrades later.

---

## Workshop
**Verdict: KEEP core machines, REFACTOR crafting to remove tier assumptions (if any), MISSING Suit Station.**
- `Goods station/` scripts (`CraftingInteractable`, `CraftingPanel`, `CraftingRecipe`, `CraftingStationUI`, `CraftingTable`) — solid, matches Goods Workbench doc shape.
- `Grinder/` (`GrinderMachine`, `GrinderInteractable`, `RawMaterial`) — already correctly outputs to `RawMaterialStorage` only (no RP anywhere in the code — **Research Station removal needs zero cleanup here, it was never wired in**).
- `Utility Station/` (`CraftedUtilityStorage`, `UtilityCraftable`, `UtilityCraftingInteractable`, `UtilityCraftingUI`) — matches Gear Workbench doc shape.
- **No Research Station code exists at all** — confirms removing it is a pure documentation change with zero code debt.
- **No Gear Upgrade Station code exists yet** — MISSING, needs building.
- **No Suit Station code exists** — MISSING, entirely new this session, build alongside Gear Workbench's new Suit Component recipes.
**Action:** confirm `CraftingRecipe` doesn't hard-code any tier/knowledge requirement fields (check before assuming — I only read class headers, not full recipe logic) before calling Goods/Gear Workbench fully clean.

---

## Recruit Quarters
**Verdict: REFACTOR — solid foundation, needs capacity/rule updates.**
`RecruitQuartersManager.cs` already has a `pendingAcceptedRecruitsById` / `pendingReservedBedsByRecruitId` flow — this matches the hire-handoff process in your old `ShopOwnershipMap` doc almost exactly (Shop generates candidate → Recruit Quarters reserves a bed → confirms on machine completion). Genuinely good existing structure.
**Needs updating for last session's decisions:**
- Bed count/capacity: currently generic (`List<RecruitBedSlot> bedSlots`), needs to actually enforce 2-per-room / 4-6-8 by level rather than whatever's currently configured in the scene.
- No retire action exists yet — MISSING, needs adding (locker-side action, -3 appeal call into Shop's appeal system once that exists).
- Debuff/status system: not visible in the files I read (`RecruitStats.cs` wasn't opened in detail) — needs checking whether any status effect currently *disables* control, since that must never happen under the new rule. Flag for direct inspection before building further.
**Data ownership problem to fix regardless of design changes:** `RecruitData.cs`, `RecruitClass.cs`, `RecruitStats.cs`, `RecruitType.cs` currently live under `Shop/DeskThree/Scripts/Core/` — i.e., **recruit data definitions are nested inside the Shop room's folder.** Per your own `Script_Architecture_Guidelines.md` ("data definitions... should usually not live only in room managers"), these should move to a shared/neutral location (e.g. `Assets/Shared/RecruitData/` or `Assets/Recruits/Definitions/`) since Recruit Quarters, Graveyard, and Expedition/Dispatch all need to reference them equally — none of them should have to reach into Shop's folder to do it.

---

## Shop
**Verdict: REFACTOR, continuing the plan you already started.**
This is your most-built room (51 scripts) and also the one you already have a real refactor plan for (`ShopRefactorPlan_Phase1_Version2.md`). Don't restart it — pick that plan back up.
- **Desk One (buying)** — `ShopManager`, `ShopBuyerNPC`, `ShopBuyerSpawner`, `ShopBrowsePoint`, `ShopQueueSpot`, `ShopDeskUI` — matches the Sales Desk doc shape well.
- **Desk Two (service — request/talking/merchant)** — `ServiceDeskManager`, `ServiceVisitorSpawner`, `RequestVisitorNPC`, `TalkingVisitorNPC`, `MerchantVisitorNPC`, plus `RequestBoardManager` and `DialogueInfoManager`.
- **Desk Three (hire/recruit)** — `HireDeskManager`, `RecruitGenerator`, `HireVisitorNPC`/Spawner — matches the Recruit Visitor flow doc well, and already connects into Recruit Quarters' pending-reservation flow.
- **`ShopCoreManager`** — already scoped correctly as the whole-Shop coordinator (Appeal, daily visitor flow, spawn cycle) per its own code comments, but Appeal/decor/daily-report logic isn't filled in yet.

### Structural decision this session resolves
- **`MerchantVisitorNPC` and the `merchants/` scripts currently live inside Shop's Desk Two.** Current docs describe Merchant as its own physical room (14 pedestals, dedicated Data Stick slot). **Recommendation: extract Merchant into its own room**, matching the doc — it has enough unique mechanical identity (daily pedestal rolls, progression-gated decor, Data Sticks) to not be a Service Desk visitor type. This is a real move-and-rework, not a rename.
- **`RequestBoardManager` + `DialogueInfoManager` are almost certainly what your old plan called "Guild"** (per `ShopOwnershipMap`: "Guild owns requests and info persistence"). Under current docs, that split cleanly into **Quest Board** (request/quest state — `RequestBoardManager`) and **Info Room** (stored info/documents — `DialogueInfoManager`). Recommend splitting these into two real rooms rather than keeping them Shop-adjacent, matching `Room_QuestBoard.md` and `Room_Info_Version2.md`.
- **Needs adding:** the new recruit-visitor chance formula (free-slot ratio, 25% floor) into whatever currently decides `HireVisitorNPC` spawn odds.

---

## Expedition
**Verdict: REBUILD — right concept, wrong model underneath.**
`ExpeditionManager`, `ExpeditionDestinationData`, `ExpeditionEntryPointData`, `ExpeditionSessionData`, `ExpeditionPrepUI`, `ExpeditionRecruitListItem`, `ExpeditionSelectedCrewItem`, `ExpeditionMemberData`, `ExpeditionBoardInteractable` — this is a complete, functioning first pass, but it's built entirely around the **old fixed-location model**: `ExpeditionDestinationData` has a `sceneName` and a fixed `List<ExpeditionEntryPointData>`, i.e. named, persistent, non-randomized places — exactly the Forsaken City structure that's been replaced by sector categories.
Also: `maxSelectedRecruits = 3` is hard-coded in `ExpeditionManager`, doesn't yet reflect flexible team sizing or the personal-party-vs-away-team split.
**Action:** this is the biggest rework in the whole codebase. Keep the *shape* (a manager, data classes, a prep UI, crew selection) but rebuild the data model around sector categories (category ID + hazard rating + random instance generation, not a fixed scene name) and split personal-party selection from away-team dispatch, which don't exist as separate concepts yet at all.

---

## Recruit Selection / Dispatch Board (Floor 2 stations)
**Verdict: Recruit Selection = REFACTOR (exists inside Expedition scripts, not separated yet). Dispatch Board = MISSING entirely, brand new.**
`ExpeditionRecruitListItem` / `ExpeditionSelectedCrewItem` look like they already do party selection, just bundled into the Expedition scripts rather than being their own station's code. Worth deciding whether to keep them merged or split out to match the doc structure (Recruit Selection System is documented as its own station).

---

## Rooms/systems with zero code yet (confirmed by search — not overlooked, genuinely not started)
- **Graveyard** (undead system)
- **Druid** (medical shop)
- **Professor** (scanner/recipe NPC)
- **Dwarf** (base owner, Upgrade Board)
- **Quest Board** as its own room (only the Shop-adjacent `RequestBoardManager` exists — see Shop section above)
- **Info Room** as its own room (only `DialogueInfoManager` exists — see Shop section above)
- **Sector Map / unlock graph**
- **Suit Station**
- **Data Stick system**

None of these are behind schedule — they were never started. This is useful to know precisely because it means there's no legacy code fighting you in any of these; they're clean-slate builds straight from the current docs.

---

## Player / Shared
**Verdict: KEEP, untouched.**
`FirstPersonController`, `StarterAssetsInputs`, `PlayerInteraction`, `PlayerInventory`, `PlayerHoldItem`, `BasicRigidBodyPush`, `InputModeManager` — standard, solid, not affected by anything design-side. `Shared/Interaction/` (`Interactable`, `PanelInteractable`, `ClosableUIPanel`) is exactly the kind of shared base your `Script_Architecture_Guidelines.md` recommends — keep building room interactions on top of these rather than inventing new interaction patterns per room.

---

## Two things to physically clean up regardless of design decisions
1. Delete `Assets/NightGameplay/Inventory/DemoScript.cs` — confirmed throwaway.
2. Move `RecruitData.cs`, `RecruitClass.cs`, `RecruitStats.cs`, `RecruitType.cs` out of `Shop/DeskThree/Scripts/Core/` into a shared location.
