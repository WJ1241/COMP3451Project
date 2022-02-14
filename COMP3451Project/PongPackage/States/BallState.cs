using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.Services.Commands;

namespace COMP3451Project.EnginePackage.States
{
    /// <summary>
    /// Class which contains conditional information for Pong Ball entities to be modified by another class e.g. BallBehaviour
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public class BallState : State, ICollisionListener, IInitialiseParam<IUpdateEventListener>
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
            // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnUpdateEvent():
            _behaviourEvent += pUpdateEventListener.OnUpdateEvent;

            // SUBSCRIBE _collisionEvent to pUpdateEventListener.OnCollisionEvent():
            _collisionEvent += (pUpdateEventListener as ICollisionEventListener).OnCollisionEvent;
        }

        #endregion
    }
}
