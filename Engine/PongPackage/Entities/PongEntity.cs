using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Abstract class for Pong Entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
    /// </summary>
    public abstract class PongEntity : UpdatableEntity, IDrawSourceRectangle, IRotation, IVelocity
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
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _position, _sourceRectangle, Color.AntiqueWhite, _rotAngle, _drawOrigin, 1f, SpriteEffects.None, 1f);
        }

        #endregion


        #region IMPLEMENTATION OF IDRAWSOURCERECTANGLE

        /// <summary>
        /// Property which allows read and write access to a Draw Position Rectangle
        /// </summary>
        public Rectangle SourceRectangle
        {
            get
            {
                // RETURN value of _sourceRectangle:
                return _sourceRectangle;
            }
            set
            {
                // SET value of _sourceRectangle to incoming value:
                _sourceRectangle = value;
            }
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


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows access to get or set value of 'texture'
        /// </summary>
        public override Texture2D Texture
        {
            get
            {
                // RETURN value of current texture:
                return _texture;
            }
            set
            {
                // INITIALISE _texture with incoming value:
                _texture = value;

                // INSTANTIATE _textureSize as a new Point, using _texture's dimensions as parameters:
                _textureSize = new Point(_texture.Width, _texture.Height);
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
