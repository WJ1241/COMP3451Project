﻿using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Player entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 09/04/22
    /// </summary>
    /// <REFERENCE> Lewin, N. (2016). MonoGame Tutorial 009 - Sprite Collision Detection and Response. Available at: https://youtu.be/CV8P9aq2gQo?t=480. (Accessed: 9 April 2022). </REFERENCE>
    public class PlayerBehaviour : PongBehaviour, IDirection, IEventListener<CollisionEventArgs>, IInitialiseParam<ICommand>
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_updatePosCommand':
        private ICommand _updatePosCommand;

        // DECLARE a Vector2, name it '_direction':
        private Vector2 _direction;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of PlayerBehaviour
        /// </summary>
        public PlayerBehaviour()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {
            // IF Paddle at top of screen:
            if ((_entity as ICollidable).HitBox.Top <= 0)
            {
                // ASSIGNMENT, set _entity.Position. to _entity's Origin Point, keeps at top of screen:
                _entity.Position = new Vector2(_entity.Position.X, (_entity as IRotation).DrawOrigin.Y);
            }

            // IF Paddle at bottom of screen:
            if (_entity.Position.Y + (_entity as IRotation).DrawOrigin.Y >= (_entity as IContainBoundary).WindowBorder.Y)
            {
                // ASSIGNMENT, set _entity.Position.Y to _windowBorder.Y - _textureSize.Y, keeps at bottom of screen:
                _entity.Position = new Vector2(_entity.Position.X, (_entity as IContainBoundary).WindowBorder.Y - (_entity as IRotation).DrawOrigin.Y);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IDIRECTION

        /// <summary>
        /// Property which allows read and write access to the value of an entity's direction
        /// </summary>
        public Vector2 Direction
        {
            get
            {
                // RETURN value of _direction:
                return _direction;
            }
            set
            {
                // SET value of _direction to incoming value:
                _direction = value;
            }
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
            // CALL StopMovement(), passing pArgs.RequiredArg as a parameter:
            StopMovement(pArgs.RequiredArg);
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>>

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

            // CALL UpdateFollowEntity():
            UpdateFollowEntity();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMAND>

        /// <summary>
        /// Initialises with an object with an ICommand instance
        /// </summary>
        /// <param name="pCommand"> ICommand instance </param>
        public void Initialise(ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // INITIALISE _updatePosCommand with reference to pCommand:
                _updatePosCommand = pCommand;
            }
            // IF pCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR pCommand does not have an active instance!");
            }
        }

        #endregion


        #region PRIVATE METHODS

        /// <summary>
        /// Updates an Entity that requires lerp positioning
        /// </summary>
        private void UpdateFollowEntity()
        {
            // INITIALISE FirstParam Property of _updatePosCommand with value of _entity's Position Property:
            (_updatePosCommand as ICommandTwoParam<Vector2, Vector2>).FirstParam = _entity.Position;

            // INITIALISE SecondParam Property of _updatePosCommand with value of _entity's TextureSize:
            (_updatePosCommand as ICommandTwoParam<Vector2, Vector2>).SecondParam = (_entity as IRotation).DrawOrigin;

            // SCHEDULE _updatePosCommand to be executed:
            (_entity as ICommandSender).ScheduleCommand(_updatePosCommand);
        }

        /// <summary>
        /// When called, stops movement of Player object
        /// </summary>
        /// <param name="scndCollidable">Reference to other object in collision</param>
        /// <CITATION> (Lewin, 2016) </CITATION>
        /// <CITATION> (Smith, 2021) </CITATION>
        private void StopMovement(ICollidable scndCollidable)
        {
            /// LEFT OF COLLIDABLE COLLISION

                /* Right of object colliding with Left of collidable AND */
            if ((_entity as ICollidable).HitBox.Right > scndCollidable.HitBox.Left &&
                /* Left of object to the Left of collidable AND */
                (_entity as ICollidable).HitBox.Left < scndCollidable.HitBox.Left &&
                /* Bottom of object pos greater than Top of collidable AND */
                (_entity as ICollidable).HitBox.Bottom > scndCollidable.HitBox.Top &&
                /* Top of object pos less than Bottom of collidable AND Moving to the Right */
                (_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y < scndCollidable.HitBox.Bottom && _velocity.X > 0)
            {
                // WRITE to console that Left side collision has occurred:
                System.Console.WriteLine("Left Collision");

                // SET _entity.Position.X value to left of collidable object:
                _entity.Position = new Vector2(scndCollidable.HitBox.Left - (_entity as IRotation).DrawOrigin.X, _entity.Position.Y);
            }

            /// RIGHT OF COLLIDABLE COLLISION
            
                /* Left of object colliding with Right of collidable AND */
            if ((_entity as ICollidable).HitBox.Left < scndCollidable.HitBox.Right &&
                /* Right of object to the Right of collidable AND */
                (_entity as ICollidable).HitBox.Right > scndCollidable.HitBox.Right &&
                /* Bottom of object pos greater than Top of collidable AND */
                (_entity as ICollidable).HitBox.Bottom > scndCollidable.HitBox.Top &&
                /* Top of object pos less than Bottom of collidable AND Moving to the Right */
                (_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y < scndCollidable.HitBox.Bottom && _velocity.X < 0)
            {
                // WRITE to console that Right side collision has occurred:
                System.Console.WriteLine("Right Collision");

                // SET _entity.Position.X value to right of collidable object:
                _entity.Position = new Vector2(scndCollidable.HitBox.Right + (_entity as IRotation).DrawOrigin.X, _entity.Position.Y);
            }

            /// TOP OF COLLIDABLE COLLISION

                /* Bottom of object colliding with Top of collidable AND */
            if ((_entity as ICollidable).HitBox.Bottom > scndCollidable.HitBox.Top &&
                /* Top of object to the Top of collidable AND */
                (_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y < scndCollidable.HitBox.Top &&
                /* Right of object pos greater than Left of collidable AND */
                (_entity as ICollidable).HitBox.Right > scndCollidable.HitBox.Left &&
                /* Top of object pos less than Bottom of collidable AND Moving Downwards */
                (_entity as ICollidable).HitBox.Left < scndCollidable.HitBox.Right && _velocity.Y > 0)
            {
                // WRITE to console that Right side collision has occurred:
                System.Console.WriteLine("Top Collision");

                // SET _entity.Position.Y value to top of collidable object:
                _entity.Position = new Vector2(_entity.Position.X, scndCollidable.HitBox.Top - (_entity as IRotation).DrawOrigin.Y);
            }


            //// BOTTOM OF COLLIDABLE COLLISION

                /* Top of object colliding with Bottom of collidable AND */
            if ((_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y < scndCollidable.HitBox.Bottom &&
                /* Bottom of object to the Bottom of collidable AND */
                (_entity as ICollidable).HitBox.Bottom > scndCollidable.HitBox.Bottom &&
                /* Right of object pos greater than Left of collidable AND */
                (_entity as ICollidable).HitBox.Right > scndCollidable.HitBox.Left &&
                /* Top of object pos less than Bottom of collidable AND Moving Upwards */
                (_entity as ICollidable).HitBox.Left < scndCollidable.HitBox.Right && _velocity.Y < 0)
            {
                // WRITE to console that Right side collision has occurred:
                System.Console.WriteLine("Bottom Collision");

                // SET _entity.Position.Y value to bottom of collidable object:
                _entity.Position = new Vector2(_entity.Position.X, scndCollidable.HitBox.Bottom);
            }
        }

        #endregion
    }
}
