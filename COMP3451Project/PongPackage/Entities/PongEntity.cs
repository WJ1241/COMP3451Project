using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Abstract class for Pong Entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
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

        // DECLARE a float, name it 'speed':
        protected float _speed;

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, colour, rotation angle, origin point, scale, effects and draw layer:
            pSpriteBatch.Draw(_texture, _position, null, Color.AntiqueWhite, _rotAngle, _drawOrigin, 1f, SpriteEffects.None, 1f);
        }

        #endregion


        #region IMPLEMENTATION OF IROTATION

        /// <summary>
        /// Property which allows only read access to the point a texture is drawn
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


        #region IMPLEMENTATION OF IVELOCITY

        /// <summary>
        /// Property which allows access to get value of an entity's velocity
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                // RETURN value of _velocity(x,y)
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
