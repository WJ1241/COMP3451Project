using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.EntityManagement
{
    /// <summary>
    /// Class which adds a DynamicCollidable entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public class DynamicCollidableEntity : SimpleCollidableEntity, ICollidable, ICollisionListener, IInitialiseParam<IEventListener<CollisionEventArgs>>
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of DynamicCollidableEntity
        /// </summary>
        public DynamicCollidableEntity()
        {
            // EMPTY CONSTRUCTOR
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
            //(_currentState as ICollisionListener).OnCollision(pScndCollidable);
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
                // INITIALISE pCollisionEventListener with this class:
                (pCollisionEventListener as IInitialiseParam<IEntity>).Initialise(this);
            }
            // IF pCollisionEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCollisionEventListener does not have an active instance");
            }
        }

        #endregion
    }
}
