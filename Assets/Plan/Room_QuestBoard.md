# Quest Board Room Plan

## 1. Room Name
Quest Board Room

---

## 2. Purpose
The Quest Board Room is the main room for managing available quests, active quests, quest rank progression, and main-line quest tracking.

It exists to:
- present available quests to the player each day
- limit and organize which quests can be taken
- track currently active quests
- track the current quest rank of the base
- connect expedition progress back into structured objectives
- provide long-term direction through normal quests and main-line quests
- display post-expedition results through the Expedition Results Display

---

## 3. What It Does
The Quest Board Room:
- stores available quests for the current day
- stores active quests
- stores main-line quests
- controls how many quests can be held at once
- controls how many quests can be active at once
- applies rank-based quest offer restrictions
- pulls from the valid quest pool each day based on current rank and unlock state
- receives quest progress from expeditions
- handles quest completion and automatic reward distribution
- applies appeal changes when quests are accepted, declined, completed, or discarded
- tracks quest rank XP and handles rank progression
- displays the current quest rank
- hosts the Expedition Results Display on Floor 2

---

## 4. What It Holds
The Quest Board Room holds or tracks:
- current quest rank
- current quest rank XP
- available normal quests for today
- available main-line quests
- active normal quests
- active main-line quests
- quest progress values
- quest slot limits
- discard penalty rules
- last expedition results data for the Expedition Results Display

---

## 5. What It Takes
The Quest Board Room takes:
- quest progress from Expeditions
- quest completion results from Expeditions
- expedition result payload for the Results Display
- accepted quest handoff from the Shop request visitor flow
- player quest selection and discard input
- appeal change triggers from quest interactions

---

## 6. What It Gives
The Quest Board Room gives:
- current active objective structure to expedition systems
- automatic quest rewards on completion
- rank progression
- appeal changes from quest interactions
- quest completion records to the Info / Computer system
- quest rank XP gain data to the Expedition Results Display

---

## 7. Connected Rooms / Systems
The Quest Board Room connects to:
- Expedition systems
- Shop Room request visitor flow
- Info Room / Computer system
- Map / Expedition Planning Room
- Economy / cobalt coin flow for quest rewards
- Appeal system

---

## 8. Player Actions
The player can:
- interact with the Quest Board to enter board view
- inspect available quests on the board
- select quests to make them active
- deselect active quests to free up active slots
- discard held quests with a confirmation prompt
- view quest rank and current rank XP
- view main-line quests on the right side of the board
- review active quest progress
- exit the board view with Tab or Esc
- view the Expedition Results Display on Floor 2

---

## 9. Board Interaction

### Physical appearance
The Quest Board is a large pinboard with papers pinned to it.

It is divided into two sides:
- left side: normal quests
- right side: main-line quests

Each quest appears as a pinned paper/card on the board.
Active quests are visually marked as selected/highlighted.
Inactive held quests are visible but not highlighted.

### How to interact with the board
The player walks up to the board and presses `E` to interact.

This locks the camera into a focused board view.

### Navigation
- `WASD` or mouse movement to move focus between quest cards
- `Left Mouse Button` or `Space` to select or deselect a quest

### Selecting a quest
Selecting a quest makes it active.

Active limits apply:
- up to 2 normal quests can be active at once
- up to 1 main-line quest can be active at once

If the player tries to select a quest when the active limit is already reached:
- the selection is blocked
- a clear warning appears indicating the player must deselect an active quest first

### Deselecting a quest
The player selects an already active quest to deselect it.
This frees up an active slot so a different quest can be selected.

### Discarding a quest
Discarding is a separate action from deselecting.

To discard a held quest:
- focus the quest card
- press a dedicated discard key (to be decided, example: `Delete` or `X`)
- a confirmation prompt appears warning of the appeal penalty
- the player confirms or cancels

### Exiting the board
- press `Tab` or `Esc` to exit the board view
- camera returns to normal

---

## 10. Rules and Limits

### Quest rank system
Ranks are (from lowest to highest):
- E
- D
- C
- B
- A
- S
- Q (main-line quests only, separate from normal rank)

The Quest Board gains XP when quests are completed.
Every 250 XP causes the Quest Board to rank up.

If the current rank is C, then only C and lower rank quests are offered.
Higher rank quests become available as the board ranks up.

### Quest capacity
- can hold 5 normal quests at once
- can hold 3 Q quests which are main-line quests

### Active limits
- only 2 normal quests can be active at once
- only 1 main-line quest can be active at once

### Quest appeal rules
All quest interactions affect appeal directly.

- Accept a quest (at Service Desk): +1 appeal
- Decline a quest (at Service Desk, before it reaches the board): -2 appeal
- Complete a quest: +2 appeal
- Discard a quest (already on the board): -3 appeal

### Quest rewards rule
Quest rewards are distributed automatically when a quest is completed.
The player does not need to manually claim rewards.

Rewards may include:
- cobalt coins
- progression XP for recruits
- other items or progression hooks as defined per quest

### Quest completion record rule
When a quest is completed:
- a record is sent to the Info / Computer system
- the record shows: quest name, mission description, reward received
- if a record for that quest already exists, no duplicate is added
- completed quest records are stored permanently

### All quests are expedition-based
All quests on the Quest Board require expedition objectives to complete.
Base-level fetch tasks or delivery quests are not part of the Quest Board system.
Smaller base-level tasks from request visitors are handled separately through the Shop Service Desk flow and do not go onto the Quest Board.

---

## 11. Quest Pool System

### Two layers of quest data

#### Quest definitions (game data, not room state)
The master list of all quests that exist in the game.

Each quest definition contains:
- quest name
- mission description
- rank requirement
- objective type and target
- reward
- base appearance chance
- unlock conditions if any

Unlock conditions allow quests to be hidden until prerequisites are met.

Example:
- completing quest A adds quests B and C to the valid pool

#### Daily available quests (room state)
Each day the Quest Board refreshes its available normal quest slots by pulling from valid quest definitions.

A quest definition is valid if:
- its rank requirement matches or is below the current board rank
- its unlock conditions have been met
- it is not already active or held on the board

The daily refresh happens automatically each morning.

### Appearance chance
Each quest definition has its own appearance chance.
This controls how likely that quest is to appear on a given day when it is valid.

Example:
- a common quest might have a 40% appearance chance
- a rare quest might have a 5% appearance chance

This means not every valid quest appears every day.

### Main-line quests
Main-line Q quests are handled separately from the daily normal quest pool.
They are not subject to daily refresh in the same way.
Their availability is tied to main-line progression conditions defined per quest.

### Main-line quest givers and slot competition
Main-line Q quests come from special NPCs, primarily the Dwarf and the Professor.
More NPC quest givers may be added later.

Slot rules for incoming main-line quests:
- the board can hold up to 3 main-line Q quests at once
- an NPC can only deliver a new main-line quest if there is a free main-line slot
- if a main-line slot is free and more than one NPC is ready to give a quest at the same time, the giver is chosen randomly between them
- if no main-line slot is free, the waiting NPC holds their quest until a slot opens
- when a slot opens, the random pick happens again among whoever is currently waiting

Main-line quests still cannot be rejected or discarded by the player once delivered.

---

## 12. Expedition Results Display

### Purpose
The Expedition Results Display is a dedicated screen located on Floor 2.

It exists to:
- show the player a clear summary of what happened during the last expedition
- present quest completions, XP gains, recruit level ups, and other outcomes in one readable place
- refresh each morning after an expedition returns
- remain visible until the next expedition begins

### Location
Floor 2, near the Map / Expedition Planning area.
This is a separate dedicated display, not the Map or any existing computer.

### Refresh rule
- the display refreshes each morning after an expedition has returned
- the display stays visible and unchanged until the next expedition begins
- when the next expedition begins, the display clears until that expedition returns

### What the display shows

#### Quests section
- list of quests completed during the expedition
- each entry shows: quest name, reward received

#### Recruit XP section
- each recruit that went on the expedition is listed
- shows XP gained per recruit
- shows level up indicator if the recruit leveled up during this expedition
- example: `Jamson — +85 XP — Level Up! LV3 → LV4`

#### Quest Board section
- shows XP gained by the Quest Board from completed quests
- shows rank up indicator if the board ranked up
- example: `Quest Board — +250 XP — Rank Up! E → D`

#### Expedition duration section
- shows how long the expedition lasted in real time
- example: `Expedition Duration — 42 minutes`

#### Documents section
- shows documents gathered during the expedition
- each entry shows document name only
- full document content can be read in the Info / Computer system

#### Recruit condition section
- shows recruits who returned with injuries or debuffs
- each entry shows: recruit name, debuff name
- example: `Mara — Poisoned`

#### Recruit deaths section
- shows recruits who died during the expedition
- each entry shows: recruit name
- example: `Torben — Killed in action`

### What the display does not show
- materials brought back (visible through Storage and Grinder flow)
- items lost
- detailed item-by-item loot breakdown

---

## 13. Upgrade Levels

### LV1
- base quest board access
- normal quest storage up to 5 slots
- main-line quest storage up to 3 slots
- basic rank display
- Expedition Results Display available from the start

### LV2
- not yet fully defined
- possible: more quest slots
- possible: better rank progression visibility

### LV3
- not yet fully defined
- possible: more quest slots
- possible: improved quest filtering

---

## 14. Data Ownership

### This room owns
- available quest list for today
- active quest list
- main-line quest list
- quest rank state
- quest rank XP
- quest slot limits
- quest appeal change rules
- quest progress state
- last expedition results data for the display

### This room does not own
- recruit roster
- expedition destination state
- shop display state
- stored physical inventory
- information archive truth
- map entry ownership
- quest definition master list (this is game data, not room state)

### Important ownership note
- Shop owns the request visitor interaction and appeal change at the Service Desk
- Quest Board owns the quest once it has been accepted and handed off from the Shop
- Info / Computer system owns the completed quest history record permanently

---

## 15. UI / Readability Needs
The player should be able to clearly see:
- current rank and rank XP progress toward next rank
- available quests for today
- active quests and their progress
- main-line quests
- current quest slots used vs available
- active quest slots used vs available
- appeal change warning when discarding
- quest reward preview before accepting
- quest rank up feedback when it happens
- Expedition Results Display clearly readable on Floor 2

---

## 16. Interaction / Animation Needs
Useful early feedback:
- camera lock into board view on interaction
- quest card focus highlight when navigating
- quest card selected / active visual state
- quest card deselected visual state
- active limit reached warning when trying to select too many
- discard confirmation prompt with appeal penalty shown clearly
- quest accepted feedback with appeal gain indicator
- quest declined feedback with appeal loss indicator
- quest discarded feedback with appeal loss indicator
- quest completed feedback with reward and appeal gain indicator
- rank progression feedback
- rank up notification
- Expedition Results Display refresh when new results arrive

---

## 17. Temporary Implementation Notes
Early implementation may use:
- simplified quest card UI
- small fixed set of test quests with manual debug complete buttons
- simplified rank progression with debug XP injection
- placeholder reward structures
- simplified appearance chance logic before full pool system
- placeholder Expedition Results Display before final art pass
- hardcoded unlock conditions before full progression system exists

---

## 18. Done Condition
The Quest Board Room is considered working when:
- daily quest pool refreshes correctly based on rank and unlock state
- quest rank rules affect available quests correctly
- active quest limits work correctly
- main-line quest limits work correctly
- expedition systems can push progress correctly
- quest completion distributes rewards automatically and correctly
- appeal changes work correctly for all four interaction types
- quest rank XP accumulates correctly and rank ups at 250 XP
- completed quest records are sent to the Info system correctly without duplicates
- Expedition Results Display refreshes correctly each morning after an expedition
- Expedition Results Display shows all required sections correctly
- board interaction locks camera correctly and exits correctly with Tab or Esc
- quest selection and deselection works correctly within active limits
- active limit warning appears correctly when limit is reached
- discard confirmation prompt appears correctly with appeal penalty warning
- ownership does not conflict with Shop, Info, Map, or expedition systems

---

## 19. Open Questions
None for now.