using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.EntityClasses
{

    /// <summary>
    /// Class which adds a Ball entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
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

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_behaviourEvent':
        private EventHandler<UpdateEventArgs> _behaviourEvent;

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_collisionEvent':
        private EventHandler<CollisionEventArgs> _collisionEvent;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Ball
        /// </summary>
        public Ball()
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
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public override void Update(GameTime pGameTime)
        {
            // DECLARE & INITIALISE an UpdateEventArgs, name it '_tempUpdateEA':
            UpdateEventArgs _tempUpdateEA = new UpdateEventArgs();

            // SET RequiredArg Property's value to pGameTime:
            _tempUpdateEA.RequiredArg = pGameTime;

            // CALL Invoke on _behaviourEvent, passing this class and _tempUpdateEA as parameters:
            _behaviourEvent.Invoke(this, _tempUpdateEA);
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
        /// <param name="pScndCollidable"> Other entity implementing ICollidable </param>
        public void OnCollision(ICollidable pScndCollidable)
        {
            // DECLARE & INITIALISE an CollisionEventArgs, name it '_tempCollisionEA':
            CollisionEventArgs _tempCollisionEA = new CollisionEventArgs();

            // SET RequiredArg Property's instance to pScndCollidable:
            _tempCollisionEA.RequiredArg = pScndCollidable;

            // CALL Invoke on _collisionEvent, passing this class and _tempCollisionEA as parameters:
            _collisionEvent.Invoke(this, _tempCollisionEA);
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