using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace OrbitalEngine.Tiles.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to create a Layout for a Game Level
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 16/02/22
    /// </summary>
    public interface ILevelLayoutMaker
    {
        #region METHODS

        /// <summary>
        /// Creates a Level and its entities related to the textures and positions specified in the Tilemap
        /// </summary>
        /// <param name="pLevelName"> Name of Level </param>
        /// <param name="pTileMap"> Tiled Map Design </param>
        /// <param name="pTexture"> Tileset Texture </param>
        void CreateLevelLayout(string pLevelName, TmxMap pTileMap, Texture2D pTexture);

        #endregion
    }
}
