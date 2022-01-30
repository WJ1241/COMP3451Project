using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands
{
    /// <summary>
    /// Class which contains an Action with ONE Parameter
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for 'T'ype </typeparam>
    public class CommandOneParam<T> : ICommandOneParam<T>
    {
        #region FIELD VARIABLES

        // DECLARE a T, name it '_data':
        private T _data;

        // DECLARE an Action, name it '_action':
        private Action<T> _action;

        #endregion


        #region IMPLEMENTATION OF ICOMMAND

        /// <summary>
        /// Executes specified method
        /// </summary>
        public void ExecuteMethod()
        {
            // CALL _action(), passing _data as a parameter:
            _action(_data);
        }

        #endregion


        #region IMPLEMENTATION OF ICOMMANDONEPARAM

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
        /// Property which allows write access to a reference of a method with ONE parameter
        /// </summary>
        public Action<T> MethodRef
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
