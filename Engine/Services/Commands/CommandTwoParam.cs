using System;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.Services.Commands
{
    /// <summary>
    /// Class which contains an Action with TWO Parameters
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    /// <typeparam name="T"> Generic Type 'T' </typeparam>
    /// <typeparam name="U"> Generic Type 'U' </typeparam>
    public class CommandOneParam<T, U> : ICommandTwoParam<T, U>
    {
        #region FIELD VARIABLES

        // DECLARE a T, name it '_first':
        private T _first;

        // DECLARE a T, name it '_second':
        private U _second;

        // DECLARE an Action, name it '_action':
        private Action<T, U> _action;

        #endregion


        #region IMPLEMENTATION OF ICOMMAND

        /// <summary>
        /// Executes specified method
        /// </summary>
        public void ExecuteMethod()
        {
            // CALL _action(), passing _first and _second as parameters:
            _action(_first, _second);
        }

        #endregion


        #region IMPLEMENTATION OF ICOMMANDTWOPARAM

        /// <summary>
        /// Property which allows write access to the desired first parameter type
        /// </summary>
        public T FirstParam
        {
            set
            {
                // SET value of _first to incoming value:
                _first = value;
            }
        }

        /// <summary>
        /// Property which allows write access to the desired second parameter type
        /// </summary>
        public U SecondParam
        {
            set
            {
                // SET value of _second to incoming value:
                _second = value;
            }
        }

        /// <summary>
        /// Property which allows write access to a reference of a method with TWO parameters
        /// </summary>
        public Action<T, U> MethodRef
        {
            set 
            {
                // SET value of _action to incoming value:
                _action = value;
            }
        }

        #endregion
    }
}
