using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands
{
    /// <summary>
    /// Interface which allows implementations to contain a method with ONE parameter
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for 'T'ype </typeparam>
    public interface ICommandOneParam<T> : ICommand
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to the desired first parameter type
        /// </summary>
        T DataType { set; }

        /// <summary>
        /// Property which allows write access to a reference of a method with ONE parameter
        /// </summary>
        Action<T> Action { set; }

        #endregion
    }
}
