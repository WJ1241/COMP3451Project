using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.InputManagement.Interfaces;

namespace COMP3451Project.RIRRPackage.States
{
    /// <summary>
    /// Class which contains conditional information for RIRR Player entities to be modified by another class e.g. PlayerBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public class PlayerState : DynamicRIRRState, ICollisionListener, IKeyboardListener, IPlayer
    {
        #region FIELD VARIABLES

        // DECLARE a PlayerIndex, name it '_playerNum':
        private PlayerIndex _playerNum;

        // DECLARE a string, name it '_activeBehaviour':
        private string _activeBehaviour;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of PlayerState
        /// </summary>
        public PlayerState()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDLISTENER

        /// <summary>
        /// Called when Publisher has new Keyboard input information for listening objects
        /// </summary>
        /// <param name="pKeyboardState"> Holds reference to Keyboard State object </param>
        public void OnKBInput(KeyboardState pKeyboardState)
        {
            // SET value of _activeBehaviour to "stationary":
            _activeBehaviour = "stationary";

            // IF Player 1:
            if (_playerNum == PlayerIndex.One)
            {
                // IF W AND A Key down:
                if (pKeyboardState.IsKeyDown(Keys.W) && pKeyboardState.IsKeyDown(Keys.A))
                {
                    // SET _activeBehaviour to "up-left":
                    _activeBehaviour = "up-left";
                }
                // IF W AND D Key down:
                else if (pKeyboardState.IsKeyDown(Keys.W) && pKeyboardState.IsKeyDown(Keys.D))
                {
                    // SET _activeBehaviour to "up-right":
                    _activeBehaviour = "up-right";
                }
                // IF S AND A Key down:
                else if (pKeyboardState.IsKeyDown(Keys.S) && pKeyboardState.IsKeyDown(Keys.A))
                {
                    // SET _activeBehaviour to "down-left":
                    _activeBehaviour = "down-left";
                }
                // IF S AND D Key down:
                else if (pKeyboardState.IsKeyDown(Keys.S) && pKeyboardState.IsKeyDown(Keys.D))
                {
                    // SET _activeBehaviour to "down-right":
                    _activeBehaviour = "down-right";
                }
                // IF ONLY W Key down:
                else if (pKeyboardState.IsKeyDown(Keys.W))
                {
                    // SET _activeBehaviour to "up":
                    _activeBehaviour = "up";
                }
                // IF ONLY S Key down:
                else if (pKeyboardState.IsKeyDown(Keys.S))
                {
                    // SET _activeBehaviour to "down":
                    _activeBehaviour = "down";
                }
                // IF ONLY A Key down:
                else if (pKeyboardState.IsKeyDown(Keys.A))
                {
                    // SET _activeBehaviour to "left":
                    _activeBehaviour = "left";
                }
                // IF ONLY D Key down:
                else if (pKeyboardState.IsKeyDown(Keys.D))
                {
                    // SET _activeBehaviour to "right":
                    _activeBehaviour = "right";
                }
            }

            // IF _activeBehaviour has changed:
            if (_activeBehaviour != _stateName)
            {
                // CALL _scheduleCommand, passing _triggerDict[_activeBehaviour] as a parameter:
                _scheduleCommand(_triggerDict[_activeBehaviour]);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IPLAYER

        /// <summary>
        /// Property which can set value of a PlayerIndex
        /// </summary>
        public PlayerIndex PlayerNum
        {
            set
            {
                // SET value of _playerNum to incoming value:
                _playerNum = value;
            }
        }

        #endregion
    }
}

