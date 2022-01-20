using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands
{
    /// <summary>
    /// Interface which allows implementations to be initialised with an ICommandScheduler object
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface IInitialiseICommandScheduler
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with an ICommandScheduler object
        /// </summary>
        /// <param name="pCommandScheduler"> ICommandScheduler object </param>
        void Initialise(ICommandScheduler pCommandScheduler);

        #endregion
    }
}
