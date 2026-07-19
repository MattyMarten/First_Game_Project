# Room Storage

## 1. Room Purpose

Storage is the central physical storage room of the base.

Its main purpose is to:
- hold important stored item/resource counts
- represent stored state physically in the world
- make the base feel readable without relying on heavy menu UI
- act as the source room that other systems pull from

Storage is not designed as a major active gameplay station.
It is mainly a visual and systemic room.

The player does not come here to perform complex actions.
The player mainly uses Storage to:
- see what the base currently has
- understand resource state physically
- read exact quantities through notes/signs
- understand what is available for other systems to use

Storage supports the game’s broader design direction of:
- physical readability
- minimal large-menu UI
- strong room identity
- system clarity through world representation

---

## 2. Room Identity

Storage is a support room, not a production room.

It does not:
- craft items
- process expedition returns directly
- assign recruit equipment directly
- sell goods directly
- power the Core directly
- manage quests or information

Instead, Storage functions as:
- the physical holder of important stored categories
- the room that visually reflects current stored state
- the shared inventory source for multiple other systems

Storage should feel like:
- a clear resource room
- a readable reserve of what the base owns
- a stable room the player can understand at a glance

---

## 3. What Storage Owns

Storage owns the stored counts and world representation for important item/resource categories.

Current owned stored categories include:
- cobalt coins
- materials
- utility items not assigned
- accessories not assigned
- belts not assigned
- cases not assigned
- goods not on display
- shop decor not currently placed

Storage is the main owner of these categories while they are not currently active elsewhere.

This means Storage is the default resting place for these resources/items when they are:
- not assigned
- not placed
- not on display
- not currently consumed by another system

---

## 4. What Storage Does Not Own

Storage does not own things that are currently active in another system or belong to another room/system state.

Examples:
- goods currently placed on shop displays
- decor currently placed in the shop
- utility items assigned to recruits
- accessories assigned to recruits
- belts assigned to recruits
- active recruit loadouts
- active workshop processes
- quest data
- information/documents
- Core loaded coin amount
- day/time state
- power state

Storage also does not directly unpack expedition materials from returned processing representations.
That part is handled through the Grinder / expedition return support flow.

---

## 5. Physical Representation Philosophy

Storage exists mainly to show the player what the base has in a physical way.

The room should visually represent all major stored categories, but not all categories use the same representation style.

### Broad stack/resource categories
These can use amount-based world visuals:
- cobalt coins
- materials

These categories can scale visually as amounts increase.
Example:
- small amount -> small visible amount
- larger amount -> larger visible amount

Visual growth can stop at a practical cap, but the stored count remains exact.

### Stored item categories
These are shown physically as stored items in the room:
- utility items not assigned
- accessories not assigned
- belts not assigned
- cases not assigned

These should appear as physical stored objects or object groups.

### Goods representation
Goods not on display should also be represented physically in Storage.

However:
- Storage should not try to place every copy of every good physically
- instead, one visible representative of each stored good type is enough

A physical note/readout should show the summed total goods count.

### Shop decor representation
Shop decor not currently placed should also be represented physically in Storage.

Shop decor is split into:
- Ground decor
- Wall decor

As with goods:
- one visible representative of each stored decor type is enough
- a physical note/readout should show the summed total decor count

This allows the room to stay physically readable without becoming overloaded with duplicated objects.

### Visual amount thresholds

Storage visuals do not need to show the exact true quantity literally.
Instead, many stored categories should use visual amount thresholds while the note/readout still shows the exact count.

#### Normal item threshold values
For normal stored item categories, use these visual amount steps:
- 1
- 5
- 10
- 20

Examples:
- if stored scrap = 1, show 1 scrap visually
- if stored scrap = 6, show 5 scrap visually and 6 on the note
- if stored scrap = 17, show 10 scrap visually and 17 on the note
- if stored scrap = 25, show 20 scrap visually and 25 on the note

#### Cobalt coin threshold values
Cobalt coins use their own larger visual amount steps:
- 1
- 25
- 50
- 100
- 250
- 500
- 1000

Examples:
- if stored coins = 12, show 1 coin-level visual and 12 on the note
- if stored coins = 44, show 25 coin-level visual and 44 on the note
- if stored coins = 270, show 250 coin-level visual and 270 on the note

#### Threshold rule
Use the highest display threshold that does not exceed the true stored amount.

This threshold logic is mainly for:
- cobalt coins
- materials
- utility items
- accessories
- belts
- cases

Goods and decor should continue using the one-visible-representative-per-type approach instead of threshold-count stacking.

---

## 6. Exact Count Display Rules

Storage should show exact counts, not rough estimates.

### Exact count rule
- each stored item/resource category should show an exact number
- broad stack/resource categories still need exact numeric readouts even if the world visual only scales by thresholds

### Goods and shop decor exception
Goods and shop decor do not need a separate exact number per visible object in Storage.

Instead:
- goods use a summed total count readout
- shop decor use a summed total count readout

### Tiered gear note rule
Tiered gear items should keep the same physical visual representation regardless of tier.

For utility items, belts, cases, and accessories:
- the physical Storage visual should represent the total stored item type normally
- tiers do **not** change the physical visual representation
- the attached note/readout should list the tier breakdown for tiers that currently exist

Example:
- Shovel T1 x2
- Shovel T2 x1
- Shovel T3 x2

Important rule:
- do not show zero-count tier lines
- do not show lines like Shovel T2 x0

This keeps the room physically simple while preserving the important gameplay readability of gear tiers.

The room should therefore combine:
- physical representation
- exact count signage/readouts
- exact tier note breakdown where relevant

---

## 7. In-Use Visibility Rule

If a stored item is currently being used elsewhere, it should no longer appear as available in Storage.

Examples:
- if a good is placed on a shop display, it no longer appears in Storage as stored
- if shop decor is placed in the shop, it no longer appears in Storage as stored
- if a utility item is assigned to a recruit, it no longer appears in Storage as stored
- if an accessory or belt is assigned to a recruit, it no longer appears in Storage as stored

Storage should reflect the currently available stored state, not the total existence of all objects regardless of use.

---

## 8. Expedition Return Relationship

Storage has an important relationship to expedition return, but it does not directly own the first visible return object near the Grinder in a literal way.

### Real equipment return
When an expedition ends:
- the recruit’s real case and gear state return as recruit equipment/state
- assigned gear remains part of recruit/loadout state as appropriate

### Processing representation
The case object that appears on Floor 1 near the Grinder is a processing representation / imitation.
It is not the literal persistent case item being stored physically there.

This processing object exists to:
- show that expedition materials have come back
- give the player something physical to process at the Grinder
- convert expedition-returned material contents into Storage-owned materials

### After grinding/processing
When the player processes the imitation case:
- the contained materials are added into Storage
- the real assigned case ownership does not become confused with the processing object

This distinction is important so the game does not treat one case as if it is both:
- recruit-assigned persistent equipment
- and a destructible/processable floor object

---

## 9. Relationship With Workshop

Workshop uses Storage as one of its main source/sink rooms.

### Storage -> Workshop
Workshop takes required materials/items from Storage when needed.

This is effectively a stored-count reduction.
Examples:
- consume materials from Storage
- consume stored utility parts from Storage if a recipe needs them
- consume source-tier gear items from Storage during gear upgrading

### Workshop -> Storage
When Workshop creates a storable result, that result is added back into Storage.

This is effectively a stored-count increase.
Examples:
- crafted goods added to Storage
- crafted utility items added to Storage
- upgraded gear added to Storage at the new tier
- processed materials added to Storage

Storage therefore acts as the main backing state for Workshop resource flow.

---

## 10. Relationship With Shop

Shop also uses Storage as a source room.

### Storage -> Shop
When the player places goods onto shop displays:
- those goods come from Storage
- the stored count is reduced accordingly
- the placed display item is no longer considered stored

### Shop decor placement
When the player places shop decor in the shop:
- the shop decor comes from Storage
- the stored count/state is reduced accordingly
- the placed shop decor is no longer shown as available in Storage

### Selection method
Shop display and shop decor placement should support the game’s physical-first interaction direction.

For goods:
- interacting with a display locks the camera to a placement view
- the player scrolls through available stored goods
- a small note/readout shows useful selection info such as name and value

For shop decor:
- shop decor is split into Ground and Wall categories
- interacting with a shop decor placement point should allow the player to choose from valid shop decor in that category
- shop decor should also have physical note/readout support explaining what the shop decor gives/does

This allows Storage to remain physically meaningful while Shop handles final placement interaction.

---

## 11. Relationship With Core

Storage and Core are connected through cobalt coin flow.

### Stored coin ownership
Storage holds the true stored cobalt coin amount as physical currency.

### Core loaded state
Core has its own separate loaded coin state.
Example:
- 50 / 150 coins loaded into the Core

### Deposit interaction
At the Core deposit point:
- the player deposits coins into the Core
- deposited coins are removed from Storage
- the Core loaded amount increases

This means:
- Storage owns the reserve
- Core owns the loaded/active machine amount

This distinction should stay clear in implementation and future docs.

---

## 12. Capacity Rules

Storage has unlimited real storage capacity.

There is no intended hard storage cap for the room itself.

However:
- physical visuals can stop expanding after a practical point
- object count/readability limits are allowed for presentation
- exact stored values must still remain correct even when visuals stop changing

This means Storage has:
- infinite logical capacity
- finite practical visual representation

---

## 13. Power Behavior

Storage does not require power to function.

This means:
- stored ownership and stored counts continue to exist normally
- Storage remains the backing inventory source for other systems
- Storage visuals and notes do not depend on powered machine state

Storage is therefore not shut down by Core power loss in the same way as powered machines.

---

## 14. Player Interaction Model

Storage is mainly a non-active room.

The player does not need a large set of direct Storage interactions.

Its main interaction role is:
- visual reading
- checking what exists
- understanding quantities
- supporting the feeling that the base really stores things physically

Most actual usage of stored items happens indirectly through other rooms:
- Workshop pulls from Storage
- Shop pulls from Storage
- Core deposit removes coins from Storage
- recruit assignment removes equipment from Storage

So while Storage is important systemically, it is not intended to be a deep hands-on gameplay station.

---

## 15. HUD / Readability Rules

Storage should not require a dedicated extra HUD.

The room itself should act as the readable interface through:
- physical objects
- piles/stacks
- shelf/group representation
- exact notes/readouts/signage

This is part of the room’s purpose.
The player should be able to understand Storage mainly by looking at it.

If later small helper UI is needed, it should remain minimal.

---

## 16. Upgrades

Storage currently has no planned upgrade path.

It is intended to remain a stable support room rather than a progression-heavy room.

That means:
- no planned storage levels
- no planned storage unlock chain
- no planned storage capacity upgrades

Future polish could improve visuals, but that is not the same as a gameplay upgrade system.

---

## 17. First-Pass Implementation Direction

The first implementation pass should stay simple.

Recommended first-pass rule:
- use one representative physical object/group per supported stored type or category
- pair it with an exact number note/readout
- for tiered gear, include tier breakdown lines only for tiers that currently exist
- improve visual richness later if needed

This means the first version does not need:
- perfect shelf logic
- large decorative storage layouts
- complex per-copy placement
- advanced clutter simulation
- expandable or alternate Storage screens

What matters first is:
- correct stored ownership
- correct count changes
- correct visibility/in-use rules
- readable physical representation
- working flow with Shop, Workshop, Core, and expedition processing

---

## 18. Done Condition

Storage is good enough for implementation when:

- the room correctly owns the intended stored categories
- broad resources are physically represented
- goods and shop decor are physically represented by type
- exact counts are shown correctly
- tiered gear notes show only existing tier lines
- in-use items disappear from stored availability
- Workshop can pull from and return items to Storage correctly
- Shop can pull goods/shop decor from Storage correctly
- Core deposit correctly removes coins from Storage and adds them to Core
- expedition processing correctly adds returned materials into Storage
- Storage remains functional during power loss
- the room is readable without depending on a large menu UI

At that point, Storage is functionally complete enough even if later visual polish is still missing.