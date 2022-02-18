using System;

namespace COMP3451Project.EnginePackage.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a void return method with ONE parameter
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for 'T'ype </typeparam>
    public interface ICommandOneParam<T> : ICommand
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to the desired first parameter type
        /// </summary>
        T Data { set; }

        /// <summary>
        /// Property which allows write access to a reference of a method with ONE parameter
        /// </summary>
        Action<T> MethodRef { set; }

        #endregion
    }
}
