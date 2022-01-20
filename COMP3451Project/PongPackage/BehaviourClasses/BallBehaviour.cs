using COMP3451Project.EnginePackage.CustomEventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.BehaviourClasses
{
    /// <summary>
    /// Class which defines the behaviour for Ball entities
    /// Author: Declan Kerby-Collins & William Smith
    /// Date: 20/01/2022
    /// </summary>
    class BallBehaviour : PongBehaviour
    {
        public override void OnUpdate(object pSource, UpdateEventArgs pArgs)
        {
            
        }

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {

        }

    }
}
