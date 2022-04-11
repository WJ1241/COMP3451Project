using System;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.States
{
    /// <summary>
    /// Class which contains conditional information for RIRR Player entities to be modified by another class e.g. an IEventListener<CollisionEventArgs> implementation
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public class CollidableState : State, ICollisionListener, IInitialiseParam<IEventListener<CollisionEventArgs>>
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_collisionEvent':
        protected EventHandler<CollisionEventArgs> _collisionEvent;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of CollidableState
        /// </summary>
        public CollidableState()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable">Other entity implementing ICollidable</param>
        public void OnCollision(ICollidable pScndCollidable)
        {
            // SET RequiredArg Property value of (_argsDict["CollisionEventArgs"] to reference to pScndCollidable:
            (_argsDict["CollisionEventArgs"] as CollisionEventArgs).RequiredArg = pScndCollidable;

            // INVOKE _collisionEvent(), passing this class and (_argsDict["CollisionEventArgs"] as parameters:
            _collisionEvent.Invoke(this, _argsDict["CollisionEventArgs"] as CollisionEventArgs);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IEVENTLISTENER<COLLISIONEVENTARGS>>

        /// <summary>
        /// Initialises an object with an IEventListener<CollisionEventArgs> object
        /// </summary>
        /// <param name="pCollisionEventListener"> IEventListener<CollisionEventArgs> object </param>
        public virtual void Initialise(IEventListener<CollisionEventArgs> pCollisionEventListener)
        {
            // IF pCollisionEventListener DOES HAVE an active instance:
            if (pCollisionEventListener != null)
            {
                // SUBSCRIBE _collisionEvent to pCollisionEventListener.OnEvent():
                _collisionEvent += pCollisionEventListener.OnEvent;
            }
            // IF pCollisionEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCollisionEventListener does not have an active instance!");
            }
        }

        #endregion
    }
}
