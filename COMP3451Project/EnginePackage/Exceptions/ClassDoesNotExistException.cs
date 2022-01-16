﻿using System;

namespace COMP3451Project.EnginePackage.Exceptions
{
    /// <summary>
    /// Exception which is used to test when a class does not exist in the program
    /// Author: William Smith
    /// Date: 17/12/21
    /// </summary>
    public class ClassDoesNotExistException : Exception
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of ClassDoesNotExistException
        /// </summary>
        /// <param name="pMessage"> string message which details why exception was thrown </param>
        public ClassDoesNotExistException(string pMessage) : base(pMessage)
        {
            // EMPTY CONSTRUCTOR, FUNCTIONALITY SPECIFIED IN CONSTRUCTOR DECLARATION
        }

        #endregion
    }
}
