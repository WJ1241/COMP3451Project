

namespace OrbitalEngine.InputManagement.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to publish Mouse input to listeners
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface IMousePublisher
    {
        #region METHODS

        /// <summary>
        /// Subscribes a Mouse listening object to be stored in a list/dictionary
        /// </summary>
        /// <param name="pMouseListener">Reference to an object implementing IMouseListener</param>
        void Subscribe(IMouseListener pMouseListener);

        /// <summary>
        /// Unsubscribes a Mouse listening object from list/dictionary using its unique name
        /// </summary>
        /// <param name="uName">Used for passing unique name</param>
        void Unsubscribe(string pUName);

        #endregion
    }
}
