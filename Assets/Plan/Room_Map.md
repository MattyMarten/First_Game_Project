# Sector Map Room Plan

## 1. Room Name
Sector Map Room (previously "Map / Expedition Planning Room")

---

## 2. Purpose
The Sector Map Room is the main room for choosing the player's **personal** expedition destination and entry point.

**Removed this session:** the single fixed location (Forsaken City) with named non-randomized entries. Replaced entirely by the sector category system.

It exists to:
- show unlocked sector categories and let the player choose one to personally explore
- generate a fresh random instance of the chosen category each visit
- track which sector categories are unlocked (via the sector unlock graph)
- respect the Core's current range and the Suit's current hazard resistance as hard gates on selection
- support expedition preparation by connecting the base to the outside world

This room only handles the player's **own** destination. Away-team destination selection happens at the separate Dispatch Board (see Dispatch_Board.md) and only allows already-unlocked categories, since away teams only loot rather than explore/unlock new connections.

---

## 3. Multiplayer Design Note
Unchanged — single player first, 1-4 player multiplayer is the end goal.

---

## 4. Room Identity
Unchanged physical form — a square map table, physical miniature representation, camera-locked table interaction, no abstract menus.

---

## 5. What It Does
The Sector Map Room:
- shows unlocked sector categories on the world map
- allows the player to choose a category to personally visit
- generates (or requests generation of) a fresh random instance of that category for this visit
- filters out any category the Core's current range cannot reach
- filters out any category the current Suit's Mask/Suit Material resistance cannot survive
- tracks camp tier state per sector category, if camps are still supported per category (see Section 12 — needs a decision)
- works during power loss — expedition planning is always available

---

## 6. What It Holds
The Sector Map Room holds or tracks:
- unlocked sector category list (replacing prior per-location entry-unlock state)
- selected current sector category for personal expedition
- selected current entry point within the generated instance, if entries still apply per-instance
- camp tier state, if retained (see open question, Section 12)

---

## 7. What It Takes
The Sector Map Room takes:
- sector category unlock results from expedition exploration (finding + powering a connection)
- current Core range from the Core Room
- current Suit hazard resistance from the Suit Station
- player category/entry selection input

---

## 8. What It Gives
The Sector Map Room gives:
- selected sector category (and freshly generated instance) to expedition systems
- selected entry point, if applicable, to expedition systems

---

## 9. Connected Rooms / Systems
The Sector Map Room connects to:
- Expedition systems
- Quest Board Room
- Core Room (range gating)
- Workshop / Suit Station (hazard gating)
- Dispatch Board (shares the unlocked-category list, but each station governs its own destination selection independently)
- exit door on Floor 2 for expedition launch

---

## 10. Player Actions
The player can:
- interact with the map table to enter map view
- browse unlocked sector categories on the world map
- select a category to generate/view its instance details
- review entry details for the generated instance
- exit the map view with Tab or Esc

The player does not launch the expedition from the Sector Map Room — that happens at the exit door on Floor 2, same as before.

---

## 11. Sector Category / Instance Interaction

### World map view
Shows unlocked sector categories with a distinct highlight. Locked/undiscovered categories are not shown at all.

### Category selection
When the player selects a category:
- the world map slides down
- a freshly generated instance of that category rises up in its place

Important: **the instance is regenerated every visit.** The player is never looking at "the same N" twice — they're looking at a new random N-style layout each time, though the category itself (its general theme, hazard rating, loot pool) stays consistent.

### Selection filtering
A category only appears as selectable if:
- it has been unlocked (via prior expedition discovery + powering — see Base_Master_Plan.md Section 5)
- it is within the Core's current range
- the current Suit can survive its hazard rating

If a category fails the range or hazard check, it should still be visible (the player knows it exists) but clearly marked unselectable, with the reason shown (out of range vs. suit insufficient).

---

## 12. Open Design Questions (carried over, not yet resolved)

This room's detailed content (exact categories beyond N and L, hazard values, instance generation rules, camp system relevance) is intentionally deferred — this doc only defines the shape, not the content, consistent with the "don't go too deep yet" direction from design discussion.

- Does the camp tier system (Camp Tier 1/2/3, storage/transmission features) still apply per sector *category*, or did it only make sense for the old fixed-location model and should be redesigned/removed?
- Do sector instances have "entry points" the way the old fixed location did, or is entry point selection no longer meaningful once instances are fully randomized?
- What exactly triggers "this category is now unlocked" — is it strictly the puzzle-solve + generator-power sequence described in the Master Plan, or are there other unlock paths?

---

## 13. Data Ownership

### This room owns
- unlocked sector category list
- selected current category and generated instance reference for personal expedition
- selected entry point, if retained

### This room does not own
- Core range (read-only input)
- Suit hazard resistance (read-only input)
- away-team destination/roster (owned by Dispatch Board)
- recruit roster
- quest active state

---

## 14. UI / Readability Needs
The player should be able to clearly see:
- which sector categories are unlocked
- which unlocked categories are currently unselectable due to range or hazard, and why
- the freshly generated instance details for a selected category
- that locked/undiscovered categories do not appear at all

---

## 15. Temporary Implementation Notes
Early implementation may use:
- two categories only (N, L) with placeholder hazard/range values
- simplified instance generation (even a single fixed layout per category as a placeholder, clearly marked temporary, until real randomized generation exists)
- debug unlock-category buttons
- debug override for range/hazard gating during testing

---

## 16. Done Condition
The Sector Map Room is considered working when:
- unlocked categories display correctly and locked ones do not appear
- category selection correctly filters by Core range and Suit hazard resistance, with correct unselectable feedback
- selecting a category correctly generates/displays a fresh instance
- the exit door on Floor 2 correctly reads the selected category/instance for personal expedition
- the room works correctly during Core power loss
- ownership does not conflict with the Dispatch Board, Core, or Suit Station

---

## 17. Open Questions
See Section 12.
