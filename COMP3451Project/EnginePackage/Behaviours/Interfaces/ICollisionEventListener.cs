using COMP3451Project.EnginePackage.CustomEventArgs;

namespace COMP3451Project.EnginePackage.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to listen for Collisions between objects
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface ICollisionEventListener
    {
        #region METHODS

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        void OnCollisionEvent(object pSource, CollisionEventArgs pArgs);

        #endregion
    }
}
