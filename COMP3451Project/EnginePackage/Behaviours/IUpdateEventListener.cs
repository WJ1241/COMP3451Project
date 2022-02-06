using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP3451Project.EnginePackage.CustomEventArgs;

namespace COMP3451Project.EnginePackage.Behaviours
{
    /// <summary>
    /// Interface which allows implementations to contain behaviour based logic
    /// Author(s): William Smith & Declan Kerby-Collins
    /// Date: 17/01/22
    /// </summary>
    public interface IUpdateEventListener
    {
        #region METHODS

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> Identification for Update() Method in EventHandler </param>
        void OnUpdateEvent(object pSource, UpdateEventArgs pArgs);

        #endregion
    }
}
