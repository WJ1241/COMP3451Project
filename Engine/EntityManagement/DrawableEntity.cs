using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.CoreInterfaces;

namespace OrbitalEngine.EntityManagement
{
    /// <summary>
    /// Abstract Class which details drawing logic for more specific entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public abstract class DrawableEntity : Entity, IDraw, ITexture
    {
        #region FIELD VARIABLES

        // DECLARE a Texture2D, name it '_texture':
        protected Texture2D _texture;

        // DECLARE a Texture2D, name it '_textureSize':
        protected Point _textureSize;


        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of DrawableEntity
        /// </summary>
        public DrawableEntity()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _position, Color.AntiqueWhite);
        }

        #endregion


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows read or write access to visible texture
        /// </summary>
        public virtual Texture2D Texture
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

        /// <summary>
        /// Property which allows read or write access to size of texture, mostly used for testing
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
    }
}
