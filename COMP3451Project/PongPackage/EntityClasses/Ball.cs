using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.EntityClasses
{

    /// <summary>
    /// Class which adds a Ball entity on screen
    /// Authors: Will Smith & Declan Kerby-Collins
    /// Date: 20/01/2022
    /// </summary>
    public class Ball : PongEntity, IInitialiseRand, IReset, ICollidable, ICollisionListener
    {
        #region FIELD VARIABLES

        // DECLARE a Random, call it '_rand':
        private Random _rand;

        // DECLARE an int, call it '_randNum':
        private int _randNum;

        // DECLARE a Vector2 and call it 'direction':
        private Vector2 _direction;

        // DECLARE an float, call it '_rotation':
        private float _rotation;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Ball
        /// </summary>
        public Ball()
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

            // INSTANTIATE new Vector, value of 1 for both X and Y:
            _direction = new Vector2(1);

            // ASSIGNMENT, set _speed to 8:
            _speed = 8;

            // ASSIGNMENT, set value of _velocity to _speed mutlipled by _direction:
            _velocity = _speed * _direction;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISERAND

        /// <summary>
        /// Initialises an object with a Random object
        /// </summary>
        /// <param name="rand">holds reference to a Random object</param>
        public void Initialise(Random rand)
        {
            // ASSIGNMENT, set instance of _rand the same as rand:
            _rand = rand;
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        public override void Update(GameTime gameTime)
        {
            // ADD & ASSIGN _velocity to _position:
            _position += _velocity;

            // CALL Boundary() method:
            Boundary();
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


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="scndCollidable">Other entity implementing ICollidable</param>
        public void OnCollision(ICollidable scndCollidable)
        {
            if (_velocity.X < 0) // IF moving left
            {
                // MINUS & ASSIGN 0.2 multiplied by _scndCollidable's Velocity, to _velocity:
                _velocity.X -= 0.2f * (scndCollidable as IVelocity).Velocity.Length();
            }
            else if (_velocity.X > 0)  // IF moving right
            {
                // ADD & ASSIGN 0.2 multiplied by _scndCollidable's Velocity, to _velocity:
                _velocity.X += 0.2f * (scndCollidable as IVelocity).Velocity.Length();
            }

            // REVERSE _velocity.X:
            _velocity.X *= -1;
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


        #region IMPLEMENTATION OF IRESET

        /// <summary>
        /// Resets an object's positional values
        /// </summary>
        public void Reset()
        {
            // ASSIGN random rotation:
            _rotation = (float)(Math.PI / 2 + (_rand.NextDouble() * (Math.PI / 1.5f) - Math.PI / 3));

            // ASSIGN velocity.X using Sine and _rotation:
            _velocity.X = (float)Math.Sin(_rotation);

            // ASSIGN velocity.Y using Cosine and _rotation:
            _velocity.Y = (float)Math.Cos(_rotation);

            // ASSIGN Random number between 1 and 2, 3 is exclusive:
            _randNum = _rand.Next(1, 3);

            if (_randNum == 2) // IF Random number is 2
            {
                // REVERSE velocity.X:
                _velocity.X *= -1;
            }

            // MULTIPLY & ASSIGN _velocity by _speed:
            _velocity *= _speed;
        }

        #endregion


        #region PROTECTED METHODS

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected void Boundary()
        {
            if (_position.Y <= 0 || _position.Y >= (_windowBorder.Y - _texture.Height)) // IF at top screen edge or bottom screen edge
            {
                // REVERSE _velocity.Y:
                _velocity.Y *= -1;
            }
            else if (_position.X <= 0 || _position.X >= (_windowBorder.X - _texture.Width)) // IF at left screen edge or right screen edge
            {
                // SET _selfDestruct to true:
                _selfDestruct = true;
            }
        }

        #endregion
    }
}