using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have X&Y axis Direction when displayed on screen
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public interface IDirection
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the value of an entity's direction
        /// </summary>
        Vector2 Direction { get; set; }

        #endregion 
    }
}