using System.Collections.Generic;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.CollisionManagement
{
    /// <summary>
    /// Interface that allows implementations to store objects which can collide with other objects
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
