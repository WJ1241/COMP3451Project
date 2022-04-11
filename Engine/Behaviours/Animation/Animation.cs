using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.Animation.Interfaces;
using OrbitalEngine.Behaviours;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;

namespace OrbitalEngine.Animation
{
    /// <summary>
    /// Class which contains the code relative to changing source rectangles for animated sprites
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 11/04/22
    /// </summary>
    /// <REFERENCE> Lioncat Dev Studio (2021) Player Animation - Making a RPG in monogame. Available at: https://www.youtube.com/watch?v=i2ef5vfWO0w. (Accessed: 16 February 2022).</REFERENCE>
    public class Animation : UpdatableBehaviour, IAnimation, ITexture
    {
        #region FIELD VARIABLES

        // DECLARE a Texture2D, name it '_spritesheet':
        private Texture2D _spriteSheet;

        // DECLARE a Texture2D, name it '_textureSize':
        private Point _spriteSheetSize;

        // DECLARE: point name it '_spriteSize':
        private Point _spriteSize;

        // DECLARE: int name it '_frames':
        private int _frames;

        // DECLARE: int name it '_frameCount':
        private int _frameCount;

        // DECLARE: int name it '_row':
        private int _row;

        // DECLARE: double name it '_gamerTime':
        private double _msPerFrame;

        // DECLARE: double name it '_time'
        private double _time;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Animation
        /// </summary>
        public Animation()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IANIMATION

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
                // SET: value of _spriteSize:
                _spriteSize = value;

                // SET value of _frames to _spriteSheetSize.X / _spriteSize.X:
                _frames = _spriteSheetSize.X / _spriteSize.X;
            }
        }

        /// <summary>
        /// Property which allows write access to how many millseconds per animation frame
        /// </summary>
        public int MsPerFrame
        {
            set
            {
                // SET value of _msPerFrame to incoming value:
                _msPerFrame = value;
            }
        }

        /// <summary>
        /// PROPERTY: int Row
        /// </summary>
        public int Row
        {
            set
            {
                // SET: value of _row:
                _row = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows read or write access to visible texture
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                // RETURN value of current _spriteSheet:
                return _spriteSheet;
            }
            set
            {
                // INITIALISE _spriteSheet with incoming value:
                _spriteSheet = value;

                // INSTANTIATE _spriteSheetSize as a new Point(), using _spriteSheet's dimensions as parameters:
                _spriteSheetSize = new Point(_spriteSheet.Width, _spriteSheet.Height);
            }
        }

        /// <summary>
        /// /// Property which allows read or write access to size of texture, mostly used for testing, as well as setting hitbox
        /// </summary>
        public Point TextureSize
        {
            get
            {
                // RETURN value of _spriteSheetSize:
                return _spriteSheetSize;
            }
            set
            {
                // SET value of _spriteSheetSize to incoming value:
                _spriteSheetSize = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> Identification for Update() Method in EventHandler </param>
        public override void OnEvent(object pSource, UpdateEventArgs pArgs)
        {
            // ASSIGNMENT: _time has the value of pArgs in milliseconds added to it:
            _time += pArgs.RequiredArg.ElapsedGameTime.TotalMilliseconds;

            // SET SourceRectangle Property of _entity to a new Rectangle() with changed positions to display animation:
            (_entity as IDrawSourceRectangle).SourceRectangle = new Rectangle(_spriteSize.X * _frameCount, _spriteSize.Y * _row, _spriteSize.X, _spriteSize.Y);

            //IF: if time is greater than or equal to _msPerFrame, _time is current time in milliseconds, _msPerFrame is threshold before frame changes:
            if (_time >= _msPerFrame)
            {
                // ASSIGNMENT: _time has the value of _gamerTime subtracted from it:
                _time -= _msPerFrame;

                // ASSIGNMENT: _frameCount increments:
                _frameCount++;
            }
            // IF: _frameCount is greater than or equal to _frames:
            if (_frameCount >= _frames)
            {
                // ASSIGNMENT: _frameShift is set to '0':
                _frameCount = 0;
            }
        }

        #endregion
    }
}
