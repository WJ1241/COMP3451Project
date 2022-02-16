using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.Services.Commands.Interfaces;

namespace COMP3451Project.EnginePackage.States
{
    /// <summary>
    /// Class which contains conditional information for Pong Paddle entities to be modified by another class e.g. PaddleBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/02/22
    /// </summary>
    public class PaddleState : State, IKeyboardListener, IPlayer
    {
        #region FIELD VARIABLES

        // DECLARE a PlayerIndex, name it '_playerNum':
        private PlayerIndex _playerNum;

        // DECLARE a string, name it '_activeBehaviour':
        private string _activeBehaviour;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of PaddleState
        /// </summary>
        public PaddleState()
        {
            // INSTANTATE _triggerDict as a new Dictionary<string, ICommand>():
            _triggerDict = new Dictionary<string, ICommand>();
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
                // IF W Key down:
                if (pKeyboardState.IsKeyDown(Keys.W))
                {
                    // SET _activeBehaviour to "up":
                    _activeBehaviour = "up";
                }

                // ELSE IF S Key down:
                else if (pKeyboardState.IsKeyDown(Keys.S))
                {
                    // SET _activeBehaviour to "down":
                    _activeBehaviour = "down";
                }
            }

            // IF Player 2:
            else if (_playerNum == PlayerIndex.Two)
            {
                // IF Up Arrow Key down:
                if (pKeyboardState.IsKeyDown(Keys.Up))
                {
                    // SET _activeBehaviour to "up":
                    _activeBehaviour = "up";
                }

                // ELSE IF Down Arrow Key down:
                else if (pKeyboardState.IsKeyDown(Keys.Down))
                {
                    // SET _activeBehaviour to "down":
                    _activeBehaviour = "down";
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
