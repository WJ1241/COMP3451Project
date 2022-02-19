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
        //DECLARE:
        private Animation _animation;

        //DECLARE:
        private float _timer;

        public Vector2 Position { get; set; }

        public AnimationManager (Animation pAnimation)
        {
            _animation = pAnimation;
        }

        public void Draw(SpriteBatch pSpriteBatch)
        {
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
            if (_animation == pAnimation)
                return;

            _animation = pAnimation;

            _animation.CurrentFrame = 0;

            _timer = 0;
        }

        public void Stop()
        {
            _timer = 0;

            _animation.CurrentFrame = 0;
        }


        public void Update(GameTime pGameTime)
        {
            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;

                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    _animation.CurrentFrame = 0;
                }
            }
        }

    }
}
