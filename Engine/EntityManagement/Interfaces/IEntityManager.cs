using System.Collections.Generic;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.EntityManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to store Entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public interface IEntityManager : IService
    {
        #region METHODS

        /// <summary>
        /// Creates an object of IEntity, using <T> as a generic type
        /// </summary>
        /// <param name="pUName">Reference to object using unique name</param>
        IEntity Create<T>(string pUName) where T : IEntity, new();

        /// <summary>
        /// Returns an instance of an IDictionary<string, IEntity>
        /// </summary>
        IDictionary<string, IEntity> GetDictionary();

        /// <summary>
        /// Terminates an entity to be removed from memory
        /// </summary>
        /// <param name="pUName">Reference to object using unique name</param>
        void Terminate(string pUName);

        #endregion
    }
}
