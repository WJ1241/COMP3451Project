using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to have velocity when displayed on screen
    /// </summary>
    public interface IVelocity
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows access to get value of an entity's velocity
        /// </summary>
        Vector2 Velocity { get; }

        #endregion 
    }
}
