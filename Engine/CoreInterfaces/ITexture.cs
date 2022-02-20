using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have a texture
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
    /// </summary>
    public interface ITexture
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read or write access to visible texture
        /// </summary>
        Texture2D Texture { get; set; }

        /// <summary>
        /// Property which allows read or write access to size of texture, mostly used for testing, as well as setting hitbox
        /// </summary>
        Point TextureSize { get; set; }

        #endregion
    }
}
