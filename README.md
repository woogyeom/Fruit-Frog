# Unity 2D Boss Battle Game

A Unity-based 2D arcade game featuring dynamic boss battles, unique enemy types, and interactive player mechanics. This project is an extension of a previous arcade shooting game concept, developed with enhanced enemy AI, player abilities, and rich gameplay mechanics to provide a challenging gaming experience.

![309144506-85a84219-a23f-4b67-8419-05583e122552](https://github.com/user-attachments/assets/fb5a9f62-7eae-4fdc-9a78-9dec82104fa6)

## Project Overview
This Unity 2D game includes boss fights, various enemy behaviors, bullet dynamics, item pickups, and level progression systems. Players navigate through waves of enemies and ultimately face off against a challenging boss. The game incorporates sophisticated enemy AI behaviors, advanced projectile mechanics, special abilities, and a dynamic HUD to keep players engaged.

## Key Features

### 1. **Boss Battle Mechanics**
   - **Charging Attack and Dissolve Effects**:
     - The boss (`Boss.cs`) utilizes a **charging attack** that requires the player to strategize positioning and timing. Charging is activated based on proximity and cooldowns, creating a dynamic challenge.
     - Upon defeat, the boss initiates a **dissolve effect** using shader material properties to visually represent fading, adding polish to the death animation.
   
### 2. **Enemy AI with Diverse Behaviors**
   - **Melee and Ranged Attacks** (`Enemy.cs`):
     - Enemies include **melee attackers** that close in directly on the player, and **ranged enemies** that maintain distance and shoot projectiles. This variety in enemy types requires different strategies from the player.
   - **Ghost-Type Enemies** (`Ghost.cs`):
     - Implemented a **disappearing ghost enemy**, which temporarily becomes invincible and invisible, forcing the player to adapt.

### 3. **Projectile System**
   - **Player and Enemy Bullets** (`Bullet.cs`, `EnemyBullet.cs`):
     - Player bullets have **boomerang effects** for specific attack types, adding an additional layer of complexity in movement and aiming.
     - Enemy bullets damage the player and trigger animations and health deductions, emphasizing the importance of dodging.

### 4. **Item and Weapon System** (`Item.cs`, `Weapon.cs`)
   - **Item Upgrades and Special Effects**:
     - Players can acquire items (`Item.cs`) that grant new weapons or special abilities, such as increasing bullet damage, increasing attack count, or boosting health.
   - **Weapon Types and Leveling**:
     - Weapons (`Weapon.cs`) include **melee and ranged** types, each with unique attack mechanics. For example, ranged weapons can shoot projectiles in various patterns, such as single shots, shotgun bursts, or even continuous fire.

### 5. **Special Abilities and Level-Up System** (`Special.cs`, `LevelUp.cs`)
   - **Special Power-Ups**:
     - Implemented **special abilities** such as increased magnet range (collecting experience orbs from afar) and health boosts (`Special.cs`).
   - **Dynamic Level-Up System**:
     - Upon leveling up, players are presented with a random selection of items to choose from (`LevelUp.cs`). This system uses weighted random selection to determine which items appear, ensuring strategic progression.

### 6. **Boss Warning System** (`Danger.cs`)
   - **Boss Alert Display**:
     - When a boss appears, a **warning message** is displayed on the screen, alerting players of the incoming challenge and giving them time to prepare.

### 7. **Dynamic HUD System** (`HUD.cs`)
   - Implemented a **real-time HUD** that shows player stats such as health, experience, level, and score. This helps players track their progress and strategize accordingly.

### 8. **Object Pooling for Performance Optimization** (`PoolManager.cs`)
   - Enemies, bullets, and other objects are managed using a **pooling system** to ensure smooth gameplay performance, reducing runtime instantiation overhead.

### 9. **Map Navigation and Spawner System** (`MapController.cs`, `Spawner.cs`)
   - **Endless Map Expansion**:
     - The **map controller** (`MapController.cs`) adjusts the map elements as the player moves, creating an illusion of an infinite map for continuous exploration.
   - **Adaptive Spawning**:
     - Enemies are spawned dynamically based on the player's current level and game time (`Spawner.cs`). Boss spawns are triggered once certain milestones are reached, with a warning indicator appearing beforehand to alert players.

### 10. **Player Control and Abilities** (`Player.cs`)
   - **Player Movement and Dissolve Effects**:
     - The player can move freely around the map using input vectors, and dissolving effects are triggered when health depletes to zero, indicating death in a visually impactful way (`Player.cs`).
   - **Attack Targeting with Scanner**:
     - The player utilizes a **scanner system** (`Scanner.cs`) to identify and target the nearest or most powerful enemy, aiding in automated targeting for ranged weapons.

## How to Play
- **Movement**: Use `W`, `A`, `S`, `D` keys to navigate the player character.
- **Attack**: The player automatically shoots bullets towards enemies.
- **Objective**: Defeat all enemies in each wave, collect experience points, and ultimately face the boss to win the level. Level up by selecting new abilities and items to become stronger.

## Technologies Used
- **Unity**: Game engine used for development.
- **C# Scripting**: All game logic and behaviors are implemented in C#.
- **Shader Effects**: Used for dissolving and fading effects for enemy and boss death animations.
- **Scriptable Objects**: Used to manage item data (`ItemData.cs`) and easily extend game content.

## Itch.io Link  
https://woogyeom.itch.io/fruit-frog09
