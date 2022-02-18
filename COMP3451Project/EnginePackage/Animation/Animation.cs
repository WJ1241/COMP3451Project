using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Animation.Interfaces;
using COMP3451Project.EnginePackage.Behaviours.Interfaces;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;

namespace COMP3451Project.EnginePackage.Animation
{
    /// <summary>
    /// Class which contains the code relative to changing source rectangles for animated sprites
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 17/02/22
    /// </summary>
    public class Animation : IAnimation, IDraw, IInitialiseParam<Texture2D>, IUpdateEventListener
    {
        #region FIELD VARIABLES

        // DECLARE: Texture2D name it '_spriteSheet':
        private Texture2D _spriteSheet;

        // DECLARE: Vector2 name it '_destination':
        private Vector2 _destination;

        // DECLARE: point name it '_spriteSheet':
        private Point _spriteSize;

        // DECLARE: int name it '_frameCount':
        private int _frameCount;

        // DECLARE: int name it '_row':
        private int _row;

        // DECLARE: int name it '_frameShift':
        private int _frameShift;

        // DECLARE: int name it '_height'
        private int _height;

        // DECLAR: int name it '_width'
        private int _width;

        // DECLAR: double name it '_time'
        private double _time;

        // DECLAR: double name it '_gamerTime'
        private double _gamerTime = 200;

        #endregion

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
        /// PROPERTY: Height
        /// </summary>
        public int Height
        { 
            get 
            { 
                return _height; 
            } 
            set 
            { 
                _height = value; 
            } 
        }

        /// <summary>
        /// PROPERTY: Width
        /// </summary>
        public int Width
        {
            get 
            { 
                return _width; 
            }
            set
            {
                _width = value;
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

        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Animation
        /// </summary>
        public Animation()
        {
            // empty constructor
        }

        #endregion




        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // IF: _frameCount is less than _frameShift
            if (_frameCount < _frameShift)
            {
                // CALL: pSpriteBatch's Draw method passing in the animation parameters: _width is multiplied by fremecount to establish the frame to be drawn, the _height is multiplied by _row to determine the series of frames to be animated
                pSpriteBatch.Draw(_spriteSheet, _destination, new Rectangle(_width * _frameCount, _height * _row, _spriteSize.X, _spriteSize.Y), Color.White);

                //IF: if time is greater than gamerTime, _time is , _gamertime is the game time ellapsed in milliseconds
                if (_time > _gamerTime)
                {
                    // ASSIGNMENT: _time has the value of _gamerTime subtracted from it
                    _time -= _gamerTime;

                    // ASSIGNMENT: _frameCount incriments
                    _frameCount++;

                    // IF: _frameCount is equel to _frameShift
                    if(_frameCount == _frameShift)
                    {
                        // ASSIGNMENT: _frameShift is set to 0
                        _frameShift = 0;
                    }
                }
            }
            
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

            // ASSIGNMENT: _frameShift is set to the value of _spriteSize.X divided by _width:
            _frameShift = _spriteSize.X / _width;
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> Identification for Update() Method in EventHandler </param>
        public void OnUpdateEvent(object pSource, UpdateEventArgs pArgs)
        {
            // ASSIGNMENT: _time has the value of pArgs in milliseconds added to it:
            _time += pArgs.RequiredArg.ElapsedGameTime.TotalMilliseconds;

        }

        #endregion
    }
}
