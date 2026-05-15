# The Tavern's Cleaner

**The Tavern's Cleaner** is a 3D physics-based arcade game developed in Unity. You play as a magically-gifted tavern busboy tasked with cleaning up after a busy night. Using telekinetic magic, you must grab volatile, physics-enabled items (like wine bottles and plates) and transport them across a room filled with wandering patrons. 

The game emphasizes momentum, physics manipulation, and careful navigation to avoid breaking items and losing hard-earned tips!

---

## How to Play
You have exactly **1 minute** to clear the tables and make as much money as possible.

* **Earning Money:** Successfully grab an item and carry it to the designated cleaning zone counter in the backroom.
* **Penalties:** If you move too erratically, dash into objects, or collide with a patron, your magical tether will break! The item will shatter on the floor, and money will be deducted from your total earnings.
* **Victory:** End the shift with a positive balance (Earnings > $0).
* **Defeat:** End the shift in debt (Earnings ≤ $0).

### Controls
* **W, A, S, D** - Move
* **Mouse** - Look around
* **E** - Grab / Hold an item (Initiates the Magical Tether)
* **Q** - Drop the item
* **ESC** - Pause Game

---

## Core Mechanics & Features

* **Magical Tether Physics:** Items are picked up using a `SpringJoint` that connects the item to the player. A custom Bezier-curve `LineRenderer` provides a visual "magical tether" between the player's hand and the object's physical center.
* **Dynamic Obstacles:** The tavern is populated by NPCs using Unity's `NavMeshAgent` system. They wander randomly around the tables, forcing you to constantly adapt your route.
* **Custom Camera System:** A 3rd-person over-the-shoulder camera utilizing `Cinemachine` orbits the player, locked on the Y-axis to ensure a clean, professional view of the action.

---

## Screenshots

<img width="935" height="523" alt="Main_Menu_Picture" src="https://github.com/user-attachments/assets/269a2bab-baf0-47ff-90a3-54452e0c23f9" />

<img width="950" height="530" alt="Magic_Tether_Picture" src="https://github.com/user-attachments/assets/827eb9ef-738f-462a-b44d-43f5759203fe" />

<img width="946" height="500" alt="Patrons_Moving_Picture" src="https://github.com/user-attachments/assets/396d9190-39dc-43d6-9c09-7e62647cc811" />

<img width="937" height="530" alt="GameOver_Picture" src="https://github.com/user-attachments/assets/f3443965-291e-40da-b1d9-1d03281a112b" />

<img width="946" height="527" alt="Money_Area_Picture" src="https://github.com/user-attachments/assets/4bd96f86-f707-44c8-bc29-defbb3b2ed3c" />

<img width="926" height="515" alt="Help_Picture" src="https://github.com/user-attachments/assets/11b0a404-9b52-407b-88d6-858a4d06221b" />

---

## Download & Installation
1. Go to the **[Releases](../../releases)** tab on the right side of this repository.
2. Download the latest `TavernGame_Windows.zip` file.
3. Extract the ZIP file to your computer.
4. Run the `.exe` file to start your shift!

---

## Assets Used & Licenses
This project utilized the following pre-made, royalty-free assets to achieve its stylized, low-poly medieval fantasy theme.

**Visual Assets:**
* **Environment & Props:** [Small Cozy Medieval Tavern Interior Asset Pack](https://n0stardust.itch.io/medieval-tavern) by n0stardust (CC0)
* **Character Models:** [Ultimate Modular Women & Men Pack](https://quaternius.com/packs/ultimatemodularwomen.html) by Quaternius (CC0)
* **User Interface:** [Fantasy Medieval UI Pack](https://ps48.itch.io/fantasy-medieval-ui-pack-buttons-inventory-icons) by PS48 (Free for Commercial/Personal Use)

**Audio Assets:**
* **Background Music:** [Fantasy Tavern Music Pack](https://chrisgruchacz.itch.io/fantasy-tavern-music-pack) by Chris Gruchacz Composer (Royalty-Free)
* **Sound Effects (Via Pixabay Content License):** * Glass Breaking by DRAGON-STUDIO
  * Cash Register (Kaching) by Modestas123123
  * Magic Button Click by humordome

---
*Created by Nerike Bosch - May 2026*
