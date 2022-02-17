using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.CoreInterfaces;

namespace COMP3451Project.EnginePackage.Animation
{
    /// <summary>
    /// Class which contains the code relative to changing source rectangles for animated sprites
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 17/02/22
    /// </summary>
    public class Animation : IAnimation, IDraw, IInitialiseParam<Texture2D>, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE: Texture2D name it '_spriteSheet':
        private Texture2D _spriteSheet;

        // DECLARE: Vector2 name it '_destination':
        private Vector2 _destination;

        // DECLARE: point name it '_spriteSheet':
        private Point _spriteSize;

        // DECLARE: Rectangle name it '_frame':
        private Rectangle _frame;

        // DECLARE: int name it '_frameCount':
        private double _frameCount;

        // DECLARE: int name it '_row':
        private int _row;

        // DECLARE: int name it '_frameShift':
        private int _frameShift;

        // DECLARE: int name it '_input':
        private string _input;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Animation
        /// </summary>
        public Animation()
        {
            // empty constructor
        }

        #endregion


        #region IMPLEMENTATION OF IANIMATION

        /// <summary>
        /// METHOD: Input gives access to the Input class and passes in pString
        /// </summary>
        /// <param name="pInput"> Key Name </param>
        public void Input(string pInput)
        {
            // IF: pInput is "W"
            if (pInput == "W")
            {
                // ASSIGNMENT: _height = Y coordinate of the Paddle:
                _spriteSize.Y = 0;
            }
            // ELSE IF: pInput is "S"
            else if (pInput == "S")
            {
                // ASSIGNMENT: _height is set to the value of 150, determines the Y coordinate of the Paddle:
                _spriteSize.Y = _spriteSheet.Height / 2;
            }
            // ELSE IF: pInput is null
            else if (pInput == null)
            {
                // ASSIGNMENT: _width is set to the value of 0, determines the X coordinate of the Paddle
                _spriteSize.X = _spriteSheet.Width;
            }

            // ASSIGNMENT: _input is set to the value of pInput:
            _input = pInput;
        }

        /// <summary>
        /// PROPERTY: int Row
        /// </summary>
        public int Row
        {
            get
            {
                // RETURN: _row:
                return _row;
            }
            set
            {
                // SET: value of _row:
                _row = value;
            }
        }

        /// <summary>
        /// PROPERTY: Vector2 Destination
        /// </summary>
        public Vector2 Destination
        {
            get
            {
                // RETURN: _destination:
                return _destination;
            }
            set
            {
                // SET: value of _destination:
                _destination = value;
            }
        }


        /// <summary>
        /// PROPERTY: Point SpriteSize
        /// </summary>
        public Point SpriteSize
        {
            get
            {
                // RETURN: _spriteSize:
                return _spriteSize;
            }
            set
            {
                // SET: value of _spriteSize
                _spriteSize = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // CALL: pSpriteBatch's Draw method passing in the animation parameters:
            pSpriteBatch.Draw(_spriteSheet, _destination, _frame, Color.White);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<TEXTURE2D>

        /// <summary>
        /// Initialises an object with a Texture2D object
        /// </summary>
        /// <param name="pSpriteSheet"> Spritesheet for Animation </param>
        public void Initialise(Texture2D pSpriteSheet)
        {
            // ASSIGNMENT: _spriteSheet is set to the value of pSpriteSheet:
            _spriteSheet = pSpriteSheet;

            // ASSIGNMENT: _frameShift is set to the value of 500:
            _frameShift = 300;
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // IF: _input is not null:
            if (_input != null)
            {
                // ASSIGNMENT: _frameCount has the value of pGameTime in milliseconds added to it:
                _frameCount += pGameTime.ElapsedGameTime.TotalMilliseconds;

                // IF: _frameCount is greater than  to _frameShift:
                if (_frameCount > _frameShift)
                {

                    // IF: _width is less than _spriteSheet's Width:
                    if (_spriteSize.X < _spriteSheet.Width)
                    {
                        // ASSIGNMENT: _width has the value of 50 added to it:
                        _spriteSize.X += (_spriteSheet.Width / 3);

                    }
                    else
                    {
                        // ASSIGNMENT: _width is set to the value of 50:
                        _spriteSize.X = (_spriteSheet.Width / 3);

                    }

                    // ASSSIGNMENT: _farme count is set to 0:
                    _frameCount = 0;
                }
            }
            // ELSE IF: _input is null:
            else
            {
                // ASSIGNMENT: _width is set to the value of 0:
                _spriteSize.X = 0;
            }

            // ASSIGNMENT: _frame is assigned to 
            //frame for paddle animation, height and width pick the point of 
            _frame = new Rectangle(_spriteSize.X, _spriteSize.Y, _spriteSheet.Width / 3, _spriteSheet.Height / 2);
        }

        #endregion
    }
}
