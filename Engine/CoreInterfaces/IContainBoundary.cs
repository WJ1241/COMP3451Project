using Microsoft.Xna.Framework;

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have access to Screen Size
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public interface IContainBoundary
    {
        #region PROPERTIES

        /// <summary>
        /// Property which has read and write access to the value of screen window borders
        /// </summary>
        Vector2 WindowBorder { get; set; }

        #endregion
    }
}
