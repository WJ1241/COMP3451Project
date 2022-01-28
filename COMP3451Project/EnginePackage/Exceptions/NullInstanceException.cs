﻿using System;

namespace COMP3451Project.EnginePackage.Exceptions
{
    /// <summary>
    /// Exception which is used to test when a class contains a null instance or reference
    /// Author: William Smith
    /// Date: 17/12/21
    /// </summary>
    public class NullInstanceException : Exception
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of NullInstanceException
        /// </summary>
        /// <param name="pMessage"> string message which details why exception was thrown </param>
        public NullInstanceException(string pMessage) : base(pMessage)
        {
            // EMPTY CONSTRUCTOR, FUNCTIONALITY SPECIFIED IN CONSTRUCTOR DECLARATION
        }

        #endregion
    }
}
