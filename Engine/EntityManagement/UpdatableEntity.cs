using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.EntityManagement
{
    /// <summary>
    /// Abstract Class which contains logic for a game loop for dynamic entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public abstract class UpdatableEntity : DrawableEntity, IInitialiseParam<IEventListener<UpdateEventArgs>>, IUpdatable
    {
        #region IMPLEMENTATION OF IINITIALISEPARAM<IEVENTLISTENER<UPDATEVENTARGS>>

        /// <summary>
        /// Initialises an object with an IEventListener<UpdateEventArgs> object
        /// </summary>
        /// <param name="pUpdateEventListener"> IEventListener<UpdateEventArgs> object </param>
        public virtual void Initialise(IEventListener<UpdateEventArgs> pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
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


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public abstract void Update(GameTime pGameTime);

        #endregion
    }
}
