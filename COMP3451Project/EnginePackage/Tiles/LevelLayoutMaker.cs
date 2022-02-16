using System;
using System.Collections.Generic;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.EnginePackage.Services.Commands.Interfaces;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;

namespace COMP3451Project.EnginePackage.Tiles
{
    /// <summary>
    /// Class which makes a Level Layout using a tilemap built in Tiled
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 16/02/22
    /// </summary>
    /// <IMPORTANT> WHEN MAKING A TILEMAP IN TILED, FOLLOW LAYER NUMBERS DECLARED IN 'ILAYER' INTERFACE TO PREVENT ISSUES</IMPORTANT>
    /// <REFERENCE> 'Teemu', Díaz, D., Gricci, S. (2016) TiledSharp-MonoGame-Example. Available at: https://github.com/Temeez/TiledSharp-MonoGame-Example. (Accessed: 16/02/22).
    public class LevelLayoutMaker : ILevelLayoutMaker, IInitialiseParam<string, IFuncCommand<IEntity>>, IService
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IFuncCommand<IEntity>>, name it '_createDict':
        private IDictionary<string, IFuncCommand<IEntity>> _createEntDict;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of LevelLayoutMaker
        /// </summary>
        public LevelLayoutMaker()
        {
            // INSTANTIATE _createEntDict as a new Dictionary<string, IFuncCommand<IEntity>>();
            _createEntDict = new Dictionary<string, IFuncCommand<IEntity>>();
        }

        #endregion


        #region IMPLEMENTATION OF ILEVELLAYOUTMAKER

        /// <summary>
        /// Creates a Level and its entities related to the textures and positions specified in the Tilemap
        /// </summary>
        /// <param name="pLevelName"> Name of Level </param>
        /// <param name="pTileMap"> Tiled Map Design </param>
        /// <param name="pTexture"> Tileset Texture </param>
        public void CreateLevelLayout(string pLevelName, TmxMap pTileMap, Texture2D pTexture)
        {
            // DECLARE & INSTANTIATE a new Point(), name it '_tileSize', passing pTileMap's TileWidth and TileHeight as parameters:
            Point _tileSize = new Point(pTileMap.Tilesets[0].TileWidth, pTileMap.Tilesets[0].TileHeight);

            // DECLARE & INSTANTIATE a new Point(), name it '_tilesetSize', passing pTexture / _tileSize as parameters:
            Point _tilesetSize = new Point(pTexture.Width / _tileSize.X, pTexture.Height / _tileSize.Y);

            // DECLARE an IEntity, name it '_tempEntity':
            IEntity _tempEntity;

            // FORLOOP, iterate until 'i' exceeds count of TileMap Layers:
            for (int i = 0; i < pTileMap.Layers.Count; i++)
            {
                // FORLOOP, iterate until 'j' exceeds count of tiles in chosen layer:
                for (int j = 0; j < pTileMap.Layers[i].Tiles.Count; j++)
                {
                    // SET _tempEntity to null, prevents issue where an entity can be addressed twice:
                    _tempEntity = null;

                    int _gid = pTileMap.Layers[i].Tiles[j].Gid;

                    // IF current tile is not empty:
                    if (_gid != 0)
                    {
                        // DECLARE & INITIALISE an int, name it '_tileFrame', give value of _gid - 1:
                        // _gid - 1 reverts to first slot in tileset
                        int _tileFrame = _gid - 1;


                        int _column = _tileFrame % _tilesetSize.X;


                        int _row = (int)Math.Floor((double)_tileFrame / (double)_tilesetSize.X);


                        float _tileXPos = (j % pTileMap.Width) * pTileMap.TileWidth;


                        float _tileYPos = (float)Math.Floor(j / (double)pTileMap.Width) * pTileMap.TileHeight;


                        if (pTileMap.Layers[i].Name == "Floors")
                        {

                            (_createEntDict["Floor"] as IFuncCommandOneParam<string, IEntity>).Data = "Floor" + j;


                            _tempEntity = _createEntDict["Floor"].ExecuteMethod();


                            (_tempEntity as ILayer).Layer = 1;
                        }

                        else if (pTileMap.Layers[i].Name == "Walls")
                        {

                            (_createEntDict["Wall"] as IFuncCommandOneParam<string, IEntity>).Data = "Wall" + j;


                            _tempEntity = _createEntDict["Wall"].ExecuteMethod();


                            (_tempEntity as ILayer).Layer = 2;
                        }

                        
                        if (_tempEntity != null)
                        {

                            (_tempEntity as ITexture).Texture = pTexture;


                            (_tempEntity as IDrawRectangle).SourceRectangle = new Rectangle(_tileSize.X * _column, _tileSize.Y * _row, _tileSize.X, _tileSize.Y);


                            (_tempEntity as IDrawRectangle).DestinationRectangle = new Rectangle((int)_tileXPos, (int)_tileYPos, _tileSize.X, _tileSize.Y);
                        }
                    }
                }
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMAND>

        /// <summary>
        /// Initialises an object with a string and an IFuncCommand<IEntity> object
        /// </summary>
        /// <param name="pFuncCommandName"> Name of Func Command </param>
        /// <param name="pFuncCommand"> IFuncCommand<IEntity> object </param>
        public void Initialise(string pFuncCommandName, IFuncCommand<IEntity> pFuncCommand)
        {
            // ADD pFuncCommandName as a key, and pFuncCommand as a value to _createEntDict:
            _createEntDict.Add(pFuncCommandName, pFuncCommand);
        }

        #endregion
    }
}
