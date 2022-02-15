using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.PongPackage.Entities;
using TiledSharp;

namespace COMP3451Project.EnginePackage.Tiles
{
    /// <summary>
    /// Class which makes a Level Layout using a tilemap built in Tiled
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/02/22
    /// </summary>
    public class LevelLayoutMaker : ILevelLayoutMaker, IInitialiseParam<ICommand>, IService
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_create':
        private ICommand _createWall;

        // DECLARE an IDictionary<string, ICommand>, name it '_createDict':
        private IDictionary<string, ICommand> _createDict;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of LevelLayoutMaker
        /// </summary>
        public LevelLayoutMaker()
        {
            // INSTANTIATE _createDict as a new Dictionary<string, ICommand>();
            _createDict = new Dictionary<string, ICommand>();
        }

        #endregion


        #region IMPLEMENTATION OF ILEVELLAYOUTMAKER

        /// <summary>
        /// Creates a Level and its entities related to the textures and positions specified in the Tilemap
        /// </summary>
        /// <param name="pTileMap"> Tiled Map Design </param>
        public void CreateLevelLayout(TmxMap pTileMap)
        {
            // FOREACH Layer in pTileMap:
            foreach (TmxLayer pTileLayer in pTileMap.Layers)
            {
                // IF Layer 1:
                if (pTileLayer.Name == "1")
                {
                    // FOREACH Tile on Layer 1:
                    foreach (TmxLayerTile pTile in pTileLayer.Tiles)
                    {
                        //pTile.X
                    }
                }
            }

            //_createWall();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMAND>

        /// <summary>
        /// Initialises an object with an ICommand object
        /// </summary>
        /// <param name="pCommand"> ICommand object </param>
        public void Initialise(ICommand pCommand)
        {
            //_create = pCommand;
        }

        #endregion
    }
}
