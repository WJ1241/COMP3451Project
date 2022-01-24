using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.InputManagement;


namespace COMP3451Project.PongPackage.EntityClasses
{
    /// <summary>
    /// Class which adds a Paddle entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
    /// </summary>
    public class Paddle : PongEntity, IPlayer, ICollidable, IKeyboardListener
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2 and call it '_direction':
        private Vector2 _direction;

        // DECLARE a PlayerIndex and call it '_playerNum':
        private PlayerIndex _playerNum;

        // DECLARE an IDictionary<string, EventHandler<UpdateEventArgs>>, name it '_behaviourEvents':
        private IDictionary<string, EventHandler<UpdateEventArgs>> _behaviourEvents;

        // DECLARE a string, name it '_activeBehaviour':
        private string _activeBehaviour;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Paddle
        /// </summary>
        public Paddle()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Initialises entity variable values
        /// </summary>
        public override void Initialise()
        {
            // INSTANTIATE _behaviourEvents as a new Dictionary<string, EventHandler<UpdateEventArgs>>():
            _behaviourEvents = new Dictionary<string, EventHandler<UpdateEventArgs>>();

            // ASSIGNMENT, set _speed to 10:
            _speed = 10;

            // ASSIGNMENT, set _activeBehaviour to "stationary":
            _activeBehaviour = "stationary";
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDLISTENER

        /// <summary>
        /// Called when Publisher has new Keyboard input information for listening objects
        /// </summary>
        /// <param name="pKeyboardState">Holds reference to Keyboard State object</param>
        public void OnKBInput(KeyboardState pKeyboardState)
        {
            // SET value of _activeBehaviour to "stationary":
            _activeBehaviour = "stationary";

            // INSTANTIATE new Vector2, set as 0 to stop movement:
            //_direction = new Vector2(0);

            // IF Player 1:
            if (_playerNum == PlayerIndex.One)
            {
                // IF W Key down
                if (pKeyboardState.IsKeyDown(Keys.W))
                {
                    // SET _activeBehaviour to "up":
                    _activeBehaviour = "up";

                    // ASSIGN direction.Y, give value of -1:
                    //_direction.Y = -1;
                }

                // ELSE IF S Key down 
                else if (pKeyboardState.IsKeyDown(Keys.S))
                {
                    // SET _activeBehaviour to "down":
                    _activeBehaviour = "down";

                    // ASSIGN direction.Y, give value of 1:
                    //_direction.Y = 1;
                }
            }

            // IF Player 2:
            else if (_playerNum == PlayerIndex.Two)
            {
                // IF Up Arrow Key down:
                if (pKeyboardState.IsKeyDown(Keys.Up))
                {
                    // SET _activeBehaviour to "up":
                    _activeBehaviour = "up";

                    // ASSIGN direction.Y, give value of -1:
                    //_direction.Y = -1;
                }

                // ELSE IF Down Arrow Key down:
                else if (pKeyboardState.IsKeyDown(Keys.Down))
                {
                    // SET _activeBehaviour to "down":
                    _activeBehaviour = "down";

                    // ASSIGN direction.Y, give value of 1:
                    //_direction.Y = 1;
                }
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> Holds reference to GameTime object </param>
        public override void Update(GameTime pGameTime)
        {
            // ASSIGNMENT, set value of _velocity to _speed mutlipled by _direction:
            _velocity = _speed * _direction;

            // DECLARE & INITIALISE an UpdateEventArgs, name it '_tempUpdateEA':
            UpdateEventArgs _tempUpdateEA = new UpdateEventArgs();

            // SET RequiredArg Property's value to pGameTime:
            _tempUpdateEA.RequiredArg = pGameTime;

            // CALL Invoke on _behaviourEvents[_activeBehaviour], passing this class and _tempUpdateEA as parameters:
            _behaviourEvents[_activeBehaviour]?.Invoke(this, _tempUpdateEA);

            /*
            // ADD & ASSIGN _velocity to _position:
            _position += _velocity;

            // CALL Boundary():
            Boundary();*/
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


        #region IMPLEMENTATION OF ICOLLIDABLE

        /// <summary>
        /// Used to Return a rectangle object to caller of property
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                // RETURN a rectangle, object current X axis location, object current Y axis location, texture width size, texture height size:
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
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


