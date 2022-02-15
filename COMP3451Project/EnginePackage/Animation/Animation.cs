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
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    public class Animation: IAnimation
    {

        #region old
        /*
        #region properties
        // PROPERTY: type int name it 'CurrentFrame'
        public int CurrentFrame { get; set; }

        // PROPERTY: type int name it 'FrameCount'
        public int FrameCount { get; set; }

        // PROPERTY: type int name it 'Direction'
        public int Direction { get; set; }

        // PROPERTY: type int name it 'FrameHeight'
        public int FrameHeight { get { return _texture.Height / Direction; } }

        // PROPERTY: type int name it 'FrameWidth'
        public int FrameWidth { get { return _texture.Width / FrameCount; } }

        // PROPERTY: type Keys name it 'FrameSpeed'
        public float FrameSpeed { get; set; }

        // PROPERTY: type bool name it 'IsLooping'
        public bool IsLooping { get; set; }

        // PROPERTY: type Texture2D name it '_texture'
        public Texture2D _texture { get; private set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pTexture"></param> sprite texture
        /// <param name="pFrameCount"></param> current FrameCount
        public Animation(Texture2D pTexture, int pFrameCount)
        {
            // ASSIGNMENT: _texture is set to the value of the parameter texture
            _texture = pTexture;

            // ASSIGNMENT: FrameCount is set to the value of the parameter FrameCount, used to set X value on sprite sheet
            FrameCount = pFrameCount;
                        
            // ASSIGNMENT: IsLooping is set to true
            IsLooping = true;

            // ASSIGNMENT: FrameSpeed is set to 0.2f
            FrameSpeed = 0.2f;

            // ASSIGNMENT: Drection set to
            Direction = 0;

        }
        */
        #endregion

        // DECLARE:
        private Texture2D _anime;

        // DECLARE: int name it '_collumn'
        private int _collumn;

        // DECLARE: int name it '_heigth'
        private int _heigth;

        // DECLARE: int name it '_width'
        private int _width;

        // DECLARE: int name it '_frame'
        private int _frame;

        // DECLARE: int name it '_c' (its a counter)
        private int _c = 0;

        // DECLARE: int name it '_frameTime'
        private int _frameTime = 0;


        /// <summary>
        /// CONSTRUCTOR: Animation
        /// </summary>
        /// <param name="pSpriteSheet"></param>
        /// <param name="pCol"></param>
        /// <param name="pWidth"></param>
        /// <param name="pHeight"></param>
        public Animation(Texture2D pSpriteSheet, int pCol, int pWidth, int pHeight)
        {
            // ASSIGNMENT: _anime is set to the value of pSpriteSheet
            _anime = pSpriteSheet;

            // ASSIGNMENT: _collumn is set to the value of pCol
            _collumn = pCol;

            // ASSIGNMENT: _heigth is set to the value of pHeight
            _heigth = pHeight;

            // ASSIGNMENT: _width is set to the value of pWidth
            _width = pWidth;

            // ASSIGNMENT: _frame is set to the value of _anime
            _frame = _anime.Height / pHeight;

        }

        /// <summary>
        /// METHOD : Draw
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        /// <param name="pRect"></param>
        /// <param name="pGameTime"></param>
        /// <param name="pMillisecPerFrame"></param>
        public void Draw(SpriteBatch pSpriteBatch, Rectangle pRect, GameTime pGameTime, int pMillisecPerFrame = 500)
        {
            // IF: if _c is less than _frame
            if (_c < _frame)
            {
                // CALL: call pSpriteBatch's Draw method passing in _width, _heigth and dimensions of 16, 16
                pSpriteBatch.Draw(_anime, pRect, new Rectangle(_width, _heigth, 16, 16), Color.White);

                // ASSIGNMENT: _frameTime has the value of pGameTime's ElapsedGameTime in Milliseconds added to it
                _frameTime += pGameTime.ElapsedGameTime.Milliseconds;

                // IF: if _frameTime is greater than pMillisecPerFrame
                if (_frameTime > pMillisecPerFrame)
                {
                    // ASSIGNMENT: _frameTime has the value of pMillisecPerFrame subtracted from it
                    _frameTime -= pMillisecPerFrame;

                    // ASSIGNMENT: _c is incremented
                    _c++;

                    // IF: if _c is equal to _frame
                    if (_c == _frame)
                    {
                        // ASSIGNMENT: _c is set to 0
                        _c = 0;
                    }
                }
            }
        }

    }
}
