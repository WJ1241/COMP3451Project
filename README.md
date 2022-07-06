# COMP3451Project

## Royal Infirmary Relic Recovery

Royal Infirmary Relic Recovery (abbreviated as RIRR from now on) is a game produced for the Advanced Game Design & Engineering module when studying at the University of Worcester, and was developed with students on the Game Art course. 'Team Orbital' was the programming team, and 'The Undecided' was the art team. It features stealth aspects where the player must navigate each level to collect an artefact and return to the starting door to progress. Between each level, the intention was for the visual novel to teach the players about different items used in different time periods within the Worcester Royal Infirmary, due to that being the client's specifications.

### Setting
RIRR is set in the Infirmary and on the Worcester City campus and surrounding area in 2006, with each level representing a different level of the building, and ending the game with a final visual novel cutscene.

### Credits

Team Orbital consists of:
- William Smith (Lead Programmer)
- Declan Kerby-Collins (Lead Designer)

The Undecided consists of:
- Harry Hartwell (VN art, logos)
- Jack Maley (Sprites, level art and animation sheets)

## Orbital Engine
### Different Runtime managers
- Features an Engine Manager, Entity Manager, Scene Manager, Scene Graphs, Collision Manager, Input Managers for both Keyboard and Mouse, Sound Effect and Song Managers
- Multiple types of Scene Graphs to allow for different level types, Scene Graph for levels, Cutscene Graph for visual novel segments (Menu Graph to be implemented at another time in future version)
### Entity Building Structure
- Splits an Entity into three different objects, State, Entity, Behaviour for understanding when an action should be decided, depending on the current state object used.
