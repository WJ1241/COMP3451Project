using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CollisionManagement
{
    /// <summary>
    /// Interface that allows implementations to have a HitBox
    /// </summary>
    public interface ICollidable
    {
        #region PROPERTIES

        /// <summary>
        /// Used to Return a rectangle object to caller of property
        /// </summary>
        Rectangle HitBox { get; }

        #endregion
    }
}
