using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.BehaviourClasses
{
    /// <summary>
    /// Abstract class for Pong Behaviour classes to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
    /// </summary>
    public abstract class PongBehaviour : Behaviour
    {
        #region IMPLEMENTATION OF IUPDATEEVENTLISTENER

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnUpdate(object pSource, UpdateEventArgs pArgs)
        {
            // ADD & APPLY _entity's velocity to current position:
            _entity.Position += (_entity as IVelocity).Velocity;

            // CALL Boundary() method:
            Boundary();
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
