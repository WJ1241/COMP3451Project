using System;

namespace COMP3451Project.EnginePackage.Exceptions
{
    /// <summary>
    /// Exception which is used to test when a class contains a null value for updating or addressing
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 31/01/22
    /// </summary>
    public class NullValueException : Exception
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of NullValueException
        /// </summary>
        /// <param name="pMessage"> string message which details why exception was thrown </param>
        public NullValueException(string pMessage) : base(pMessage)
        {
            // EMPTY CONSTRUCTOR, FUNCTIONALITY SPECIFIED IN CONSTRUCTOR DECLARATION
        }

        #endregion
    }
}
