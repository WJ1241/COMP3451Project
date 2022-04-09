using System;
using Microsoft.Xna.Framework;

namespace OrbitalEngine.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for a 'Matrix' event
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public class MatrixEventArgs : EventArgs
    {
        #region FIELD VARIABLES

        // DECLARE a Matrix, name it '_matrix':
        private Matrix _matrix;

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows get and set access to a GameTime object
        /// </summary>
        public Matrix RequiredArg
        {
            get
            {
                // RETURN instance of _matrix:
                return _matrix;
            }
            set
            {
                // SET value of _matrix to incoming value:
                _matrix = value;
            }
        }

        #endregion
    }
}
