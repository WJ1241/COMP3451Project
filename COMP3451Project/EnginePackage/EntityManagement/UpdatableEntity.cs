using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Behaviours.Interfaces;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement.Interfaces;
using COMP3451Project.EnginePackage.Exceptions;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Abstract Class which contains logic for a game loop for dynamic entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public abstract class UpdatableEntity : DrawableEntity, IInitialiseParam<IUpdateEventListener>, IUpdatable
    {
        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public virtual void Initialise(IUpdateEventListener pUpdateEventListener)
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
