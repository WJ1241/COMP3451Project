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
    /// Authors: Declan Kerby-Collins & William Smith
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    public class AnimationManager
    {
        // DECLARE: Animation name it '_animation'
        private Animation _animation;

        // DECLARE: float name it '_timer'
        private float _timer;

        // PROPERTY: Vector2 naem it 'Position'
        public Vector2 Position { get; set; }

        /// <summary>
        /// METHOD: Constructor
        /// </summary>
        /// <param name="pAnimation"></param>
        public AnimationManager (Animation pAnimation)
        {
            // ASSIGNMENT: _animation is given the value of pAnimation
            _animation = pAnimation;
        }


        /// <summary>
        /// METHOD: Draw
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // Call: pSpriteBatch's 'Draw' method, passing in the params of '_animation's _texture', Position and a rectangle
            pSpriteBatch.Draw(_animation._texture,
                                Position,
                                new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                0,
                                _animation.FrameWidth,
                                _animation.FrameHeight),
                                Color.White);
        }

        /// <summary>
        /// METHOD: Play
        /// </summary>
        /// <param name="pAnimation"></param>
        public void Play(Animation pAnimation)
        {
            // IF: if _animation is equal to pAnimation, if so return 
            if (_animation == pAnimation)
                return;

            // ASSIGNMENT: _animation is given the value of pAnimation
            _animation = pAnimation;

            // ASSIGNMENT: _animation's CurrentFrame parameter is set to the value of 0
            _animation.CurrentFrame = 0;

            // ASSIGNMENT: _timer is set to the value of 0
            _timer = 0;
        }

        /// <summary>
        /// METHOD: Stop
        /// </summary>
        public void Stop()
        {
            // ASSIGNMENT: _timer is set to the value of 0
            _timer = 0;

            // ASSIGNMENT: _animation's CurrentFrame parameter is set to the value of 0
            _animation.CurrentFrame = 0;
        }

        /// <summary>
        /// METHOD: Update
        /// </summary>
        /// <param name="pGameTime"></param>
        public void Update(GameTime pGameTime)
        {
            // IF: if _timer is greater than _animation.FrameSpeed
            if (_timer > _animation.FrameSpeed)
            {
                // ASSIGNMENT: _timer is set to the value of 0
                _timer = 0f;

                // ASSIGNMENT: _animation's CurrentFrame is incremented
                _animation.CurrentFrame++;

                // IF: if _animation.CurrentFrame greater than or equal to _animation.FrameCount
                if (_animation.CurrentFrame >= _animation.FrameCount)
                {
                    // ASSIGNMENT: _animation's CurrentFrame parameter is set to the value of 0
                    _animation.CurrentFrame = 0;
                }
            }
        }

    }
}
