# Dispatch Board Plan (New System, Floor 2)

## 1. System Name
Dispatch Board

---

## 2. Purpose
The Dispatch Board is a new Floor 2 station where the player sends an **away team** — recruits who resolve a sector visit without the player present — separately from the player's own personal expedition party.

It exists to:
- give meaning to a larger recruit roster beyond "who's in my party today"
- add passive risk/reward that doesn't require the player to personally play every recruit
- let the player choose to skip personally expeditioning some (or all) nights while still making progress
- reinforce recruit scarcity by rolling independent death chances per away-team recruit

This is distinct from the Recruit Selection station (which handles the player's own party) and from the Sector Map (which handles the player's own destination). The Dispatch Board only handles **already-unlocked** sector categories, since an away team only loots — it never explores or discovers new connections the way the player personally does.

---

## 3. Room Identity / Physical Form
A station visually distinct from the Recruit Selection stand, to reinforce that this team is not physically present with the player. Suggested form: a manifest/clipboard-style board rather than standee pods — a roster list, not a lineup.

Located on Floor 2, paired near the Recruit Selection stand (since "who's in my party" and "who's on the roster to send elsewhere" are conceptually related decisions, even though the interactions differ).

---

## 4. What It Does
The Dispatch Board:
- shows a scrollable roster of all recruits not currently assigned to the player's own party and not dead/retired
- lets the player select any number of these recruits (including just one) to form an away team
- lets the player choose a destination from the already-unlocked sector category list
- shows a computed success/risk percentage and a loot preview for the selected category before confirming
- on confirmation, marks the team as deployed for the night
- resolves the outcome the next Morning (or immediately if the player also skips their own personal expedition — see Section 7)

---

## 5. What It Holds
- current away-team roster selection
- currently selected away-team destination
- deployed/resolved state for the current night

---

## 6. What It Takes
- the list of recruits not in the player's own party and currently available (not dead/retired/already assigned)
- the unlocked sector category list (shared source with the Sector Map, but this station applies its own filtering — see Section 8)
- Suit hazard resistance (same hard gate as personal expeditions — if the Suit can't survive a category, it can't be selected here either)
- player roster and destination selection input

---

## 7. What It Gives
- an away-team result payload each morning: per-recruit outcome (survived/injured/died), loot gained
- this feeds the same Expedition Results Display used for personal expeditions, as a separate "away team" section
- if the player deploys an away team without a personal expedition, the day skips straight to the next Morning (see Base_Master_Plan.md Section 3)

---

## 8. Rules and Limits

### Team size
Fully player-controlled. A team of one recruit is valid. There is no minimum or fixed split — the player decides how much of their remaining roster to risk.

### Destination filtering
Only sector categories already unlocked can be selected (matches the Sector Map's unlock list) — but unlike the Sector Map, there is no fresh "instance" concept needed here, since the away team never actually explores a specific generated layout. Each category has:
- a **risk %** (its success chance, likely derived from or equal to the category's hazard rating — see Base_Master_Plan.md Section 8, "one stat, two systems" idea)
- a **loot preview** (what kind of materials/items that category's away-team loot table can produce)

### Resolution — independent per-recruit rolls
This is the core scarcity mechanic:
```
for each recruit on the away team:
    roll against the category's success %
    pass -> recruit returns safely
    fail -> recruit is injured or dies (weighted roll, TBD exact table)
```
A larger team does not roll once for the whole group — **every recruit individually rolls**, so bigger teams face more total risk exposure even though the base success % doesn't change per recruit. This is intentional: it means players can't simply throw the whole spare roster at a location and expect them all to come home safely.

### Loot resolution
Loot is granted based on the category's loot table, likely scaled by team size and/or how many recruits survived (exact formula TBD — a reasonable first pass: base loot amount scales with team size, regardless of survival, since loot happens during the trip before any death roll resolves at the end).

---

## 9. Interaction

### Entering the view
Walk up, press `E`, camera locks to the manifest.

### Navigation
- `W` / `S` — scroll the recruit list
- `Space` — toggle a recruit onto/off the away-team manifest
- separately, select destination (from the same station or via a linked prompt — exact UI TBD, likely a secondary panel showing unlocked categories once at least one recruit is selected)

### Confirmation
Once at least one recruit and a destination are selected, confirm to deploy. Exiting without confirming discards the in-progress selection.

### Feedback while deployed
The board should clearly show "X recruits — [category] — Deployed" for a team currently out, so the player doesn't lose track of who's away.

---

## 10. Connected Rooms / Systems
- Recruit Quarters (roster source, injury/death outcome target)
- Graveyard (death outcome target — may produce multiple simultaneous deaths, requiring the Graveyard's replacement queue, see Room_Graveyard.md)
- Sector Map (shares the unlocked-category list; does not share instance/entry state)
- Workshop / Suit Station (hazard gating)
- Quest Board / Expedition Results Display (away-team report)

---

## 11. Data Ownership

### This system owns
- current away-team roster selection
- current away-team destination selection
- deployed/resolving state

### This system does not own
- recruit roster truth (Recruit Quarters)
- undead roster (Graveyard)
- sector unlock state (Sector Map / global unlock system)
- Suit hazard resistance (Suit Station)

---

## 12. UI / Readability Needs
- clear list of available (non-deployed, non-party, alive) recruits
- clear risk % and loot preview per selectable category
- clear "currently deployed" state while a team is out
- clear away-team report the following Morning, distinct from the personal-expedition report

---

## 13. Temporary Implementation Notes
- placeholder risk %/loot values per category until sector hazard/loot design is finalized
- simplified manifest UI (even a plain list before final art)
- debug force-resolve button for testing outcomes without waiting for morning

---

## 14. Done Condition
The Dispatch Board is considered working when:
- available recruit list correctly excludes the player's own party, dead, and retired recruits
- team size selection is fully free (1 to full remaining roster)
- destination selection correctly filters to unlocked-only categories and respects Suit hazard gating
- risk %/loot preview displays correctly before confirmation
- deploying without a personal expedition correctly skips straight to next Morning
- resolution correctly rolls independently per recruit rather than once for the team
- results correctly feed the Expedition Results Display and Graveyard (with queued handling for multiple simultaneous deaths)
- ownership does not conflict with Recruit Quarters, Graveyard, Sector Map, or Suit Station

---

## 15. Open Questions
- Exact away-team success % formula (proposed starting shape: base % + recruit level factor − category hazard factor, clamped)
- Exact loot scaling formula relative to team size/survival
- Exact physical UI for destination selection at this station (secondary panel vs. shared prompt with Sector Map)
