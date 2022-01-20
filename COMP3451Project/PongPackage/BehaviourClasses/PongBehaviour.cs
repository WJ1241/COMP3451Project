using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP3451Project.EnginePackage.Behaviours;

namespace COMP3451Project.PongPackage.BehaviourClasses
{
    /// <summary>
    /// Abstract class for Pong Behaviour classes to inherit from
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public abstract class PongBehaviour : Behaviour
    {
        #region FIELD VARIABLES

        

        #endregion





        #region PROTECTED METHODS

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected abstract void Boundary();

        #endregion
    }
}
