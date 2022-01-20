using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.InputManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.EntityClasses
{
    /// <summary>
    /// Class which adds a Paddle entity on screen
    /// Authors: Will Smith & Declan Kerby-Collins
    /// Date: 20/01/2022
    /// </summary>
    public class Paddle : PongEntity, IPlayer, ICollidable, IKeyboardListener
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2 and call it '_direction':
        private Vector2 _direction;

        // DECLARE a PlayerIndex and call it '_playerNum':
        private PlayerIndex _playerNum;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Paddle
        /// </summary>
        public Paddle()
        {

        }

        #endregion


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Initialises entity variable values
        /// </summary>
        public override void Initialise()
        {
            // SET _selfDestruct to false:
            _selfDestruct = false;

            // ASSIGNMENT, set _speed to 10:
            _speed = 10;
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDLISTENER

        /// <summary>
        /// Called when Publisher has new Keyboard input information for listening objects
        /// </summary>
        /// <param name="keyboardState">Holds reference to Keyboard State object</param>
        public void OnKBInput(KeyboardState keyboardState)
        {
            // INSTANTIATE new Vector2, set as 0 to stop movement:
            _direction = new Vector2(0);

            if (_playerNum == PlayerIndex.One)
            {
                if (keyboardState.IsKeyDown(Keys.W))
                {
                    // ASSIGN direction.Y, give value of -1:
                    _direction.Y = -1;
                }
                else if (keyboardState.IsKeyDown(Keys.S))
                {
                    // ASSIGN direction.Y, give value of 1:
                    _direction.Y = 1;
                }
            }
            else if (_playerNum == PlayerIndex.Two)
            {
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    // ASSIGN direction.Y, give value of -1:
                    _direction.Y = -1;
                }
                else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    // ASSIGN direction.Y, give value of 1:
                    _direction.Y = 1;
                }
            }

        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        public override void Update(GameTime gameTime)
        {
            // ASSIGNMENT, set value of _velocity to _speed mutlipled by _direction:
            _velocity = _speed * _direction;

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
                // ASSIGNMENT give _playerNum value of whichever class is modifying value
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


        #region PROTECTED METHODS

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected void Boundary()
        {
            if (_position.Y <= 0) // IF paddle at top of screen
            {
                // ASSIGNMENT, set _position.Y to 0:
                _position.Y = 0; // Keeps at top of screen
            }
            else if (_position.Y >= (_windowBorder.Y - _texture.Height)) // IF paddle at bottom of screen
            {
                // ASSIGNMENT, set _position.Y to _windowBorder.Y - _texture.Height:
                _position.Y = _windowBorder.Y - _texture.Height; // Keeps at bottom of screen
            }
        }

        #endregion

    }
}


