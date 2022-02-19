using System.Collections.Generic;
using OrbitalEngine.EntityManagement.Interfaces;

namespace OrbitalEngine.CollisionManagement.Interfaces
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
        /// Initialises object with an IReadOnlyDictionary<string, IEntity>
        /// </summary>
        /// <param name="pEntityDictionary">holds reference to 'IReadOnlyDictionary<string, IEntity>'</param>
        void Initialise(IReadOnlyDictionary<string, IEntity> pEntityDictionary);

        #endregion
    }
}
