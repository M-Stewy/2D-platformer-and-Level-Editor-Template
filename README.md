<div id="Header" align="center">
</div>

<h1>
basic template for a 2D platformer game
  </h1>


Comes with a basic level editor and state machine based player controller  

If you want to use it to create your own games out of feel free to,  
it was just made as a summer project

<h2>
HOW TO ADD NEW TILE TO EDITOR:
</h2>

to make a new tile for the level editor just:  <br />
  1. if it should only have basic ground functionality, simply make a new Scriptable Object (under Create/LevelEditor/Tile)   <br />
  2. Give the tileScriptableObject an Id (can be anything)  <br />
  3. a refernce to the unity tile you want it to place  <br />
  4. an image of that tile,  <br />
  5. set it to use the Ground Tilemap (can be a different one if you want it to have different functionailty (more on this later) )  <br />
  6. and an ID number (IMPORANT: ALL MUST BE NUMBERED IN ALPHABETACAL ORDER, ex( Atile == 0, Btile == 1 etc)  <br />
  7.  if you want special functionailty for the tile:  <br />
     A. make a new tilemap under the grid parent in the heirachy (In both the level Editor Scene, and EmptyScene)  <br />
     B. give that tilemap whatever functionailty you want these tiles to have  <br />
     C. add the same name of that tilemap to the enum TileMaps at the top of LevelEditorLoadAndSave.cs  <br />
     D. make sure the tileScriptable has this new enum selected instead of Ground  <br />
<br />
<br />
<br />
<br />

Some code used from:  
https://www.youtube.com/watch?v=qXbjyzBlduY  
https://www.youtube.com/watch?v=snUe2oa_iM0  
