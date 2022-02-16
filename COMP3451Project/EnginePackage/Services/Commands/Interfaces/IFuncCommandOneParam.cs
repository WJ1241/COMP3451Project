using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a type 'R' return method with ONE parameter
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 16/02/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for 'T'ype </typeparam>
    /// <typeparam name="R"> 'R'eturn Value Type </typeparam>
    public interface IFuncCommandOneParam<T, R> : IFuncCommand<R>
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to the desired first parameter type
        /// </summary>
        T Data { set; }

        /// <summary>
        /// Property which allows write access to a reference of a type 'R' return method with ONE parameter
        /// </summary>
        Func<T, R> MethodRef { set; }

        #endregion
    }
}
