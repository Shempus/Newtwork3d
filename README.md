# Unity 3d network stuff
Some based on https://medium.com/eincode/getting-started-with-multiplayer-player-movement-f9f7f6a4217

Using the new input system https://gamedevbeginner.com/input-in-unity-made-easy-complete-guide-to-the-new-system/

Camera stuff from https://www.youtube.com/watch?v=zYqJWb356aA
with a bit of help from https://stackoverflow.com/a/75714146 so I don't need Mirror

Mouse look from first 4ish minutes of this... not using new InputSystem: https://www.youtube.com/watch?v=f473C43s8nE

Looking at cleaning up movement and camera, looking here https://mymasterdesigner.com/2022/08/12/fps-movement-in-unity-detailed/

Win/Lose Images generated at https://www.textstudio.com/

Player HUD idea: https://discussions.unity.com/t/player-hud-only-visible-to-instantiated-player/909028/4

Unique ids per player NetworkManager.LocalClientId always gives 0?

NetworkManager count of connections can only be called on the server, so only works on the host, and SeverRPC functions cannot return values (must be void) so we know the transforms are unique per player so we will use transform.name to store a unique value. A UUID initially but should be the name chosen in a lobby.
