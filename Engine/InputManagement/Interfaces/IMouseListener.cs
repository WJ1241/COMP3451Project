using Microsoft.Xna.Framework.Input;

namespace OrbitalEngine.InputManagement.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to listen for Mouse input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface IMouseListener
    {
        #region METHODS

        /// <summary>
        /// Called when Publisher has new mouse input information for listening objects
        /// </summary>
        /// <param name="pMouseState"> Holds reference to MouseState object </param>
        void OnMouseInput(MouseState pMouseState);

        #endregion
    }
}
