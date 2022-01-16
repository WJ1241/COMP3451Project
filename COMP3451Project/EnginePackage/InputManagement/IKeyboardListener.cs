using Microsoft.Xna.Framework.Input;

namespace COMP3451Project.EnginePackage.InputManagement
{
    /// <summary>
    /// Interface which allows implementations to listen for Keyboard input
    /// </summary>
    public interface IKeyboardListener
    {
        #region METHODS

        /// <summary>
        /// Called when Publisher has new Keyboard input information for listening objects
        /// </summary>
        /// <param name="keyboardState">Holds reference to Keyboard State object</param>
        void OnKBInput(KeyboardState keyboardState);

        #endregion
    }
}
