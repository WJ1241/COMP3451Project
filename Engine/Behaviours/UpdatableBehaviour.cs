using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Behaviours
{
    /// <summary>
    /// Class which contains basic variables and methods needed for updatable behaviour classes
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public abstract class UpdatableBehaviour : Behaviour, IEventListener<UpdateEventArgs>
    {
        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public abstract void OnEvent(object pSource, UpdateEventArgs pArgs);

        #endregion
    }
}
