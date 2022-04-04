﻿using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain behaviour based logic
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 17/01/22
    /// </summary>
    public interface IUpdateEventListener
    {
        #region METHODS

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> Identification for Update() Method in EventHandler </param>
        void OnUpdateEvent(object pSource, UpdateEventArgs pArgs);

        #endregion
    }
}