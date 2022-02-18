using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have velocity when displayed on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public interface IVelocity
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the value of an entity's velocity
        /// </summary>
        Vector2 Velocity { get; set; }

        #endregion 
    }
}
