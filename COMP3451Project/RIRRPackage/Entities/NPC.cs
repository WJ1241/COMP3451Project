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
    /// Class which adds an NPC entity on screen
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 04/04/22
    /// </summary>
    public class NPC : PongEntity, ICollidable, ICollisionListener, IInitialiseParam<Random>
    {
        #region FIELD VARIABLES

        // DECLARE a Random, name it '_rand':
        private Random _rand;

        // DECLARE an int, name it '_randNum':
        private int _randNum;

        // DECLARE a Vector2 and name it 'direction':
        private Vector2 _direction;


        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of NPC
        /// </summary>
        public NPC()
        {
            // EMPTY CONSTRUCTOR
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
    }
}
