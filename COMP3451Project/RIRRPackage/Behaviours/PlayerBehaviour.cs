using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Player entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 09/04/22
    /// </summary>
    public class PlayerBehaviour : PongBehaviour, IDirection
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
                // ASSIGNMENT, set _position.Y to _entity's Origin Point, keeps at top of screen:
                _entity.Position = new Vector2(_entity.Position.X, (_entity as IRotation).DrawOrigin.Y);
            }

            // IF Paddle at bottom of screen:
            if (_entity.Position.Y + (_entity as IRotation).DrawOrigin.Y >= (_entity as IContainBoundary).WindowBorder.Y)
            {
                // ASSIGNMENT, set _position.Y to _windowBorder.Y - _textureSize.Y, keeps at bottom of screen:
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
            //UpdateFollowEntity();
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
            (_updatePosCommand as ICommandTwoParam<Vector2, Vector2>).SecondParam = new Vector2((_entity as ITexture).TextureSize.X, (_entity as ITexture).TextureSize.Y);

            // SCHEDULE _updatePosCommand to be executed:
            (_entity as ICommandSender).ScheduleCommand(_updatePosCommand);
        }

        #endregion
    }
}
