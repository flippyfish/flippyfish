Flippy Fish
===========

Jump your way to the ocean while avoiding cars and obstacles!



- A, D / Left, Right: Rotate camera

- Drag right-click: Rotate camera

- Scroll wheel: Zoom




- Hold left-click to charge a jump

- Release left-click to jump

- Cancel a charged jump by overcharging it

- The farther you jump, the more likely you are to go off course!



If you hit a hazard, you respawn at the latest pond you touched.


## Description

The player moves the fish to the ocean via numerous jumps. Touching a hazard (moving cars, campfires) sends the fish back to the latest pond.

The fish cannot jump backward, and the camera cannot be turned far left or right. This emphasises an arcade-like forward gameplay.

Holding and releasing a click to make the fish jump is the core mechanic.  
Levels consist of flat terrain with trees and rocks and other scenic obstacles, and roads with moving cars. An ocean is at the end of each level.  
The gameplay loop is to select a level and beat it. The complete game could have progressive level unlocks.

If we were to expand the gameplay, we might add an oxygen bar that replenishes in ponds. If you ran out of oxygen, you would respawn at the latest pond. This would impose a time limit without putting the entire level at stake.


## Development

I started development with a jumping script to move an orange box in an empty plane. This required a raycast from the mouse x,y coordinates onto the world. The fish rotates to look at the specified point on the world, and when the mouse is released, it applies a jump force in that direction.

At first, depending on terrain, the raycasted point could be higher or lower than the fish, meaning the front or back of the fish would clip into the ground when looking at the point. This interfered with the jump, causing jump distance to be inconsistent. I fixed the issue by replacing the y position of the look point with the fish's current y position.

Later I made an empty plane specifically for raycasting and put it on its own layer, and I told the raycast to only work with that layer. This ensured that obstacles and moving cars would not interfere with jump direction.

Much of the remaining work involved collider and trigger detection, and tags to indicate what the fish and cars were colliding with.

No external libraries or assets were used in this project.


## Issue

When running the built game, sometimes the camera may shake when charging a leap. This does not happen in the Unity editor! The shaking seems more severe on my laptop than my desktop.

Because the camera tracks the player, I tried raising the fish when charging to avoid clipping with the ground, and I tried rounding the camera x,y,z values, but neither approach stopped the shaking.

Perhaps this may be resolved by someone else, if this prototype is chosen to become a full game.