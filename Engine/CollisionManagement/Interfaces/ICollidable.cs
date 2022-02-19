using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CollisionManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to have a HitBox
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
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
