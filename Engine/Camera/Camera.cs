using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.Camera.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;

namespace OrbitalEngine.Camera
{
    /// <summary>
    /// Class which creates a camera to follow a user based entity
    /// Authors: William Smith, Declan Kerby-Collins & 'axlemke'
    /// Date: 11/04/22
    /// </summary>
    /// <REFERENCE> axlemke (2014) XNA 2D Camera, zoom into player. Available at: https://gamedev.stackexchange.com/questions/68978/xna-2d-camera-zoom-into-player. (Accessed: 20 April 2021). </REFERENCE>
    public class Camera : UpdatableEntity, ICamera, IContainBoundary, IZoom
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2, name it '_drawOrigin':
        private Vector2 _drawOrigin;

        // DECLARE a float, name it '_zoom':
        private float _zoom;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Camera
        /// </summary>
        public Camera()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ICAMERA

        /// <summary>
        /// Changes Positional values of Camera
        /// </summary>
        /// <param name="pPosition"> Incoming Position value </param>
        /// <param name="pCenteringValue"> Incoming Position value </param>
        /// <CITATION> (axlemke, 2014) </CITATION>
        public void ChangeCamPos(Vector2 pPosition, Vector2 pCenteringValue)
        {
            // SET value of _position to value of pPosition:
            _position = Vector2.Lerp(_position, (pPosition * _zoom) + new Vector2(pCenteringValue.X / _zoom, pCenteringValue.Y / _zoom), 0.1f);
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _position / _zoom, null, Color.White, 0, _drawOrigin, 1f, SpriteEffects.None, 1);
        }

        #endregion


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows read or write access to visible texture
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

                // IF _textureSize HAS NOT been set already:
                if (_textureSize.X == 0 && _textureSize.Y == 0)
                {
                    // INSTANTIATE _textureSize as a new Point(), using _texture's dimensions as parameters:
                    _textureSize = new Point(_texture.Width, _texture.Height);

                    // INSTANTIATE _drawOrigin as a new Vector2(), using _textureSize's dimensions as parameters:
                    _drawOrigin = new Vector2(_textureSize.X / 2, _textureSize.Y / 2);
                }
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
            // CALL _currentState on Update(), passing pGameTime as a parameter:
            (_currentState as IUpdatable).Update(pGameTime);
        }

        #endregion


        #region IMPLEMENTATION OF IZOOM

        /// <summary>
        /// Property which can set view zoom value
        /// </summary>
        public float Zoom
        {
            get
            {
                // RETURN value of _zoom:
                return _zoom;
            }
            set
            {
                // SET value of _zoom to incoming value:
                _zoom = value;
            }
        }

        #endregion
    }
}
