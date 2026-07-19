# Shop Room Plan

## 1. Room Name
Shop Room

---

## 2. Purpose
The Shop Room is the main money-earning room of the base.

It exists to:
- sell crafted goods to customer NPCs
- generate cobalt coin income
- connect base preparation with daily earnings
- provide recruit, talking, and request visitor opportunities
- turn Storage goods and shop decor into daily progression results

The Shop is one of the most active rooms in the base because it mixes:
- item selling
- physical setup
- customer flow
- social/progression opportunities
- appeal-related outcomes

---

## 3. Room Identity

The Shop is not just a menu-based sale screen.

It is a physical room with:
- customer entry flow
- displays placed on a shop floor grid
- decor spots on floors and walls
- one desk for buyers
- one desk for non-buyer/service visitors
- a recruit machine inside the Shop
- a Shop Monitor that shows daily shop results and important base values

The Shop should feel like:
- a real store space
- a room the player prepares before opening
- a room where customer movement matters
- a room where progression opportunities arrive physically through visitors

---

## 4. What It Does

The Shop Room:
- opens and closes during the Day phase
- prepares a full daily visitor/customer session at shop opening
- generates buying customers, request visitors, talking visitors, and recruit visitors into one shared daily pool
- randomizes that pool order
- spawns visitors one by one over time
- lets buying customers browse, reserve goods, carry them, and bring them to the Sales Desk
- lets request, talking, and recruit visitors use the Service Desk
- tracks display placement and display contents
- keeps unsold items on displays after shop closing until they are sold or manually removed
- tracks placed shop decor
- applies display bonuses and penalties
- applies dirt penalties
- owns and applies appeal effects
- converts sold goods directly into cobalt coin income
- starts recruit intake flow by sending accepted recruit visitors to the recruit machine
- starts request flow by presenting request opportunities
- starts talking/info flow by presenting talking visitor outcomes
- updates the Shop Monitor with current shop information and daily progress

---

## 5. Physical Room Structure

The Shop currently has these important physical parts:

### Entrance doors
- the Shop has two entrance/exit doors
- NPCs can enter and leave through these doors
- the player does not use these doors to leave the room
- outside the doors are spawn/despawn flow points for NPCs

This supports:
- customer arrival
- customer exit
- clean room-based NPC flow

### Decor spots
Shop decor can be placed on:
- floor spots
- wall spots

These spots are physically marked by:
- floor plates
- wall frames

The player interacts with these spots to change placed shop decor.

### Display carpet / display zone
The Shop has a dedicated display floor/grid area where displays can be placed.

Important rule:
- displays can only be moved within the display carpet/grid area

This area is the main item-selling setup zone.

### Sales Desk
The Sales Desk is used by buying customers.

This desk handles:
- purchase decision confirmation
- buy/decline prompts
- buyer queue flow

### Service Desk
The Service Desk is used by:
- request visitors
- talking visitors
- recruit visitors

This desk handles non-buying progression/social interactions.

### Recruit machine
The recruit machine is physically located inside the Shop.

Accepted recruit visitors go from the Service Desk into the recruit machine, where recruit setup is completed before the recruit is sent to Recruit Quarters.

### Shop Monitor
The Shop Monitor is the room’s main information display for current shop performance and important shop/base values.

It should show:
- total base cobalt coins
- current appeal
- items sold today
- visitors/customers seen today
- cobalt coins gained today

---

## 6. What It Holds

The Shop Room holds or tracks:
- shop open / closed state
- daily customer/session state
- generated visitor list for the day
- visitor order/queue flow for the day
- Sales Desk queue state
- Service Desk queue state
- display carpet/grid occupancy
- placed display layout
- display type per placed display
- display slot contents
- display reserved-item state if needed
- local shop decor placement
- dirt spot state
- local sale modifiers
- local visitor/buyer chance modifiers
- shop level
- current shop capacity values
- Shop Monitor display state
- daily sold item count
- daily visitor/customer count
- daily cobalt coin earned count
- current appeal value

---

## 7. What It Takes

The Shop Room takes:
- sellable goods from Storage
- shop decor from Storage
- day/phase rules so it knows when it can open
- customer/visitor generation rules
- recruit/request/talking visitor rules
- quest system checks when request opportunities are accepted
- info system hooks when talking visitors provide information
- recruit intake handoff flow when recruit visitors are accepted
- power-state input from Core where needed for lighting/state feedback
- current total cobalt coin amount for Shop Monitor display

Important note:
- the Shop currently still functions during Core offline state
- lighting changes with Core state
- the Shop should still remain usable even during emergency/offline power state

---

## 8. What It Gives

The Shop Room gives:
- cobalt coin income from sold goods
- recruit visitor opportunities
- request opportunities
- talking/info opportunities
- item sale results back into the inventory/economy flow
- local appeal-changing outcomes if player choices affect appeal
- handoff into Recruit Machine flow
- handoff into Quest/request flow
- handoff into Info/talking result flow
- readable shop progress/results through the Shop Monitor

Important current currency rule:
- item sold -> immediately adds cobalt coins directly
- cobalt coins are the generic currency used by the base

---

## 9. Connected Rooms / Systems

The Shop Room connects to:
- Storage Room
- Core Room
- Recruit Machine Room
- Recruit Quarters
- Quest Board / quest system
- Info Room / info system
- day/phase system
- economy / cobalt coin flow
- shop decor / item definition systems

Important ownership note:
- the Shop owns appeal directly

---

## 10. Player Actions

The player can:
- place displays
- move displays while the shop is closed
- change display type while the shop is closed
- place goods into display slots
- place and remove shop decor while the shop is closed
- open the shop
- close the shop early
- clean dirt spots
- respond to buying customers at the Sales Desk
- respond to request visitors at the Service Desk
- respond to talking visitors at the Service Desk
- respond to recruit visitors at the Service Desk
- complete recruit setup through the recruit machine after acceptance
- inspect the Shop Monitor

---

## 11. Customer / Visitor Categories

The Shop currently has four broad visitor categories:

### Buying customers
These enter the Shop to browse and purchase items.

### Request visitors
These come to the Service Desk and present a request/opportunity.

### Talking visitors
These come to the Service Desk and provide dialogue interactions that can result in rewards or consequences.

### Recruit visitors
These come to the Service Desk and ask to join.
If accepted, they begin recruit intake flow through the recruit machine.

### Visual readability rule
The player should not be able to tell buyer/visitor type apart just from the NPC model alone.

Important rules:
- NPCs can use different models for variety
- buyer/visitor role should mainly be understood through behavior, destination, and interaction
- buyer personality types should remain mostly hidden rather than intentionally readable through dialogue/behavior

---

## 12. Daily Visitor Pool and Spawn Flow

### Shared visitor pool rule
Buying customers, request visitors, talking visitors, and recruit visitors are all part of the same daily visitor pool.

At shop opening:
- the full day’s visitor list is generated
- visitor categories are first determined by their own rules/amount ranges
- all generated visitors are then placed into one shared pool
- that pool order is randomized

This means the visitor categories are not balanced by one shared weighted chance at spawn time.
Instead:
- each category generates its own amount first
- then all generated visitors are mixed together into one randomized order

### Spawn interval rule
Visitors appear one by one with an interval of:
- 5 seconds

### Failed spawn fallback rule
If a visitor cannot spawn, such as:
- no valid physical space
- no items for sale for a buyer
- required queue/desk/pathing conditions are blocked

then:
- that visitor is moved to the bottom of the list
- spawn retry interval becomes 1 second
- the system keeps trying until a valid visitor can appear

This helps the daily flow continue instead of stalling.

---

## 13. Buyer Rules and Visitor Amounts

### Buyer types
Buyer NPC types:
- Normal Buyer
- Haggle Buyer
- Generous Buyer
- Non-buyer

### Buyer chances
- Normal Buyer: 75%
- Haggle Buyer: 15%
- Generous Buyer: 5%
- Non-buyer: 5%

### Daily non-buyer visitor amounts
- Request Visitor: 0-2
- Talking Visitor: 0-3
- Recruit Visitor: generated based on free recruit housing space (see rule below), not a flat range

### Recruit visitor generation rule (added this session)
Whether a recruit visitor is generated for the day's pool is no longer a flat 0-1 range. Instead:

```
chance = free recruit slots / total recruit slots
minimum floor = 25%
```

Examples: 4 free / 4 total -> 100% chance. 1 free / 12 total -> floor applies, still 25% chance. 0 free / any total -> 0% chance, no recruit visitor is generated at all that day.

This check happens once during daily visitor pool generation at shop opening, using Recruit Quarters' current capacity/roster state (see Room_RecruitQuarters.md). No ownership change: the Shop still owns generating the recruit visitor opportunity, it just now reads free-slot state from Recruit Quarters as an input.

### Important buyer rule
Buying customers can only appear if there are actual goods available for sale on displays.

If there are no goods available:
- buying customers should not be generated as valid purchasing visitors for that day/session
- or should fail spawn and be pushed down the list until valid conditions exist if the shared pool logic already created them

### Visitor pacing direction
Current visitor amount ranges are acceptable for now.
Long-term pacing can still be adjusted later through playtesting if needed.

---

## 14. Display System

### Display carpet / grid rule
Displays exist on a dedicated shop grid/carpet area.

Important rules:
- displays can only be placed inside that area
- displays can only be moved while the Shop is closed
- displays may use different shapes
- every display must still fit within the grid

### Starting display state
At the start:
- the Shop has 3 normal displays
- each normal display is 2x1 size

### Display movement / change interaction
When interacting with a display:
- pressing `T` allows the player to move it
- pressing `Q` / `E` allows the player to change display type if supported

### Display info readability
Each display should have physical readable information on two sides so the player can understand:
- what display type it is
- what effect/bonus it gives

### Goods placement interaction
The player places goods on a display by pressing `E` on the display.

This moves the camera into a focused placement view where the player can see:
- the selected display slot
- a note/readout showing item name and value
- the item physically shown on the display spot

### Placement controls
- `A` / `D` = change selected good
- `Q` / `E` = change selected display slot

### Display side interaction rule
Each display has item interaction points on two sides.

When a buying customer chooses an item:
- the NPC also chooses which side to take the item from

### Display persistence rule
Items placed on displays remain on those displays after the Shop closes.

They stay there until:
- sold
- manually removed by the player

This means the player does not need to fully restock/rebuild displays every day unless they want to change them.

---

## 15. Display Types and Effects

### General display rules
- displays can hold 1-4 items depending on type
- normal display has 2 spaces and no special effect

### Special display effects
- Gem items sell for more (2 space, +10%)
- Wood items sell for more (2 space, +10%)
- Metal items sell for more (2 space, +10%)
- One-slot display, first item sold for +100%
- Four-slot display, first item sold for -50%
- Wood items sell for more but metal sells for less (3 space)
- Metal items sell for more but wood sells for less (3 space)

---

## 16. Decor System

### Decor ownership/use rule
The Shop uses **shop decor**, not generic decor wording.

Shop decor is split into:
- Wall Decoration
- Floor Decoration

### Decor placement rule
Shop decor can only be placed in marked decor spots.

These spots are physically shown as:
- floor plates
- wall frames

### Decor interaction
When interacting with a decor spot:
- press `E` to use the spot
- use `Q` / `E` to switch between valid decor options

A note/readout under the decor should show useful information about what the currently selected decor does.

### Decor items
Each decor item is a separate purchasable piece with its own name and effect.
Multiple decor pieces can share the same effect category at different strengths.
Owning a weaker version does not upgrade into the stronger one — they are separate items.

The Merchant's progression gating rule applies here:
- weaker versions of an effect must be purchased before stronger versions can appear in the Merchant stock.

**All values below are rough placeholders and subject to balancing.**

---

#### Buyer amount decor
Three separate decor pieces, each increases daily buyer count.

- +1 Buyer Decor
- +2 Buyer Decor
- +3 Buyer Decor

---

#### Sale value decor
Three separate decor pieces, each increases item sale price.

- +5% Sale Value Decor
- +10% Sale Value Decor
- +15% Sale Value Decor

---

#### Appeal decor
Three separate decor pieces, each increases appeal.

- +2 Appeal Decor
- +4 Appeal Decor
- +6 Appeal Decor

---

#### Recruit chance decor
One decor piece, increases recruit visitor chance.

- +1 Recruit Visitor Decor

---

#### Dirt reduction decor
Two separate decor pieces, each reduces dirt spawn chance per purchase.

- -3% Dirt Chance Decor
- -7% Dirt Chance Decor

---

#### Generous buyer decor
Two separate decor pieces, each increases generous buyer chance.

- +3% Generous Buyer Decor
- +6% Generous Buyer Decor

---

#### Haggle buyer reduction decor
Two separate decor pieces, each reduces haggle buyer chance.

- -2% Haggle Buyer Decor
- -5% Haggle Buyer Decor

---

#### Non-buyer reduction decor
Two separate decor pieces, each reduces non-buyer chance.

- -2% Non-Buyer Decor
- -3% Non-Buyer Decor

---

### Decor naming note
Final in-game names for each decor piece do not need to use the effect values directly.
These names are placeholder labels for documentation clarity.
Final names can be more flavourful once art direction is decided.

## 17. Dirt Rules

### Dirt spawn rule
- each purchase rolls a 15% chance to spawn a dirt spot

### Dirt spawn location rule
Dirt spawns randomly among valid dirt spawn points.

### Dirt cleaning rule
- dirt spots must be cleaned by going to them and pressing `E`

### Dirt penalty rule
- each dirt spot lowers item prices by 5%

Dirt is one of the room’s local maintenance penalties and should remain clearly visible/readable.

---

## 18. Appeal Rules

Appeal is owned directly by the Shop.

Store appeal affects:
- buyer count
- sale bonus

Current appeal range effects:

- 0-10: -3 buyers, -20% sale bonus
- 11-20: -2 buyers, -15% sale bonus
- 21-40: -1 buyers, -10% sale bonus
- 41-60: +0 buyers, +0% sale bonus
- 61-80: +1 buyers, +10% sale bonus
- 81-90: +2 buyers, +15% sale bonus
- 91-100: +3 buyers, +20% sale bonus

Appeal should be changed by Shop-related outcomes and stored as Shop-owned state unless later architecture is intentionally changed.

---

## 19. Final Sale Price Order

Current final sale price order:

1. start with the item base value
2. apply display modifiers
3. apply shop decor sale modifiers
4. apply dirt penalties
5. apply Shop Appeal price modifier
6. apply customer negotiation modifier
7. round the final result using the game’s final price rounding rule

This order creates a clean logic stack:
- item setup
- room preparation
- room condition
- shop state
- customer-specific adjustment
- final rounding

### Example
- Base value = 10
- Display +10% -> 11
- Decor +10% -> 12.1
- Dirt -5% -> 11.495
- Appeal -10% -> 10.3455
- Haggle -10% -> 9.31095
- Final rounding rule applied -> final price

Important rule:
- customer negotiation should always happen after the Shop-level price has already been determined

---

## 20. Sales Desk

### Purpose
The Sales Desk is the desk used by buying customers.

### Physical layout
The Sales Desk should contain:
- the purchased item placed physically on the desk
- a front-bottom screen/readout showing:
  - item name
  - item value
- a separate small desk-side machine/screen with exactly 2 decision buttons:
  - green = Accept
  - red = Decline

### Customer popup
A popup appears in front of the customer and should show:
- customer name
- short spoken text

Important readability rule:
- this popup should be positioned so the desk item does not cover it

### Buying flow
A buying customer:
1. enters the Shop
2. chooses 2-4 standing/look-around points in the Shop
3. stands at each chosen browse point for a random time between 4 and 8 seconds
4. chooses and reserves a good
5. physically takes the selected item
6. carries it to the Sales Desk
7. places it on the desk
8. waits for player response

### Prompt behavior
The player does not need to separately interact with the desk to see the decision prompt.

When the customer reaches the desk:
- the customer popup appears
- the item/value readout is visible
- the Accept/Decline button machine is ready to use

### Sales Desk queue
The Sales Desk has:
- 3 queue points
- 1 active desk point

This means up to 4 buying customers can exist in the desk line structure at once.

---

## 21. Service Desk

### Purpose
The Service Desk is the desk used for non-buying visitor interactions.

It handles:
- request visitors
- talking visitors
- recruit visitors

### Physical layout
The Service Desk should contain:
- a left-side note/paper area
- NPC popup shown in front of the visitor
- a right-side display/button panel with up to 4 text-capable buttons

This allows more text-heavy interactions than the Sales Desk.

### Service Desk queue
The Service Desk has:
- 3 queue points
- 1 active desk point

This means up to 4 service visitors can exist in the desk line structure at once.

---

## 22. Request Visitor Flow

A request visitor:
- comes to the Service Desk
- presents a request
- places a paper/request sheet on the desk
- shows a popup with visitor name and short dialogue/backstory flavor

The right-side Service Desk panel can present buttons such as:
- Accept
- Decline
- Change
- Back / More Info

### Rare / progression-gated request direction
Some request visitors can be rarer/progression-gated variants.

These can depend on factors such as:
- current day
- base progression
- Quest Board rank
- what has already been done
- who has already been talked to
- other progression/history conditions

Each request visitor can also have its own explicit appearance chance, such as:
- 1%
- 5%
- or other appearance chances as needed

This means some rare visitors can be lightly gated, while others can be strongly gated.

### Important ownership note
The Shop starts the request opportunity, but should not become the final owner of the quest list.

The downstream quest system should own:
- actual held quest list
- actual active quest list
- final quest replacement validation

### Current quest capacity note
Current known quest capacity:
- 5 normal quests
- 3 main-line quests

---

## 23. Talking Visitor Flow

A talking visitor:
- comes to the Service Desk
- says something or asks something
- presents 2-4 answer choices

Possible answer results can include:
- gain cobalt coins
- gain info
- gain appeal
- lose appeal
- no effect
- multiple effects at once

If info is gained:
- the NPC says the information before leaving
- the information is stored in the computer/info system

### Rare / progression-gated talking direction
Some talking visitors can be rarer/progression-gated variants.

These can depend on factors such as:
- current day
- base progression
- Quest Board rank where relevant
- what has already been done
- who has already been talked to
- other progression/history conditions

Each talking visitor can also have its own explicit appearance chance, such as:
- 1%
- 5%
- or other appearance chances as needed

This means some rare visitors can be lightly gated, while others can be strongly gated.

### Important ownership note
The Shop owns the talking interaction opportunity and answer flow.
The final long-term info archive should belong to the Info system, not the Shop.

---

## 24. Recruit Visitor Flow

A recruit visitor:
- comes to the Service Desk
- gives a reason for wanting to join
- places a paper/profile on the desk

### Recruit popup
A popup appears in front of the recruit visitor showing:
- recruit name
- short spoken text / reason for wanting to join

Example style:
- `Jamson: Hey, I saw that you are looking for recruits. I would happily help for a roof over my head.`

### Recruit desk paper
The desk paper/profile shows:
- name
- level
- trait

This is the intended recruit information shown before acceptance.

The player can:
- Accept
- Decline

If accepted:
- the recruit visitor goes into the recruit machine inside the Shop
- the machine closes
- the nearby monitor/setup interface turns on
- the player completes recruit setup
- after confirmation, the recruit is sent to Recruit Quarters through the pipe

### Current recruit setup choices
At the machine, the player can choose:
- name change if desired
- color
- bed
- class

### Full recruit capacity rule
If there is no free recruit space:
- the recruit cannot be accepted
- the player should get a warning/notification that there is no available recruit space

Important rule:
- long-term roster replacement belongs to Recruit Quarters/recruit management, not the Shop desk

---

## 25. Shop Closing Rule

The player can close the Shop early at any time.

When the Shop closes early:
- all remaining shop NPCs leave
- the Shop closes
- unsold display items remain on displays
- the daily shop session ends

Important current decision:
- early closing has no appeal penalty or progression penalty

---

## 26. Upgrade Levels

### LV1
- Display space: 3
- Decor space: 4 (2 ground, 2 wall)
- NPC amounts:
  - Recruit: 0-1
  - Talking: 0-3
  - Request: 0-2
  - Buyer: 6-9

### LV2
- Display space: 4
- Decor space: 6 (2 ground, 4 wall)
- NPC amounts stay the same
- bigger room / model changes

### LV3
- Display space: 5
- Decor space: 8 (4 ground, 4 wall)
- NPC amounts:
  - Recruit: 0-2
  - Talking: 0-4
  - Request: 0-3
  - Buyer: 7-10
- bigger room / model changes

---

## 27. Data Ownership

### This room owns
- shop open / closed state
- current daily customer/visitor session
- visitor order/queue flow
- Sales Desk queue state
- Service Desk queue state
- display carpet occupancy/layout
- display placement
- display type assignment
- display slot contents
- local reserved sale flow for buyers
- dirt state
- local shop decor placement
- shop level
- local shop modifiers
- local buyer/service visitor interaction flow
- Shop Monitor runtime display data
- Shop-owned appeal value and appeal changes

### This room does not own
- master Storage inventory
- recruit final roster
- Recruit Machine long-term recruit output ownership
- quest master list
- info archive truth
- global room unlocks
- expedition results
- global knowledge tiers
- global day/phase ownership
- Core power ownership

### Important ownership split
The Shop:
- creates recruit opportunities
- creates request opportunities
- creates talking/info opportunities
- generates direct cobalt coin income from sales
- owns appeal directly

But downstream systems own their final truth:
- Recruit systems own final recruit state
- Quest systems own final quest state
- Info systems own final stored information state

---

## 28. UI / Readability Needs

The player should be able to clearly see:
- whether the Shop is open or closed
- how many customers/visitors remain
- what items are currently in displays
- display slot count / display type
- current dirt spot count
- dirt penalty effect
- current appeal range / bonus state
- current decor effects where relevant
- sale result feedback
- current Shop level and capacity
- request/talking/recruit prompt info when active
- Shop Monitor values:
  - total base cobalt coins
  - appeal
  - sold count
  - visitor/customer count
  - cobalt gained today
- whether a recruit acceptance would fail because of no room

---

## 29. Interaction / Animation Needs

Useful early feedback:
- display highlight when selected
- display move mode feedback
- display type change feedback
- item placed into display feedback
- decor placement highlight
- decor switched feedback
- open / close Shop feedback
- customer arrival feedback
- customer browse/wait behavior feedback
- customer taking item feedback
- customer placing item on Sales Desk feedback
- purchase accepted / declined feedback
- dirt spot visible appearance
- dirt cleaning feedback
- request/talking/recruit visitor popup appearance
- recruit enters machine feedback
- disabled movement/edit feedback while the Shop is open
- Shop Monitor updates when values change

---

## 30. Temporary Implementation Notes

Early implementation may use:
- placeholder customer visuals
- simple spawn points
- simplified browse-point selection
- simplified 4-8 second browse timing logic
- simple customer queue UI
- placeholder dirt visuals
- simplified buyer logic before full personality behavior
- simple display grid before advanced shapes
- simplified service visitor prompt flow
- temporary price calculation debug output
- simple Shop Monitor text display before final art pass

---

## 31. Done Condition

The Shop Room is considered working when:
- the player can prepare displays while the Shop is closed
- the player can place and change shop decor while the Shop is closed
- the Shop can open only during the correct phase
- a full daily customer/visitor pool is generated correctly
- visitors spawn one by one correctly
- invalid spawns are handled correctly without stalling the day
- buying customers browse, wait, reserve, carry, and present items correctly
- Sales Desk purchase flow works correctly
- request/talking/recruit visitors appear correctly at the Service Desk
- accepted recruit visitors correctly enter Recruit Machine flow
- sold items are removed correctly
- cobalt coin income is added directly and correctly
- Shop-owned appeal changes correctly
- unsold items remain on displays after closing
- dirt can spawn and be cleaned correctly
- decor and appeal modifiers affect outcomes correctly
- Shop Monitor shows correct values
- Shop ownership does not conflict with Storage, Recruit systems, Quest systems, or Info systems

---

## 32. Open Questions

None for now.