using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.InputManagement.Interfaces;

namespace COMP3451Project.RIRRPackage.States
{
    /// <summary>
    /// Class which contains conditional information for RIRR Player entities to be modified by another class e.g. PlayerBehaviour / HealthBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public class PlayerState : AnimatedRIRRState, ICollisionListener, IInitialiseParam<EventHandler<UpdateEventArgs>, EventHandler<CollisionEventArgs>>, IKeyboardListener, IPlayer
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_healthUpdateEvent':
        private EventHandler<UpdateEventArgs> _healthUpdateEvent;

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_healthCollisionEvent':
        private EventHandler<CollisionEventArgs> _healthCollisionEvent;

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


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable">Other entity implementing ICollidable</param>
        public override void OnCollision(ICollidable pScndCollidable)
        {
            // SET RequiredArg Property value of (_argsDict["CollisionEventArgs"] to reference to pScndCollidable:
            (_argsDict["CollisionEventArgs"] as CollisionEventArgs).RequiredArg = pScndCollidable;

            // INVOKE _collisionEvent(), passing this class and (_argsDict["CollisionEventArgs"] as parameters:
            _collisionEvent.Invoke(this, _argsDict["CollisionEventArgs"] as CollisionEventArgs);

            // INVOKE _healthCollisionEvent(), passing this class and (_argsDict["CollisionEventArgs"] as parameters:
            _healthCollisionEvent.Invoke(this, _argsDict["CollisionEventArgs"] as CollisionEventArgs);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<EVENTHANDLER<UPDATEEVENTARGS>, EVENTHANDLER<COLLISIONEVENTARGS>>

        /// <summary>
        /// Initialises an object with TWO EventHandlers, one for Update and one for Collision
        /// </summary>
        /// <param name="pUpdateEvent"> Event for Updating an object </param>
        /// <param name="pCollisionEvent"> Event for activating collision behaviour </param>
        public void Initialise(EventHandler<UpdateEventArgs> pUpdateEvent, EventHandler<CollisionEventArgs> pCollisionEvent)
        {
            // IF BOTH Events DO HAVE active references:
            if (pUpdateEvent != null && pCollisionEvent != null)
            {
                // SUBSCRIBE _healthUpdateEvent with a reference to pUpdateEvent:
                _healthUpdateEvent += pUpdateEvent;

                // SUBSCRIBE _healthCollisionEvent with a reference to pCollisionEvent:
                _healthCollisionEvent += pCollisionEvent;
            }
            // IF BOTH Events DO NOT HAVE active references:
            else
            {
                // THROW a new NullReferenceException(), with corresponding message:
                throw new NullReferenceException("ERROR: pUpdateEvent OR pCollisionEvent do not have active references!");
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

            // INVOKE _healthUpdateEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _healthUpdateEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);

            // INVOKE _animationEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _animationEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);
        }

        #endregion
    }
}

