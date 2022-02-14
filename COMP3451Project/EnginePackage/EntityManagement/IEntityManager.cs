﻿using System.Collections.Generic;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.Services;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to store Entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public interface IEntityManager : IService
    {
        #region METHODS

        /// <summary>
        /// Creates an object of IEntity, using <T> as a generic type
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        IEntity Create<T>(string uName) where T : IEntity, new();

        /// <summary>
        /// Returns an instance of an IDictionary<string, IEntity>
        /// </summary>
        IDictionary<string, IEntity> GetDictionary();

        /// <summary>
        /// Terminates an object from entity manager and other managers
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        void Terminate(string uName);

        

        #endregion
    }
}
