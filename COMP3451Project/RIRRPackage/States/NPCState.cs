using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.States;

namespace COMP3451Project.RIRRPackage.States
{
    /// <summary>
    /// Class which contains conditional information for RIRR NPC entities to be modified by another class e.g. NPCBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public class NPCState: State, ICollisionListener
    {
        // DECLARE an EventHandler<UpdateEventArgs>, name it '_animationEvent':
        private EventHandler<UpdateEventArgs> _animationEvent;

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_collisionEvent':
        private EventHandler<CollisionEventArgs> _collisionEvent;

        // DECLARE a string, name it '_activeBehaviour':
        private string _activeBehaviour;

        /// <summary>
        /// Constructor for objects of NPCState:
        /// </summary>
        public NPCState()
        {
            // empty constructor
        }

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
                // IF pUpdateEventListener DOES implement IAnimation:
                if (pUpdateEventListener is IAnimation)
                {
                    // SUBSCRIBE _animationEvent to pUpdateEventListener.OnUpdateEvent():
                    _animationEvent += pUpdateEventListener.OnUpdateEvent;
                }
                // IF pUpdateEventListener DOES NOT implement IAnimation:
                else
                {
                    // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnUpdateEvent():
                    _behaviourEvent += pUpdateEventListener.OnUpdateEvent;

                    // SUBSCRIBE _collisionEvent to pUpdateEventListener.OnCollisionEvent():
                    _collisionEvent += (pUpdateEventListener as ICollisionEventListener).OnCollisionEvent;
                }
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance!");
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
            // SET RequiredArg Property value of(_argsDict["UpdateEventArgs"] to reference to pGameTime:
            (_argsDict["UpdateEventArgs"] as UpdateEventArgs).RequiredArg = pGameTime;

            // INVOKE _behaviourEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _behaviourEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);

            // INVOKE _animationEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _animationEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);
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


    }
}
