using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.Models
{
    /// <summary>
    /// public class AnimationManager
    /// Manages Animation
    /// Authors: Declan Kerby-Collins & Oyyou
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    public class AnimationManager
    {
        //DECLARE: Animation name it '_animation'
        private Animation _animation;

        //DECLARE: float name it '_timer'
        private float _timer;

        //DECLARE:Vector2 name it 'Position'
        public Vector2 Position { get; set; }


        /// <summary>
        /// CONSTRUCTOR: AnimationManager 
        /// </summary>
        /// <param name="pAnimation"></param>
        public AnimationManager (Animation pAnimation)
        {
            //ASSIGNMENT: _animation set to the value of 'pAnimation'
            _animation = pAnimation;

        }

        /// <summary>
        /// METHOD: Draw
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            //CALL: pSpriteBatch's Draw method
            pSpriteBatch.Draw(_animation._texture,
                                Position,
                                new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                0,
                                _animation.FrameWidth,
                                _animation.FrameHeight),
                                Color.White);
        }


        public void Play(Animation pAnimation)
        {
            //IF: _animation is equel to 'pAnimation'
            if (_animation == pAnimation)
                return;
            //ASSIGNMENT: _animation set to the value of 'pAnimation'
            _animation = pAnimation;
            //ASSIGNMENT: _animation.CurrentFrame set to the value of '0'
            _animation.CurrentFrame = 0;
            //ASSIGNMENT: _timer set to the value of '0'
            _timer = 0;
        }

        public void Stop()
        {
            //ASSIGNMENT: _animation.CurrentFrame set to the value of '0'
            _animation.CurrentFrame = 0;
            //ASSIGNMENT: _timer set to the value of '0'
            _timer = 0;
        }


        public void Update(GameTime pGameTime)
        {
            //IF: _timer is greater than the _animation.FrameSpeed
            if (_timer > _animation.FrameSpeed)
            {
                //ASSIGNMENT: _timer set to the value of '0'
                _timer = 0f;
                //ASSIGNMENT: _animation.CurrentFrame is incremented
                _animation.CurrentFrame++;

                //IF: tests if the _animation.CurrentFrame is grater than or equel to _animation.FrameCount
                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    //ASSIGNMENT: _animation.CurrentFrame set to the value of '0'
                    _animation.CurrentFrame = 0;
                }
            }
        }

    }
}
