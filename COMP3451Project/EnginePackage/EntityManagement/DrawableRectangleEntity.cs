using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Class which allows an entity to be drawn on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public class DrawableRectangleEntity : DrawableEntity, IDrawRectangle
    {
        #region FIELD VARIABLES

        // DECLARE a Rectangle, name it '_destinationRect':
        protected Rectangle _destinationRect;

        // DECLARE a Rectangle, name it '_sourceRect':
        protected Rectangle _sourceRect;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of DrawableRectangleEntity
        /// </summary>
        public DrawableRectangleEntity()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IDRAWRECTANGLE

        /// <summary>
        /// Property which allows read and write access to an on screen Position Rectangle
        /// </summary>
        public Rectangle DestinationRectangle
        {
            get
            {
                // RETURN value of _destinationRect:
                return _destinationRect;
            }
            set
            {
                // SET value of _destinationRect to incoming value:
                _destinationRect = value;
            }
        }

        /// <summary>
        /// Property which allows only write access to a Draw Position Rectangle
        /// </summary>
        public Rectangle SourceRectangle
        {
            set
            {
                // SET value of _sourceRect to incoming value:
                _sourceRect = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given destination rectangle, source rectangle (where to draw) and colour
            pSpriteBatch.Draw(_texture, _destinationRect, _sourceRect, Color.AntiqueWhite);
        }

        #endregion


        #region IMPLEMENTATION OF ITERMINATE

        /// <summary>
        /// Disposes resources to the garbage collector
        /// </summary>
        public override void Terminate()
        {
            // No functionality, MonoGame deals with object and texture in garbage collector already
        }

        #endregion
    }
}
