using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Abstract class for Pong Entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public abstract class PongEntity : Entity, IInitialiseParam<IUpdateEventListener>, IDraw, IRotation, ITexture, IUpdatable, IVelocity
    {
        #region FIELD VARIABLES

        // DECLARE a Texture2D, name it '_texture':
        protected Texture2D _texture;

        // DECLARE a Texture2D, name it '_textureSize':
        protected Point _textureSize;

        // DECLARE a Vector2, name it '_drawOrigin':
        protected Vector2 _drawOrigin;

        // DECLARE a Vector2, name it '_velocity':
        protected Vector2 _velocity;

        // DECLARE a float, name it '_rotAngle':
        protected float _rotAngle;

        // DECLARE a float, name it 'speed':
        protected float _speed;

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public abstract void Initialise(IUpdateEventListener pUpdateEventListener);

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpritebatch"> Needed to draw entity's texture on screen </param>
        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _position, Color.AntiqueWhite);
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
        public Texture2D Texture
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

                // INSTANTIATE _drawOrigin as a new Vector2, with value of centre of _texture:
                _drawOrigin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            }
        }

        /// <summary>
        /// Property which allows read or write access to size of texture
        /// </summary>
        public Point TextureSize
        {  
            get
            {
                // RETURN value of _textureSize:
                return _textureSize;
            }
            set
            {
                // SET value of _textureSize to incoming value:
                _textureSize = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public abstract void Update(GameTime pGameTime);

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
