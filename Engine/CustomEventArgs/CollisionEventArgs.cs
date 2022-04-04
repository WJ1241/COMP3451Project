using System;
using OrbitalEngine.CollisionManagement.Interfaces;

namespace OrbitalEngine.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for a 'Collision' event
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public class CollisionEventArgs : EventArgs
    {
        #region FIELD VARIABLES

        // DECLARE a ICollidable, name it '_scndCollidable':
        private ICollidable _scndCollidable;

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows get and set access to an ICollidable object
        /// </summary>
        public ICollidable RequiredArg
        {
            get
            {
                // RETURN instance of _scndCollidable:
                return _scndCollidable;
            }
            set
            {
                // SET value of _scndCollidable to incoming value:
                _scndCollidable = value;
            }
        }

        #endregion
    }
}
