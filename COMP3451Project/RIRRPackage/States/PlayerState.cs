using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.States;

namespace COMP3451Project.RIRRPackage.States
{
    /// <summary>
    /// Class which contains conditional information for RIRR Player entities to be modified by another class e.g. PlayerBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 10/04/22
    /// </summary>
    public class PlayerState : State, ICollisionListener, IKeyboardListener, IPlayer
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_animationEvent':
        private EventHandler<UpdateEventArgs> _animationEvent;

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_collisionEvent':
        private EventHandler<CollisionEventArgs> _collisionEvent;

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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IEVENTLISTENER<UPDATEEVENTARGS>>

        /// <summary>
        /// Initialises an object with an IEventListener<UpdateEventArgs> object
        /// </summary>
        /// <param name="pUpdateEventListener"> IEventListener<UpdateEventArgs> object </param>
        public override void Initialise(IEventListener<UpdateEventArgs> pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // IF pUpdateEventListener DOES implements IAnimation:
                if (pUpdateEventListener is IAnimation)
                {
                    // SUBSCRIBE _animationEvent to pUpdateEventListener.OnEvent():
                    _animationEvent += pUpdateEventListener.OnEvent;
                }
                // IF pUpdateEventListener DOES NOT implements IAnimation:
                else
                {
                    // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnEvent():
                    _behaviourEvent += pUpdateEventListener.OnEvent;

                    // SUBSCRIBE _collisionEvent to pUpdateEventListener.OnEvent():
                    _collisionEvent += (pUpdateEventListener as IEventListener<CollisionEventArgs>).OnEvent;
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


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable">Other entity implementing ICollidable</param>
        public void OnCollision(ICollidable pScndCollidable)
        {
            // SET RequiredArg Property value of (_argsDict["CollisionEventArgs"] to reference to pScndCollidable:
            (_argsDict["CollisionEventArgs"] as CollisionEventArgs).RequiredArg = pScndCollidable;

            // INVOKE _collisionEvent(), passing this class and (_argsDict["CollisionEventArgs"] as parameters:
            _collisionEvent.Invoke(this, _argsDict["CollisionEventArgs"] as CollisionEventArgs);
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


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public override void Update(GameTime pGameTime)
        {
            // SET RequiredArg Property value of(_argsDict["UpdateEventArgs"] to reference to pGameTime:
            (_argsDict["UpdateEventArgs"] as UpdateEventArgs).RequiredArg = pGameTime;

            // INVOKE _behaviourEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _behaviourEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);

            // INVOKE _animationEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _animationEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);
        }

        #endregion
    }



}

