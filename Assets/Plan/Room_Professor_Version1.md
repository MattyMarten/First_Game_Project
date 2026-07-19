# Professor Room Plan

## 1. Room Name
Professor Room

---

## 2. Purpose
The Professor Room is the lab of the Professor, a droid scientist who studies the monsters the player encounters.

It exists to:
- give the player the Scanner recipe so enemies can be scanned
- drive the enemy scanning progression loop
- reward the player for scanning monsters
- provide scanner tier progression through new recipes
- give main line quests and information tied to scanning and monster study

The Professor is the reason the enemy scan system in the Info Room has value.
He turns scanning from a passive activity into a rewarded progression track.

---

## 3. Room Identity
The Professor Room should feel like:
- a working laboratory
- a room lined with tubes containing captured or studied monsters
- a place of study and experimentation
- unsettling but not hostile

The room visually contains:
- monster tubes
- lab equipment
- the Professor himself

The Professor is a droid.
He is methodical, precise, and curious about biology in a way a machine should not be.

---

## 4. Who He Is
The Professor is a droid who wants to build himself a biological body.

This is his personal agenda and the reason he studies monsters.
Every monster the player scans feeds his research toward understanding biology well enough to one day construct a biobody for himself.

He is not tied to the Dwarf or any other special NPC.
None of the special NPCs are connected to each other — each has their own story and reason for being at the base.

His tone is:
- clinical and precise
- quietly obsessive about his goal
- interested in the player only as far as they bring him data

---

## 5. What It Does
The Professor Room:
- becomes available after the Professor is found in the caves of Forsaken City
- permanently grants the Scanner recipe when he joins
- gives main line quests tied to scan milestones
- rewards the player with money and materials on quest completion
- gives new scanner recipes that replace older scanner recipes
- gives information and lore through conversations
- calls the player to the lab through the Notification Display when he has something to say
- closes during Core offline/unpowered state

---

## 6. What It Holds
The Professor Room holds or tracks:
- Professor unlocked / joined state
- conversation and quest trigger state
- scanner recipe tier currently granted
- which scan milestone quests have been given and completed
- Professor accessibility state

---

## 7. What It Takes
The Professor Room takes:
- scan count progress from the enemy scan system
- day progression for quest trigger conditions
- quest completion results
- power availability from Core

---

## 8. What It Gives
The Professor Room gives:
- the Scanner recipe (permanent on joining)
- new scanner tier recipes as quest/progression rewards
- money and materials as quest rewards
- main line quests to the Quest Board
- information and lore through conversations
- Notification Display calls when he wants to speak

---

## 9. Connected Rooms / Systems
The Professor Room connects to:
- Gear Workbench (Scanner recipe lives here once granted)
- Info Room enemy scan system
- Quest Board Room
- Storage Room for material rewards
- Economy system for money rewards
- Notification Display system
- Core Room
- world NPC unlock flow

---

## 10. Player Actions
The player can:
- interact with the Professor to trigger or continue conversations
- receive the Scanner recipe when he joins
- receive main line quests
- receive scanner tier recipe rewards
- review Professor accessibility state

---

## 11. The Professor — Conversation and Calls

### Conversation style
Conversations happen by interacting with the Professor directly.

When the player interacts with the Professor:
- a popup UI appears in front of him, staying physically attached to his space
- text appears showing what he says
- this works the same way as the Shop Service Desk and Dwarf conversation style

### When he calls the player
The Professor calls the player to his lab only when:
- he has a new quest to give
- he has something to say or a new recipe to grant

The Notification Display on Floor 1 shows an alert when the Professor wants to speak.

Format example:
```
Floor [X] - Professor Room - Professor Wants to Speak
```

He does not call the player for no reason.

---

## 12. Scanner System

### Scanner recipe grant
When the Professor joins the base:
- the Tier 1 Scanner recipe is permanently added to the Gear Workbench
- the player can now craft a Tier 1 Scanner

### Scanner tier progression through recipe swaps
The Scanner is a special case among gear.
It does not use the Gear Upgrade Station.

Instead:
- the Professor grants new scanner recipes as progression rewards
- when a new scanner recipe is granted, the previous tier recipe is removed from the Gear Workbench
- the player crafts the new tier from the new recipe

Example:
- Professor grants Tier 2 Scanner recipe
- Tier 1 Scanner recipe disappears from the Gear Workbench
- player crafts a Tier 2 Scanner

### Old scanners become useless
When a new scanner tier recipe is granted:
- any existing lower-tier scanner the player already crafted becomes useless
- the player should craft the new tier scanner to continue scanning at the higher level

### Scanner level and scan slots
Scanner level determines how deep a scan can go on the enemy 3-slot system in the Info Room.

- Tier 1 Scanner — can scan Slot 1 (Lore)
- Tier 2 Scanner — can scan up to Slot 2 (Abilities and Strengths)
- Tier 3 Scanner — can scan up to Slot 3 (Weaknesses and Counters)

### Losing the scanner
If the player loses their scanner:
- they must craft a new one from the current scanner recipe
- the scanner is the only expedition item beyond the Medkit that has this kind of importance

### Scanner crafting and use note
The Scanner is crafted at the Gear Workbench.
The actual scanning of enemies happens during expeditions.
Scan duration increases with each deeper slot.
The expedition-side scanning mechanics will be built during the expedition phase, not the base phase.

---

## 13. Scan Reward Quests

### Quest type
The Professor's scan milestone quests are main line quests.
They go to the Quest Board through conversations like other main line quests.
They cannot be rejected.

### Milestone structure
The Professor gives quests based on scanning milestones.

Example progression:
- Quest: have 5 monsters scanned
- Quest: have 10 monsters scanned
- Quest: have 10 monsters scanned to Tier 2 knowledge
- further milestones continue this pattern

### Quest rewards
Completing scan milestone quests rewards:
- cobalt coins
- materials

### Scanner recipe rewards are separate
Scanner tier recipe grants are handled separately from the scan-count quest rewards.

The Professor checks progression conditions to decide when to grant a new scanner recipe.

Example condition:
- if at least 2 Professor quests are complete and the day is 15 or later
- the Professor calls the player to the lab and grants the next scanner recipe

This keeps the scanner recipe progression on its own track, gated by both quest progress and time, rather than tied directly to a single quest reward.

---

## 14. Upgrade Levels

The Professor Room does not currently have a finalized LV1/LV2/LV3 upgrade path.

Possible future upgrade effects:
- faster reward processing
- more monster tubes
- additional research services

For now the Professor Room is defined by his unlock state, scanner recipe progression, and quest track.

---

## 15. Power Dependency
- if the Core is unpowered, the Professor Room closes
- the Professor does not have backup power
- the player cannot interact with him while the room is closed
- like all special NPC rooms, the Professor Room is power dependent

---

## 16. Data Ownership

### This room owns
- Professor unlocked / joined state
- conversation and quest trigger state
- current granted scanner recipe tier
- scan milestone quest given/completed tracking
- scanner recipe progression condition checks
- Professor accessibility state

### This room does not own
- the Scanner recipe storage (lives in Gear Workbench craft list once granted)
- enemy scan data and slot unlock state (owned by Info Room)
- scan count tracking truth (owned by the scan/expedition system)
- Quest Board state
- Storage inventory
- Core power state

### Important ownership note
- the Info Room owns the stored enemy scan archive and slot unlock state
- the Professor reads scan count progress to drive his quests and rewards
- the Professor grants recipes but the Gear Workbench owns the active craft list
- the Professor owns only his own progression and quest-trigger state

---

## 17. UI / Readability Needs
The player should be able to clearly see:
- the Professor conversation popup clearly
- which scan milestone quest is currently active (via Quest Board)
- when a new scanner recipe has been granted
- that the old scanner recipe has been replaced in the Gear Workbench
- whether the Professor Room is closed due to Core power loss
- Notification Display alert when the Professor wants to speak

---

## 18. Interaction / Animation Needs
Useful early feedback:
- Professor turns to face the player when interacted with
- conversation popup appears and is readable
- scanner recipe granted feedback
- quest given feedback
- quest reward received feedback
- monster tubes ambient lab feedback
- Professor Room closed feedback when Core is unpowered
- Notification Display call when he wants to speak

---

## 19. Temporary Implementation Notes
Early implementation may use:
- simplified conversation popup before final art pass
- debug unlock Professor button
- debug grant scanner recipe button
- debug set scan count button to test milestones
- placeholder Professor droid and lab visuals
- placeholder monster tube visuals
- hardcoded quest milestones before full quest data exists
- fixed reward values before balancing

---

## 20. Done Condition
The Professor Room is considered working when:
- the Professor can be unlocked correctly after being found in the caves
- the Tier 1 Scanner recipe is granted permanently on joining
- scan milestone quests are given to the Quest Board correctly as main line quests
- quest rewards of coins and materials are granted correctly
- new scanner recipes replace old scanner recipes in the Gear Workbench correctly
- old scanners correctly become useless when a new tier is granted
- scanner recipe progression conditions are checked correctly
- the Professor calls the player through the Notification Display correctly
- the Professor Room closes correctly when Core is unpowered
- ownership does not conflict with Info Room, Gear Workbench, Quest Board, or scan systems

---

## 21. Open Questions
- What are the exact scan milestone numbers and reward values beyond the first few examples?
- Should there be a final scanner tier beyond Tier 3, or is Tier 3 the maximum?
- Should the Professor offer any repeatable reward after all milestone quests are complete, or does his quest track simply end?
