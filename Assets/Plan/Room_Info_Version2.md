# Info Room Plan

## 1. Room Name
Info Room

---

## 2. Purpose
The Info Room is the room where the player reviews all non-physical information gained through gameplay.

It exists to:
- store and present information found during expeditions
- store and present information from talking NPCs
- store and present completed quest records
- store and present enemy scan data
- give the player real gameplay value through enemy knowledge
- keep non-physical knowledge separate from physical inventory

The Info Room is presented as a computer with a command line interface.

---

## 3. Room Identity
The Info Room should feel like:
- an old terminal room
- a place where the player sits down and digs through information
- text-driven, deliberate, and focused
- distinct from every other room because it is operated by typing

The room centerpiece is a computer the player interacts with directly.
When in use, the camera moves close to the screen and the player operates it through typed commands.

---

## 4. What It Does
The Info Room:
- stores discovered documents
- stores enemy scan records
- stores talking NPC information
- stores completed quest records
- presents all of this through a command line computer
- gives the player enemy gameplay knowledge through scans
- does not require the player to leave the terminal to read any stored information

---

## 5. What It Holds
The Info Room holds or tracks:
- discovered document entries
- enemy scan entries with up to 3 info slots each
- talking NPC information entries
- completed quest records
- command definitions for the terminal

---

## 6. What It Takes
The Info Room takes:
- documents from expedition exploration
- enemy scan data from expeditions
- information from talking NPCs in the Shop
- completed quest records from the Quest Board
- player typed command input at the terminal

---

## 7. What It Gives
The Info Room gives:
- readable stored information for the player
- enemy gameplay knowledge such as weaknesses and counters
- a permanent record of completed quests
- lore and world understanding

Important rule:
- information in this room is for reading only
- it does not directly unlock anything mechanically
- some documents contain hints about what can be found during expeditions, but reading them does not trigger unlocks

---

## 8. Connected Rooms / Systems
The Info Room connects to:
- Expedition systems (documents and enemy scans)
- Shop Room talking NPC flow
- Quest Board Room (completed quest records)

---

## 9. Player Actions
The player can:
- interact with the computer to enter terminal view
- type commands to navigate information
- scroll through listed entries
- type an entry name to open its details
- read documents, enemy data, NPC info, and completed quests
- exit the terminal

---

## 10. Computer / Command Line System

### Interaction
When the player interacts with the computer:
- the camera moves close to the screen
- the player can scroll and type commands
- the player cannot move or do anything else while at the terminal until they exit

### Full typing
The terminal uses full typed command input.
The player types a command and presses Enter to run it.

### Partial command matching
Commands work even if only part of the command is typed.

Rule:
- the terminal checks if any known command starts with what the player typed
- if exactly one command matches the typed prefix, that command runs
- if multiple commands match, the terminal lists the matching options
- if nothing matches, the terminal shows a short not-recognized message

Example:
- typing `docu` runs `documents` because only one command starts with `docu`
- typing `e` might match both `enemies` and `exit`, so the terminal lists both

### Starting command set
- `help` — lists all available commands with short descriptions
- `documents` — lists all discovered documents
- `enemies` — lists all scanned enemies
- `npc` — lists all talking NPC information entries
- `quests` — lists all completed quest records
- `clear` — clears the terminal screen
- `exit` — exits the terminal and returns the camera to normal

### Opening an entry
After listing a category, the player opens a specific entry by typing its name.

Example:
- type `enemies` to see the list of scanned enemy names
- type the enemy name to read that enemy's information

The same pattern applies to documents and NPC info.

### Secret commands
Some secret commands will be added later.
These are not shown by `help` and must be discovered by the player.
Secret commands are reserved for future content and not part of the first implementation.

---

## 11. Information Categories

### Documents
- found during expeditions
- only appear in the terminal after being found
- contain lore and sometimes hints about what can be found during expeditions
- read-only, no mechanical unlock effect

### Enemy Scans
- created by scanning enemies during expeditions
- each enemy entry has 3 info slots
- each slot requires a separate scan with a longer scan duration than the last
- slots only show their content once unlocked through scanning

#### Enemy scan slot structure
- **Slot 1** — Lore (background and description)
- **Slot 2** — Abilities and Strengths
- **Slot 3** — Weaknesses and Counters

#### Enemy scan display rule
- only unlocked slots show their information
- locked slots do not show at all and give no hint of missing information
- the enemy only appears in the list once at least Slot 1 has been scanned

#### Enemy scan gameplay value
Enemy scans provide real gameplay value.
Knowing an enemy's weaknesses and counters helps the player prepare better expeditions.

### NPC Info
- gained from talking NPCs in the Shop
- only appears after the information has been given
- read-only

### Completed Quests
- sent from the Quest Board when a quest is completed
- each record shows: quest name, mission, reward
- duplicates are not added
- read-only permanent record

---

## 12. Discovery Rule
Information only appears in the terminal once it has been obtained.

- documents must be found during expeditions to appear
- enemies must be scanned to appear, and each slot must be scanned to unlock its content
- NPC info must be given by a talking NPC to appear
- completed quests appear when finished

The player has no knowledge of information they have not yet discovered.
There are no placeholder or locked entries hinting at undiscovered information.

---

## 13. Upgrade Levels

### LV1
- basic terminal access
- supports documents, enemy scans, NPC info, and completed quests
- core command set

### LV2
- not yet fully defined
- possible: better listing and sorting commands

### LV3
- not yet fully defined
- possible: search commands or filtering

Potential future upgrade effects:
- better sorting
- search functionality
- more terminal commands

---

## 14. Power Dependency
- the Info Room computer requires power
- when the Core is unpowered, the computer does not work
- the player cannot access stored information during power loss

---

## 15. Data Ownership

### This room owns
- discovered document archive
- enemy scan archive including slot unlock state
- talking NPC information archive
- completed quest record archive
- terminal command definitions

### This room does not own
- quest active state (owned by Quest Board)
- shop customer session state
- recruit roster
- physical inventory
- expedition location selection
- enemy runtime behavior during expeditions

### Important ownership note
- the Quest Board sends completed quest records here, but the Info Room owns the stored record
- expedition systems send documents and scan data here, but the Info Room owns the stored archive
- the Info Room is the truth owner for all stored information entries

---

## 16. UI / Readability Needs
The player should be able to clearly see:
- the terminal screen clearly when the camera moves in
- typed input as they type it
- command output in a readable format
- category listings when a list command is run
- full entry content when an entry is opened
- enemy entries showing only unlocked slots
- a clear not-recognized message when a command does not match
- the list of matching options when a partial command is ambiguous

---

## 17. Interaction / Animation Needs
Useful early feedback:
- camera moves to screen on interaction
- typed text appears on screen
- command output appears clearly
- screen clears on the clear command
- camera returns to normal on exit
- new information arrival can be hinted through the Notification Display if desired later

---

## 18. Technical Note — Building the Command Line

This note is implementation guidance for building the terminal.

### Core structure
- a text input field captures what the player types
- pressing Enter submits the typed string
- the submitted string is checked against a list of command definitions

### Command definitions
Each command should be a definition containing:
- the command name
- a short description for the help command
- what the command does when run

This fits the data-definition approach in the Script Architecture Guidelines.
Commands are definitions. The terminal is the runtime system that reads input and runs them.

### Partial matching logic
- compare the typed string against the start of each known command name
- collect all commands whose name starts with the typed string
- if exactly one match, run it
- if more than one match, list the matches
- if no match, show a not-recognized message

### Output
- the terminal keeps a scrollable list of output lines
- each command adds its output to the list
- the clear command empties the list

### Keep it simple first
- start with the core command set only
- do not build secret commands or search until the core works
- do not build enemy scanning logic here — that belongs to expedition systems
- this room only reads and displays information that other systems provide

---

## 19. Temporary Implementation Notes
Early implementation may use:
- simple text terminal before final screen visuals
- debug add-document, add-enemy-scan, add-npc-info buttons
- hardcoded test entries before real expedition and NPC flow exists
- core command set only
- placeholder computer and room visuals
- manual test entries to verify listing and opening works

---

## 20. Done Condition
The Info Room is considered working when:
- the player can interact with the computer and the camera moves to the screen
- typed commands work correctly
- partial command matching works correctly
- ambiguous partial commands list the matching options correctly
- the core command set works correctly
- documents, enemy scans, NPC info, and completed quests list correctly
- typing an entry name opens its details correctly
- enemy entries show only unlocked slots correctly
- undiscovered information does not appear at all
- completed quest records arrive from the Quest Board without duplicates
- the computer correctly does not work during Core power loss
- the player can exit the terminal correctly
- ownership does not conflict with Quest Board or expedition systems

---

## 21. Open Questions
- Should the terminal hint at total counts, like showing how many documents exist in the world versus found, or keep everything fully hidden until discovered?
- Should there be a command to review the last expedition's newly added information specifically?
