

namespace COMP3451Project.EnginePackage.CollisionManagement
{
    /// <summary>
    /// Interface that allows implementations to listen for collisions with other objects
    /// </summary>
    public interface ICollisionListener
    {
        #region METHODS

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="scndCollidable">Other entity implementing ICollidable</param>
        void OnCollision(ICollidable scndCollidable);

        #endregion
    }
}
