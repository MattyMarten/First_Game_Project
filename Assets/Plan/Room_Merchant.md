# Merchant Room Plan

## 1-9. (Unchanged from previous version)
Room name, purpose, identity, what it does/holds/takes/gives, connected rooms, and player actions are unchanged. Only the pedestal layout (Section 10) and daily stock rules (Section 11) change this session, to add the Data Stick pedestal.

---

## 10. Pedestal Layout (updated this session)

The Merchant Room now has **14 pedestals** (increased from 13), organized by category.

### Utility pedestals — 3 pedestals
Unchanged.

### Case and Belt pedestals — 1 pedestal
Unchanged.

### Accessory pedestals — 2 pedestals
Unchanged.

### Material pedestals — 3 pedestals
Unchanged. (Note: materials rolled here are now flat/non-tiered, consistent with the removal of material knowledge tiers — see Room_Workshop.md.)

### Shop decor pedestals — 4 pedestals
Unchanged.

### Data Stick pedestal — 1 pedestal (new this session)
- holds 0 or 1 Data Stick
- each morning, this pedestal has a flat **10% chance** of rolling a Data Stick
- if it rolls, the specific stick offered is drawn from the Merchant's own **curated Data Stick pool** (a fixed list of recipes the Merchant is allowed to sell — see Room_Workshop.md Section 9 for the curated-pool model)
- if it does not roll, the pedestal is empty for the day
- price is set the same way as other pedestals (base price with the standard -10% to +10% daily variation)
- purchasing it auto-consumes it exactly like any other Data Stick acquisition (unlocks the recipe immediately, or converts to materials/coins if already unlocked)

---

## 11. Daily Stock Rules (updated)

Unchanged in structure — all pedestals (now including the Data Stick pedestal) roll independently each morning and do not refresh mid-day. The Data Stick pedestal's roll is a simple 10% chance check rather than "always rolls something," since it's meant to feel rare.

---

## 12-21. (Unchanged from previous version)
Price variation, purchase rules, shop decor progression gating, power dependency, upgrade levels, data ownership, UI/readability, interaction/animation, and temporary implementation notes are all unchanged, aside from the addition noted above.

---

## 22. Done Condition (updated)

Same as previous version's conditions, plus:
- the Data Stick pedestal correctly rolls a 10% daily chance
- when it rolls, the offered stick correctly comes from the Merchant's curated pool only
- purchasing a Data Stick correctly triggers the same auto-consume/duplicate-conversion behavior as any other source

---

## 23. Open Questions
- Exact contents of the Merchant's curated Data Stick pool (which specific recipes the Merchant is allowed to ever offer)
