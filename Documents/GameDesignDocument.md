# Don't Stop Me Now! #

## Team ##
- [Sara Merengo](https://github.com/SaraMerengo) (team leader, developer)
- [Emanuele Santoro](https://github.com/emanuelesantoro) (developer)
- [Andrea Sanguineti](https://github.com/AndreaNeti) (developer)
- [Alessandro Ricci](https://github.com/alessandro-ricci-16) (developer)
- [Mohammadjavad Sami](https://github.com/MJSami13) (art)

## Overview ##

Game genre: 2D platformer, puzzle

The game will contain a series of short levels organized by themed sections. Levels will alternate between more frenetic and fast-paced platformer sequences and calmer more puzzle-like levels.

Platforms: PC, possibly mobile

## Setting ##

The protagonist of the game is an ice cube, living in a bag of ice inside a supermarket frozen products aisle. One day, it breaks free of the rest of the bag of ice and decide to explore the freezer world: its goal will be to traverse all the freezer compartments in order to reach the end.

Possible motivations:
- it saw other ice cubes being taken away from the freezer and wants to find them
- ice cube life is boring, it wants to explore the world and reach the colorful and fantastic ice cream freezer compartment
- ice cube religion says there’s something at the end of the freezer

The levels will be laid out on a map representing the different freezer compartments, with every section having a different theme (e.g. frozen vegetables, ice creams, pizza,…) and new mechanics introduced. Each section will have a series of short levels.

## Game Mechanics ##

### Core gameplay ###

**Movement**

The ice cube will move constantly and the player will not be able to stop it or change its direction directly, only to jump, dash or ground pound. Hitting obstacles such as walls or platforms will result in the ice cube changing direction. It is possible to bounce off everything except the floor. Every time the ice cube reaches the floor its vertical velocity will be zero.

The ice cube is affected by gravity and can fall off platforms. Falling off platforms does not damage the ice cube.

The horizontal velocity is fixed and does not change, except for the dash and ground pound moves (see later).

Vertical velocity is determined by gravity, jump and ground pound input.

**Jump** 

The player can make the ice cube jump. The jump has variable height up to a fixed maximum height; the height of the jump depends on how long the player presses the "jump" button for. 

The player can jump only if the character is on the ground or on a wall (wall jump). Only one wall jump is allowed until the character comes back to the ground.

Both the normal jump and the wall jump have coyote time and jump buffer timers that can be set independently.

While the player is in the air, the following actions are allowed:
1. Wall jump
2. Mid-air dash
3. Ground pound

**Ground pound**

While mid-air, the player can press the "ground pound" input to make the character quickly fall down to the ground. Horizontal velocity is temporarily set to zero as the character begins falling down, then resumes in the previous direction once the player hits the ground. If the player presses the input "ground pound", all other input is suspended until the character reaches the ground. Horizontal position is frozen until the character reaches the ground, the character moves perfectly vertically during ground pound.

**Mid-air dash**

While the character is in the air, the player can perform a dash. Vertical velocity is set to 0 as well as gravity and the character receives an impulse in the current direction (the player cannot choose in which direction to dash). The characters moves horizontally at a fixed and constant speed (determined by the initial impulse) for a fixed amount of time which does not depend on player input. At the end of the dash, gravity begins to function as normal again and the default speed is restored.

The player is not allowed to dash while the ground pound action is executing. The player is not allowed to dash while touching the ground.

**Recap**

The following commands are allowed:
1. jump
2. ground pound
3. accelerate
4. slow down
5. mid-air dash

There are two input mappings.
1. First input mapping:
    - C: jump
    - X: ground pound
    - Z: dash
2. Second input mapping:
    - space bar or left mouse click: jump
    - : ground pound
    - : dash

Both input mappings are active at any moment since they do not interfere with each other.

The following diagram represents the states finite state machine. (x input is the horizontal input)

![image](Diagrams/IceCubeInputFSA.jpg)

### World ###

The world around the ice cube represents the inside of a freezer. All elements and collisions are perfectly horizontal or vertical with only 90 degrees angles. There are no slopes in the world.

### Gameplay elements ###

The world elements that are part of the fridge or of the background are placed on a grid.

Grid elements (all of these elements are static):
1. Regular platforms: these are platforms the ice cube can slide on and bounce off with no consequence. They represent patches of ice, parts of the plastic shelves or floor in the freezer, other contents of the freezer such as boxes of food,...
2. Spikes: these represent ice shards in the freezer or other static obstacles which will be letal to the ice cube. The ice cube will die if it collides directly with them. They can be attached to other regular platforms in all four directions.
3. Heated platforms: some sections of the level will represent the engine of the freezer and will be heated. The ice cube can slide on them but only for a maximum allowed period of time: when the ice cube enters a heated platform, a health bar will appear next to it and start decreasing to signify the amount of time the ice cube can stay on the platform before melting and dying. If the ice cube temporarily steps away from the platform, the health bar will start filling up again. When the health bar is completely full it will disappear.

All of these elements can be placed on the floor of the level but also mid-air with nothing supporting them (there is no regard to gravity in this sense).

Stand-alone elements:
1. Rolling / moving obstacles, such as rolling bottles / ice cream tubs, moving ice cubes,...: the ice cube will die if it collides directly with them. They move following gravity rules and will fall to the ground. Some obstacles may have a specific trigger so they start moving only when the character collides with the trigger.
2. Fans: fans will produce wind, which will affect the speed and direction of the ice cube by applying a force to the ice cube. Fans should only be placed so that the force is vertical, so they will not affect the horizontal speed of the cube and will not change its direction.
3. Breakable platforms: platforms that break if the ice cube dashes or ground pounds on them
4. Enemies: some levels will contain enemies in the form of other ice cubes. Enemies will be able to shoot ice shards or fire balls at the character, and additionally cause the player to die if it collides directly with them. (to be discussed)
5. Projectiles: some levels will contain projectiles, spawned either by enemies or other static parts of the environment. The projectiles may either have a straight trajectory or follow the laws of gravity.
6. Movable objects: these objects do not perform any action. The player can move them around using the cube, for example to place them on spikes to be able to pass them.

To be discussed:
1. Fans: should the force be the same in the entire force field or be modulated depending on how close you get to the fan?
2. Heated platforms: alternative ways of showing the ice cubes is about to die, such as gradually changing the color of the ice cube to red

### Death ###

The ice cube shatters and dies if:
1. It falls off a bottomless pit
2. It collides directly with non-allowed elements: spikes, obstacles, enemies, projectiles,...
3. It stays in contact with a heated platform more than the allowed time.

After death the ice cube restarts at the nearest check-point.

### Game Flow and level structure ###

The game is made up of multiple short levels, which are organised in sections. Each section represents a freezer compartment and has a different theme: vegetables, ice creams, pizza,...

The first few levels only have the basic jump mechanic and normal blocks; new level and sections will gradually introduce more mechanics and combine them. 

To be discussed: order of introduction of additional elements

Levels must be completed sequentially (it is necessary to complete a level to be able to access the following ones). The player can select which level to play from a level map and can select to replay a previously completed level.

The camera is not static and moves around following the player. Levels are mainly side-scrolling to the right but they can also have vertical sequences (both going upwards and downwards) and side scrolling in the opposite direction to give the player the sense of moving in a coherent map. 

**Checkpoints**

Each level is contained in a single scene and can contain up to 3/4 checkpoints. 
- When the player dies, the scene will reload and the character will be spawned at the position of the last completed checkpoint. Previously modified aspects of the level (broken platforms, moved objects,...) will reset with the death of the character. 
- Checkpoint completion is automatic when the character collides with the corresponding trigger and does not need any action on the player's part.
- Completing a checkpoint does not change anything about the world and does not prevent the player from going back to previous sections of the level. 
- Checkpoints are not saved between different runs of the game, if the player closes the game the checkpoint progress is lost.

**Bonus levels**

Each world will contain a bonus level at the end. It will not be necessary to complete the bonus level to progress in the game, it is just an additional and optional challenge. 

Bonus levels contain a certain number of collectibles on the main path. The goal of the bonus level is to progress as far as possible and eventually collect all of them. Partial progress will be saved between runs (for example the level selection screen will show that the player has collected 5 out of 10 collectibles). The bonus levels contain no checkpoints. 

Bonus levels are meant to be very fast and very challenging, with mainly horizontal scrolling to the right. The character may have different parameters than the normal levels, for example higher speed, higher jumps, faster dash and ground pound. This is to make the level more challenging and force the player to react fast.

The aesthetic of the bonus levels will be completely different from the rest of the freezer, with only colored neon outlines and silhouettes. In the logic of the game, this could represent a black-out in the freezer where the light goes completely dark. 

## Deadlines ##

Official deadlines:
- 6th November 2023 - Game Design Document
- 10th December 2023 - Prototype Submission
- 13th January 2024 - Beta Submission
- 27th February 2024- Final Project Submission

### Deadline 1 - 2nd November 2023 ###
- Finalise the player controller (Sara Merengo, Emanuele Santoro)
    - revise the controller and make the script more modular
    - add dashing
    - add wall jumps
    - add ground pound
    - adapt the speed updates for fans (forces instead of velocity updates)
- Implement spikes and moving obstacles (Alessandro Ricci)
- Implement heated platforms (Emanuele Santoro)
- GameManager class (death and respawn) (Andrea Sanguineti)
- Camera boundaries (Alessandro Ricci)

### Deadline 2 - 14th November 2023 ###
- Controller:
    - correct jump height calculation and ground pound (Sara Merengo)
    - only one wall jump until the character hits the ground again (Sara Merengo)
    - redo player input: input map instead of hard coded in the controller (Emanuele Santoro)
- Implement basic menu and level selection layout, UIManager (Sara Merengo)
- Create a basic level prototype containing only basic blocks (everyone). Experiment with:
    - ice cube movement parameters
    - ice cube size with respect to the platform tiles
    - screen size, camera zoom
    - horizontal scrolling vs one screen per checkpoint
- Breakable platforms (Andrea Sanguineti)
- Fans
- Basic character asset and tileset (Mohammadjavad Sami)

### Deadline 3 - 30th November 2023 ###

- Implement checkpoints (Sara Merengo)
- Implement moving and falling obstacles, with and without trigger (Alessandro Ricci)
- Design a small tutorial-like level (Alessandro Ricci)
- Design a level containing only the jump action and no other mechanic or world elements except for limited use of spikes / death zones (Sara Merengo, Alessandro Ricci, Emanuele Santoro, Andrea Sanguineti)
- Design a level containing only the jump and one additional action of choice (Sara Merengo, Alessandro Ricci, Emanuele Santoro, Andrea Sanguineti)
- Implement a way to disable actions for early levels (Emanuele Santoro)
- Assets for spikes and heated platforms (Mohammadjavad Sami)

## Implementation details ##

This section is meant for the programmers to keep track of implementation details.

### Ice cube ##

**Ice Cube GameObject**

The ice cube GameObject should have the following components:
1. IceCubeInput script, with the correct IceCubeParameters as parameter
2. Rigidbody2D set to dynamic, having a PhysicsMaterial2D with 0 friction; collision detection should be set to continuous to avoid problems with the collisions at high speeds
3. BoxCollider2D with no rounding at the edges (this is needed for precise collision detection)
4. IceCubeAnimatorManager script
5. Animator
6. SpriteRenderer
7. IceCubeStateManager script, with the same IceCubeParameters as the IceCubeInput script (TODO: find a fix for this)
8. "Player" tag (this is necessary for death zones to work)

**Ice cube parameters**
- Ice cube scale: 1.5
- Default speed: 17
- Jump height, wall jump height: 6
- Upward gravity scale: 20
- Downward gravity scale: 30
- Groundpound speed: 50
- Dash speed: 50
- Dash duration: 0.1
- Max Jump Coyote Time and Buffer Time: 0.05
- Max Wall Jump Coyote Time and Buffer Time: 0.1

### Tilemaps ###

A tilemap must have the following components:
1. TilemapCollider2D with flag used by composite set to true
2. Rigidbody2D set to static
3. CompositeCollider2D (this is to make sure the tilemap has a single collider with no lines for every single tile, which makes collision handling better)