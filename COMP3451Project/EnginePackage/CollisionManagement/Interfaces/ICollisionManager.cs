using System.Collections.Generic;
using COMP3451Project.EnginePackage.EntityManagement.Interfaces;

namespace COMP3451Project.EnginePackage.CollisionManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to store objects which can collide with other objects
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public interface ICollisionManager
    {
        #region METHODS

        /// <summary>
        /// Initialises object with a IReadOnlyDictionary<string, IEntity>
        /// </summary>
        /// <param name="entityDictionary">holds reference to 'IReadOnlyDictionary<string, IEntity>'</param>
        void Initialise(IReadOnlyDictionary<string, IEntity> entityDictionary);

        #endregion
    }
}
