using System;

namespace COMP3451Project.EnginePackage.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a void return method with ZERO parameters
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface ICommandZeroParam : ICommand
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to a reference of a method with ZERO parameters
        /// </summary>
        Action MethodRef { set; }

        #endregion
    }
}
