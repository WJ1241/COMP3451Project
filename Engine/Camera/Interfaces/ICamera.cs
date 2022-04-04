using Microsoft.Xna.Framework;
using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Camera.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to change position of a camera
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 03/04/22
    /// </summary>
    public interface ICamera
    {
        #region METHODS

        /// <summary>
        /// Draws Camera using positional values set in implementation
        /// </summary>
        /// <returns> A Matrix object to be used within Draw Methods </returns>
        Matrix DrawCam();

        /// <summary>
        /// Changes Positional values so it is updated to stay with caller source's position
        /// </summary>
        /// <param name="pSource"> Object that is changing Position </param>
        /// <param name="pArgs"> EventArgs for a Positioned object </param>
        void ChangeCamPos(object pSource, PositionEventArgs pArgs);

        #endregion
    }
}
