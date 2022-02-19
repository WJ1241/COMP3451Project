using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.EntityManagement;

namespace OrbitalEngine.Tiles
{
    /// <summary>
    /// Class which spawns a Wall on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public class Wall : DrawableRectangleEntity, ICollidable
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Wall
        /// </summary>
        public Wall()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLIDABLE

        /// <summary>
        /// Used to Return a rectangle object to caller of property
        /// </summary>
        public Rectangle HitBox
        {
           get
           {
                // RETURN value of _destinationRect:
                return _destinationRect;
           }
        }

        #endregion
    }
}
