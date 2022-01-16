using Microsoft.Xna.Framework.Graphics;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to draw an object on screen
    /// </summary>
    public interface IDraw
    {
        #region METHODS

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        void Draw(SpriteBatch spriteBatch);

        #endregion

    }
}
