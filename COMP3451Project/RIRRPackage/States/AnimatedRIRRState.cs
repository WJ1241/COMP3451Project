using System;
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
    /// Class which contains conditional information for Animated RIRR entities to be modified by another class e.g. NPCBehaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 13/04/22
    /// </summary>
    public class AnimatedRIRRState : UpdatableCollidableState
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_animationEvent':
        protected EventHandler<UpdateEventArgs> _animationEvent;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of AnimatedRIRRState
        /// </summary>
        public AnimatedRIRRState()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable">Other entity implementing ICollidable</param>
        public override void OnCollision(ICollidable pScndCollidable)
        {
            // SET RequiredArg Property value of (_argsDict["CollisionEventArgs"] to reference to pScndCollidable:
            (_argsDict["CollisionEventArgs"] as CollisionEventArgs).RequiredArg = pScndCollidable;

            // INVOKE _collisionEvent(), passing this class and (_argsDict["CollisionEventArgs"] as parameters:
            _collisionEvent.Invoke(this, _argsDict["CollisionEventArgs"] as CollisionEventArgs);
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
                // IF pUpdateEventListener DOES implement IAnimation:
                if (pUpdateEventListener is IAnimation)
                {
                    // SUBSCRIBE _animationEvent to pUpdateEventListener.OnEvent():
                    _animationEvent += pUpdateEventListener.OnEvent;
                }
                // IF pUpdateEventListener DOES NOT implement IAnimation:
                else
                {
                    // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnEvent():
                    _behaviourEvent += pUpdateEventListener.OnEvent;

                    // SUBSCRIBE _collisionEvent to pUpdateEventListener.OnEvent():
                    _collisionEvent += (pUpdateEventListener as IEventListener<CollisionEventArgs>).OnEvent;
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
    }
}
