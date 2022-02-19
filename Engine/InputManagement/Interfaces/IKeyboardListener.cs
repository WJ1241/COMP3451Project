using Microsoft.Xna.Framework.Input;

namespace COMP3451Project.EnginePackage.InputManagement.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to listen for Keyboard input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface IKeyboardListener
    {
        #region METHODS

        /// <summary>
        /// Called when Publisher has new Keyboard input information for listening objects
        /// </summary>
        /// <param name="pKeyboardState"> Holds reference to Keyboard State object </param>
        void OnKBInput(KeyboardState pKeyboardState);

        #endregion
    }
}
