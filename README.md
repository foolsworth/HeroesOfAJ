# HeroesOfAJ
A challenge provided by Monadical

Hello And thanks for reviewing my entry for the monadical programming challenge

My project contains the following:
- A basic attribute tracking system.
- A basic hero creation system with customizable stats.
- An inventory system for creation, spawning and equipping stat altering equipment.

Instructions:
- When you run the project, your hero is the grey sphere.
- Collectable objects are present int the scene as orbs with an icon.
- if the orbs touch the hero (by moving their position in editor) the items are added to the inventory on the right.
- Items can be dragged from the inventory into the right or left equipment slot at the bottom right or to any empty inventory slot.
- Items can be discarded by dragging them to the slot with the trash symbol.
- The number of slots in the inventory are customizable in the inventory prefab.
- Once an item is equiped the player stats are updated and the weapon appears in the corresponding hand.
- Detailed stats can be seen if the stat bar in the bottom left of the screen is hovered over.
- Detailed stats will show the character stats as well as the stats modified by the currently equiped items.

To create a character with stats:
- A prefab copy of the Hero prefab should be made.
- In the root object it has a stats container component. 
- Stats can be browsed and added in the inspector. 
- Custom attributes can also be added through the field at the very bottom.
- All attributes are stored in a scriptable object.

To create an item:
- Access the scriptable object Assets/Data/ItemCompendium.
- Fill the form and press add item to add a new item to the library.
- You can set the item name, description, UI icon, mesh and stats here. 
- It uses the same stat creation system as the stats container on the hero.
