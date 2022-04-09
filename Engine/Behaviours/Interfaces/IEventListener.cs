

namespace OrbitalEngine.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to listen for an event specified via the generic placeholder 'EA'
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    /// <typeparam name="EA"> Generic EventArgs </typeparam>
    public interface IEventListener<EA>
    {
        #region EVENTS

        /// <summary>
        /// Event called when needing to perform specified behaviour via EA Placeholder
        /// </summary>
        /// <param name="pSource"> Caller of Event </param>
        /// <param name="pArgs"> Generic EventArgs object </param>
        void OnEvent(object pSource, EA pArgs);

        #endregion
    }
}
