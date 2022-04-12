using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.States.Interfaces;
using COMP3451Project.RIRRPackage.Entities.Interfaces;
using COMP3451Project.RIRRPackage.Interfaces;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds a Player entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public class Player : RIRREntity, IPlayer, IChangeTexColour, ICollidable, ICollisionListener, IDrawSourceRectangle, IGetSpeed, IHaveHealth, IHaveObjective, IKeyboardListener
    {
        #region FIELD VARIABLES

        // DECLARE a PlayerIndex and name it '_playerNum':
        private PlayerIndex _playerNum;

        // DECLARE a Color, name it '_texColour':
        private Color _texColour;

        // DECLARE a bool, name it '_objectiveComplete':
        private bool _objectiveComplete;

        // DECLARE an int, name it '_maxHealthPoints':
        private int _maxHealthPoints;

        // DECLARE an int, name it '_healthPoints':
        private int _healthPoints;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Player
        /// </summary>
        public Player()
        {
            // INITIALISE _texColour with value of Color.White:
            _texColour = Color.White;

            // SET _objectiveComplete to false:
            _objectiveComplete = false;

            // INITIALISE _speed with a value of '1':
            _speed = 1;

            // INITIALISE maxHealthPoints with a value of '3':
            _maxHealthPoints = 3;

            // INITIALISE _healthPoints with value of _maxHealthPoints:
            _healthPoints = _maxHealthPoints;
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


        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to a texture colour
        /// </summary>
        public Color TexColour
        {
            get
            {
                // RETURN value of _texColour:
                return _texColour;
            }
            set
            {
                // SET value of _texColour to incoming value:
                _texColour = value;
            }
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


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable"> Other entity implementing ICollidable </param>
        public void OnCollision(ICollidable pScndCollidable)
        {
            // CALL OnCollision() on _currentState, passing pScndCollidable as a parameter:
            (_currentState as ICollisionListener).OnCollision(pScndCollidable);
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
            pSpriteBatch.Draw(_texture, _position, _sourceRectangle, _texColour, _rotAngle, _drawOrigin, 1f, SpriteEffects.None, 1f);
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


        #region IMPLEMENTATION OF IHAVEHEALTH

        /// <summary>
        /// Property which has read and write access to an implementation's health points
        /// </summary>
        public int HealthPoints
        {
            get
            {
                // RETURN value of _healthPoints:
                return _healthPoints;
            }
            set
            {
                // SET value of _healthPoints to incoming value:
                _healthPoints = value;
            }
        }

        /// <summary>
        /// Property which has read and write access to an implementation's maximum health points
        /// </summary>
        public int MaxHealthPoints
        {
            get
            {
                // RETURN value of _maxHealthPoints:
                return _maxHealthPoints;
            }
            set
            {
                // SET value of _maxHealthPoints to incoming value:
                _maxHealthPoints = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IHAVEOBJECTIVE

        /// <summary>
        /// Property which allows read and write access to an objective boolean
        /// </summary>
        public bool ObjectiveComplete
        {
            get
            {
                // RETURN value of _objectiveComplete:
                return _objectiveComplete;
            }
            set
            {
                // SET value of _objectiveComplete to incoming value:
                _objectiveComplete = value;
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
    }
}
