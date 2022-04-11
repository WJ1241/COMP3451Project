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
    /// Class which contains conditional information for RIRR Player entities to be modified by another class e.g. Artefact Behaviour
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 11/04/22
    /// </summary>
    public class ArtefactState : State, ICollisionListener
    {
        #region FEILD VARIABLES

        // DECLARE an EventHandler<CollisionEventArgs>, name it '_collisionEvent':
        private EventHandler<CollisionEventArgs> _collisionEvent;

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
                
                // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnEvent():
                _behaviourEvent += pUpdateEventListener.OnEvent;

                // SUBSCRIBE _collisionEvent to pUpdateEventListener.OnEvent():
                _collisionEvent += (pUpdateEventListener as IEventListener<CollisionEventArgs>).OnEvent;
                
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance!");
            }
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
        }

        #endregion
    }
}
