using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to store a PlayerIndex value
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public interface IPlayer
    {
        #region PROPERTIES

        /// <summary>
        /// Property which can set value of a PlayerIndex
        /// </summary>
        PlayerIndex PlayerNum { set; }

        #endregion
    }
}
