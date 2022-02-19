using System;
using Microsoft.Xna.Framework.Input;

namespace OrbitalEngine.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for an 'KeyboardInput' event
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public class KBInputEventArgs : EventArgs
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows get access to current keyboard state
        /// </summary>
        public KeyboardState RequiredArg
        {
            get
            {
                // RETURN Keyboard.GetState():
                return Keyboard.GetState();
            }
        }

        #endregion
    }
}
