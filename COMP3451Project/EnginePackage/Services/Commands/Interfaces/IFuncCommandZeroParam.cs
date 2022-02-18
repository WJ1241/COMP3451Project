using System;

namespace COMP3451Project.EnginePackage.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a type 'R' return method with ZERO parameters
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 16/02/22
    /// </summary>
    /// <typeparam name="R"> 'R'eturn Value Type </typeparam>
    public interface IFuncCommandZeroParam<R> : IFuncCommand<R>
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to a reference of a type 'R' return method with ZERO parameters
        /// </summary>
        Func<R> MethodRef { set; }

        #endregion
    }
}
