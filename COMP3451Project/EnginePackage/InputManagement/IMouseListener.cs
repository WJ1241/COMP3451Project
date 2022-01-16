﻿using Microsoft.Xna.Framework.Input;

namespace COMP3451Project.EnginePackage.InputManagement
{
    /// <summary>
    /// Interface which allows implementations to listen for Mouse input
    /// </summary>
    public interface IMouseListener
    {
        #region METHODS

        /// <summary>
        /// Called when Publisher has new mouse input information for listening objects
        /// </summary>
        /// <param name="mouseState">Holds reference to MouseState object</param>
        void OnMouseInput(MouseState mouseState);

        #endregion
    }
}
