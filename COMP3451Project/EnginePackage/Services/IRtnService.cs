using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services
{
    /// <summary>
    /// Interface which allows implementations to return an instance of a service to be used within engine
    /// Author: William Smith
    /// Date: 19/12/21
    /// </summary>
    public interface IRtnService
    {
        #region METHODS

        /// <summary>
        /// Returns an instance of an IService object specified in place of the generic 'C'
        /// </summary>
        /// <typeparam name="C"> Generic for class needed </typeparam>
        /// <returns> Instance of IService </returns>
        IService GetService<C>() where C : IService, new();

        #endregion
    }
}
