using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TiledSharp;

namespace COMP3451Project.EnginePackage.Tiles
{
    /// <summary>
    /// Interface which allows implementations to create a Layout for a Game Level
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/02/22
    /// </summary>
    public interface ILevelLayoutMaker
    {
        #region METHODS

        /// <summary>
        /// Creates a Level and its entities related to the textures and positions specified in the Tilemap
        /// </summary>
        /// <param name="pTileMap"> Tiled Map Design </param>
        void CreateLevelLayout(TmxMap pTileMap);

        #endregion
    }
}
