using Microsoft.Xna.Framework.Graphics;


namespace COMP3451Project.EnginePackage.Camera
{
    /// <summary>
    /// Interface which allows implementations to draw displayable objects operating with a camera
    /// </summary>
    public interface IDrawCamera
    {
        #region METHODS

        /// <summary>
        /// When called, draws entity's texture on screen, as well as reposition a a camera object
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        /// <param name="camera">Needed to move camera position on screen</param>
        void Draw(SpriteBatch spriteBatch, ICamera camera);

        #endregion
    }
}
