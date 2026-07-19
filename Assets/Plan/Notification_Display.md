# Notification Display

## 1. System Name
Notification Display

---

## 2. Purpose
The Notification Display is a base-wide alert system that keeps the player informed about things that need their attention.

It exists to:
- surface important base events without interrupting gameplay
- direct the player to the correct floor and room
- show the current Core state at all times
- reduce the chance of missing important events like recruit deaths or finished upgrades
- support the base's physical-first design by keeping information in the world rather than in menus

---

## 3. Physical Appearance
The Notification Display is a small vertical monitor mounted on the wall near the stairs between floors on Floor 1.

It is always visible from the main base area.

The screen shows two sections:

### Core State section
Always visible at the top of the display regardless of other notifications.

Format:
```
CORE STATE: Normal
```
```
CORE STATE: Warning
```
```
CORE STATE: Offline
```

This line is permanent and updates automatically as the Core state changes.
It is not a notification — it is a live status readout.

### Notification list section
Below the Core State line, active notifications are listed.

Each notification is one short line in this format:
```
Floor [X] - [Room Name] - [What]
```

Examples:
```
Floor 0 - Dwarf Room - Talk to Dwarf
Floor 1 - Workshop - Research Complete
Floor 1 - Graveyard - New Undead Available
Floor 0 - Dwarf Room - Upgrade Complete
```

---

## 4. Notification Rules

### Maximum visible notifications
The display shows a maximum of 5 notifications at once.

If more than 5 notifications are active:
- the oldest unaddressed ones wait off screen
- when a notification clears, the next waiting one appears

### No priority ordering
Notifications appear in the order they arrived.
There is no priority system — first in, first shown.

### No player interaction needed
The player does not need to walk to the display and press anything to manage it.
Notifications clear automatically when the player addresses the thing that caused them.

Examples:
- Upgrade Complete clears when the player interacts with the finished upgrade room
- Research Complete clears when the player interacts with the Research Station
- New Undead Available clears when the player visits the Graveyard
- Talk to Dwarf clears when the player has the conversation with the Dwarf

### Appearance and disappearance
Notifications appear as soon as the triggering event happens.
They disappear as soon as the player addresses the cause.
No animation or dismissal interaction is needed beyond addressing the source.

---

## 5. Current Notification Types

### Talk to Dwarf
```
Floor 0 - Dwarf Room - Talk to Dwarf
```
Triggered when the Dwarf wants to speak to the player.
Clears after the conversation is completed.

---

### Upgrade Complete
```
Floor 0 - Dwarf Room - Upgrade Complete
```
Triggered when a room or station upgrade finishes overnight.
Clears when the player visits the Dwarf Room or the upgraded room.

---

### Research Complete
```
Floor 1 - Workshop - Research Complete
```
Triggered when a Research Station research finishes.
Clears when the player interacts with the Research Station.

---

### New Undead Available
```
Floor 1 - Graveyard - New Undead Available
```
Triggered when a recruit dies and a new undead has been created or is waiting for a replacement decision.
Clears when the player visits the Graveyard and addresses the pending state.

---

### New Main-Line Quest
```
Floor 2 - Quest Board - New Main-Line Quest
```
Triggered when the Dwarf or Professor delivers a new main-line quest to the Quest Board.
Clears when the player views the Quest Board.

---

### Professor Wants to Speak
```
Floor 0 - Professor Room - Professor Wants to Speak
```
Triggered when the Professor has a new quest, a new scanner recipe, or something to say.
Clears after the conversation is completed.

---

### Special NPC Call
The Dwarf and Professor actively use the Notification Display today.
Other special NPCs such as the Druid and Rat Merchant may call the player for their own reasons as their stories develop later.

Format:
```
Floor [X] - [NPC Room] - [NPC Name] Wants to Speak
```

---

## 6. Core State Display

The Core State line is separate from the notification list.
It is always shown at the top of the display.
It updates automatically and does not count toward the 5 notification limit.

### States
- **Normal** — base is powered, no issues
- **Warning** — Core reserves are too low for the next upkeep payment
- **Offline** — Core has shut down, power-dependent systems are inactive

The Core State line does not clear — it simply updates as the state changes.

---

## 7. Data Ownership

### This system owns
- active notification list
- notification arrival order
- notification clear conditions

### This system does not own
- Core power state (owned by Core Room)
- upgrade progress state (owned by Dwarf Room)
- research progress state (owned by Research Station)
- recruit death state (owned by Graveyard)
- Dwarf conversation trigger state (owned by Dwarf Room)
- Professor conversation trigger state (owned by Professor Room)
- main-line quest delivery state (owned by Quest Board)

### Important ownership note
The Notification Display is a read-only output system.
It reads state from other rooms and systems and displays it.
It does not own any of the underlying state it shows.

---

## 8. Connected Rooms / Systems
The Notification Display connects to:
- Core Room
- Dwarf Room
- Professor Room
- Quest Board Room
- Workshop / Research Station
- Graveyard
- any future NPC room that triggers a call notification

---

## 9. UI / Readability Needs
The player should be able to clearly see:
- current Core state at all times from the display
- up to 5 active notifications at once
- exactly which floor and room needs attention
- exactly what action is needed

The display should be readable from a normal standing distance without needing to walk up to it.

---

## 10. Temporary Implementation Notes
Early implementation may use:
- simple text list with placeholder monitor visuals
- hardcoded notification trigger points before full event system exists
- debug buttons to manually fire test notifications
- simplified clear conditions before full room interaction tracking exists

---

## 11. Done Condition
The Notification Display is considered working when:
- Core State line updates correctly for all three Core states
- all current notification types fire correctly when triggered
- notifications clear correctly when the player addresses their cause
- maximum 5 notifications are shown at once with overflow waiting correctly
- notifications appear in arrival order with no priority system
- the display is readable from a normal standing distance
- ownership does not conflict with any room or system it reads from

---

## 12. Open Questions
- Should the display have a subtle ambient glow or pulse when new notifications arrive to draw the player's eye, or stay completely static?
- Should NPC call notifications include the NPC's name or just the room name?
