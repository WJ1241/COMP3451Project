using System;
using Microsoft.Xna.Framework;

namespace OrbitalEngine.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for a 'Position' event
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 03/04/22
    /// </summary>
    public class PositionEventArgs : EventArgs
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2, name it '_position':
        private Vector2 _position;

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows get and set access to a Vector2 struct
        /// </summary>
        public Vector2 RequiredArg
        {
            get
            {
                // RETURN value of _position:
                return _position;
            }
            set
            {
                // SET value of _position to incoming value:
                _position = value;
            }
        }

        #endregion
    }
}
