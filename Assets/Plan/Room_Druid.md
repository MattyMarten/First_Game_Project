# Druid Room Plan

## 1-10. (Unchanged from previous version)
Room name, purpose, identity, what it does/holds/takes/gives, connected rooms, player actions, and how item use works are all unchanged from the previous version. Only the debuff effect list (Section 11) changes this session.

---

## 11. Recruit Debuffs and Effects (updated this session)

**Core change:** no debuff may ever make a recruit fully unplayable. Every debuff below is a harsh penalty only — the recruit can always still be controlled and sent on expedition, just at reduced effectiveness. This replaces the previous rule where Exhaustion and Dead Man Walking made a recruit fully unusable.

---

### Exhaustion (changed)
- **Previously:** recruit was completely unusable while active.
- **Now:** recruit suffers a severe stat penalty (proposed: significantly reduced stamina, health, and speed — exact values TBD) but can still be controlled and sent on expedition.
- lasts 1-3 days
- heals over time without treatment
- treated by: **Energy Pills**

---

### Poison
Unchanged — lasts 3 days, heals over time if the recruit rests, results in death if the recruit expeditions with 0 days remaining. Treated by: **Antidote**.

---

### Injury
Unchanged — reduced maximum health, lasts 1-3 days, heals over time. Treated by: **Bandage** (removes one day) or **Medkit** (fully removes).

---

### Dead Man Walking (changed)
- **Previously:** implied the recruit needed to be treated before being "safe to use," bordering on a soft lockout.
- **Now:** recruit has critically low HP and a severe combat/survivability penalty, but can still be controlled and sent on expedition at real risk (very fragile). Does not heal over time; must be treated to remove the risk. Treated by: **Medkit** or **Elixir**.

---

### Cursed
Unchanged in structure — disables one random recruit trait per stack, stacks up to the recruit's total trait count, does not heal over time. Treated by: **Elixir** (removes all stacks at once). Note: even at max stacks (all traits disabled), the recruit remains fully controllable — trait loss is a penalty, not a lockout.

---

### Broken Bone
Unchanged — reduced movement speed and stamina, lasts 2-4 days, heals over time, recruit can still be played and sent on expeditions but slower and tires faster. Treated by: **Splint** (removes one day) or **Medkit** (fully removes).

---

### Shell Shocked
Unchanged — random startle every 2 minutes during expeditions, lasts 1-2 days, heals over time. Treated by: **Valerian Drops**.

---

### Weakened
Unchanged — 50% reduced damage, lasts 1-2 days, heals over time. Treated by: **Tonic**.

---

## 12. Medical Items List
Unchanged from previous version — Bandage, Medkit, Antidote, Energy Pills, Splint, Elixir, Valerian Drops, Tonic.

---

## 13-20. (Unchanged from previous version)
Stock rules, upgrade levels, power dependency, data ownership, UI/readability, interaction/animation, and temporary implementation notes are all unchanged.

---

## 21. Done Condition (updated)

Same as previous version's conditions, plus:
- no debuff, at any severity or stack count, ever prevents a recruit from being controlled or sent on expedition
- Exhaustion and Dead Man Walking correctly apply severe penalties rather than a usability lock

---

## 22. Open Questions
- Exact penalty values for Exhaustion and Dead Man Walking under the new non-lockout model (placeholder until balancing)
