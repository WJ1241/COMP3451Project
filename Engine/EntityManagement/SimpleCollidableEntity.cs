using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;

namespace OrbitalEngine.EntityManagement
{
    /// <summary>
    /// Class which spawns a SimpleCollidableEntity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 09/04/22
    /// </summary>
    public class SimpleCollidableEntity : DrawableRectangleEntity, ICollidable
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SimpleCollidableEntity
        /// </summary>
        public SimpleCollidableEntity()
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
