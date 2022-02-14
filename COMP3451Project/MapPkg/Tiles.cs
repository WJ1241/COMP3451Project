using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.PongPackage.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.EntityClasses
{
    /// <summary>
    /// Class Tiles
    /// used in tile and map creation
    /// Author: Declan Kerby-Collins & William Smith
    /// Date: 24/01/22
    /// <REFERENCE> Oyyou (2013) XNA Tutorial 40 - Creating a Tile Map (1/3). Available at: https://www.youtube.com/watch?v=PKlHcxFAEk0. (Accessed: 25 January 2022). </REFERENCE>
    /// </summary>
    public class Tiles: PongEntity
    {
        //DECLARE ContentManager name it '_content'
        private static ContentManager _content;
        //DECLARE Rectangle name it '_rect'
        private Rectangle _rect;


        //PROPERTY ContentManager name it 'Content'
        public static ContentManager Content
        {
            protected get { return _content; }

            set { _content = value; }
        }

        //PROPERTY Rectangle name it 'Rect'
        public Rectangle Rect
        {
            get { return _rect; }
            set { _rect = value; }
        }

        /// <summary>
        /// Construyctor Tiles 
        /// </summary>
        public Tiles()
        {
            //empty constructor
        }



        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// hides inherited draw method for unique parram set for the draw rectangle
        /// </summary>
        /// <param name="spritebatch">Needed to draw entity's texture on screen</param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _rect, Color.AntiqueWhite);
        }

        #endregion


        #region implimentation of interfaces

        /// <summary>
        /// MEHTOD: Initialise
        /// </summary>
        public override void Initialise()
        {

        }
        /// <summary>
        /// MEHTOD: Initialise
        /// </summary>
        public override void Terminate()
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pGameTime"></param>
        public override void Update(GameTime pGameTime)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pUpdateEventListener"></param>
        public override void Initialise(IUpdateEventListener pUpdateEventListener)
        {
            throw new NotImplementedException();
        }
        #endregion


    }

}
