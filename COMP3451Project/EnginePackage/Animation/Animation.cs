using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Animation
{
    /// <summary>
    /// public class Animation
    /// contains code for Animation
    /// Authors: Declan Kerby-Collins & William Smith
    /// date: 16/02/2022
    /// </summary>
    public class Animation : IAnimation, IInitialiseParam<Texture2D>
    {
        // DECLARE: 
        private Texture2D _spriteSheet;

        // DECLARE: int name it '_height'
        private int _height;

        // DECLARE: int name it '_width'
        private int _width;

        // DECLARE: int name it '_frameCount'
        private int _frameCount;

        // DECLARE: int name it '_frameShift'
        private int _frameShift;

        // DECLARE: int name it '_input'
        private string _input;

        // DECLARE: Vector2 name it '_position'
        private Vector2 _position;

        // DECLARE: Rectangle name it '_frame'
        private Rectangle _frame;

        // PROPPERTY: int name it 'Height'
        public int Height { get { return _height; } set { _height = value; } }

        // PROPPERTY: int name it 'Width'
        public int Width { get { return _width; } set { _width = value; } }

        // PROPPERTY: Texture2D name it 'SpriteSheet'
        public Texture2D SpriteSheet { get { return _spriteSheet; } set { _spriteSheet = value; } }



        /// <summary>
        /// CONSTRUCTOR: Animation
        /// </summary>
        public Animation()
        {
            // empty constructor
        }

        /// <summary>
        /// METHOD: Initialise
        /// </summary>
        /// <param name="pSpriteSheet"></param>
        public void Initialise(Texture2D pSpriteSheet)
        {
            // ASSIGNMENT: _spriteSheet is set to the value of pSpriteSheet;
            _spriteSheet = pSpriteSheet;

            // ASSIGNMENT: _frameShift is set to the value of 500;
            _frameShift = 500;
        }

        /// <summary>
        /// METHOD: Input
        /// </summary>
        /// <param name="pInput"></param>
        public void Input (string pInput)
        {
            #region _height for game spritesheets

            //if (pInput == "W")
            //{
            //    //_height = Y coordinate of the walking up frames
            //    _height = 96;
            //}
            //else if (pInput == "S")
            //{
            //    //_height = Y coordinate of the walking down frames
            //    _height = 64;
            //}
            //else if (pInput == "A")
            //{
            //    //_height = Y coordinate of the walking left frames
            //    _height = 32;
            //}
            //else if (pInput == "D")
            //{
            //    //_height = Y coordinate of the walking right frames, which thanks to our assets will be the same as the lft just rotate flipped 180
            //    _height = 0;                               
            //}
            #endregion


            // IF: pInput is "W"
            if (pInput == "W")
            {
                // ASSIGNMENT: _height = Y coordinate of the Paddle
                _height = 0;
            }
            // ELSE IF: pInput is "S"
            else if (pInput == "S")
            {
                // ASSIGNMENT: _height is set to the value of 150, determines the Y coordinate of the Paddle
                _height = 150;
            }
            // ELSE IF: pInput is null
            else if (pInput == null)
            {
                // ASSIGNMENT: _width is set to the value of 0, determines the X coordinate of the Paddle
                _width = 0;
            }

            // ASSIGNMENT: _input is set to the value of pInput
            _input = pInput;
        }

        /// <summary>
        /// METHOD: Update
        /// </summary>
        /// <param name="pGameTime"></param>
        public void Update(GameTime pGameTime)
        {

            // IF: _input is not null
            if (_input != null)
            {
                // ASSIGNMENT: 
                _frameCount += (int)pGameTime.ElapsedGameTime.TotalMilliseconds;

                if(_frameCount >= _frameShift)
                {
                    if (_width < _spriteSheet.Width)
                    {
                        // ASSIGNMENT: 
                        _width += 50;
                    }
                    else if (_width >= _spriteSheet.Width)
                    {
                        // ASSIGNMENT: 
                        _width = 50;
                    }
                }
            }
            else if (_input == null)
            {
                _width = 0;
            }

            //frame for paddle animation, height and width pick the piont of 
            _frame = new Rectangle(_height, _width, 50, 150);

        }


        public void Draw(SpriteBatch pSpriteBatch)
        {

            pSpriteBatch.Draw(_spriteSheet, _position, _frame, Color.White);

        }

    }
}
