using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds a Ball entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public class Ball : RIRREntity, ICollidable, ICollisionListener, IInitialiseParam<Random>, IReset
    {
        #region FIELD VARIABLES

        // DECLARE a Random, name it '_rand':
        private Random _rand;

        // DECLARE an int, name it '_randNum':
        private int _randNum;

        // DECLARE a Vector2 and name it 'direction':
        private Vector2 _direction;

        // DECLARE an float, name it '_rotation':
        private float _rotation;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Ball
        /// </summary>
        public Ball()
        {
            // INSTANTIATE new Vector, value of 1 for both X and Y:
            _direction = new Vector2(1);

            // ASSIGNMENT, set _speed to 8:
            _speed = 8;

            // ASSIGNMENT, set value of _velocity to _speed mutlipled by _direction:
            _velocity = _speed * _direction;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IEVENTLISTENER<UPDATEEVENTARGS>>

        /// <summary>
        /// Initialises an object with an IEventListener<UpdateEventArgs> object
        /// </summary>
        /// <param name="pUpdateEventListener"> IEventListener<UpdateEventArgs> object </param>
        public override void Initialise(IEventListener<UpdateEventArgs> pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // SET value of _pUpdateEventListener's Velocity Property to value of _velocity:
                (pUpdateEventListener as IVelocity).Velocity = _velocity;

                // INITIALISE pUpdateEventListener with this class:
                (pUpdateEventListener as IInitialiseParam<IEntity>).Initialise(this);
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<RANDOM>

        /// <summary>
        /// Initialises an object with a Random object
        /// </summary>
        /// <param name="pRand"> Random object </param>
        public void Initialise(Random pRand)
        {
            // IF pRand DOES HAVE an active instance:
            if (pRand != null)
            {
                // INITIALISE _rand with reference to pRand:
                _rand = pRand;
            }
            // IF pRand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("pRand does not have an active instance!");
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
            // CALL Update() on _currentState, passing pGameTime as a parameter:
            (_currentState as IUpdatable).Update(pGameTime);
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
        /// Resets an object's initial values
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

            // IF Random number is 2
            if (_randNum == 2)
            {
                // REVERSE velocity.X:
                _velocity.X *= -1;
            }

            // MULTIPLY & ASSIGN _velocity by _speed:
            _velocity *= _speed;
        }

        #endregion
    }
}