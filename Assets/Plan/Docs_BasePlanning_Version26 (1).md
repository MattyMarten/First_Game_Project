# 1. Shop

## Purpose
- Main economy building
- Main customer interaction building
- Handles selling, special visitors, and survivor hiring
- Connects player economy, Guild progression, and recruit flow

## Structure
The Shop has **3 desks**:
- **Desk 1 — Customer Desk / Goods Desk**
- **Desk 2 — Merchant / Request / Talking Desk**
- **Desk 3 — Hiring / Survivor Recruitment Desk**

The Shop also contains:
- **Shop Displays / Display Area**
- **Browse Points**
- **Shop Appeal**
- **Daily visitor generation**
- **Daily report**

---

## Shop day structure
- The Shop can be opened **once per day**
- When the Shop opens, the game generates that day’s full visitor list
- Visitors spawn from that list over time during the shop phase
- The Shop stays open until the player closes it
- When the player closes the Shop:
  - active visitors leave
  - remaining unspawned visitors are discarded
  - a daily report is shown
  - time skips to evening

### Early close rule
- If the player tries to close the Shop while there are still meaningful unresolved visitors, show a confirmation prompt
- Prompt example:
  - "Are you sure you want to close the shop before talking to all the customers?"
- If the player confirms, the Shop closes immediately
- There is no extra punishment for closing early

### Early close warning logic
Show the close confirmation only if at least one of the following is true:
- there is an active visitor currently in the Shop
- there is an unspawned Desk 2 visitor remaining in the daily list
- there is an unspawned Desk 3 visitor remaining in the daily list
- there is an unspawned buying customer remaining in the daily list **and** at least one valid displayed good exists

If only blocked buying customers remain and there are no valid displayed goods, do not show the early close warning.

---

## Shop Appeal

### Purpose
- Shop Appeal is the hidden value that represents how well the Shop is doing
- It is shown to the player only as an icon and a number

### Range
- Shop Appeal range = **0–100**
- Starting Shop Appeal = **50**

### Appeal bands
- **0–19** = very low
- **20–39** = low
- **40–59** = normal
- **60–79** = good
- **80–100** = great

### Clamp rule
- Shop Appeal cannot go below **0**
- Shop Appeal cannot go above **100**

### Buying customer amount modifier by Appeal band
- **0–19** = -2 buying customers
- **20–39** = -1 buying customers
- **40–59** = +0 buying customers
- **60–79** = +1 buying customers
- **80–100** = +2 buying customers

### Sale price modifier by Appeal band
- **0–19** = -20%
- **20–39** = -10%
- **40–59** = 0%
- **60–79** = +10%
- **80–100** = +20%

### Final sale price order
1. Start with the item base value
2. Apply the Shop Appeal price modifier
3. Apply the customer negotiation modifier
4. Round the final result using the game’s final price rounding rule

Example:
- Base value = 10
- Shop Appeal = 25 → low band = -10%
- Price becomes 9
- If the customer then gets -10% through negotiation, that is applied to 9, not 10

### Current Act 1 planned Shop Appeal changes

#### Desk 1
- Accept sale = **+1**
- Reject sale = **-1**

#### Desk 2 — Merchant
- Reject merchant = **-5**
- Open wares but buy nothing = **-5**
- Buy at least one item = **+1**

#### Desk 2 — Request
- Accept request = **+1**
- Reject request = **-2**
- Complete request = **+2**
- Shred stored request = **-3**

#### Desk 2 — Talking
- Depends on response result
- Can increase, decrease, or not change Shop Appeal

#### Desk 3 — Hiring
- Accept hire = **+1**
- Reject hire = **-2**
- Sending an existing recruit away to make room = **-3**

---

## Daily visitor generation

### Generation timing
- The full visitor list is generated when the Shop opens for the day
- The game first determines how many visitors of each type will appear
- It then creates the full visitor list
- The final list is randomized into spawn order

### Visitor types in the daily list
- Desk 1:
  - Buying customers
- Desk 2:
  - Talking visitors
  - Request visitors
  - Merchant visitors
- Desk 3:
  - Hire visitors

### Visitor spawn loop
- The Shop starts a spawn cycle every **8 seconds**
- The game checks the first visitor entry in the daily list
- If the visitor can spawn:
  - the visitor is spawned
  - that entry is removed from the list
- If the visitor cannot spawn:
  - that entry is moved to the end of the list
  - after **1 second**, the next first entry is checked
- This continues until:
  - one visitor successfully spawns
  - or all current entries have been checked and none can spawn
- If no visitor can spawn during that cycle, the system waits until the next 8-second spawn cycle

### End of day rule
- Any visitor entries still left in the daily list when the Shop closes are lost for that day

---

## Desk capacity rules
Only desk caps are used.

- **Desk 1 cap = 4**
- **Desk 2 cap = 3**
- **Desk 3 cap = 2**

### Desk 1 spawn condition
A buying customer can only spawn if:
- at least one valid displayed good exists
- Desk 1 cap allows spawn

### Desk 2 spawn condition
A Desk 2 visitor can only spawn if:
- Desk 2 cap allows spawn

This applies to:
- Merchant
- Request
- Talking

### Desk 3 spawn condition
A hire visitor can only spawn if:
- Desk 3 cap allows spawn

---

## Shop level progression

### Level 1 Shop
- Buying customers = **6–9 per day**
- Hire visitors = **0–1 per day**
- Talking visitors = **1–3 per day**
- Request visitors = **0–2 per day**
- Merchant frequency = **every 3rd day**
- Display slots = **6**
- Uses base Shop appearance

### Level 2 Shop
- Buying customers = **8–11 per day**
- Hire visitors = **1 per day**
- Talking visitors = **1–3 per day**
- Request visitors = **0–2 per day**
- Merchant frequency = **every 3rd day**
- Display slots = **9**
- Shop appearance upgrades

### Level 3 Shop
- Buying customers = **10–12 per day**
- Hire visitors = **1–2 per day**
- Talking visitors = **1–3 per day**
- Request visitors = **0–2 per day**
- Merchant frequency = **every 2nd day**
- Display slots = **11**
- Shop appearance upgrades again

### Buying customer generation rule
- Daily buying customer count is determined by Shop level range
- That amount is then modified by Shop Appeal band
- Final buying customer count cannot go below **3**

### Merchant day rule
Merchant appearance is based on the current day number.

#### Shop Level 1–2
- Merchant can be added to the daily visitor list on days divisible by **3**
- Examples:
  - Day 3
  - Day 6
  - Day 9
  - Day 12

#### Shop Level 3
- Merchant can be added to the daily visitor list on days divisible by **2**
- Examples:
  - Day 2
  - Day 4
  - Day 6
  - Day 8

### Shop level content gating
Some content can also require a minimum Shop level.

#### Request content
Requests can include:
- minimum Shop level

#### Talking content
Talking dialogue entries can include:
- minimum Shop level

This means Shop upgrades can unlock new request and dialogue content without changing their daily amount.

---

## Shop Displays / Display Area

### Purpose
- Holds goods that are available for customer purchase
- Provides the item pool customers can buy from

### Core rules
- Displays hold goods based on slot count
- One slot holds one good
- Goods are placed manually by the player

### Display slots by Shop level
- Shop Level 1 = **6**
- Shop Level 2 = **9**
- Shop Level 3 = **11**

### Player interaction
1. Interact with display
2. Select slot
3. Select item
4. Item is placed into that slot

### Item flow
- Placing a good on a display takes it from Storage
- Removing a good from a display returns it to Storage
- If a customer selects a displayed item, that item becomes reserved
- If the sale succeeds, the item is sold
- If the sale is rejected, the item is returned to Storage

### Valid displayed good
A valid displayed good is:
- in a filled display slot
- currently available for sale
- not already reserved by another customer

---

## Browse Points

### Purpose
- Browse Points are fixed standing points in the Shop used by buying customers before choosing an item

### Rules
- The Shop contains a set of browse points
- Example initial amount: **6**
- Each browse point can only be occupied by one customer at a time
- Browse points are reserved before a customer starts moving toward them
- This prevents two customers from choosing the same browse point at the same time
- The browse point is freed after the customer finishes browsing and leaves that point

---

## Desk 1 — Customer Desk / Goods Desk

### Purpose
- Handles customers who want to buy displayed goods
- Resolves the selling interaction between customer and player
- Produces money income
- Supports simple negotiation outcomes
- Updates Shop Appeal through sale outcomes

### Customer flow
1. A buying customer spawns
2. The customer chooses an available browse point
3. The customer goes to that point
4. The customer browses for **5 seconds**
5. The customer chooses a **random valid displayed item**
6. If no valid displayed item exists at that point, the customer leaves
7. If a valid displayed item is found, the customer takes it and goes to Desk 1
8. If the desk is busy, the customer joins the queue while holding the selected item
9. A desk interaction starts when it is their turn
10. Player chooses whether to:
   - accept the sale
   - reject the sale
   - respond to a negotiation request if one occurs
11. Sale outcome affects money gain
12. Sale outcome updates Shop Appeal
13. Customer leaves after interaction is resolved

### Desk 1 cap meaning
- Desk 1 cap = maximum active buying customers in the full buying flow
- Maximum active buying customers = **4**

This includes customers who are:
- browsing
- moving to take an item
- holding an item
- waiting in queue
- currently being served

A new buying customer can only spawn when an active buying customer fully leaves the system.

### Rules
- Customer selects one random valid displayed item
- Each customer selects only one item
- Player can accept or reject the sale
- Some customers can negotiate
- Negotiation can be either:
  - asking for a discount
  - offering to pay more
- Final price = item base value modified first by Shop Appeal and then by customer behavior
- If the player rejects the sale, the item goes back to Storage

### Customer types for Act 1
- Normal Buyer
- Discount Customer
- Eager Buyer
- Browser / Leaves

### Suggested frequency
- 60% Normal Buyer
- 20% Discount Customer
- 10% Eager Buyer
- 10% Browser / Leaves

### Negotiation values
Possible value modifiers:
- 10%
- 20%
- 25%
- 30%
- 50%

These values can be used for either:
- discount requests
- higher payment offers

### Recommended modifier weighting
- 10% = common
- 20% = common
- 25% = medium
- 30% = uncommon
- 50% = rare

### Dialogue design for Act 1
Use simple item-name-based dialogue with small variation.

Examples:
- "I’d like to buy this [item name]."
- "Could you do 20% off for this [item name]?"
- "I’ve been looking for this [item name]. I’d pay 25% more."

---

## Desk 2 — Merchant / Request / Talking Desk

### Purpose
- Handles special desk visitors
- Supports buying, requests, and dialogue-based interactions
- Uses the daily generated visitor list

### Desk 2 visitor types
- Merchant
- Request
- Talking

---

### Merchant

#### Purpose
- A special visiting seller who offers goods to the player

#### Unlock rule
- Merchants are unlocked by finding them during night expeditions
- At the start of the game, only one merchant is available

#### Merchant inventory structure
- Each merchant has a distinct possible inventory pool
- Different merchants offer different item lists

#### Merchant stock per visit
Each merchant visit can offer:
- 1–4 utility item entries
- 1–2 misc item entries
- 4–10 material entries

Each entry has a quantity.

Example:
- 2x Shovel
- 3x Flashlight
- 1x Pickaxe
- 1x Lava Charm
- 2x Small Backpack
- 6x Iron
- 9x Wood

#### Merchant daily count
- On a merchant-eligible day, exactly **1 merchant** visitor is added to the daily visitor list

#### Interaction flow
1. Merchant arrives at Desk 2
2. Merchant says an opening line
3. Player chooses:
   - See wares
   - Reject
4. If player chooses See wares:
   - merchant inventory opens
   - player can inspect and buy items
5. Player can leave interaction with goodbye / close
6. Merchant leaves

#### Price rule
- Each offered item entry rolls its own price
- Price range is based on base value with a modifier from -15% to +15%

#### Buying rule
- Player can buy as many items as wanted as long as:
  - the merchant still has quantity left
  - the player has enough money

#### Item destination
- Bought items go to Storage

#### Wait rule
- Merchant visitors do not timeout
- They can wait indefinitely until the player interacts with them or closes the Shop

---

### Request

#### Purpose
- A special Desk 2 visitor that offers a request to the player
- Accepted requests are stored at the Guild
- Requests are one of the main ways to drive Guild progression and recruit progression

#### Player interaction
When a Request visitor arrives at Desk 2, the player can:
- Accept
- Reject

If accepted:
- the request is stored at the Guild if there is space

If rejected:
- the request is not stored
- the request remains available to appear again later if still eligible

#### Request categories
There are 2 request categories:
- Regular requests
- Mainline requests

Regular requests use normal rank progression.
Mainline requests use the special **Q** category.

#### Guild request storage
The Guild stores accepted requests.

Storage limits:
- 5 Regular requests
- 3 Mainline requests

There is no time limit on requests.
Requests remain stored until completed.

A stored request can also be manually removed / shredded.

#### Guild rank system
The Guild has its own rank progression.

Ranks:
- E
- D
- C
- B
- A
- S

Mainline requests use:
- Q

Guild rank increases through successful request completion.

A request can only be offered if its rank is allowed by the current Guild rank.

Examples:
- If Guild rank is D, then only D and E regular requests can be offered
- If Guild rank is C, then C, D, and E regular requests can be offered

Q requests are handled separately as mainline progression requests.

#### Request eligibility rules
A request can become eligible to appear if:
- the current day is high enough
- the Guild rank is high enough
- all prerequisite requests are completed
- the request is not already stored at the Guild
- the request is not already completed in a way that blocks it
- the minimum Shop level is met, if required

If a request is eligible but is not offered, it remains in the pool for future rolls.

#### Day dependency
Requests can require a minimum day before they can appear.

#### Request chaining
Requests can unlock follow-up requests.

Example:
- if Request 7 is completed
- then Request 7.2 can now become eligible

#### Regular request appearance
- Regular requests are rolled from the eligible regular request pool

#### Mainline request appearance
Mainline requests:
- appear when their conditions are met
- have **100% chance to appear**
- can only appear if there is space in the Mainline request storage at the Guild

#### Reward structure
Request rewards give:
- Money
- Guild XP
- Recruit XP

On successful completion:
- Money goes to the player economy
- Guild XP goes to Guild progression
- Recruit XP goes to the assigned recruit

#### Request data structure
Each request should contain:
- Request ID
- Title
- Category:
  - Regular
  - Mainline
- Rank
- Minimum day
- Minimum Shop level if needed
- Prerequisite request IDs
- Objective / completion condition
- Reward:
  - Money
  - Guild XP
  - Recruit XP
- Completion state

#### Wait rule
- Request visitors do not timeout
- They can wait indefinitely until the player interacts with them or closes the Shop

---

### Talking

#### Purpose
- A special Desk 2 visitor who starts a short dialogue interaction
- Gives the player dialogue choices with possible rewards or consequences
- Can provide Money, Info, Shop Appeal changes, or no result

#### Player interaction
When a Talking visitor arrives at Desk 2:
- the visitor says or asks something
- the player is given 2–4 response options
- each response has its own result

Possible response results:
- Money
- Info
- Shop Appeal up
- Shop Appeal down
- nothing

#### Info system
- Info gained from Talking visitors is stored at the Guild
- Info available from Talking visitors depends on:
  - current day
  - base level
  - guild level/rank

#### Talking dialogue availability
A Talking entry can appear if:
- the current day is high enough
- the required base level is met, if any
- the required guild level/rank is met, if any
- the minimum Shop level is met, if required

If a Talking entry is eligible, it can be rolled as one of the available Talking visitor interactions.

#### Talking dialogue structure
Each Talking entry should contain:
- Dialogue ID
- Minimum day
- Minimum base level if needed
- Minimum guild level/rank if needed
- Minimum Shop level if needed
- Visitor opening line
- 2–4 player response options
- A result for each response

Possible response results:
- give money
- give info
- increase Shop Appeal
- decrease Shop Appeal
- no result

#### Info reward structure
A dialogue response can:
- give a specific fixed info entry
- or give a random info entry from the currently eligible info pool

Eligible info depends on:
- day
- base level
- guild level/rank

Info gained is stored at the Guild.

#### Wait rule
- Talking visitors do not timeout
- They can wait indefinitely until the player interacts with them or closes the Shop

---

## Desk 3 — Hiring / Survivor Recruitment Desk

### Purpose
- Handles survivor hiring
- Lets the player inspect and accept new recruits
- Adds new recruits to the base if there is space in Recruit Quarters

### Visitor type
- Hire / Survivor

### Interaction flow
1. A survivor comes to Desk 3
2. The survivor says they are looking for shelter and are willing to work for it
3. The player can choose:
   - Accept
   - Reject
   - See stats
4. If the player checks stats, the survivor’s stats and trait are shown
5. If the player accepts and there is space, the survivor becomes a recruit
6. If there is no space, the player can choose to send an existing recruit away to free space
7. The recruit goes through recruit processing and is added to the base
8. If rejected, the survivor leaves

### Recruit stats
Recruits use the following stats:
- Health
- Strength
- Endurance
- Sight
- Stealth

### Base stat generation
- A level 0 recruit starts with **20 total stat points**
- These points are distributed randomly across the recruit stats

### Recruit level rule
- The level of hires that can appear depends on the highest recruit level the player currently has or has ever had
- Maximum hire level found = highest recruit level reached - 1
- Maximum hire level found is capped at **5**
- Maximum recruit level overall is **10**

Example:
- If the player has or has had a level 5 recruit, level 4 hires can now appear
- If the player has or has had a level 6 recruit, level 5 hires can now appear

### Recruit level roll weighting
- Once the current maximum available hire level is known, the final recruit level is rolled using weighted chances
- Higher available recruit levels should have better odds than lower ones

Example if the current maximum available hire level is 4:
- Level 0 = 10%
- Level 1 = 15%
- Level 2 = 20%
- Level 3 = 25%
- Level 4 = 40%

This makes higher recruit levels more likely once they are unlocked, while still allowing lower-level hires to appear.

### Traits
- Each recruit has traits
- Traits can be simple stat modifiers or special passive effects

Examples:
- Sneaky = +2 Stealth
- Persistent = can survive a fatal blow

### Act 1 trait rule
- Each recruit gets **1 trait**

### Recruit processing after acceptance
After a recruit is accepted:
- they go to the machine
- a class is assigned
- suit color is assigned
- a name is assigned
- a bed is assigned in Recruit Quarters
- they are injected
- a device is placed around their hand
- they go to their assigned bed

### Class rule
- Each recruit is assigned a class on acceptance
- Class affects recruit stats

### Name rule
- Each recruit receives a generated name
- The player can rename the recruit later

### Recruit Quarters rule
- Recruits can only be accepted if there is free space / a free bed in Recruit Quarters
- If there is no space, the player may send an existing recruit away to make room

### Wait rule
- Hire visitors do not timeout
- They can wait indefinitely until the player interacts with them or closes the Shop

---

## Daily report

### Purpose
- Summarizes the Shop day when the Shop closes

### Suggested report values
- Day
- Money made
- Money spent
- Requests accepted
- Info gained
- Hires accepted
- Shop Appeal change for the day
- Total customers served
- Total customers rejected

### Money made rule
Money made includes:
- money earned from sales
- money gained from Talking interactions

Money made does not include:
- request reward money

---

## Act 1 Goal
The Shop is defined well enough for Act 1 when:
- the Shop can open once per day
- the daily visitor list is generated and rotated correctly
- Desk 1, Desk 2, and Desk 3 all function
- Shop Appeal updates correctly
- Shop Appeal affects both buying customer amount and sale price
- buying customers browse, choose goods, and resolve sales correctly
- Merchant, Request, Talking, and Hire visitors all work
- Requests and Info can be stored at the Guild
- accepted recruits can be processed into the base
- the Shop can close correctly and show a daily report