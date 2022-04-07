using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TiledSharp;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Tiles.Interfaces;

namespace OrbitalEngine.Tiles
{
    /// <summary>
    /// Class which makes a Level Layout using a tilemap built in Tiled
    /// Authors: William Smith, Declan Kerby-Collins, 'Teemu', Díaz, D. & Gricci, S.
    /// Date: 07/04/22
    /// </summary>
    /// <IMPORTANT> WHEN MAKING A TILEMAP IN TILED, FOLLOW LAYER NUMBERS DECLARED IN 'ILAYER' INTERFACE TO PREVENT ISSUES </IMPORTANT>
    /// <REFERENCE> 'Teemu', Díaz, D. & Gricci, S. (2016) TiledSharp-MonoGame-Example. Available at: https://github.com/Temeez/TiledSharp-MonoGame-Example. (Accessed: 16/02/22). </REFERENCE>
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
            // INSTANTIATE _createEntDict as a new Dictionary<string, IFuncCommand<IEntity>>():
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
        /// <CITATION> ('Teemu' et al., 2016) </CITATION>
        public void CreateLevelLayout(string pLevelName, TmxMap pTileMap, Texture2D pTexture)
        {
            // IF pTileMap DOES HAVE an active instance:
            if (pTileMap != null)
            {
                // IF pTileMap DOES NOT HAVE an active instance:
                if (pTexture != null)
                {
                    // DECLARE & INSTANTIATE a new Point(), name it 'tileSize', passing pTileMap's TileWidth and TileHeight as parameters:
                    Point tileSize = new Point(pTileMap.Tilesets[0].TileWidth, pTileMap.Tilesets[0].TileHeight);

                    // DECLARE & INSTANTIATE a new Point(), name it 'tilesetSize', passing pTexture / _tileSize as parameters:
                    Point tilesetSize = new Point(pTexture.Width / tileSize.X, pTexture.Height / tileSize.Y);

                    // DECLARE an IEntity, name it 'tempEntity':
                    IEntity tempEntity;

                    // FORLOOP, iterate until 'i' exceeds count of TileMap Layers:
                    for (int i = 0; i < pTileMap.Layers.Count; i++)
                    {
                        // FORLOOP, iterate until 'j' exceeds count of tiles in chosen layer:
                        for (int j = 0; j < pTileMap.Layers[i].Tiles.Count; j++)
                        {
                            // SET tempEntity to null, prevents issue where an entity can be addressed twice:
                            tempEntity = null;

                            // DECLARE an int, name it 'gid' give value of whether tile is being used and what tile of tileset its using:
                            int gid = pTileMap.Layers[i].Tiles[j].Gid;

                            // IF current tile is not empty:
                            if (gid != 0)
                            {
                                // DECLARE & INITIALISE an int, name it 'tileFrame', give value of _gid - 1:
                                // _gid - 1 reverts to first slot in tileset
                                int tileFrame = gid - 1;

                                // DECLARE & INITIALISE an int, name it '_column', give remainder value of tileFrame divided by tilesetSize.X:
                                int column = tileFrame % tilesetSize.X;

                                // DECLARE & INITIALISE an int, name it '_row', give rounded down value of tileFrame / tilesetSize width:
                                int row = (int)Math.Floor((double)(tileFrame / tilesetSize.X));

                                // DECLARE & INITIALISE a float, name it 'tileXPos', give value of j / pTileMap's width's remainder value multiplied by pTileMap's tile width:
                                float tileXPos = j % pTileMap.Width * pTileMap.TileWidth;

                                // DECLARE & INITIALISE a float, name it 'tileYPos', give rounded down value of current value of 'j' multiplied by pTileMap's width which is then multiplied by the tile height:
                                float tileYPos = (float)Math.Floor(j / (double)pTileMap.Width) * pTileMap.TileHeight;


                                // TRY checking if ExecuteMethod() throws a ClassDoesNotExistException:
                                try
                                {
                                    // IF Layer name is "Floors":
                                    if (pTileMap.Layers[i].Name == "Floors")
                                    {
                                        // SET Data of _createEntDict["Floor"] to "Floor" + j:
                                        (_createEntDict["Floor"] as IFuncCommandOneParam<string, IEntity>).Data = "Floor" + j;

                                        // INITIALISE tempEntity with return value from _createEntDict["Floor"].ExecuteMethod():
                                        tempEntity = _createEntDict["Floor"].ExecuteMethod();

                                        // SET Layer Property value of tempEntity to 1:
                                        (tempEntity as ILayer).Layer = 1;
                                    }
                                    // IF Layer name is "Walls":
                                    else if (pTileMap.Layers[i].Name == "Walls")
                                    {
                                        // SET Data of _createEntDict["Wall"] to "Wall" + j:
                                        (_createEntDict["Wall"] as IFuncCommandOneParam<string, IEntity>).Data = "Wall" + j;

                                        // INITIALISE tempEntity with return value from _createEntDict["Wall"].ExecuteMethod():
                                        tempEntity = _createEntDict["Wall"].ExecuteMethod();

                                        // SET Layer Property value of tempEntity to 2:
                                        (tempEntity as ILayer).Layer = 2;
                                    }
                                    // IF Layer name is "StaticObstacles" for tables, boxes etc
                                    else if (pTileMap.Layers[i].Name == "StaticObstacles")
                                    {
                                        // SET Data of _createEntDict["StaticObstacles"] to "StaticObstacles" + j:
                                        (_createEntDict["StaticObstacles"] as IFuncCommandOneParam<string, IEntity>).Data = "StaticObstacles" + j;

                                        // INITIALISE tempEntity with return value from _createEntDict["StaticObstacles"].ExecuteMethod():
                                        tempEntity = _createEntDict["StaticObstacles"].ExecuteMethod();

                                        // SET Layer Property value of tempEntity to 3:
                                        (tempEntity as ILayer).Layer = 3;
                                    }
                                    // IF Layer name is "Items" for the artifacts
                                    else if (pTileMap.Layers[i].Name == "Items")
                                    {
                                        // SET Data of _createEntDict["Items"] to "Items" + j:
                                        (_createEntDict["Items"] as IFuncCommandOneParam<string, IEntity>).Data = "Items" + j;

                                        // INITIALISE tempEntity with return value from _createEntDict["Items"].ExecuteMethod():
                                        tempEntity = _createEntDict["Items"].ExecuteMethod();

                                        // SET Layer Property value of tempEntity to 4:
                                        (tempEntity as ILayer).Layer = 4;
                                    }
                                    // IF Layer name is "LevelChange" for level transition 
                                    else if (pTileMap.Layers[i].Name == "Trans")
                                    {
                                        // SET Data of _createEntDict["LevelChange"] to "LevelChange" + j:
                                        (_createEntDict["LevelChange"] as IFuncCommandOneParam<string, IEntity>).Data = "LevelChange" + j;

                                        // INITIALISE tempEntity with return value from _createEntDict["LevelChange"].ExecuteMethod():
                                        tempEntity = _createEntDict["LevelChange"].ExecuteMethod();

                                        // SET Layer Property value of tempEntity to 5:
                                        (tempEntity as ILayer).Layer = 5;
                                    }
                                    // tempEntity DOES have an active instance:
                                    if (tempEntity != null)
                                    {
                                        // SET Texture Property value of tempEntity to same as pTexture:
                                        (tempEntity as ITexture).Texture = pTexture;

                                        // SET SourceRectangle Property value of tempEntity to a new Rectangle() with drawn positions based on its column and row, as well as the size of the drawn rectangle: 
                                        (tempEntity as IDrawSourceRectangle).SourceRectangle = new Rectangle(tileSize.X * column, tileSize.Y * row, tileSize.X, tileSize.Y);

                                        // SET SourceRectangle Property value of tempEntity to a new Rectangle() with drawn positions based on its column and row, as well as the size of the drawn rectangle: 
                                        (tempEntity as IDrawDestinationRectangle).DestinationRectangle = new Rectangle((int)tileXPos, (int)tileYPos, tileSize.X, tileSize.Y);
                                    }
                                }
                                // CATCH ClassDoesNotExistException from ExecuteMethod():
                                catch (ClassDoesNotExistException e)
                                {
                                    // THROW a new ClassDoesNotExistException(), with corresponding message:
                                    throw new ClassDoesNotExistException(e.Message);
                                }
                            }
                        }
                    }
                }
                // IF pTexture DOES NOT HAVE an active instance:
                else
                {
                    // THROW a new NullInstanceException(), with corresponding message:
                    throw new NullInstanceException("ERROR: pTexture does not have an active instance!");
                }
            }
            // IF pTileMap DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pTileMap does not have an active instance!");
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
            // IF pFuncCommand DOES HAVE an active instance:
            if (pFuncCommand != null)
            {
                // IF _createEntDict DOES NOT    contain pFuncCommandName as a key:
                if (!_createEntDict.ContainsKey(pFuncCommandName))
                {
                    // ADD pFuncCommandName as a key, and pFuncCommand as a value to _createEntDict:
                    _createEntDict.Add(pFuncCommandName, pFuncCommand);
                }
                // IF _createEntDict DOES contain pFuncCommandName as a key:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: " + pFuncCommandName + " already stored in _createEntDict!");
                }
            }
            // IF pFuncCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException, with corresponding message:
                throw new NullInstanceException("ERROR: pFuncCommand does not have an active instance!");
            }
        }

        #endregion
    }
}
