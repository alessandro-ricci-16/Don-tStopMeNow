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

The levels will be laid out on a map representing the different freezer compartments, with every section having a different theme (e.g. frozen vegetables, frozen meat, ice creams,…) and new mechanics introduced. Each section will have a series of short levels.

## Gameplay ##

### Base mechanic ###

The base mechanic of the game is jumping. The ice cube will move at a constant speed and the player will not be able to stop it, only to jump. 

It is possible to “cancel jumps” mid-air, which results in the ice cube falling down more quickly and then continuing in the previous direction.

There are two commands:
- up → jump
- down → cancel jump, fall down

Hitting obstacles such as walls or platforms will result in the ice cube changing direction. It is possible to bounce off everything except the floor.

The jump always reaches a fixed height, if the ice cube hits a wall or other obstacle mid-jump the trajectory simply gets mirrored to the other direction and the jump continues.

### Additional elements ###
1. Spikes / traps → if the ice cube falls directly on a spike / trap, it will die
2. Breakable platforms
    1. that break after the ice cube passes on them once
    2. that break if the ice cube performs a jump cancel and falls down quickly on them
3. Stack on other ice cubes → the ice cube can stack with other ice cubes inside the freezer by jumping on top of them
    1. hit platforms or other obstacles to get rid of the additional ice cubes
    2. double jump by separating the rest of the “body” made of ice cubes
    3. if you land on spikes, only the ice cube at the bottom gets shattered
4. Enemy ice cubes coming at you → possibly different behaviours, e.g. mirroring your actions
5. Dashing
6. Ziplines
7. Leave a trail and make yourself smaller to go faster
8. The last level of every section is close to the freezer door which can open periodically, therefore you must be careful to complete the level without melting

### Death ###

The ice cube shatters and dies if:
1. It falls off a bottomless pit
2. It falls on spikes / traps
3. It crashes into enemy ice cubes

After death the ice cube restarts at the beginning of the level.

## Deadlines ##

Official deadlines:
- November 6th - Game Design Document
- December 10th - Prototype Submission
- January 13th - Beta Submission
- February 27th - Final Project Submission
