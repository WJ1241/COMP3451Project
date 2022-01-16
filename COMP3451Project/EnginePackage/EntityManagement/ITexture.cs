using Microsoft.Xna.Framework.Graphics;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to have a texture
    /// </summary>
    public interface ITexture
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows access to get or set value of 'texture'
        /// </summary>
        Texture2D Texture { get;  set; }

        #endregion
    }
}
