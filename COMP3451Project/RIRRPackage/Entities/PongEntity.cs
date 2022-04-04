using Microsoft.Xna.Framework;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Class which contains variables and methods for Pong entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 04/04/22
    /// </summary>
    public abstract class PongEntity : UpdatableEntity, IRotation, IVelocity
    {
        #region FIELD VARIABLES

        // DECLARE a Rectangle, name it '_sourceRectangle':
        protected Rectangle _sourceRectangle;

        // DECLARE a Vector2, name it '_drawOrigin':
        protected Vector2 _drawOrigin;

        // DECLARE a Vector2, name it '_velocity':
        protected Vector2 _velocity;

        // DECLARE a float, name it '_rotAngle':
        protected float _rotAngle;

        // DECLARE an int, name it '_speed':
        protected int _speed;

        #endregion


        #region IMPLEMENTATION OF IROTATION

        /// <summary>
        /// Property which allows read and write access to the point a texture is drawn
        /// </summary>
        public Vector2 DrawOrigin
        {
            get
            {
                // RETURN value of _drawOrigin:
                return _drawOrigin;
            }
            set
            {
                // SET value of _drawOrigin to incoming value:
                _drawOrigin = value;
            }
        }

        /// <summary>
        /// Property which allows read and write access to how much a texture is rotated
        /// </summary>
        public float Angle
        {
            get
            {
                // RETURN value of _rotAngle:
                return _rotAngle;
            }
            set
            {
                // SET value of _rotAngle to incoming value:
                _rotAngle = value;
            }
        }

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the value of an entity's velocity
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                // RETURN value of _velocity:
                return _velocity;
            }
            set
            {
                // SET value of _velocity to incoming value:
                _velocity = value;
            } 
        }

        #endregion 
    }
}
