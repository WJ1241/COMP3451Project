using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.EntityManagement;


namespace COMP3451Project.PongPackage.EntityClasses
{
    /// <summary>
    /// Class Map
    /// used to draw the positions of the walls for each level
    /// Author: Declan Kerby-Collins & William Smith
    /// Date: 24/01/22
    /// <REFERENCE> Oyyou (2013) XNA Tutorial 40 - Creating a Tile Map (1/3). Available at: https://www.youtube.com/watch?v=PKlHcxFAEk0. (Accessed: 25 January 2022). </REFERENCE>
    /// </summary>
    public class Map
    {
        #region Feild Variables
        //DECLARE Texture2D name it '_texture'
        private Texture2D _texture;

        //DECLARE IList name it '_wallTiles'
        private IList<Tiles> _wallTiles = new List<Tiles>();

        //DECLARE int name it '_width'
        private int _width;

        //DECLARE int name it '_height'
        private int _height;


        #endregion


        #region Properties

        //PROPERTY WallTiles
        public IList<Tiles> WallTiles
        {
            get { return _wallTiles; }
        }

        //PROPERTY Width
        public int Width
        {
            get { return _width; }
        }

        //PROPERTY Height
        public int Height
        {
            get { return _height; }
        }

        #endregion

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Map()
        {
            
        }


        /// <summary>
        /// implimenting initialise 
        /// METHOD Initialise
        /// </summary>
        public void Initialise()
        {
            
        }

        /// <summary>
        /// METHOD BuildWalls  
        /// called to determin tile coordinites 
        /// </summary>
        /// <param name="pMap"></param>
        /// <param name="pSize"></param>
        public void BuildWalls(int[,] pMap, int pSize)
        {
            //nested for loop goes through all positions on x then y axis
            for(int x = 0; x < pMap.GetLength(1); x++)
            {
                for(int y = 0; y < pMap.GetLength(0); y++)
                {
                    //_number recives 
                    int _number = pMap[y, x];

                    //if _number is grerater than 0 then draw a tile 
                    if (_number > 0)
                    {
                        //Adds wall object to _wallTiles list
                        _wallTiles.Add(new Wall(_number,new Rectangle(x * pSize, y * pSize, pSize, pSize)));
                    }

                    //sets _width and _height to a value of 1 so as not to multiply by 0 and then multiply by pSize
                    _width = (x + 1) * pSize;
                    _height = (y + 1) * pSize;
                }
            }
        }

        /// <summary>
        /// METHOD Draw
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            //foreach wall object in _wallTiles call the Wall's Draw method
            foreach (Wall pTile in _wallTiles)
            {
                //calls the Draw method in the pTile
                pTile.Draw(pSpriteBatch);
            }
        }

    }
}
