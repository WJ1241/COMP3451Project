using Microsoft.Xna.Framework;

namespace OrbitalEngine.Camera.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to change position of a camera
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public interface ICamera
    {
        #region METHODS

        /// <summary>
        /// Changes position of camera object when used as a parameter in a Draw Method
        /// </summary>
        /// <returns>A Matrix object to be used within Draw Methods</returns>
        Matrix ChngCamPos();

        #endregion
    }
}
