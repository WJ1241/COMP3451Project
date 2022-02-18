using System;

namespace COMP3451Project.EnginePackage.Exceptions
{
    /// <summary>
    /// Exception which is used to test when an object already contains a value
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/21
    /// </summary>
    public class ValueAlreadyStoredException : Exception
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of ValueAlreadyStoredException
        /// </summary>
        /// <param name="pMessage"> string message which details why exception was thrown </param>
        public ValueAlreadyStoredException(string pMessage) : base(pMessage)
        {
            // EMPTY CONSTRUCTOR, FUNCTIONALITY SPECIFIED IN CONSTRUCTOR DECLARATION
        }

        #endregion
    }
}
