using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Behaviours.Interfaces;
using COMP3451Project.EnginePackage.CoreInterfaces;

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
        public abstract void Initialise(IUpdateEventListener pUpdateEventListener);

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
