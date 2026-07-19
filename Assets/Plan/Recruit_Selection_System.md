# Recruit Selection System

## 1. System Name
Recruit Selection System (Floor 2)

---

## 2. Purpose
The Recruit Selection System is the Floor 2 station where the player chooses which recruits go on the next expedition.

It exists to:
- let the player assemble the expedition party
- keep expedition party selection separate from full recruit management
- present recruit identity, info, and equipment clearly before launch
- support both living recruits and undead in the same party

This is the Floor 2 counterpart to Recruit Quarters. Recruit Quarters (Floor 1) handles long-term recruit management; this station only handles who goes on the expedition.

---

## 3. Multiplayer Design Note
This game is designed first as single player, with a 1 to 4 player multiplayer end goal.
The 4-recruit party size aligns with the eventual 4-player cap.
This system should be built with that future in mind.

---

## 4. Room Identity / Physical Form
The Recruit Selection System is a deployment stand on Floor 2.

It physically contains:
- 4 deployment pods/spots arranged on a stand
- a small monitor beside the stand

Each filled pod shows a small standee/representation of the assigned recruit.
The monitor shows detailed info for the currently highlighted recruit.

The stand is distinct from the other Floor 2 stations:
- the Map is a table
- the Quest Board is a pinboard
- the Info Room is a computer
- the Recruit Selection is a physical squad stand with a monitor

---

## 5. What It Does
The Recruit Selection System:
- displays the current expedition party across 4 pods
- locks pod 1 to the recruit the player is currently controlling
- lets the player assign available recruits to pods 2, 3, and 4
- shows recruit name, info, and equipment on the monitor
- allows mixing living recruits and undead in the same party
- provides the final party selection to the expedition launch flow

---

## 6. What It Holds
The Recruit Selection System holds or tracks:
- current expedition party assignments (pods 1-4)
- which pod is currently focused during selection

It does not hold the recruit roster itself — that is owned by Recruit Quarters (living) and Graveyard (undead).

---

## 7. What It Takes
The Recruit Selection System takes:
- the currently controlled recruit (auto-assigned to pod 1)
- available living recruits from Recruit Quarters
- available undead from the Graveyard
- recruit availability state (exhausted recruits are excluded)

---

## 8. What It Gives
The Recruit Selection System gives:
- the final expedition party of up to 4 recruits to the expedition launch flow

---

## 9. Connected Rooms / Systems
The Recruit Selection System connects to:
- Recruit Quarters (living recruit roster and availability)
- Graveyard (undead roster and availability)
- Expedition launch flow (exit door reads the selected party)
- Map Room (destination and entry are selected separately)

---

## 10. Party Rules

### Party size
- up to 4 recruits per expedition

### Pod 1 — the controlled recruit
- pod 1 is always locked to the recruit the player is currently controlling
- the player cannot change pod 1 at this station
- to change pod 1, the player leaves the station and takes control of a different recruit in Recruit Quarters or the Graveyard, then returns
- this keeps control-switching in one consistent place (interacting with recruits directly)

### Pods 2, 3, 4 — companions
- the player freely assigns available recruits to these pods
- each recruit can only occupy one pod
- a pod can be set to empty

### Living and undead mixing
- a single party can mix living recruits and undead freely
- both appear in the same available pool during selection

### Availability rule
- exhausted recruits cannot be selected and do not appear as available
- exhaustion is the only condition that blocks selection
- all other debuffed recruits (Poisoned, Injured, Broken Bone, etc.) can still be selected

---

## 11. Selection Interaction

### Entering the view
- the player walks up to the stand and presses `E`
- the camera locks into the selection view

### Navigation
- `Q` / `E` — choose which pod slot is focused (pods 2, 3, 4; pod 1 is locked)
- `A` / `D` — cycle through available recruits for the focused pod
- "empty" is always one of the options when cycling, so a pod can be cleared

### Monitor readout
While a recruit is highlighted, the monitor shows:
- recruit name
- recruit info (class, level, traits, condition)
- recruit equipment

### Locked pod rule
- pod 1 cannot be focused or changed
- attempting to select pod 1 is blocked
- pod 1 visibly shows it is the controlled recruit (for example a "YOU" marker or distinct frame)

### Exiting
- `Tab` or `Esc` exits the view and unlocks the camera
- both keys do the same thing

---

## 12. Power Dependency
- the Recruit Selection System works during Core power loss
- expedition planning and selection should never be blocked by power state
- the monitor may lose its readout during power loss, matching how other powered screens behave, but the selection itself still functions

(Confirm during implementation whether the monitor stays readable or goes dark during power loss. The selection function itself must remain usable.)

---

## 13. Data Ownership

### This system owns
- current expedition party assignments (pods 1-4)
- currently focused pod during selection

### This system does not own
- living recruit roster (owned by Recruit Quarters)
- undead roster (owned by Graveyard)
- recruit equipment (owned by Recruit Quarters / Graveyard)
- which recruit is controlled (owned by recruit control flow)
- expedition destination and entry (owned by Map Room)
- expedition launch state (owned by expedition systems)

### Important ownership note
- this station only owns the party assignment for the next expedition
- it reads recruit data from Recruit Quarters and Graveyard
- pod 1 reflects the controlled recruit but does not own control state

---

## 14. UI / Readability Needs
The player should be able to clearly see:
- all 4 pods and which are filled vs empty
- which pod is the locked controlled recruit
- the highlighted recruit's name, info, and equipment on the monitor
- which recruits are available vs unavailable
- that exhausted recruits do not appear
- the currently focused pod during selection

---

## 15. Interaction / Animation Needs
Useful early feedback:
- camera lock into selection view on interaction
- pod focus highlight when navigating
- recruit assigned to pod feedback
- pod cleared to empty feedback
- locked pod 1 indication
- monitor updates as recruits are cycled
- exit view on Tab or Esc

---

## 16. Temporary Implementation Notes
Early implementation may use:
- simplified pod visuals or placeholder standees
- simple monitor text before final UI
- debug buttons to set the controlled recruit for testing
- placeholder recruit data if roster systems are not fully connected yet
- simplified availability checks before full debuff system exists

---

## 17. Done Condition
The Recruit Selection System is considered working when:
- the station enters selection view on interaction and exits on Tab or Esc
- pod 1 correctly locks to the controlled recruit and cannot be changed
- pods 2-4 can be assigned and cleared correctly
- empty is correctly available as an option
- exhausted recruits are correctly excluded from selection
- living and undead can be mixed in one party
- the monitor correctly shows name, info, and equipment for the highlighted recruit
- the final party is correctly passed to the expedition launch flow
- the system works during Core power loss
- ownership does not conflict with Recruit Quarters, Graveyard, or expedition systems

---

## 18. Open Questions
- During power loss, should the monitor go dark (matching other powered screens) while selection still works, or stay readable?
- Should the station show a warning if the player launches with fewer than 4 recruits, or is launching with a smaller party always allowed silently?
