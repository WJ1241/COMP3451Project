using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to execute a type 'R' return method
    /// Authors: William Smith & Declan Kerby-Collins
    /// </summary>
    /// <typeparam name="R"> 'R'eturn value type </typeparam>
    public interface IFuncCommand<R>
    {
        #region METHODS

        /// <summary>
        /// Executes specified method, and returns value of type 'R'
        /// </summary>
        /// <returns> Value of Type 'R' </returns>
        R ExecuteMethod();

        #endregion
    }
}
