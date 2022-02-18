using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.Behaviours.Interfaces;
using COMP3451Project.EnginePackage.CollisionManagement.Interfaces;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement.Interfaces;
using COMP3451Project.EnginePackage.InputManagement.Interfaces;
using COMP3451Project.EnginePackage.States.Interfaces;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Class which adds a Paddle entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public class Paddle : PongEntity, IPlayer, ICollidable, IGetSpeed, IKeyboardListener
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
                // RETURN a rectangle, object current X axis location, object current Y axis location, texture width size, texture height size:
                return new Rectangle((int)_position.X, (int)_position.Y, _textureSize.X, _textureSize.Y);
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
                // RETURN value of speed;
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
            // INITIALISE _currentState with instance of pState:
            _currentState = pState;

            // SET PlayerNum Property of _currentState to value of _playerNum:
            (_currentState as IPlayer).PlayerNum = _playerNum;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with a reference to an IUpdateEventListener instance
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public override void Initialise(IUpdateEventListener pUpdateEventListener)
        {
            // INITIALISE pUpdateEventListener with this class:
            (pUpdateEventListener as IInitialiseParam<IEntity>).Initialise(this);
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