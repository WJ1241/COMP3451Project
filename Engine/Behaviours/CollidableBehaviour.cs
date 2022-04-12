using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Behaviours
{
    /// <summary>
    /// Class which contains basic variables and methods needed for collision behaviour classes
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public abstract class CollidableBehaviour : Behaviour, IEventListener<CollisionEventArgs>
    {
        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method called when needing to perform behaviour based on collision
        /// </summary>
        /// <param name="pSource"> Object that has collided with another object </param>
        /// <param name="pArgs"> EventArgs for an Collidable object </param>
        public abstract void OnEvent(object pSource, CollisionEventArgs pArgs);

        #endregion
    }
}
