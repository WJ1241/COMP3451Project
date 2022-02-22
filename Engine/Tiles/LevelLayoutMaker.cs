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
    /// Date: 16/02/22
    /// </summary>
    /// <IMPORTANT> WHEN MAKING A TILEMAP IN TILED, FOLLOW LAYER NUMBERS DECLARED IN 'ILAYER' INTERFACE TO PREVENT ISSUES </IMPORTANT>
    /// <REFERENCE> 'Teemu', Díaz, D., Gricci, S. (2016) TiledSharp-MonoGame-Example. Available at: https://github.com/Temeez/TiledSharp-MonoGame-Example. (Accessed: 16/02/22). </REFERENCE>
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

                            // DECLARE an int, name it '_gid' give value of whether tile is being used and what tile of tileset its using:
                            int _gid = pTileMap.Layers[i].Tiles[j].Gid;

                            // IF current tile is not empty:
                            if (_gid != 0)
                            {
                                // DECLARE & INITIALISE an int, name it '_tileFrame', give value of _gid - 1:
                                // _gid - 1 reverts to first slot in tileset
                                int _tileFrame = _gid - 1;

                                // DECLARE & INITIALISE an int, name it '_column', give remainder value of _tileFrame divided by _tilesetSize.X:
                                int _column = _tileFrame % _tilesetSize.X;

                                // DECLARE & INITIALISE an int, name it '_row', give rounded down value of _tileFrame / _tilesetSize width:
                                int _row = (int)Math.Floor((double)(_tileFrame / _tilesetSize.X));

                                // DECLARE & INITIALISE a float, name it '_tileXPos', give value of j / pTileMap's width's remainder value multiplied by pTileMap's tile width:
                                float _tileXPos = j % pTileMap.Width * pTileMap.TileWidth;

                                // DECLARE & INITIALISE a float, name it '_tileYPos', give rounded down value of current value of 'j' multiplied by pTileMap's width which is then multiplied by the tile height:
                                float _tileYPos = (float)Math.Floor(j / (double)pTileMap.Width) * pTileMap.TileHeight;


                                // TRY checking if ExecuteMethod() throws a ClassDoesNotExistException:
                                try
                                {
                                    // IF Layer name is "Floors":
                                    if (pTileMap.Layers[i].Name == "Floors")
                                    {
                                        // SET Data of _createEntDict["Floor"] to "Floor" + j:
                                        (_createEntDict["Floor"] as IFuncCommandOneParam<string, IEntity>).Data = "Floor" + j;

                                        // INITIALISE _tempEntity with return value from _createEntDict["Floor"].ExecuteMethod():
                                        _tempEntity = _createEntDict["Floor"].ExecuteMethod();

                                        // SET Layer Property value of _tempEntity to 1:
                                        (_tempEntity as ILayer).Layer = 1;
                                    }
                                    // IF Layer name is "Walls":
                                    else if (pTileMap.Layers[i].Name == "Walls")
                                    {
                                        // SET Data of _createEntDict["Wall"] to "Wall" + j:
                                        (_createEntDict["Wall"] as IFuncCommandOneParam<string, IEntity>).Data = "Wall" + j;

                                        // INITIALISE _tempEntity with return value from _createEntDict["Wall"].ExecuteMethod():
                                        _tempEntity = _createEntDict["Wall"].ExecuteMethod();

                                        // SET Layer Property value of _tempEntity to 2:
                                        (_tempEntity as ILayer).Layer = 2;
                                    }

                                    // _tempEntity DOES have an active instance:
                                    if (_tempEntity != null)
                                    {
                                        // SET Texture Property value of _tempEntity to same as pTexture:
                                        (_tempEntity as ITexture).Texture = pTexture;

                                        // SET SourceRectangle Property value of _tempEntity to a new Rectangle() with drawn positions based on its column and row, as well as the size of the drawn rectangle: 
                                        (_tempEntity as IDrawSourceRectangle).SourceRectangle = new Rectangle(_tileSize.X * _column, _tileSize.Y * _row, _tileSize.X, _tileSize.Y);

                                        // SET SourceRectangle Property value of _tempEntity to a new Rectangle() with drawn positions based on its column and row, as well as the size of the drawn rectangle: 
                                        (_tempEntity as IDrawDestinationRectangle).DestinationRectangle = new Rectangle((int)_tileXPos, (int)_tileYPos, _tileSize.X, _tileSize.Y);
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
