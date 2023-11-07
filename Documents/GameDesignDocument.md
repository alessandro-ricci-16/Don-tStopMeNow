# Don't Stop Me Now! #

## Team ##
- [Sara Merengo](https://github.com/SaraMerengo) (team leader, developer)
- [Emanuele Santoro](https://github.com/emanuelesantoro) (developer)
- [Andrea Sanguineti](https://github.com/AndreaNeti) (developer)
- [Alessandro Ricci](https://github.com/alessandro-ricci-16) (developer)
- Mohammadjavad Sami (art)

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

The ice cube will move constantly and the player will not be able to stop it or change the direction directly, only to jump. Hitting obstacles such as walls or platforms will result in the ice cube changing direction. It is possible to bounce off everything except the floor. Everytime the ice cube reaches the floor its vertical velocity will be zero.

The ice cube is affected by gravity and can fall off platforms. Falling off platforms does not damage the ice cube.

The horizontal speed can be affected by:
1. Interactions with the environment: sliding on a frosty platform will make the cube slower, while sliding on a slippery platform will make the cube faster.
2. User input: the player can temporarily slow down or accelerate the character within an allowed range of speed.

While the player keeps the "accelerate" ("decelerate") input pressed, the ice cube will accelerate (decelerate) at a constant rate until it reaches a fixed "fast (slow) speed" value. When the "accelerate" ("decelerate") input is released, the ice cube will decelerate (accelerate) until it reaches the default speed again. 

In general, the ice cube is always trying to reach or keep a certain fixed horizontal speed which depends on the "accelerate"/"decelerate" input (or absence thereof). The only exception is when using the "ground pound" and "mid-air dash" moves (see later).

Vertical speed is determined by gravity, jump and ground pound input.

**Ground pound**

While mid-air, the player can press the "ground pound" input to make the character quickly fall down to the ground. Horizontal velocity is temporarily set to zero as the character begins falling down, then resumes in the previous direction. If the player presses the input "ground pound", all other input is suspended until the character reaches the ground.

**Jump** 

The player can make the ice cube jump. The jump has variable height up to a fixed maximum height; the height of the jump depends on how long the player presses the "jump" button for. 

The player can jump only if the player is on the ground or on a wall (wall jump). Only one wall jump is allowed until the character comes back to the ground.

While the player is in the air, the following actions are allowed:
1. Wall jump
2. Mid-air dash: to be discussed
3. Ground pound

**Mid-air dash**

To be further discussed.

**Recap**

The following commands are allowed:
1. jump
2. ground pound
3. accelerate
4. slow down
5. mid-air dash

**Input mapping**

- W: jump
- A: left; accelerate when the current direction of the character is left, decelerate if the current direction is right
- D: right; accelerate when the current direction of the character is right, decelerate if the current direction is left
- S: ground pound
- Q: mid-air dash

The following diagram represents the states finite state machine. (x input is the horizontal input)

![image](Diagrams/IceCubeInputFSA.jpg)

**Other decisions**

- The ice cube will not rotate
- There are no slopes in the world

### Gameplay elements ###

The world elements that are part of the fridge or of the background are placed on a grid.

Grid elements (all of these elements are static):
1. Regular platforms: these are platforms the ice cube can slide on and bounce off with no consequence. They represent patches of ice, parts of the plastic shelves or floor in the freezer, other contents of the freezer such as boxes of food,...
2. Spikes: these represent ice shards in the freezer or other static obstacles which will be letal to the ice cube. The ice cube will die if it collides directly with them. They can be attached to other regular platforms in all four directions.
3. Heated platforms: some sections of the level will represent the engine of the freezer and will be heated. The ice cube can slide on them but only for a maximum allowed period of time: when the ice cube enters a heated platform, a health bar will appear next to it and start decreasing to signify the amount of time the ice cube can stay on the platform before melting and dying. If the ice cube temporarily steps away from the platform, the health bar will start filling up again. When the health bar is completely full it will disappear.

All of these elements can be placed on the floor of the level but also mid-air with nothing supporting them (there is no regard to gravity in this sense).

Stand-alone elements:
1. Rolling / moving obstacles, such as rolling bottles / ice cream tubs, moving ice cubes,...: the ice cube will die if it collides directly with them. They move following gravity rules and will fall to the ground.
2. Fans: fans will produce wind, which will affect the speed and direction of the ice cube by applying a force to the ice cube. If the force of the fan is strong enough to contrast the acceleration of the ice cube, it can change its direction.
3. Breakable platforms
    1. that break after the ice cube passes on them once
    2. that break if the ice cube ground pounds or dashes on them
4. Collectibles: every level will contain 3 collectible objects away from the main path
5. Enemies: some levels will contain enemies in the form of other ice cubes. Enemies will be able to shoot ice shards or fire balls at the character, and additionally cause the player to die if it collides directly with them.
6. Projectiles: some levels will contain projectiles, spawned either by enemies or other static parts of the environment. The projectiles may either have a straight trajectory or follow the laws of gravity.

Other mechanics:
1. Stack on other ice cubes: the ice cube can stack with other ice cubes inside the freezer by jumping on top of them
    1. hit platforms or other obstacles to get rid of the additional ice cubes
    2. double jump by separating the rest of the “body” made of ice cubes
    3. if the player lands on spikes, only the ice cube at the bottom gets shattered

To be discussed:
1. Fans: should the force be the same in the entire force field or be modulated depending on how close you get to the fan?
2. Heated platforms: alternative ways of showing the ice cubes is about to die, such as gradually changing the color of the ice cube to red

### Death ###

The ice cube shatters and dies if:
1. It falls off a bottomless pit
2. It collides directly with non-allowed elements: spikes, obstacles, enemies, projectiles,...

After death the ice cube restarts at the nearest check-point.

### Game Flow ###

The game is made up of multiple short levels, which are organised in sections. Each section represents a freezer compartment and has a different theme: vegetables, ice creams, pizza,...

The first few levels only have the basic jump mechanic and normal blocks; new level and sections will gradually introduce more mechanics and combine them. 

To be discussed: order of introduction of additional elements

Levels must be completed sequentially (it is necessary to complete a level to be able to access the following ones). The player can select which level to play from a level map and can select to replay a previously completed level.

Each level will contain multiple check-points, so that the player does not have to repeat previously completed sequences.

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

## Implementation details ##

This section is meant for the programmers to keep track of implementation details.

### Ice cube ##

There are two scripts influencing the behaviour of the cube:
1. IceCubePhysics describes the physics behaviours (movement, jump, ground pound, dash...); it contains all functions to actually move the ice cube in the physical world. It does not contain input handling.
2. IceCubeInput inherits from IceCubePhysics and is the script to be attached to the ice cube GameObject. It contains all input handling as well as handling coyote time and jump buffer timers. It interacts with IceCubePhysics by setting true boolean variables ShouldJump, ShouldGroundPound,... that are protected variables from IceCubePhysics.

**Ice Cube GameObject**

The ice cube GameObject should have the following components:
1. IceCubeInput script
2. Rigidbody2D set to dynamic, having a PhysicsMaterial2D with 0 friction; collision detection should be set to continuous to avoid problems with the collisions at high speeds
3. BoxCollider2D with no rounding at the edges (this is needed for precise collision detection)
4. IceCubeAnimatorManager script
5. Animator
6. SpriteRenderer

**Ice Cube Physics**

The physics of the ice cube exploits Unity physics simulation (which is why the Rigidbody2D should be set to dynamic). The ice cube is affected by gravity and its speed is modified only by applying forces to the Rigidbody2D.

IceCubePhysics stores a variable Vector2 _currentDirection which indicates whether the cube should be moving left or right. _currentDirection should only be either Vector2.left or Vector2.right, it does not account for vertical movement. This variable is updated when the ice cube collides with other objects (see function HandleCollisions).

All physics is handled within FixedUpdate. Also in FixedUpdate, the script checks if there is any input to be acted on through the variables ShouldJump, ShouldGroundPound,... and calls the appropriate function.

The position of the cube should *never* be set explicitly to avoid problems with the collision detection. The velocity should also never be set explicitly for the same reason; the only exception is when it is set to Vector2.zero during the GroundPound function.

### Tilemaps ###

A tilemap must have the following components:
1. TilemapCollider2D with flag used by composite set to true
2. Rigidbody2D set to static
3. CompositeCollider2D (this is to make sure the tilemap has a single collider with no lines for every single tile, which makes collision handling better)