using Microsoft.Xna.Framework;

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to be rotated via an origin point and a specified angle
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
    /// </summary>
    public interface IRotation
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the point a texture is drawn
        /// </summary>
        Vector2 DrawOrigin { get; set; }

        /// <summary>
        /// Property which allows read and write access to how much a texture is rotated
        /// </summary>
        float Angle { get; set; }

        #endregion
    }
}
