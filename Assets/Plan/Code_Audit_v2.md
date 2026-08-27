# Code Audit — Keep / Rework / Cut / Build New

Grounded in an actual read of the repo (`First_Game_Project`, ~11,000 lines of C# across ~100 scripts), mapped against `Master_Plan_v2.md`. Verdict up front: **reuse and prune, don't restart.** The foundation is genuinely solid — `DayPhaseSystem` alone is well-architected (event-driven, phase-locked, singleton, already commented against the master plan's own section numbers). Most of what needs to change is scoped and specific, not "start over."

*(Note: this replaces the spirit of the old `Code_Audit_KeepChangeCut.md`, deleted in the 8.27.26 plan cleanup — worth having a current one now that the plan itself changed.)*

---

## Keep as-is

- **`DayPhaseSystem.cs`** — matches the phase rules exactly, no changes needed.
- **`InputModeManager.cs`** — clean Player/Inventory/UI/Debug mode switching. Extend with a third `Interaction` mode rather than rewrite (see Build New).
- **`Interactable.cs`, `PanelInteractable.cs`, `ClosableUIPanel.cs`** — the interaction base to build the physical UI system on top of.
- **Storage** (`CobaltCoinStorage`, `RawMaterialStorage`, `StorageInteractable`, etc.) — already data-only, matches the "virtual storage for the demo" decision directly.
- **Data Sticks** (`DataStickConsumer`, `DataStickItem`, `RecipeUnlockManager`, `IUnlockableRecipe`) — untouched by the rework, keep as-is.
- **Grid Inventory** (`GridInventory`, `GridInteract`, `LootRotation`, `TooltipManager`, etc.) — genuinely good news: this is already a working shaped-grid inventory with placement validity preview and item rotation. That's most of the engineering the new locker gear-grid (2×1/2×2 items) needs already built. Just needs wiring into the new 3-part locker instead of wherever it's currently hooked up.
- **Recruit Quarters** (`RecruitBedSlot`, `RecruitLocker`, `RecruitQuartersManager`, `RecruitQuartersActor`, `ReturnToPointWalker`, `ShopkeeperSwitchInteractable`) — already close to the new plan (beds, locker, interact-to-switch-control). Needs the locker connected to the 3-part layout (accessories / gear grid / backpack).
- **Core** (`CoreRoomManager`, `CoreDepositInteractable`) — keep as the foundation, extend with blackout + emergency-refuel logic.
- **Grinder** (`GrinderInteractable`, `GrinderMachine`, `RawMaterial`) — matches the plan directly, no changes.
- **Shop Display** (`DisplayInteract`, `DisplayInteractable`, `DisplayStand`, `GoodsDisplay`, `GoodStorage`, etc.) — matches the self-service, stock-gated display system already.

## Rework (real logic exists, doesn't match the new rules)

- **Shop DeskOne** (`ShopManager`, `ShopDeskInteractable`, `ShopBuyerNPC`, `ShopBuyerSpawner`, `ShopBrowsePoint`, `ShopQueueSpot`) — this is the closest thing to the new single desk. Likely becomes *the* desk; adapt spawn/queue logic to the confirmed numbers (stock-gated, ~1 buyer/X sec, cap 5 concurrent).
- **Shop DeskTwo** (`ServiceDeskManager`, `TalkingVisitorNPC`, `RequestVisitorNPC`, `HireVisitorNPC`, dialogue data) — currently merges talking + quest-request + hire visitors into one desk. New plan splits these three ways: talking stays on the shop desk, request moves to Quest Board, hire moves to the new Recruit Machine. `TalkingVisitorNPC` and the dialogue/translation display logic are probably directly reusable for the shop desk's talker flow.
- **Quest Board** (`RequestBoardManager`, `QuestBoardRoomManager`) — adapt to 6 slots / 1–2 NPCs per day / 0% at full / 3-day silent expiry. Drop the Appeal linkage if it's wired in currently.
- **`QuestDefinition`, `QuestReward`, `RequestDifficulty`** — likely keep the shape, strip whatever's rank-dependent.
- **Workshop crafting** (`CraftingInteractable`, `CraftingPanel`, `CraftingRecipe`, `CraftingStationUI`, `CraftingTable`) — this is the base to build the single merged Workbench from; extend recipe categories to cover Gear and Accessories, not just Goods.
- **Expedition core** (`ExpeditionManager`, `ExpeditionSessionData`, `ExpeditionMemberData`) — the underlying session/roster state is probably reusable as-is.
- **Expedition selection UI** (`ExpeditionBoardInteractable`, `ExpeditionEntryPointData/UIItem`, `ExpeditionDestinationData`, `ExpeditionRecruitListItem`, `ExpeditionSelectedCrewItem`, `ExpeditionPrepUI`) — built around sector-map entry-point selection. The new plan wants two simple select stations instead of a map, so this UI layer needs real rework — but the underlying data (who's selected, where) likely survives.

## Cut entirely

- **Shop Decor** (`DecorEffectType`, `DecorItemData`, `DecorManager`, `DecorMenuUI`, `DecorSpot`, `DecorSpotInteractable`) — no shop upgrades/decor in the new plan. This is a meaningful chunk of the Shop folder's 5,700 lines.
- **`QuestRankManager`** and anything rank-dependent in `QuestRank.cs` — rank/XP system fully removed.
- **Gear Upgrade Station** (`GearUpgradeInteractable`, `GearUpgradeStationManager`, `GearUpgradeUI`) — the whole Upgrade Station is cut from the demo.
- **Utility Station** (`UtilityCraftable`, `UtilityCraftingInteractable`, `UtilityCraftingUI`, `CraftedUtilityStorage`) — the shovel/axe utility-item concept is gone, replaced by the multi-tool.
- **Shop DeskThree recruit generation logic specifically** (`RecruitGenerator`'s Free/Paid split, 5-stat point-buy, stat-derived class) — doesn't match the new machine (name/color/class-of-4 chosen by player, random trait). `RecruitRosterManager` might survive as the roster tracker; the generation logic itself needs replacing, not adapting.

## Needs a decision from you

- **Shop "Dirt"** (`DirtManager`, `DirtSpawnPoint`, `DirtSpotInteractable`) — this doesn't map to anything in the current plan at all. Still wanted (some kind of shop-cleaning mechanic?), or safe to cut with Decor?

## Doesn't exist yet — net new work

- The **Interaction** camera-lock control state itself (extends `InputModeManager`) — build first, everything else in Buildings/ plugs into it.
- **Recruit Machine** physical flow (queue line, "next," name/color/class selection UI, spawn-chance formula).
- **Multi-tool** (dig / attack / scan / cut).
- **Undead Capsule.**
- **Core blackout + emergency material-refuel** logic.
- **Water Dispenser** + filter maintenance.
- **Personal-expedition safehouse** + per-recruit staggered Battery + death-cascade switching + rescue mechanic.
- **Away-team** focus-category selection + survival/loot roll resolution + results display.
- **All enemy/threat AI** (passive/aggressive/territorial, the Doll, hiding-spot ambush, spiders, traps) and the shared line-of-sight/noise detection system underneath them.
- **World interactions**: dig spots, breakable gems, wire-minigame doors.

---

**Suggested order, matching the build-order already agreed:** Interaction state first, then prune Shop (Decor out, three desks down to one + Recruit Machine), then Workshop merge, then Core blackout, then Recruit Quarters/locker wiring, then Expedition rework — all before touching enemy AI, since none of that blocks on the map/threat content existing.
