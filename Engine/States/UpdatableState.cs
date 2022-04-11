using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.States
{
    /// <summary>
    /// Class which contains conditional information for RIRR Player entities to be modified by another class e.g. an IEventListener<UpdateEventArgs> implementation
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public class UpdatableState : State, IInitialiseParam<IEventListener<UpdateEventArgs>>, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_behaviourEvent':
        protected EventHandler<UpdateEventArgs> _behaviourEvent;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of UpdatableState
        /// </summary>
        public UpdatableState()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IEVENTLISTENER<UPDATEEVENTARGS>>

        /// <summary>
        /// Initialises an object with an IEventListener<UpdateEventArgs> object
        /// </summary>
        /// <param name="pUpdateEventListener"> IEventListener<UpdateEventArgs> object </param>
        public virtual void Initialise(IEventListener<UpdateEventArgs> pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnEvent():
                _behaviourEvent += pUpdateEventListener.OnEvent;
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
        public virtual void Update(GameTime pGameTime)
        {
            // SET RequiredArg Property value of(_argsDict["UpdateEventArgs"] to reference to pGameTime
            (_argsDict["UpdateEventArgs"] as UpdateEventArgs).RequiredArg = pGameTime;

            // INVOKE _behaviourEvent(), passing this class and _argsDict["UpdateArgs"] as parameters:
            _behaviourEvent.Invoke(this, _argsDict["UpdateEventArgs"] as UpdateEventArgs);
        }

        #endregion
    }
}
