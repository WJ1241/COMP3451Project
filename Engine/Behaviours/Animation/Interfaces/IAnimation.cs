using Microsoft.Xna.Framework;

namespace OrbitalEngine.Animation.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to run animations
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 19/02/22
    /// </summary>
    public interface IAnimation
    {
        #region PROPERTIES

        /// <summary>
        /// PROPERTY: Point SpriteSize
        /// </summary>
        Point SpriteSize { get;  set; }

        /// <summary>
        /// Property which allows write access to how many millseconds per animation frame
        /// </summary>
        int MsPerFrame { set; }

        /// <summary>
        /// PROPERTY: int Row
        /// </summary>
        int Row { set; }

        #endregion
    }
}
