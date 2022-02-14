using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.PongPackage.Models;



namespace COMP3451Project.PongPackage.Sprites
{
    /// <summary>
    /// public class Sprite
    /// contains sprite code 
    /// Authors: Declan Kerby-Collins & Oyyou
    /// date: 07/02/2022
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

        //DECLARE: Input object name it '_input
        public Input _input;

        //DECLARE: float name it '_speed'
        public float _speed = 1f;

        //DECLARE: Vector2 name it '_velocity'
        public Vector2 _velocity;
        #endregion

        #region Properties 

        //PROPERTY: Vector2 name it 'Position'
        public Vector2 Position 
        { 
            //return the local '_position' variable
            get { return _position; }

            //sets the local _position variable
            set { _position = value;

                //tests if _animationManager is not null
                if (_animationManager != null)
                    {
                    //ASSIGNMENT: _animationManager.Position is set to the value of '_position'
                    _animationManager.Position = _position;
                    }
                }
        }


        #endregion

        /// <summary>
        /// CONSTRUCTOR: Sprite constructor for non static textures
        /// </summary>
        /// <param name="pAnimations"> Dictionary of animations </param>
        public Sprite(Dictionary<string, Animation> pAnimations)
        {
            //ASSIGNMENT: _animations is set to the value of 'pAnimations';
            _animations = pAnimations;

            //ASSIGNMENT: _animationManager is set to the value of the first animation in the dictionary
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        /// <summary>
        /// CONSTRUCTOR: Sprite constructor for static textures
        /// </summary>
        /// <param name="pTexture"></param>
        public Sprite(Texture2D pTexture)
        {
            //ASSIGNMENT: _texture is set to the value of 'pTexture'
            _texture = pTexture;
        }

        /// <summary>
        /// METHOD: Draw draws the sprite batches.
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
            //IF: tests if the _texture is not null
            if (_texture != null)
            {
                //CALL: pSpriteBatch's 'Draw' method
                pSpriteBatch.Draw(_texture, Position, Color.White);
            }
            //ELSE IF: _animationManager not null 
            else if (_animationManager != null)
            {
                //CALL: _animationManager's 'Draw' method
                _animationManager.Draw(pSpriteBatch);
            }
            //EXCEPTION: nothing is being drawn string printed to anounce the error
            else throw new Exception("nothing being drawn");
        }

        /// <summary>
        /// METHOD: Update
        /// </summary>
        /// <param name="pGameTime"></param>
        /// <param name="pSprites"></param>
        public virtual void Update(GameTime pGameTime, List<Sprite> pSprites)
        {
            //CALL: Move()
            Move();

            //CALL: SetAnimations()
            SetAnimations();

            //CALL: calls _animationManager's Update method
            _animationManager.Update(pGameTime);

            //ASSIGNMENT: Position has the value of _velocity added to it
            Position += _velocity;

            //ASSIGNMENT: _velocity becomes the value of Vector2.Zero
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// METHOD: Move
        /// </summary>
        protected virtual void Move()
        {
            //IF: keybord state key 'Up' is in the down position
            if (Keyboard.GetState().IsKeyDown(_input.Up))
                //ASSIGNMENT: _velocity.Y is set to -_speed
                _velocity.Y = -_speed;

            //ELSE IF: keybord state key 'Down' is in the down position
            else if (Keyboard.GetState().IsKeyDown(_input.Down))
                //ASSIGNMENT: _velocity.Y is set to _speed;
                _velocity.Y = _speed;

            //ELSE IF: keybord state key 'Left' is in the down position
            else if (Keyboard.GetState().IsKeyDown(_input.Left))
                //ASSIGNMENT: _velocity.Y is set to _speed;
                _velocity.X = -_speed;

            //ELSE IF: keybord state key 'Right' is in the down position
            else if (Keyboard.GetState().IsKeyDown(_input.Right))
                //ASSIGNMENT: _velocity.X is set to _speed;
                _velocity.X = _speed;
        }

        /// <summary>
        /// METHOD: SetAnimations
        /// </summary>
        protected virtual void SetAnimations()
        {
            //IF: _velocity.X is greater than 0
            if (_velocity.X > 0)
            {
                //CALL: calls _animationManager's 'Play'  
                _animationManager.Play(_animations["walkRight"]);
            }
            //ELSE IF: _velocity.X is less than 0
            else if (_velocity.X < 0)
            {
                //CALL: calls _animationManager's 'Play' 
                _animationManager.Play(_animations["walkLeft"]);
            }
            //ELSE IF: _velocity.Y is greater than 0
            else if (_velocity.Y > 0)
            {
                //CALL: calls _animationManager's 'Play' 
                _animationManager.Play(_animations["walkDown"]);
            }
            //ELSE IF: _velocity.Y is less than 0
            else if (_velocity.Y < 0)
            {
                //CALL: calls _animationManager's 'Play' 
                _animationManager.Play(_animations["walkUp"]);
            }
        }


    }
}
