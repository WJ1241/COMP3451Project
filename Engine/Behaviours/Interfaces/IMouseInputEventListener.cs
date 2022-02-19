using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to listen for Keyboard Input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface IMouseInputEventListener
    {
        #region METHODS

        /// <summary>
        /// Method which is called after an object that requires Mouse input has been notified of new user input
        /// </summary>
        /// <param name="pSource"> Object that requires output from Mouse input </param>
        /// <param name="pArgs"> MouseInputEventArgs object </param>
        void OnMouseInputEvent(object pSource, MouseInputEventArgs pArgs);

        #endregion
    }
}
