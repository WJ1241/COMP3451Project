using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have Speed when displayed on screen
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public interface IGetSpeed
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the value of an entity's Speed
        /// </summary>
        float GetSpeed { get; }

        #endregion
    }
}
