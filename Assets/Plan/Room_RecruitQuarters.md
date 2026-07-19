# Recruit Quarters Room Plan

## 1. Room Name
Recruit Quarters

---

## 2. Purpose
Unchanged from previous version — main room for storing, managing, and preparing recruits, and the main long-term recruit ownership room.

**Changed this session:** capacity numbers, bed-per-room count, and a new retire action. See Sections 16 and 16a.

---

## 3-9. (Unchanged from previous version)
Room identity, what it does, what it holds, what it takes, what it gives, connected rooms, player actions all remain as previously written. Not repeated here in full — see prior version for complete text. Only the sections below are new or changed.

---

## 10. Physical Room Structure (updated)

### Recruit rooms
Recruit Quarters is divided into recruit rooms. **Each recruit room now contains 2 beds (reduced from 4).**

### Bed ranges (updated)
- Room 1 = Beds 1-2
- Room 2 = Beds 3-4
- (Room 3, if LV2: Beds 5-6)
- (Room 4, if LV3: Beds 7-8)

The base **starts completely full**: the Dwarf's four fixed starting recruits (Mara, Brok, Pip, Vael) occupy all 4 starting beds from game start. There is no empty starting capacity.

Everything else in this section (arrival pipe behavior, spawn/fall/walk-to-bed flow) is unchanged.

---

## 11-15. (Unchanged from previous version)
Outside display, locker interaction model, locker equipment layout, equipment assignment rules, and locker controls are all unchanged.

---

## 16. Rules and Limits (updated)

### Recruit control rule
Unchanged.

### Recruit progression rules
Unchanged (level 1 start, 1 trait, new trait at LV5 and LV10, hidden stat growth).

### Debuff rule — changed this session
**Recruit debuffs/status effects are never a full unplayability lock.** Every debuff is a harsh penalty (reduced stats, reduced speed, disabled traits, etc.) but the recruit can always still be controlled and sent on expedition, at reduced effectiveness. This replaces the previous design where Exhaustion made a recruit fully unusable. See Room_Druid.md for the updated per-debuff effect list.

This removes the previous softlock risk entirely (an earlier draft proposed an auto-skip-time safety net for "what if every recruit is unusable" — that safety net is no longer needed, since no combination of debuffs can ever fully lock out every recruit).

### Trait generation rule
Unchanged.

### Capacity rules — changed this session
- **LV1: 4 recruits** (2 rooms × 2 beds) — matches the 4 fixed starting recruits exactly
- **LV2: 6 recruits** (3 rooms × 2 beds)
- **LV3: 8 recruits** (4 rooms × 2 beds)

(Previously LV1/2/3 was 8/10/12 with 4-per-room. This was revised down specifically to keep recruit scarcity meaningful throughout the whole game rather than only at the start.)

A recruit cannot be added if no valid recruit slot/bed exists.

---

## 16a. Retiring a Recruit (new this session)

The player can retire a living recruit, permanently removing them from the roster.

- Triggered at the recruit's own locker, with a confirmation prompt (to prevent accidental loss).
- Applies a **-3 appeal** penalty.
- Does **not** create an undead. Only actual expedition/away-team death feeds the Graveyard — retirement is a clean removal, not a death.
- Retiring frees a recruit slot, which in turn affects the Shop's recruit-visitor spawn chance (see Room_Shop.md, free-slot ratio formula).

This gives the player deliberate control over their own scarcity/capacity, rather than only reacting to death RNG.

---

## 17. Upgrade Levels (updated)

### LV1
- can hold 4 recruits
- 2 recruit rooms (2 beds each)
- basic recruit management, basic locker assignment support

### LV2
- can hold 6 recruits
- 3 recruit rooms (2 beds each)

### LV3
- can hold 8 recruits
- 4 recruit rooms (2 beds each)
- largest current recruit roster support

---

## 18-21. (Unchanged from previous version)
Data ownership, UI/readability needs, and interaction/animation needs are unchanged in shape — only the numeric capacity values and the addition of a retire action apply, as described above. Add "recruit retired feedback" to the interaction/animation needs list.

---

## 22. Done Condition (updated)

Recruit Quarters is considered working when (previous conditions still apply, plus):
- capacity is correctly 4/6/8 across LV1/2/3 with 2 beds per room
- the base correctly starts completely full with the four fixed starting recruits
- retiring a recruit correctly removes them from the roster, applies -3 appeal, and does not create an undead
- no debuff or combination of debuffs ever fully blocks a recruit from being controlled or sent on expedition
- ownership does not conflict with Storage, Recruit Machine, Druid, or Graveyard

---

## 23. Open Questions
None new this session beyond what's carried in Open_Architecture_Questions.md.
