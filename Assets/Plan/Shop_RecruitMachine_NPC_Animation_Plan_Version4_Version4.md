# Shop + Recruit Machine NPC Animation Plan

## 1. Purpose

This document defines the NPC animation needs for:
- Shop Room
- Recruit Machine flow

Its goal is to:
- support readable NPC behavior
- define minimum required animation scope
- separate must-have animations from later polish
- make implementation planning easier

This is not a final art-quality animation document.
It is a gameplay readability and implementation support document.

---

## 2. Animation Philosophy

NPC animation in the Shop should prioritize:
- readability
- state clarity
- interaction understanding
- manageable implementation scope

The player should be able to understand:
- what an NPC is doing
- what stage of interaction they are in
- whether they are buying, requesting, talking, recruiting, waiting, or leaving

Animation should support gameplay first.
Realism and polish come after clarity.

---

## 3. NPC Behavior Groups

The Shop and Recruit Machine currently need animation support for these groups:

1. Buying customers
2. Request visitors
3. Talking visitors
4. Recruit visitors
5. Recruit Machine recruit-transfer flow

---

## 4. Shared NPC Animation Set

These animations should be available to most or all Shop visitors.

### Required shared set
- Walk
- Idle
- Queue Idle
- Turn In Place
- Leave / Exit Walk

### Nice-to-have shared set
- Look Around Idle
- Step Forward In Queue
- Weight Shift Idle
- Glance Left / Right

These shared animations provide the base readability layer for all Shop NPCs.

---

## 5. Buying Customer Animation Set

Buying customers need the largest animation set because they perform the most physical actions.

### Must-have
- Browse Idle
- Inspect Display
- Pick Up Item
- Carry Item Walk / Carry Item Idle
- Place Item On Sales Desk
- Wait For Response
- Positive Reaction
- Negative Reaction

### Nice-to-have
- Alternate Browse Idle
- Stronger Inspect Lean
- Carry Wait Variant
- Impatient Wait Variant
- Haggle Gesture
- Happy / Excited Variant

### Purpose
These animations support the full buying loop:
- browse
- choose
- pick up
- carry
- present
- react
- leave

### Personality readability rule
Buyer personality types should **not** get intentionally distinct animation/body-language differences for now.

---

## 6. Request Visitor Animation Set

### Must-have
- Walk To Service Desk
- Queue Idle
- Place Paper On Desk
- Wait At Desk
- Small Talk Gesture
- Accept Reaction
- Decline Reaction
- Leave

### Nice-to-have
- Formal Paper Presentation Variant
- Mild Frustration Reaction
- Pleased Reaction
- Thinking / Waiting Pose

### Purpose
The key readability action here is:
- placing the request paper on the desk

---

## 7. Talking Visitor Animation Set

### Must-have
- Walk To Service Desk
- Queue Idle
- Talk Gesture A
- Talk Gesture B
- Wait At Desk
- Reaction
- Leave

### Nice-to-have
- Talk Gesture C
- Questioning Gesture
- Laugh / Amused Reaction
- Confused / Disappointed Reaction

### Purpose
Talking visitors need enough gesture variety to avoid feeling static.

### Rare visitor intro rule
Rare request/talking visitors do **not** need unique intro animations for now.

---

## 8. Recruit Visitor Animation Set

### Must-have
- Walk To Service Desk
- Queue Idle
- Place Profile / Paper
- Hopeful / Nervous Idle
- Talk Gesture
- Accept Reaction
- Decline Reaction
- Walk To Recruit Machine
- Enter Machine Position

### Nice-to-have
- Relieved / Grateful Accept Reaction
- Sad / Rejected Reaction
- Nervous Fidget Variant

### Purpose
Recruit visitors should feel slightly more emotionally important than standard service visitors.

### Important current flow note
- if there is no valid recruit space, the recruit should not be accepted
- if accepted recruits stack up, they should wait in a machine queue rather than a special pending-full state

---

## 9. Recruit Machine Animation Set

### Recruit-side must-have
- Stand In Machine Position
- Wait In Machine Queue

### Machine-side must-have
- Machine Open Idle
- Machine Close
- Machine Closed Idle
- Machine Open After Confirm
- Monitor Off -> On

### Queue direction
The Recruit Machine should support:
- 2 queue spots

This allows accepted recruits to queue for machine processing if more than one recruit is accepted.

### Nice-to-have
- Machine Light Pulse
- Transfer Effect
- Confirm Success Feedback
- Blocked / Warning Feedback

### Purpose
These animations support:
- recruit intake
- setup readability
- transfer completion

---

## 10. Must-Have Tier Summary

### Shared
- Walk
- Idle
- Queue Idle
- Turn In Place
- Leave

### Buying customers
- Browse Idle
- Inspect Display
- Pick Up Item
- Carry Item
- Place Item On Sales Desk
- Wait For Response
- Positive Reaction
- Negative Reaction

### Request visitors
- Place Paper On Desk
- Small Talk Gesture
- Accept Reaction
- Decline Reaction

### Talking visitors
- Talk Gesture A
- Talk Gesture B
- Reaction

### Recruit visitors
- Place Profile / Paper
- Hopeful / Nervous Idle
- Walk To Recruit Machine
- Enter Machine Position
- Wait In Machine Queue

### Recruit Machine
- Machine Open Idle
- Machine Close
- Machine Closed Idle
- Machine Open After Confirm
- Monitor Off -> On

---

## 11. Nice-To-Have Tier Summary

- Look Around Idle
- Queue Step Forward
- Alternate Browse Idle
- Stronger Inspect Lean
- Carry Wait Variant
- Haggle Gesture
- Additional Talk Gestures
- Better Reactions
- Recruit Fidget Variants
- Machine Light / Transfer FX

---

## 12. Polish Tier Summary

- richer body-language differences between visitor types
- stronger emotional response variation
- more cinematic machine presentation
- deeper animation blending and context sensitivity

---

## 13. State-by-State Behavior Summary

### Buying customer flow
1. Spawn
2. Enter
3. Browse
4. Inspect
5. Pick up item
6. Carry to Sales Desk
7. Queue
8. Place item on desk
9. Wait for response
10. React
11. Leave

### Request visitor flow
1. Spawn
2. Enter
3. Go to Service Desk
4. Queue
5. Place request paper
6. Present request
7. React
8. Leave

### Talking visitor flow
1. Spawn
2. Enter
3. Go to Service Desk
4. Queue
5. Start conversation
6. Gesture during dialogue
7. React
8. Leave

### Recruit visitor flow
1. Spawn
2. Enter
3. Go to Service Desk
4. Queue
5. Place recruit profile
6. Present recruit dialogue
7. Accepted or declined
8. If accepted, walk to machine
9. Wait in machine queue if needed
10. Enter machine
11. Transfer after setup

---

## 14. First-Pass Implementation Recommendation

The first pass should focus on:
- shared movement/idle set
- buyer readability actions
- service desk paper/talk actions
- recruit walk-to-machine flow
- recruit machine queue readability
- machine close/open states

Do not block implementation on:
- high animation variety
- cinematic polish
- advanced personality-specific behaviors

The first successful version should aim for:
- correct behavior readability
- correct state transitions
- correct interaction understanding

### Machine presentation priority rule
Recruit Machine presentation should stay simple in first pass.
Use only simple VFX and clear readable machine states.
Do not spend time on fancy cinematic presentation early.

---

## 15. Open Questions

None for now.