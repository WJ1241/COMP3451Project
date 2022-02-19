using System;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.Services.Commands
{
    /// <summary>
    /// Class which contains a Func with ZERO Parameters
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 16/02/22
    /// </summary>
    /// <typeparam name="R"> 'R'eturn Value Type </typeparam>
    public class FuncCommandZeroParam<R> : IFuncCommandZeroParam<R>
    {
        #region FIELD VARIABLES

        // DECLARE a Func<R>, name it '_func':
        private Func<R> _func;

        #endregion


        #region IMPLEMENTATION OF IFUNCCOMMAND<R>

        /// <summary>
        /// Executes specified method, and returns 
        /// </summary>
        /// <returns> Value of Type 'R' </returns>
        public R ExecuteMethod()
        {
            // RETURN value from calling _func():
            return _func();
        }

        #endregion


        #region IMPLEMENTATION OF IFUNCCOMMANDZEROPARAM<R>

        /// <summary>
        /// Property which allows write access to a reference of a return method with ZERO parameters
        /// </summary>
        public Func<R> MethodRef
        {
            set
            {
                // SET value of _func to incoming value:
                _func = value;
            }
        }

        #endregion
    }
}
