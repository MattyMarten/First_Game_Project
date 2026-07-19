# Base Global Systems

(This file is unchanged except for Section 10, which is removed, and Section 3's list of current decisions, updated below. All other sections — Day/Phase System, Save/Load Coordination, Expedition Result/Location State System, Progression/Unlock System, Economy/Currency Flow, Appeal — remain exactly as previously written. See the earlier version for their full text.)

---

## 3. First-Pass Global Systems List — updated current decisions

Important current decisions (updated this session):
- Appeal is **not** a global system right now. Shop owns appeal directly.
- cobalt coin truth is **not** owned by a global economy system right now. Storage owns cobalt coin truth.
- **Research points (RP) and material knowledge tiers no longer exist as a system.** (Previously: "RP / unlocked knowledge truth belongs to the Research Station / research system side, not Storage." This is now removed entirely rather than reassigned — see Section 10.)
- **Recipe unlocking is now handled via Data Sticks**, a physical-item mechanic owned by Workshop (see Room_Workshop.md). This is not a global system — it's room-owned, same as before, just via a different mechanism.
- first-pass expedition/location scope stays simple for now, and now also includes the sector unlock graph and away-team dispatch routing (new this session — see Section 6a).
- **The Core is now upgradeable** (previously explicitly non-upgradeable). Core upgrades are gated by the Progression/Unlock System (Section 7) the same way special NPC unlocks are — see Room_Core.md.

---

## 6a. Sector Unlock Graph (new this session, extends Section 6)

The Expedition Result / Location State System (Section 6, unchanged in its general shape) now also needs to track:
- which sector categories are currently unlocked
- which connections within a given sector instance have already been found/powered (contributing to unlocking their target category)
- away-team dispatch routing, in addition to personal-expedition routing

This is an extension of the existing system's scope, not a new system. The "first-pass scope stays simple" principle still applies — the sector graph itself does not need to be deeply designed yet, only the ownership boundary (this system tracks it, no room owns it directly).

---

## 10. Research / Knowledge Progression — REMOVED THIS SESSION

This section previously defined RP and material knowledge tier ownership under the Research Station / Workshop. **The Research Station has been removed entirely from the game**, along with RP and material knowledge tiers. There is no longer a "research side" system to own this state, because the state no longer exists.

Recipes are now simply locked or unlocked via Data Sticks (see Room_Workshop.md Section 9). This is a much simpler ownership story: Workshop owns the unlocked-recipe flag list directly, with no intermediate progression currency involved.

The previously-open architecture question "should RP/knowledge remain under Workshop long-term, or split into a separate system?" is now **resolved by removal** rather than by decision.

---

## 13. Open Questions — updated

Previous open question removed (resolved by removal of the system it concerned). New open questions from this session:
- Exact milestone that unlocks the Core's upgrade slot on the Upgrade Board
- Exact sector unlock graph structure and hazard-rating values (deferred, not yet designed)
- Exact away-team result routing detail (new payload shape needed for Dispatch Board results, distinct from personal expedition results)

---

## 14. Notes
Unchanged from previous version.
