# brownjona1.md


## Header

- Name: Jonathan Brown
- Username: brownjona1
- Animal role: Owl
- Primary responsibility: Core game functions, the odd jobs


## Code discussion

### Parts worked on

My main coding involved the player fish. Continuing where the prototype left off, I added the water/oxygen time system, and made the jumping not so buggy. When playtesters asked for them, I added dynamic jump indicators for jump direction and distance.

I also worked with the three designers regarding code for their levels.
- For the Factory, I added support for sprinklers that replenish the fish's water without updating its respawn.
- For the City, I provided scripts to spawn cars and move them in a direction. When cars were not despawning upon leaving the level boundary, I worked with the designer to fix the problem.
- For the Forest and Beach, I wrote a script for a seagull enemy which is highly involved in two levels.

Here are the scripts I worked on:

- FishMovement: Most
- FishCollision: All
- FishOxygen: All
- FishIndicators: All
- CameraTrackPlayer: All
- RotateCamera: All
- RaycastPlanes: All
- LoadSceneScript: All
- LoadSceneSimple: All
- DestroyOnBoundary: All
- SpawnCar: Some
- Seagull: All


### Most interesting code

[FishMovement.cs, DoJump(), lines 117-132](../Assets/Scripts/Fish%20Player/FishMovement.cs)

This concerns the spin on the fish during a jump. In the prototype, the fish's spin was a result of its tail fin clipping into the ground and then being pushed out of the ground collider. This was an unintended but positive bug!

When I improved the jump implementation, this clipping force was removed. I subsequently replicated the spin via the angular velocity of the fish rigidbody. The problem was, the angular velocity was not relative to which way the fish was facing! I had to localize the velocity myself using sine and cosine functions. Once I had this all figured out, I even made an alternative spin the fish could randomly choose.


### Most proud of code

[FishCollision.cs](../Assets/Scripts/Fish%20Player/FishCollision.cs)

I am most proud of my FishCollision script because I think it is intuitive and easy to understand, and because I think it follows good code practice.
- The <i>respawning</i> variable is private, with a public get function
- Instant and delayed respawns both call the same Respawn() function, reusing code
- Numbers are either declared as public variables (eg. <i>respawnTime</i>), or as local variables (eg. <i>yOffset</i> in line 143, <i>threshold</i> in line 189) -- avoiding "magic numbers"
- The OnCollision and OnTrigger functions are easy to read and expand
- Important variable checks are placed at the beginning of OnTriggerEnter() and OnTriggerExit(), so that they are only coded once, and that the functions terminate immediately if checks fail

I did miss one thing though! The OnCollision and OnTrigger functions contain several If clauses, even though only one can execute per function call. If Else clauses would terminate the functions as soon as possible.


## Reflection

The transition from "finished" prototype to work-in-progress full game was humbling. Given the prototype was just a demonstration, and that I was still new to Unity, my implementation for raycasting and fish jumping was crude and buggy. This worked in the prototype environment, more or less, but when the prototype was chosen to become a full game, I had to revisit all the problems I had left unsolved. This was a hard lesson on quality of work.

Learning to use the Unity API was a good experience. Previously I thought learning to code just meant learning a programming language, but now I know that learning an API can be just as significant. So many times during the prototype, and even for a while after, I got stuck on simple tasks simply because I did not know a certain Unity function or technique. However, as a result, this project has proven to me that I can learn an API on the fly.

I did enjoy working with other students on the project. It was great being able to depend on another coder to get things done, and to see my work from the prototype improved. However, splitting work between the three of us was harder than I realized, because we had different comforts within Unity, and because I was advantaged by having worked on the original prototype. If I were to do it again, I would try to decide with the other coders what overall responsibility each of us would have, rather than allocating only one or two jobs at a time.

Even when I work on solo projects in the future, I would like to use git. With just a few commands in the terminal, I can backup my entire project! It also makes it very easy to transfer my work between my laptop and desktop.

I would also like to do research into game engines. This project showed me that though engines can be difficult, they are exciting and something I can learn.