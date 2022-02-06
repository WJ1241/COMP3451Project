using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.PongPackage.Models;
using COMP3451Project.PongPackage.EntityClasses;



namespace COMP3451Project.PongPackage.Sprites
{
    /// <summary>
    /// public class Sprite
    /// contains sprite code 
    /// Authors: Declan Kerby-Collins & Oyyou
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    class Sprite
    {
        #region Feilds

        //DECLARE: Texture2D name it '_texture'
        protected Texture2D _texture;

        //DECLARE: AnimationManager name it '_animationManager'
        protected AnimationManager _animationManager;

        //DECLARE: Dictionary anme it '_animations'
        protected Dictionary<string, Animation> _animations;

        //DECLARE: Vector2 name it '_position'
        protected Vector2 _position;

        #endregion

        #region Properties 

        public Input Input;

        //DECLARE: float name it 'Speed'
        public float Speed = 1f;

        //DECLARE: Vector2 name it 'Velocity'
        public Vector2 Velocity;

        public Vector2 Position 
        { 
            get { return _position; }

            set { _position = value;

                if (_animationManager != null)
                    {
                        _animationManager.Position = _position;
                    }
                }
        }


        #endregion


        public Sprite(Dictionary<string, Animation> pAnimations)
        {
            _animations = pAnimations;

            _animationManager = new AnimationManager(_animations.First().Value);
        }


        public Sprite(Texture2D pTexture)
        {
            _texture = pTexture;
        }


        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
            if (_texture != null)
            {
                pSpriteBatch.Draw(_texture, Position, Color.White);
            }
            else if (_animationManager != null)
            {
                _animationManager.Draw(pSpriteBatch);
            }
            else throw new Exception("nothing being drawn");
        }

        public virtual void Update(GameTime pGameTime, List<Sprite> pSprites)
        {
            Move();

            SetAnimations();

            _animationManager.Update(pGameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;
        }

        protected virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            else if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
        }

        protected virtual void SetAnimations()
        {
            if (Velocity.X > 0)
            {
                _animationManager.Play(_animations["walkRight"]);
            }
            else if (Velocity.X < 0)
            {
                _animationManager.Play(_animations["walkLeft"]);
            }
            else if (Velocity.Y > 0)
            {
                _animationManager.Play(_animations["walkDown"]);
            }
            else if (Velocity.Y < 0)
            {
                _animationManager.Play(_animations["walkUp"]);
            }
        }


    }
}
