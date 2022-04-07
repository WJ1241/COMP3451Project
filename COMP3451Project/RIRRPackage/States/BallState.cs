using System;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.States;

namespace COMP3451Project.RIRRPackage.States
{
    /// <summary>
    /// Class which contains conditional information for Pong Ball entities to be modified by another class e.g. BallBehaviour
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public class BallState : State, ICollisionListener
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_collisionEvent':
        private EventHandler<CollisionEventArgs> _collisionEvent;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of BallState
        /// </summary>
        public BallState()
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public override void Initialise(IUpdateEventListener pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnUpdateEvent():
                _behaviourEvent += pUpdateEventListener.OnUpdateEvent;

                // SUBSCRIBE _collisionEvent to pUpdateEventListener.OnCollisionEvent():
                _collisionEvent += (pUpdateEventListener as ICollisionEventListener).OnCollisionEvent;
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance");
            }
        }

        #endregion
    }
}
