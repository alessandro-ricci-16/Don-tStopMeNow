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

The base mechanic of the game is jumping. The ice cube will move constantly and the player will not be able to stop it or change the direction directly, only to jump. Hitting obstacles such as walls or platforms will result in the ice cube changing direction. It is possible to bounce off everything except the floor.

The horizontal speed can be affected by:
1. Interactions with the environment: sliding on a frosty platform will make the cube slower, while sliding on a slippery platform will make the cube faster.
2. User input: the player can temporarily slow down or accelerate the character within an allowed range of speed.

However, the horizontal speed will never be zero. 

When jumping, the player can choose the duration of the jump by pressing the "jump" button for a different time duration, up to a maximum jump height.

While the player is in the air, the following actions are allowed:
1. Wall jump: if the player hits a platform or a wall, they can press the "jump" button again to jump off the obstacle. This is allowed only once until the character touches the ground again.
2. Mid-air dash: to be discussed
3. Fall down quicker: the player can press a "fall down" button in order to fall down quicker if the jump was too long.

The following commands are allowed:
1. jump
2. fall down quicker
3. accelerate
4. slow down
5. mid-air dash

Other decisions:
- The ice cube collider will not rotate but be static
- There are no slopes in the world

### Gameplay elements ###

Priorities:
1. Spikes / traps: if the ice cube falls directly on a spike / trap, it will die
2. Rolling / moving obstacles, such as rolling bottles / ice cream tubs, moving ice cubes,...: the ice cube will die if it collides directly with them
3. Fans: fans will produce wind, which will affect the speed and direction of the ice cube
4. Heated platforms: some sections of the level will represent the engine of the freezer and will be heated. The ice cube can slide on them but only for a maximum allowed period of time: when the ice cube enters a heated platform, a health bar will appear next to it and start decreasing to signify the amount of time the ice cube can stay on the platform before melting and dying. If the ice cube temporarily steps away from the platform, the health bar will start filling up again. When the health bar is completely full it will disappear.
To be discussed: alternative ways of showing the ice cubes is about to die, such as gradually changing the color of the ice cube to red.

If time permits:
1. Breakable platforms
    1. that break after the ice cube passes on them once
    2. that break if the ice cube performs a jump cancel and falls down quickly on them
2. Stack on other ice cubes: the ice cube can stack with other ice cubes inside the freezer by jumping on top of them
    1. hit platforms or other obstacles to get rid of the additional ice cubes
    2. double jump by separating the rest of the “body” made of ice cubes
    3. if the player lands on spikes, only the ice cube at the bottom gets shattered
4. Collectibles: every level will contain 3 collectible objects away from the main path
5. Enemies: enemy ice cubes, e.g. using ice shards as projectiles


### Death ###

The ice cube shatters and dies if:
1. It falls off a bottomless pit
2. It collides directly with non-allowed elements: spikes, obstacles, enemies

After death the ice cube restarts at the beginning of the level.

To be discussed: check-points

### Game Flow ###

The game is made up of multiple short levels, which are organised in sections. Each section represents a freezer compartment and has a different theme: vegetables, ice creams, pizza,...

The first few levels only have the basic jump mechanic and normal blocks; new level and sections will gradually introduce more mechanics and combine them. 

To be discussed: order of introduction of additional elements

Levels must be completed sequentially (it is necessary to complete a level to be able to access the following ones). The player can select which level to play from a level map and can select to replay a previously completed level.


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
    - add fall down quicker
    - adapt the speed updates for fans (forces instead of velocity updates)
- Implement spikes and moving obstacles (Alessandro Ricci)
- Implement heated platforms (Emanuele Santoro)
- GameManager class (death and respawn) (Andrea Sanguineti)
- Camera boundaries (Alessandro Ricci)