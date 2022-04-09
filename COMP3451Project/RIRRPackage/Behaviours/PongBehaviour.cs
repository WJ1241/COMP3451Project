using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Abstract class for Pong Behaviour classes to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public abstract class PongBehaviour : Behaviour, IVelocity
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2, name it '_velocity':
        protected Vector2 _velocity;

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnEvent(object pSource, UpdateEventArgs pArgs)
        {
            // SET value of _entity's Velocity Property to value of _velocity:
            (_entity as IVelocity).Velocity = _velocity;

            // ADD & APPLY velocity to current position:
            _entity.Position += _velocity;

            // CALL Boundary() method:
            Boundary();
        }

        #endregion


        #region IMPLEMENTATION OF IVELOCITY

        /// <summary>
        /// Property which allows read and write access to the value of an entity's velocity
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                // RETURN value of _velocity:
                return _velocity;
            }
            set
            {
                // SET value of _velocity to incoming value:
                _velocity = value;
            }
        }

        #endregion 


        #region PROTECTED METHODS

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected abstract void Boundary();

        #endregion
    }
}
