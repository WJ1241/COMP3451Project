using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.States
{
    /// <summary>
    /// Class which contains conditional information for Pong Paddle entities to be modified by another class e.g. PaddleBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
    /// </summary>
    public class PaddleState : State, IKeyboardListener, IPlayer
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_animationEvent':
        private EventHandler<UpdateEventArgs> _animationEvent;

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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public override void Initialise(IUpdateEventListener pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // IF pUpdateEventListener DOES implements IAnimation:
                if (pUpdateEventListener is IAnimation)
                {
                    // SUBSCRIBE _animationEvent to pUpdateEventListener.OnUpdateEvent():
                    _animationEvent += pUpdateEventListener.OnUpdateEvent;
                }
                // IF pUpdateEventListener DOES NOT implements IAnimation:
                else
                {
                    // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnUpdateEvent():
                    _behaviourEvent += pUpdateEventListener.OnUpdateEvent;
                }
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance!");
            }
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


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public override void Update(GameTime pGameTime)
        {
            // DECLARE & INITIALISE an UpdateEventArgs, name it '_tempUpdateEA':
            UpdateEventArgs _tempUpdateEA = new UpdateEventArgs();

            // SET RequiredArg Property's value to pGameTime:
            _tempUpdateEA.RequiredArg = pGameTime;

            // CALL Invoke() on _behaviourEvent, passing this class and _tempUpdateEA as parameters:
            _behaviourEvent.Invoke(this, _tempUpdateEA);

            // CALL Invoke() on _animationEvent, passing this class and _tempUpdateEA as parameters:
            _animationEvent.Invoke(this, _tempUpdateEA);
        }

        #endregion
    }
}
