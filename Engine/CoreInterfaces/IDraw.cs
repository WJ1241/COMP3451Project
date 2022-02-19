using Microsoft.Xna.Framework.Graphics;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to draw an object on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public interface IDraw
    {
        #region METHODS

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        void Draw(SpriteBatch pSpriteBatch);

        #endregion
    }
}
