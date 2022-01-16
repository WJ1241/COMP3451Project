using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services
{
    /// <summary>
    /// Interface which allows implementations to be initialised with an IService object
    /// Author: William Smith
    /// Date: 19/12/21
    /// </summary>
    public interface IInitialiseIService
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with a reference to an IService instance
        /// </summary>
        /// <param name="pService"> IService instance </param>
        void Initialise(IService pService);
        
        #endregion
    }
}
