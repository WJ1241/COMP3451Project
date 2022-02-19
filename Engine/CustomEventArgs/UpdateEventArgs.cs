using System;
using Microsoft.Xna.Framework;

namespace OrbitalEngine.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for an 'Update' event
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 17/01/22
    /// </summary>
    public class UpdateEventArgs : EventArgs
    {
        #region FIELD VARIABLES

        // DECLARE a GameTime, name it '_gameTime':
        private GameTime _gameTime;

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows get and set access to a GameTime object
        /// </summary>
        public GameTime RequiredArg
        {
            get
            {
                // RETURN instance of _gameTime:
                return _gameTime;
            }
            set
            {
                // SET value of _gameTime to incoming value:
                _gameTime = value;
            }
        }

        #endregion
    }
}
