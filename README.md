# Candy Invaders


####**Unity version:** 5.5
####**Author:** Javier Bullrich

Total development time: 6 hours.
**Controls**: X to shot, arrows to move.

---

## How it was made

The game is split in two parts, the game part and the menu part.

The game is composed of a game manager, which handles all the other manager. Every notification to the game pass through here, one ship disappeared? Tell the manager. Player pressed the shooting button? Ask the manager for a bullet.
The manager has a Pool Manager and a Sound Manager which has all the sounds that the game can play.
####Game Manager
The game manager has the most important part of all, a public static float called **DeltaTime** which every moving script calls instead of the Time.DeltaTime. The reason of this is simple, sometimes the game must be paused, but not the Invokes, Coroutines or anything that use unity's time as a reference, so instead I provide my own delta time. An example of this is, when the game is over, it shows a lost screen and cancel the DeltaTime every script use, but use a Invoke to restart the game, so it pause everything it depends on it, but not Unity.
The Game Manager also is the one who communicates between the scripts, adding score, resetting the game (there is an interface called IReset which it's Respawn function is called to reset the gameObject withouth reloading the scene) or returning a script it was asked for.
####Sound Manager
The sound manager main function is to store the sounds of the game, every time a gameobject wants to play a sound, it shall be asked to the gamemanager, which will take the sound file from it's sound manager and sent it to the player.
####Pool Manager
The pool manager works like the sound manager, a gameobject must be asked to the GameManager, either by asking by the prefab's name or the prefab's object. The pool manager then find a copy inactive of that object and sends it to the script asking for it.
###Other elements


####Grid System
The grid system was a tricky part, the user can set how many rows and columns he desire, with the distant between each item, and the grid will show in real time where the elements will spawn.
This is the gizmos function:

            private void OnDrawGizmos()
        {
            Vector3 spawnPos = transform.position;
            float size = .3f;
            if (!Application.isPlaying)
            {
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Vector3 spawnPosition = spawnPos + new Vector3(distanceBetween * j, 0);
                        Gizmos.DrawLine(spawnPosition - Vector3.up * size, spawnPosition + Vector3.up * size);
                        Gizmos.DrawLine(spawnPosition - Vector3.left * size, spawnPosition + Vector3.left * size);
                    }
                    spawnPos.y -= distanceBetween;
                }
            }
        }
When the game start the grid system will instantiate a prefab into each space, and add all of them under a parent gameobject, then, every distance of time setted up by the user, the grid will move to the side, if the grid detect that it's most distant element to that side is over the 97% of the screen's width if going to the left, or below the 3% of the width, it will move the container one place down and move to the other side.

Every time it moves, the grid is also checking how many elements are active, and change the movement pause accordly (when there are less ship active it takes shorter pauses).

Another element that the grid has is the chain destruction, each ship setups a random color which it saves in itself. When an element is destroyed it call all its adjacent elements and check if they belong to the same color, if so, it destroy them and add the combo multiplier to the score that it's going to give to the game manager.

            private IGridElement[] getAdjacentElements(int shipY, int shipX)
        {
            IGridElement[] els = new IGridElement[4];

            if (shipY > 0)
                els[0] = elements[shipY - 1, shipX];

            if (shipY < height - 1)
            {
                els[1] = elements[shipY + 1, shipX];
            }

            if (shipX > 0)
                els[2] = elements[shipY, shipX - 1];

            if (shipX < width - 1)
                els[3] = elements[shipY, shipX + 1];

            return els;
        }
####UI Manager
The ui manager is simple, it show the text, the amount of lives and the game over screen when the game is over, and it is only accessed by the Game Manager.
####Protections
The protections were hard, I wanted the game to not have rigidbodies, so I had to make them detect collisions in another way. The protections are raycasting to all it's side in search of an enemy, this ray is extremely short, so it only hits when at a collision distance. The rays are casted from the box collider 2D limits.
####Bullets
The bullets are a simple script that goes up or down, depending on who fired it, and they raycast to their direction in search of a target with the interface **IDamagable** to let the target handle the damage in it's own way (for example the protections check how many life point it has and change to the apropiate sprite or simply disable itself).
####Player
The player is pretty simple, a sprite with a box collider 2D, that calls bullet from the pool to it's position, move to the side (and stop if it's over the 97% or below the 3% of the screen width) and if it's hit by a raycast, it's IDamagable interface function is called and it destroy itself.

###Main Menu
The main menu is an asset in which I'm currently working, so I decided to use it. It has a main scene which all the inputs are loaded, and it instantiates enough all the corresponding buttons for loading a scene, quitting the game and making custom behavior. It's extremely customizable, every button can have a different color and different action.
####Pause menu
This asset also brings a pause menu which use the inputs loaded from the Main Menu Scene, so if this asset is used without passing through the other scene it may not work.
It simply pause the game setting Unity's Time Scale to 0. It have two buttons, one for continuing the game and another to go back to the main menu's scene.

---

## Git commits nomenclature

* __+ sign for new additions__.
* __- sign for deleted stuff__.
* **! examplation sign for modified stuff**.
* __* for bugfixing__.
* __? sign for Bugs found__.

```
Example
New commit by @USER
+ Added new feature to the hunter
+ Added a popup animation to the UI button.
* Fixed error on Player.Awake();
! Changed NPC behavior from inheritance to interfaces.
- Deleted WhenPlayerPoops() code block.
? If 4 NPC have the same target, one will destroy itself
```