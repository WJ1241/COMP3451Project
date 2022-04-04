﻿using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to listen for Keyboard Input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface IKBInputEventListener
    {
        #region METHODS

        /// <summary>
        /// Method which is called after an object that requires Keyboard input has been notified of new user input
        /// </summary>
        /// <param name="pSource"> Object that requires output from Keyboard input </param>
        /// <param name="pArgs"> KBInputEventArgs object </param>
        void OnKBInputEvent(object pSource, KBInputEventArgs pArgs);

        #endregion
    }
}