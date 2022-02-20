using System;
using System.Collections.Generic;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.States
{
    /// <summary>
    /// Class which contains conditional information for Pong Ball entities to be modified by another class e.g. BallBehaviour
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
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
            // INSTANTATE _triggerDict as a new Dictionary<string, ICommand>():
            _triggerDict = new Dictionary<string, ICommand>();
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable">Other entity implementing ICollidable</param>
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
