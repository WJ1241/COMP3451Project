using System;

namespace OrbitalEngine.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a void return method with TWO parameters
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    /// <typeparam name="T"> Generic Type 'T' </typeparam>
    /// <typeparam name="U"> Generic Type 'U' </typeparam>
    public interface ICommandTwoParam<T, U> : ICommand
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to the desired first parameter type
        /// </summary>
        T FirstParam { set; }

        /// <summary>
        /// Property which allows write access to the desired second parameter type
        /// </summary>
        U SecondParam { set; }

        /// <summary>
        /// Property which allows write access to a reference of a method with TWO parameters
        /// </summary>
        Action<T, U> MethodRef { set; }

        #endregion
    }
}
