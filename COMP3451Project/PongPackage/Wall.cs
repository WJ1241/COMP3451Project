using COMP3451Project.EnginePackage.CollisionManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.EntityClasses
{
    /// <summary>
    /// Class Wall
    /// specifically a wall tile, child of Tiles
    /// Author: Declan Kerby-Collins & William Smith
    /// Date: 24/01/22
    /// <REFERENCE> Oyyou (2013) XNA Tutorial 40 - Creating a Tile Map (1/3). Available at: https://www.youtube.com/watch?v=PKlHcxFAEk0. (Accessed: 25 January 2022). </REFERENCE>
    /// </summary>
    public class Wall: Tiles, ICollidable
    {
        /// <summary>
        /// spesifies the type of tile used for creation of collidable tiles
        /// </summary>
        /// <param name="i"></param>
        /// <param name="pRect"></param>
        public Wall( int i, Rectangle pRect)
        {
            // ASSIGNMENT: _texture is set to the value of the tile + i
            _texture = Content.Load<Texture2D>("Tile" + i);

            // ASSIGNMENT: this.Rect is set to the value of pRect
            this.Rect = pRect;
        }

        #region IMPLEMENTATION OF ICOLLIDABLE

        /// <summary>
        /// Used to Return a rectangle object to caller of property
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                // RETURN a rectangle, object current X axis location, object current Y axis location, texture width size, texture height size:
                return new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            }
        }

        #endregion
    }
}
