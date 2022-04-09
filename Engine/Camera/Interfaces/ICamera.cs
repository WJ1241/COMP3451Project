using Microsoft.Xna.Framework;

namespace OrbitalEngine.Camera.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to change position of a camera
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public interface ICamera
    {
        #region METHODS

        /// <summary>
        /// Changes Positional values of Camera
        /// </summary>
        /// <param name="pPosition"> Incoming Position value </param>
        /// <param name="pCenteringValue"> Incoming Position value </param>
        void ChangeCamPos(Vector2 pPosition, Vector2 pCenteringValue);

        #endregion
    }
}
