using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.States.Interfaces;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds a Paddle entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
    /// </summary>
    public class Paddle : PongEntity, IPlayer, ICollidable, IDrawSourceRectangle, IGetSpeed, IKeyboardListener
    {
        #region FIELD VARIABLES

        // DECLARE a PlayerIndex and name it '_playerNum':
        private PlayerIndex _playerNum;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Paddle
        /// </summary>
        public Paddle()
        {
            // ASSIGNMENT, set _speed to 10:
            _speed = 10;
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLIDABLE

        /// <summary>
        /// Used to Return a rectangle object to caller of property
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                // RETURN new Rectangle() using _position and _textureSize as parameters:
                return new Rectangle((int)_position.X - (int)_drawOrigin.X, (int)_position.Y - (int)_drawOrigin.Y, _textureSize.X, _textureSize.Y);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, _sourceRectangle, colour, rotation angle, origin point, scale, effects and draw layer:
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


        #region IMPLEMENTATION OF IGETSPEED

        /// <summary>
        /// Property which allows read and write access to the value of an entity's Speed
        /// </summary>
        public float GetSpeed
        {
            get
            {
                // RETURN value of speed:
                return _speed;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ISTATE>

        /// <summary>
        /// Initialises an object with a reference to an IState instance
        /// </summary>
        /// <param name="pState"> IState instance </param>
        public override void Initialise(IState pState)
        {
            // IF pState DOES HAVE an active instance:
            if (pState != null)
            {
                // INITIALISE _currentState with instance of pState:
                _currentState = pState;

                // SET PlayerNum Property of _currentState to value of _playerNum:
                (_currentState as IPlayer).PlayerNum = _playerNum;
            }
            // IF pState DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pState does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDLISTENER

        /// <summary>
        /// Called when Publisher has new Keyboard input information for listening objects
        /// </summary>
        /// <param name="pKeyboardState">Holds reference to Keyboard State object</param>
        public void OnKBInput(KeyboardState pKeyboardState)
        {
            // CALL OnKBInput on _currentState, passing pKeyboardState as a parameter:
            (_currentState as IKeyboardListener).OnKBInput(pKeyboardState);
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> Holds reference to GameTime object </param>
        public override void Update(GameTime pGameTime)
        {
            // CALL Update() on _currentState, passing pGameTime as a parameter:
            (_currentState as IUpdatable).Update(pGameTime);
        }

        #endregion


        #region IMPLEMENTATION OF IPLAYER

        /// <summary>
        /// Property which can set value of a PlayerIndex
        /// </summary>
        public PlayerIndex PlayerNum
        {
            set
            {
                // SET value of _playerNum to incoming value:
                _playerNum = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ITERMINATE

        /// <summary>
        /// Disposes resources to the garbage collector
        /// </summary>
        public override void Terminate()
        {
            // No functionality, MonoGame deals with object and texture in garbage collector already
        }

        #endregion
    }
}