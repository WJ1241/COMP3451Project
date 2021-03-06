using System;
using Microsoft.Xna.Framework.Input;

namespace OrbitalEngine.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for a 'MouseInput' event
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public class MouseInputEventArgs : EventArgs
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows get access to current mouse state
        /// </summary>
        public MouseState RequiredArg
        {
            get
            {
                // RETURN Mouse.GetState():
                return Mouse.GetState();
            }
        }

        #endregion
    }
}
