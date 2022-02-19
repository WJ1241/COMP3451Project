using Microsoft.Xna.Framework;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;

namespace COMP3451Project.PongPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Paddle entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 15/02/22
    /// </summary>
    public class PaddleBehaviour : PongBehaviour, IDirection
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2, name it '_direction':
        private Vector2 _direction;

        #endregion


        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {
            // IF Paddle at top of screen:
            if (_entity.Position.Y - (_entity as IRotation).DrawOrigin.Y <= 0)
            {
                // ASSIGNMENT, set _position.Y to _entity's Origin Point, keeps at top of screen:
                _entity.Position = new Vector2(_entity.Position.X, (_entity as IRotation).DrawOrigin.Y);
            }

            // IF Paddle at bottom of screen:
            else if (_entity.Position.Y + (_entity as IRotation).DrawOrigin.Y >= (_entity as IContainBoundary).WindowBorder.Y)
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


        #region IMPLEMENTATION OF IUPDATEEVENTLISTENER

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnUpdateEvent(object pSource, UpdateEventArgs pArgs)
        {
            // ASSIGNMENT, set value of _velocity to _speed mutlipled by _direction:
            _velocity = (_entity as IGetSpeed).GetSpeed * _direction;

            // SET value of _entity's Velocity Property to value of _velocity:
            (_entity as IVelocity).Velocity = _velocity;

            // ADD & APPLY velocity to current position:
            _entity.Position += _velocity;

            // CALL Boundary() method:
            Boundary();
        }

        #endregion
    }
}
