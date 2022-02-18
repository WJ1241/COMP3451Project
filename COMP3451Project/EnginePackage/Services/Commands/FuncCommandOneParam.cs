using System;
using COMP3451Project.EnginePackage.Services.Commands.Interfaces;

namespace COMP3451Project.EnginePackage.Services.Commands
{
    /// <summary>
    /// Class which contains a Func with ONE Parameter
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 16/02/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for 'T'ype </typeparam>
    /// <typeparam name="R"> 'R'eturn Value Type </typeparam>
    public class FuncCommandOneParam<T, R> : IFuncCommandOneParam<T, R>
    {
        #region FIELD VARIABLES

        // DECLARE a Func<T, R>, name it '_func':
        private Func<T, R> _func;

        // DECLARE a T, name it '_data':
        private T _data;

        #endregion


        #region IMPLEMENTATION OF IFUNCCOMMAND<R>

        /// <summary>
        /// Executes specified method, and returns 
        /// </summary>
        /// <returns> Value of Type 'R' </returns>
        public R ExecuteMethod()
        {
            // RETURN value from calling _func(), passing _data as a parameter:
            return _func(_data);
        }

        #endregion


        #region IMPLEMENTATION OF IFUNCCOMMANDONEPARAM<T, R>

        /// <summary>
        /// Property which allows write access to the desired first parameter type
        /// </summary>
        public T Data
        {
            set
            {
                // SET value of _data to incoming value:
                _data = value;
            }
        }

        /// <summary>
        /// Property which allows write access to a reference of a return method with ONE parameter
        /// </summary>
        public Func<T, R> MethodRef
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
