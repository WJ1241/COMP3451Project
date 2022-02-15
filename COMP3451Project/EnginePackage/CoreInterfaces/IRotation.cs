using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to be rotated via an origin point and a specified angle
    /// Authors: William Smith
    /// Date: 15/02/22
    /// </summary>
    public interface IRotation
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows only read access to the point a texture is drawn
        /// </summary>
        Vector2 DrawOrigin { get; }

        /// <summary>
        /// Property which allows read and write access to how much a texture is rotated
        /// </summary>
        float Angle { get; set; }

        #endregion
    }
}
