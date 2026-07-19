# Entry Base Bonus Loadout System

## 1. System Name
Entry Base Bonus Loadout System (Floor 2)

---

## 2. Purpose
The Entry Base Bonus Loadout System is the Floor 2 station where the player assigns extra items to take to the expedition camp.

It exists to:
- let the player pre-load items into the selected entry's camp
- give the camp meaningful value as a support zone
- connect crafted items to expedition preparation without assigning them to recruits
- make camp tier progression matter through more loadout slots

These items spawn waiting at the camp when the expedition starts. They are not carried on a recruit's body — they sit at the camp for recruits to use or collect during the expedition.

---

## 3. Relationship to the Camp System
The number of available slots is defined entirely by the camp tier of the selected entry.
The camp tier capacity (in the Map Room doc) and the entry base bonus slots are the same thing, viewed from two sides:
- the Map Room defines how many slots each camp tier has
- this station is where those slots are filled

### Slot counts by camp tier
- Camp Tier 1: 2 case slots + 2 utility slots
- Camp Tier 2: 3 case slots + 3 utility slots
- Camp Tier 3: 4 case slots + 4 utility slots

Non-camp entries have no base bonus slots at all.

The slots available at this station always match the camp tier of the entry currently selected on the Map.

---

## 4. Room Identity / Physical Form
The Entry Base Bonus Loadout System is a set of hangers on Floor 2.

Each hanger represents one bonus slot.
A hanger can hold a case or a utility item depending on its slot type.

Below each hanger is an info note showing what is currently assigned.

The hangers are distinct from the Recruit Selection stand and the recruit lockers — these are specifically the camp loadout hangers.

---

## 5. What It Does
The Entry Base Bonus Loadout System:
- shows hangers matching the selected entry's camp tier slot count
- lets the player assign cases and utility items from Storage into hangers
- auto-refills a hanger from Storage when its item type is available
- clears a hanger if Storage can no longer back it
- sends the assigned items to spawn at the camp when the expedition launches

---

## 6. What It Holds
The Entry Base Bonus Loadout System holds or tracks:
- current hanger assignments per slot
- which item type each filled hanger is set to

It does not own the items themselves — Storage owns item counts.

---

## 7. What It Takes
The Entry Base Bonus Loadout System takes:
- the selected entry and its camp tier from the Map Room (determines slot count)
- available cases and utility items from Storage
- player assignment input at the hangers

---

## 8. What It Gives
The Entry Base Bonus Loadout System gives:
- the set of camp bonus items to the expedition launch flow, which spawns them at the camp

---

## 9. Connected Rooms / Systems
The Entry Base Bonus Loadout System connects to:
- Map Room (selected entry and camp tier)
- Storage Room (item source and count truth)
- Expedition launch flow (items spawn at the camp on launch)

---

## 10. Slot Types

### Case slots
- hold cases only

### Utility slots
- hold utility items only

A hanger only accepts the item type matching its slot.

---

## 11. The Item Flow Loop

Items move through a clear cycle:

```
hanger -> entry base (camp) -> recruit -> storage -> hanger
```

Step by step:
1. the player assigns an item to a hanger; Storage count for that item drops by 1
2. on launch, the item spawns at the camp
3. during the expedition, a recruit may pick up and use the item from the camp
4. when the expedition ends, the item returns through normal return flow into Storage
5. back at base, the hanger checks Storage and auto-refills itself if that item type is available, dropping the Storage count again

### Auto-refill rule
- a filled hanger remembers its item type
- as long as Storage has that item type in stock, the hanger keeps itself filled
- when the player returns from an expedition, hangers automatically re-pull from Storage if stock exists

### Out-of-stock rule
- if Storage runs out of that item type, the hanger goes empty
- an empty-from-out-of-stock hanger does NOT remember what it wanted
- there is no waiting state — the hanger simply clears and forgets
- the player must re-assign an item when stock is available again

### Count ownership
- an item is only ever in one place: Storage, a recruit, or a hanger
- while assigned to a hanger, the item is removed from Storage's available count
- example: Storage has 5 shovels, 1 is in a hanger, so Storage shows 4 available

---

## 12. Hanger Interaction

### Entering the view
- the player walks up to the hangers and presses `E`
- the camera locks to the first hanger
- the info note below the focused hanger shows what is assigned

### Navigation
- `A` / `D` — choose which hanger slot is focused
- `Q` / `E` — cycle through valid items for the focused hanger
- "empty" is always one of the options when cycling, so a hanger can be cleared

### Item validity
- case slots only show cases
- utility slots only show utility items
- only items currently available in Storage can be selected
- selecting "empty" unassigns the hanger and returns its item to Storage availability

### Exiting
- `Tab` or `Esc` exits the view and unlocks the camera
- both keys do the same thing

---

## 13. Power Dependency
- the Entry Base Bonus Loadout System works during Core power loss
- expedition planning and loadout should never be blocked by power state

(Confirm during implementation whether hanger info notes stay readable during power loss. The assignment function itself must remain usable.)

---

## 14. Data Ownership

### This system owns
- current hanger assignments
- the item type each filled hanger is set to
- auto-refill and out-of-stock clearing behavior

### This system does not own
- item counts (owned by Storage)
- camp tier definitions and slot counts (owned by Map Room)
- selected entry (owned by Map Room)
- expedition launch state (owned by expedition systems)

### Important ownership note
- Storage owns the true item counts
- this system reduces Storage availability while items sit in hangers
- the Map Room determines how many hangers are available based on the selected entry's camp tier
- an item is never in two places at once

---

## 15. UI / Readability Needs
The player should be able to clearly see:
- how many hangers are available (matching the selected entry's camp tier)
- which hangers are filled vs empty
- what each filled hanger is set to (via the info note)
- which slot is currently focused during assignment
- which items are available to assign from Storage
- that a hanger cleared because Storage ran out

---

## 16. Interaction / Animation Needs
Useful early feedback:
- camera lock to first hanger on interaction
- hanger focus highlight when navigating
- info note updates as items are cycled
- item assigned to hanger feedback
- hanger cleared to empty feedback
- hanger auto-refill feedback when returning from expedition
- exit view on Tab or Esc

---

## 17. Temporary Implementation Notes
Early implementation may use:
- fixed slot count before camp tier system is connected
- placeholder hanger visuals
- simple info note text before final UI
- debug buttons to set Storage stock for testing
- simplified auto-refill before full return flow exists

---

## 18. Done Condition
The Entry Base Bonus Loadout System is considered working when:
- the number of hangers correctly matches the selected entry's camp tier
- non-camp entries correctly show no hangers
- case slots accept only cases and utility slots accept only utility items
- assigning an item correctly reduces Storage availability
- empty is correctly available as an option and clears a hanger
- the auto-refill loop works correctly when returning from an expedition
- a hanger correctly clears and forgets when Storage runs out
- an item is never counted in two places at once
- assigned items correctly spawn at the camp on launch
- the system works during Core power loss
- ownership does not conflict with Storage, Map Room, or expedition systems

---

## 19. Open Questions
- During power loss, should hanger info notes go dark while assignment still works, or stay readable?
- If the player switches the selected entry to one with fewer camp slots (or a non-camp entry), what happens to items already in hangers that no longer have a slot? (Suggested: they return to Storage automatically.)
