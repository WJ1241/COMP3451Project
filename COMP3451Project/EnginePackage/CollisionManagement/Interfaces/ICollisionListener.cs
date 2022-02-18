

namespace COMP3451Project.EnginePackage.CollisionManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to listen for collisions with other objects
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public interface ICollisionListener
    {
        #region METHODS

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable">Other entity implementing ICollidable</param>
        void OnCollision(ICollidable pScndCollidable);

        #endregion
    }
}
