# Graveyard Room Plan

## 1-12. (Unchanged from previous version)
Room name, purpose, identity, what it does/holds/takes/gives, connected rooms, player actions, physical room structure, and undead creation rules are all unchanged. Only Section 13 (Overflow Rule) changes this session, to support multiple simultaneous pending deaths.

---

## 13. Undead Replacement / Overflow Rule (updated this session — now a queue)

### Why this changed
Previously this system assumed only one death could be pending a replacement decision at a time. **Away-team dispatch (new this session) can produce multiple deaths in a single morning report**, since each recruit on an away team rolls independently. The Graveyard must now handle a **queue of pending replacement decisions**, resolved one at a time, rather than a single pending state.

### If there is free undead space
Unchanged — when a recruit dies and there is free tube space, a new undead is created directly and assigned to a free tube.

### If there is no free undead space — now a queue
When a recruit dies and all undead tube slots are already filled:
- the new undead candidate is added to a **pending replacement queue** instead of a single pending slot
- if multiple recruits die in the same report (e.g. from one away-team dispatch) and there isn't room for all of them, **each one is queued individually**
- Graveyard sign glows red and stays red while the queue is non-empty
- a small alert sound plays when a new candidate is added to the queue
- the Graveyard computer boots up and presents **one candidate at a time**, in the order they were added (first in, first resolved)

### Resolving the queue
For the current candidate at the front of the queue, the computer shows their data (name, class, level, color) and the same **Accept / Decline** flow as before:

#### If player chooses Accept
- enters **Choose a Tube** mode for this candidate only
- player picks which existing undead tube to replace
- that undead is replaced by the new dead recruit's undead version
- the resolved candidate is removed from the queue
- if the queue still has more candidates, the computer automatically presents the next one

#### If player chooses Decline
- this candidate is discarded (not converted to an undead roster entry)
- the resolved candidate is removed from the queue
- if the queue still has more candidates, the computer automatically presents the next one

### Queue persistence
- the queue persists across player visits — if the player leaves without resolving every candidate, the remaining candidates stay queued and the sign stays red
- the queue does not expire or auto-resolve on its own

### Important rule (unchanged)
- replacement does **not** need a side-by-side comparison screen before final confirmation
- this replacement system is only used when there is **no free space** at the time each individual candidate is being resolved (note: since space may free up between resolving queued candidates if the player replaces multiple tubes, later candidates in the same queue could potentially find free space opened by an earlier Accept — implementation should re-check free space per candidate rather than assuming the original full-capacity state throughout)

---

## 14-17. (Unchanged from previous version)
Undead progression/repeated death rules, example undead traits, capacity rules, and upgrade levels are all unchanged.

---

## 18. Data Ownership (updated)

### This room owns
Everything previously listed, plus:
- **the pending replacement queue** (replacing the previous single pending-candidate state)
- queue order and per-candidate resolution state

### This room does not own
Unchanged from previous version.

---

## 19-21. (Unchanged from previous version, with one addition)
UI/readability needs should add: "how many candidates remain in the queue" as a visible readout when the Graveyard sign is active. Interaction/animation needs should add: "queue advances to next candidate" feedback after each Accept/Decline resolution.

---

## 22. Done Condition (updated)

Same as previous version's conditions, plus:
- multiple simultaneous deaths (e.g. from one away-team dispatch) correctly queue individually rather than overwriting or being lost
- the computer correctly presents queued candidates one at a time, in order
- the queue correctly persists if the player leaves before resolving all candidates
- resolving one candidate (Accept or Decline) correctly advances to the next queued candidate automatically

---

## 23. Open Questions
- Whether free space opened mid-queue (by an earlier Accept replacing a different tube than intended) should be automatically offered to later queued candidates instead of forcing another replacement decision
