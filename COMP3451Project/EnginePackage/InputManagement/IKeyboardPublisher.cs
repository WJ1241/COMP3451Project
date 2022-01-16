

namespace COMP3451Project.EnginePackage.InputManagement
{
    /// <summary>
    /// Interface which allows implementations to publish Keyboard input to listeners
    /// </summary>
    public interface IKeyboardPublisher
    {
        #region METHODS

        /// <summary>
        /// Subscribes a Keyboard listening object to be stored in a list/dictionary
        /// </summary>
        /// <param name="keyboardListener">Reference to an object implementing IKeyboardListener</param>
        void Subscribe(IKeyboardListener keyboardListener);

        /// <summary>
        /// Unsubscribes a Keyboard listening object from list/dictionary using its unique name
        /// </summary>
        /// <param name="uName">Used for passing unique name</param>
        void Unsubscribe(string uName);

        #endregion
    }
}
