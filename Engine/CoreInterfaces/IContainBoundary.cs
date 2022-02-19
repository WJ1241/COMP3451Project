using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have access to Screen Size
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
    /// </summary>
    public interface IContainBoundary
    {
        #region PROPERTIES

        /// <summary>
        /// Property which has read and write access to the value of screen window borders
        /// </summary>
        Point WindowBorder { get; set; }

        #endregion
    }
}
