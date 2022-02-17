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
    /// date: 17/02/2022
    /// </summary>
    public class Animation : IAnimation, IInitialiseParam<Texture2D>, IDraw
    {

        // DECLARE: Texture2D name it '_spriteSheet'
        private Texture2D _spriteSheet;

        // DECLARE: int name it '_row'
        private int _row;

        // DECLARE: point name it '_spriteSheet'
        private Point _spriteSize;

        // DECLARE: int name it '_frameCount'
        private double _frameCount;

        // DECLARE: int name it '_frameShift'
        private int _frameShift;

        // DECLARE: int name it '_input'
        private string _input;

        // DECLARE: Vector2 name it '_position'
        private Vector2 _position;

        // DECLARE: Rectangle name it '_frame'
        private Rectangle _frame;



        // PROPPERTY: Texture2D name it 'SpriteSheet'
        public Texture2D SpriteSheet 
        { get 
            { 
                return _spriteSheet; 
            }
            set 
            { 
                _spriteSheet = value; 
            } 
        }

        // PROPPERTY: Vector2 name it 'Position'
        public Vector2 Position 
        { 
            get 
            { 
                return _position; 
            } 
            set 
            { 
                _position = value; 
            } 
        }

        // PROPPERTY: int name it 'Row'
        public int Row 
        { 
            get 
            { 
                return _row; 
            } 
            set 
            { 
                _row = value; 
            } 
        }

        // PROPPERTY: point name it 'SpriteSheet'
        public Point SpriteSize 
        { 
            get 
            {
                return _spriteSize; 
            } 
            set 
            { 
                _spriteSize = value; 
            } 
        }

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
            _frameShift = 300;
        }

        /// <summary>
        /// METHOD: Input
        /// </summary>
        /// <param name="pInput"></param>
        public void Input(string pInput)
        {



            // IF: pInput is "W"
            if (pInput == "W")
            {
                // ASSIGNMENT: _height = Y coordinate of the Paddle
                _spriteSize.Y = 0;
            }
            // ELSE IF: pInput is "S"
            else if (pInput == "S")
            {
                // ASSIGNMENT: _height is set to the value of 150, determines the Y coordinate of the Paddle
                _spriteSize.Y = _spriteSheet.Height / 2;
            }
            // ELSE IF: pInput is null
            else if (pInput == null)
            {
                // ASSIGNMENT: _width is set to the value of 0, determines the X coordinate of the Paddle
                _spriteSize.X = _spriteSheet.Width;
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
                // ASSIGNMENT: _frameCount has the value of pGameTime in milliseconds added to it
                _frameCount += pGameTime.ElapsedGameTime.TotalMilliseconds;

                // IF: _frameCount is greater than  to _frameShift
                if (_frameCount > _frameShift)
                {

                    // IF: _width is less than _spriteSheet's Width
                    if (_spriteSize.X < _spriteSheet.Width)
                    {
                        // ASSIGNMENT: _width has the value of 50 added to it
                        _spriteSize.X += (_spriteSheet.Width / 3);

                    }
                    else
                    {
                        // ASSIGNMENT: _width is set to the value of 50
                        _spriteSize.X = (_spriteSheet.Width / 3);

                    }

                    // ASSSIGNMENT: _farme count is set to 0
                    _frameCount = 0;
                }


            }
            // ELSE IF: _input is null 
            else
            {
                // ASSIGNMENT: _width is set to the value of 0
                _spriteSize.X = 0;
            }

            // ASSIGNMENT: _frame is assigned to 
            //frame for paddle animation, height and width pick the piont of 
            _frame = new Rectangle(_spriteSize.X, _spriteSize.Y, _spriteSheet.Width / 3, _spriteSheet.Height / 2);

        }

        /// <summary>
        /// METHOD: Draw 
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // CALL: pSpriteBatch's Draw method passing in the animation parameters
            pSpriteBatch.Draw(_spriteSheet, _position, _frame, Color.White);

        }

        
    }
}
