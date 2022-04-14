using System;
using System.Collections.Generic;
using COMP3451Project.RIRRPackage.Entities.Interfaces;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for NPC entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 13/04/22
    /// </summary>
    public class NPCBehaviour : RIRRBehaviour, IInitialiseParam<ICommand>, IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>, IInitialiseParam<string, EventHandler<UpdateEventArgs>>,
        IEventListener<CollisionEventArgs>
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, EventHandler<UpdateEventArgs>>, name it '_animationEventDict':
        private IDictionary<string, EventHandler<UpdateEventArgs>> _animationEventDict;

        // DECLARE an ICommand, name it '_sfxCommand':
        private ICommand _sfxCommand;

        // DECLARE a string, name it '_currentAnimation':
        private string _currentAnimation;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of NPCBehaviour
        /// </summary>
        public NPCBehaviour()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public void OnEvent(object pSource, CollisionEventArgs pArgs)
        {
            #region OBSTACLE COLLISION

            // IF pArgs.RequiredArg is on Layer 2/3/5:
            if ((pArgs.RequiredArg as ILayer).Layer == 2 || (pArgs.RequiredArg as ILayer).Layer == 3 || (pArgs.RequiredArg as ILayer).Layer == 5)
            {
                // DECLARE & INITIALISE a Vector2 with value of _direction, name it 'tempVel':
                Vector2 tempDir = _direction;

                // IF _entity has collided with the bottom of the other collidable and is moving UPWARDS:
                if ((_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y <= pArgs.RequiredArg.HitBox.Bottom
                 && (_entity as ICollidable).HitBox.Bottom >= pArgs.RequiredArg.HitBox.Bottom && _direction.Y < 0)
                {
                    // SET Position of _entity to the bottom of the collidable:
                    _entity.Position = new Vector2(_entity.Position.X, pArgs.RequiredArg.HitBox.Bottom);

                    // INVERSE tempDir.Y:
                    tempDir.Y *= -1;

                    // IF a "Down" animation is stored in _animationEventDict:
                    if (_animationEventDict.ContainsKey("Down"))
                    {
                        // SET value of _currentAnimation to "Down":
                        _currentAnimation = "Down";
                    }
                }

                // IF _entity has collided with the top of the other collidable and is moving DOWNWARDS:
                if ((_entity as ICollidable).HitBox.Bottom >= pArgs.RequiredArg.HitBox.Top
                    && (_entity as ICollidable).HitBox.Top <= pArgs.RequiredArg.HitBox.Top && _direction.Y > 0)
                {
                    // SET Position of _entity to the top of the collidable:
                    _entity.Position = new Vector2(_entity.Position.X, pArgs.RequiredArg.HitBox.Top - (_entity as IRotation).DrawOrigin.Y);

                    // INVERSE tempDir.Y:
                    tempDir.Y *= -1;

                    // IF an "Up" animation is stored in _animationEventDict:
                    if (_animationEventDict.ContainsKey("Up"))
                    {
                        // SET value of _currentAnimation to "Up":
                        _currentAnimation = "Up";
                    }
                }

                // IF _entity has collided with the right of the other collidable and is moving to the LEFT:
                if ((_entity as ICollidable).HitBox.Left <= pArgs.RequiredArg.HitBox.Right
                 && (_entity as ICollidable).HitBox.Right >= pArgs.RequiredArg.HitBox.Right && _direction.X < 0)
                {
                    // SET Position of _entity to the right of the collidable:
                    _entity.Position = new Vector2(pArgs.RequiredArg.HitBox.Right + (_entity as IRotation).DrawOrigin.X, _entity.Position.Y);

                    // INVERSE tempDir.X:
                    tempDir.X *= -1;

                    // IF a "Right" animation is stored in _animationEventDict:
                    if (_animationEventDict.ContainsKey("Right"))
                    {
                        // SET value of _currentAnimation to "Right":
                        _currentAnimation = "Right";
                    }
                }

                // IF _entity has collided with the left of the other collidable and is moving to the RIGHT:
                if ((_entity as ICollidable).HitBox.Right >= pArgs.RequiredArg.HitBox.Left
                 && (_entity as ICollidable).HitBox.Left <= pArgs.RequiredArg.HitBox.Left && _direction.X > 0)
                {
                    // SET Position of _entity to the left of the collidable:
                    _entity.Position = new Vector2(pArgs.RequiredArg.HitBox.Left - (_entity as IRotation).DrawOrigin.X, _entity.Position.Y);

                    // INVERSE tempDir.X:
                    tempDir.X *= -1;

                    // IF a "Left" animation is stored in _animationEventDict:
                    if (_animationEventDict.ContainsKey("Left"))
                    {
                        // SET value of _currentAnimation to "Left":
                        _currentAnimation = "Left";
                    }
                }

                // INITIALISE _direction with modified tempDir value:
                _direction = tempDir;
            }

            #endregion


            #region PLAYER COLLISION

            // IF pArgs.RequiredArg implements IPlayer and is not currently damaged:
            if (pArgs.RequiredArg is IPlayer && !(pArgs.RequiredArg as ITakeDamage).Damaged && pArgs.RequiredArg.HitBox.Top + (pArgs.RequiredArg as IRotation).DrawOrigin.Y < (_entity as ICollidable).HitBox.Bottom)
            {
                // INITIALISE FirstParam Property of _sfxCommand with value of "Attack":
                (_sfxCommand as ICommandOneParam<string>).FirstParam = "Attack";

                // SCHEDULE _sfxCommand to play:
                (_entity as ICommandSender).ScheduleCommand(_sfxCommand);
            }

            #endregion
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnEvent(object pSource, UpdateEventArgs pArgs)
        {
            // ASSIGNMENT, set value of _velocity to _speed mutlipled by _direction:
            _velocity = (_entity as IGetSpeed).GetSpeed * _direction;

            // SET value of _entity's Velocity Property to value of _velocity:
            (_entity as IVelocity).Velocity = _velocity;

            // ADD & APPLY velocity to current position:
            _entity.Position += _velocity;

            // INVOKE _animationEventDict[_currentAnimation], passing this class and pArgs as parameters:
            _animationEventDict[_currentAnimation].Invoke(this, pArgs);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMAND>

        /// <summary>
        /// Initialises an object with an ICommand object
        /// </summary>
        /// <param name="pCommand"> ICommand object </param>
        public void Initialise(ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // INITIALISE _sfxCommand with reference to pCommand:
                _sfxCommand = pCommand;
            }
            // IF pCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, EVENTHANDLER<UPDATEEVENTARGS>>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, EventHandler<UpdateEventArgs> instance
        /// </summary>
        /// <param name="pUpdateEventDict"> IDictionary<string, EventHandler<UpdateEventArgs> instance </param>
        public void Initialise(IDictionary<string, EventHandler<UpdateEventArgs>> pUpdateEventDict)
        {
            // IF pUpdateEventDict DOES HAVE an active instance:
            if (pUpdateEventDict != null)
            {
                // INITIALISE _animationEventDict with reference to pUpdateEventDict:
                _animationEventDict = pUpdateEventDict;
            }
            // IF pUpdateEventDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pUpdateEventDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, EVENTHANDLER<UPDATEEVENTARGS>>>

        /// <summary>
        /// Initialises an object with a string and a reference to an UpdateEventArgs Event
        /// </summary>
        /// <param name="pEventName"> Name of Event </param>
        /// <param name="pUpdateEvent"> Update Event Reference </param>
        public void Initialise(string pEventName, EventHandler<UpdateEventArgs> pUpdateEvent)
        {
            // IF pUpdateEvent IS a valid reference:
            if (pUpdateEvent != null)
            {
                // IF _animationEventDict DOES NOT contain pEventName as a key:
                if (!_animationEventDict.ContainsKey(pEventName))
                {
                    // IF _currentAnimation HAS NOT been initialised yet:
                    if (_currentAnimation == null)
                    {
                        // INITIALISE _currentAnimation with value of pEventName:
                        _currentAnimation = pEventName;
                    }

                    // ADD pEventName as a key, and pUpdateEvent as a value to _animationEventDict:
                    _animationEventDict.Add(pEventName, pUpdateEvent);
                }
                // IF _animationEventDict DOES contain value of pEventName already:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pEventName already stored in _animationEventDict!");
                }
            }
            // IF pUpdateEvent IS NOT a valid reference:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEvent is not a valid event reference!");
            }
        }

        #endregion
    }
}
