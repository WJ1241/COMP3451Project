using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.Camera.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;

namespace OrbitalEngine.Camera
{
    /// <summary>
    /// Class which creates a camera to follow a user based entity
    /// Authors: William Smith, Declan Kerby-Collins & 'axlemke'
    /// Date: 10/04/22
    /// </summary>
    /// <REFERENCE> axlemke (2014) XNA 2D Camera, zoom into player. Available at: https://gamedev.stackexchange.com/questions/68978/xna-2d-camera-zoom-into-player. (Accessed: 20 April 2021). </REFERENCE>
    public class Camera : ICamera, IContainBoundary, IDraw, IEntity, IInitialiseParam<EventHandler<MatrixEventArgs>>, IInitialiseParam<MatrixEventArgs>, ILayer, ITerminate, ITexture, IZoom
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<MatrixEventArgs>, name it '_camPosChangeEvent':
        private EventHandler<MatrixEventArgs> _camPosChangeEvent;

        // DECLARE a MatrixEventArgs, name it '_matrixArgs':
        private MatrixEventArgs _matrixArgs;

        // DECLARE a Texture2D, name it '_texture':
        private Texture2D _texture;

        // DECLARE a Matrix, name it '_camTransform':
        private Matrix _camTransform;

        // DECLARE a Vector2, name it '_camPos':
        private Vector2 _position;

        // DECLARE a Vector2, name it '_drawOrigin':
        private Vector2 _drawOrigin;

        // DECLARE a Point, name it '_textureSize':
        private Point _textureSize;

        // DECLARE a Point, name it '_windowSize':
        private Point _windowSize;

        // DECLARE a string, name it '_uName':
        private string _uName;

        // DECLARE a float, name it '_zoom':
        private float _zoom;

        // DECLARE an int, name it '_uID':
        private int _uID;

        // DECLARE an int, name it '_layer':
        private int _layer;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Camera
        /// </summary>
        public Camera()
        {
            // ASSIGNMENT, set value of _position to '0' on both X and Y axis:
            _position = Vector2.Zero;
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
            _position = Vector2.Lerp(_position, (pPosition * _zoom) + new Vector2(pCenteringValue.X / _zoom, pCenteringValue.Y / _zoom), 0.15f);

            // ASSIGNMENT, set value of _camTransform to zoom in using _zoom value, and position in centre of screen. Scale is changed first before Translation, does not work other way around, matrix rule ISROT applies:
            _camTransform = // SET scale using zoom, on X,Y axes:
                            Matrix.CreateScale(_zoom, _zoom, 1f)

                          // SET value of _camTransform to a new Vector3, passing parameters of the camera user's X,Y position, excluding Z as game operates in 2D:
                          * Matrix.CreateTranslation(new Vector3(-_position.X + (_windowSize.X / 2), -_position.Y + (_windowSize.Y / 2), 0));

            // INITIALISE _matrixArgs with value of _camTransform:
            _matrixArgs.RequiredArg = _camTransform;

            // INVOKE _camPosChangeEvent(), passing this class and _matrixArgs as parameters:
            _camPosChangeEvent.Invoke(this, _matrixArgs);
        }

        #endregion


        #region IMPLEMENTATION OF ICONTAINBOUNDARY

        /// <summary>
        /// Property which can set value of screen window borders
        /// </summary>
        public Point WindowBorder
        {
            get
            {
                // RETURN value of _windowSize:
                return _windowSize;
            }
            set
            {
                // SET value of _windowSize to incoming value:
                _windowSize = value;
            }
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
            pSpriteBatch.Draw(_texture, _position / _zoom, null, Color.AntiqueWhite, 0, _drawOrigin, 1f, SpriteEffects.None, 1);
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

        /// <summary>
        /// /// Property which allows read or write access to size of texture, mostly used for testing, as well as setting hitbox
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


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Property which can get and set value of an entity's position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // RETURN value of current location(x,y):
                return _position;
            }
            set
            {
                // ASSIGNMENT give location(x,y) value of external class modified value:
                _position = value;
            }
        }

        /// <summary>
        /// Property which can get and set value of an entity's unique ID
        /// </summary>
        public int UID
        {
            get
            {
                // RETURN value of current _uID:
                return _uID;
            }
            set
            {
                // ASSIGNMENT give _uID value of external class modified value:
                _uID = value;
            }
        }

        /// <summary>
        /// Property which can get and set value of an entity's unique Name
        /// </summary>
        public string UName
        {
            get
            {
                // RETURN value of current _uName:
                return _uName;
            }
            set
            {
                // ASSIGNMENT give _uName value of external class modified value:
                _uName = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<EVENTHANDLER<MATRIXEVENTARGS>>

        /// <summary>
        /// Initialises an object with an EventHandler<MatrixEventArgs> reference
        /// </summary>
        /// <param name="pEventHandler"> EventHandler<MatrixEventArgs> reference </param>
        public void Initialise(EventHandler<MatrixEventArgs> pEventHandler)
        {
            // IF pEventHandler DOES HAVE a valid method reference:
            if (pEventHandler != null)
            {
                // INITIALISE _camPosChangeEvent with reference to pEventHandler:
                _camPosChangeEvent = pEventHandler;
            }
            // IF pEventHandler DOES NOT HAVE a valid method reference:
            else
            {
                // THROW a new NullReferenceException(), with corresponding message:
                throw new NullReferenceException("ERROR: pEventHandler does not have a valid method reference!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<MATRIXEVENTARGS>

        /// <summary>
        /// Initialises an object with a MatrixEventArgs instance
        /// </summary>
        /// <param name="pArgs"> MatrixEventArgs instance </param>
        public void Initialise(MatrixEventArgs pArgs)
        {
            // IF pArgs DOES HAVE an active instance:
            if (pArgs != null)
            {
                // INITIALISE _matrixArgs with reference to pArgs:
                _matrixArgs = pArgs;
            }
            // IF pArgs DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullReferenceException(), with corresponding message:
                throw new NullReferenceException("ERROR: pArgs does not have a valid method reference!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF ILAYER

        /// <summary>
        /// Property which can get and set layer value
        /// Layer 1: Floors
        /// Layer 2: Walls
        /// Layer 3: Static Obstacles
        /// Layer 4: Items
        /// Layer 5: Level Change
        /// Layer 6: Player/NPC
        /// Layer 7: GUI
        /// </summary>
        public int Layer
        {
            get
            {
                // RETURN value of _layer:
                return _layer;
            }
            set
            {
                // SET value of _layer to incoming value:
                _layer = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ITERMINATE

        /// <summary>
        /// Disposes resources to the garbage collector
        /// </summary>
        public void Terminate()
        {

        }

        #endregion


        #region IMPLEMENTATION OF IZOOM

        /// <summary>
        /// Property which can set view zoom value
        /// </summary>
        public float Zoom
        {
            set
            {
                // SET value of _zoom to incoming value:
                _zoom = value;
            }
        }

        #endregion
    }
}
