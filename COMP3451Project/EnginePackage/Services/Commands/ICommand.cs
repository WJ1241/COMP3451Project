using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands
{
    /// <summary>
    /// Interface which allows implementations to execute a method
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface ICommand
    {
        #region METHODS

        /// <summary>
        /// Executes specified method
        /// </summary>
        void ExecuteMethod();

        #endregion
    }
}
