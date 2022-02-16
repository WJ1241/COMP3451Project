using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CoreInterfaces;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
