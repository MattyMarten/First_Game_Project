# Room Naming Conventions

This file defines naming rules for rooms, room-related assets, and room-related systems.

The goal is to keep room names clear, consistent, and easy to follow as the project grows.

---

## 1. Purpose

Use this file to:
- keep room names stable
- avoid mixed naming styles
- make scene hierarchy easier to read
- make scripts easier to understand
- reduce rename confusion during refactors

---

## 2. Main Naming Rule

Use **Room** consistently in naming for base spaces.

Examples:
- Shop Room
- Storage Room
- Workshop Room
- Recruit Quarters
- Core Room
- Merchant Room
- Druid Room
- Dwarf Room

If a space is a true room in the underground base, name it as a room.

If something is not a room but a system, name it as a system.

Examples:
- Day Phase System
- Expedition System
- Appeal System
- Economy System

---

## 3. Document Naming Rules

### Room documents
Use:
- `Room_Shop.md`
- `Room_Storage.md`
- `Room_Workshop.md`
- `Room_RecruitQuarters.md`
- `Room_Core.md`

Format:
- `Room_<Name>.md`

For multi-word names, use PascalCase without spaces in file names.

Examples:
- `Room_QuestBoard.md`
- `Room_RecruitMachine.md`

### Global/base documents
Use descriptive names with underscores:
- `Base_Master_Plan.md`
- `Base_Implementation_Checklist.md`
- `Implementation_Status.md`
- `Open_Architecture_Questions.md`

---

## 4. Scene Hierarchy Naming Rules

Use readable object names in the scene.

### Room roots
Format:
- `<RoomName>Room`

Examples:
- `ShopRoom`
- `StorageRoom`
- `WorkshopRoom`
- `CoreRoom`
- `MerchantRoom`

Exception:
- `RecruitQuarters`
- `QuestBoardRoom`

### Room contents
Format:
- `<RoomName>_<Purpose>`

Examples:
- `Shop_Displays`
- `Shop_DecorSlots`
- `Shop_CustomerEntry`
- `Storage_Shelves`
- `Workshop_GoodsStation`
- `Workshop_ResearchMachine`
- `RecruitQuarters_Beds`

This keeps related objects grouped and easy to scan.

---

## 5. Script Naming Rules

### Room managers
Format:
- `<RoomName>RoomManager`

Examples:
- `ShopRoomManager`
- `StorageRoomManager`
- `WorkshopRoomManager`
- `CoreRoomManager`

### Sub-system managers inside a room
Format:
- `<FeatureName>Manager`

Examples:
- `DisplayManager`
- `ShopCustomerSessionManager`
- `ResearchMachineManager`
- `RecruitLoadoutManager`

Use these only if the room is complex enough to justify them.

### Shared systems
Format:
- `<SystemName>System`
- `<SystemName>Manager`

Examples:
- `DayPhaseSystem`
- `EconomySystem`
- `AppealSystem`
- `ExpeditionResultRouter`

---

## 6. UI Naming Rules

Format:
- `<RoomName>UI`
- `<RoomName>Panel`
- `<FeatureName>Panel`
- `<FeatureName>View`

Examples:
- `ShopUI`
- `ShopStatusPanel`
- `WorkshopRecipePanel`
- `RecruitDetailsPanel`
- `CoreStatusPanel`

---

## 7. Temporary Object Naming Rules

All temporary objects should be clearly marked.

Use one of these prefixes:
- `TEMP_`
- `DEBUG_`
- `PROTO_`

Examples:
- `TEMP_ShopSpawnPoint`
- `DEBUG_AddMoneyButton`
- `PROTO_StorageShelf`

This helps identify what is not final.

---

## 8. Placeholder Asset Naming Rules

Use consistent prefixes for placeholder assets:
- `PH_` for placeholder
- `TEMP_` for temporary
- `TEST_` for test-only assets

Examples:
- `PH_ShopCounter`
- `TEMP_CustomerMarker`
- `TEST_RecruitCard`

---

## 9. Avoid These Naming Problems

Avoid:
- vague names like `Manager`, `System`, or `Controller` with no context
- mixed naming like `ShopManager`, `RoomShop`, `shop_room_manager` all in one project
- old "building" names mixed with new "room" names
- naming by scene purpose one time and by logic purpose another time without a rule

Bad examples:
- `MainManager`
- `StuffHandler`
- `DataThing`
- `BuildingShopRoom`
- `BaseRoomShopControllerV2`

---

## 10. Rename Guidance

If an old name still uses "building" but the project is now using "room", prefer renaming when:
- the script is still early
- the room is not deeply connected yet
- the change reduces future confusion

If an old name is already deeply used:
- note it
- plan a dedicated rename pass later
- do not half-rename only part of the related objects

---

## 11. Notes

The goal is not perfect naming.
The goal is:
- readable naming
- consistent naming
- naming that matches architecture

When in doubt:
- use room for places
- use system for cross-room logic
- use manager for a concrete owner of behavior in code