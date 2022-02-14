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
    /// Authors: Declan Kerby-Collins & William Smith
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    class Sprite
    {
        #region Feilds

        // DECLARE: Texture2D name it '_texture'
        protected Texture2D _texture;

        // DECLARE: AnimationManager name it '_animationManager'
        protected AnimationManager _animationManager;

        // DECLARE: Dictionary anme it '_animations'
        protected Dictionary<string, Animation> _animations;

        // DECLARE: Vector2 name it '_position'
        protected Vector2 _position;

        #endregion

        #region Properties 

        // DECLARE: Input name it 'Input'
        public Input _input;

        // DECLARE: float name it 'Speed'
        public float _speed = 1f;

        // DECLARE: Vector2 name it 'Velocity'
        public Vector2 _velocity;
        #endregion

        // DECLARE: Position
        public Vector2 Position 
        { 
            // GET: returns _position 
            get { return _position; }

            // SET: _position is given the value of the incoming value
            set { _position = value;

                // TEST: if _animationManager is not null
                if (_animationManager != null)
                    {
                        // ASSIGNMENT: _animationManager's 'Position' property is set to the value of _position 
                        _animationManager.Position = _position;
                    }
                }
        }


        #endregion

        /// <summary>
        /// CONSTRUCTOR: Sprite
        /// </summary>
        /// <param name="pAnimations"></param>
        public Sprite(Dictionary<string, Animation> pAnimations)
        {
            // ASSIGNMENT: _animations is set to the value of pAnimations
            _animations = pAnimations;

            // ASSIGNMENT: _animationManager is set to the value of a new AnimationManager and passed the values of _animations first value
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        /// <summary>
        /// CONSTRUCTOR: Sprite
        /// </summary>
        /// <param name="pTexture"></param>
        public Sprite(Texture2D pTexture)
        {
            // ASSIGNMENT: _texture is set to the value of pTexture
            _texture = pTexture;
        }

        /// <summary>
        /// METHOD: Draw
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        public virtual void Draw(SpriteBatch pSpriteBatch)
        {
            // IF: if _texture is not null
            if (_texture != null)
            {
                // CALL: calls pSpriteBatch's Draw method and passes in the parrameters _texture, Position and Color.White
                pSpriteBatch.Draw(_texture, Position, Color.White);
            }
            // ELSE IF: if _animationManager is not null
            else if (_animationManager != null)
            {
                //CALL: _animationManager's Draw method passing in the pSpriteBatch as a parameter
                _animationManager.Draw(pSpriteBatch);
            }
            // ELSE: thow exception
            else throw new Exception("nothing being drawn");
        }

        /// <summary>
        /// METHOD: Update
        /// </summary>
        /// <param name="pGameTime"></param>
        /// <param name="pSprites"></param>
        public virtual void Update(GameTime pGameTime, List<Sprite> pSprites)
        {
            // CALL: call the Move method
            Move();

            // CALL: call the SetAnimations method
            SetAnimations();

            // CALL: calls _animationManager's update method
            _animationManager.Update(pGameTime);

            // ASSIGNMENT: Position has the value of _velocity added to it
            Position += _velocity;

            // ASSIGNMENT: _velocty is set to the value of Vector2.Zero
            _velocity = Vector2.Zero;
        }

        /// <summary>
        /// METHOD: Move
        /// </summary>
        protected virtual void Move()
        {
            // IF: if the up key is pressed
            if (Keyboard.GetState().IsKeyDown(_input.Up))
            {
                //ASSIGNMENT: _velocity's Y is set to -_speed
                _velocity.Y = -_speed;
            }
            // ELSE IF: if the down key is pressed
            else if (Keyboard.GetState().IsKeyDown(_input.Down))
            {
                //ASSIGNMENT: _velocity's Y is set to _speed
                _velocity.Y = _speed;
            }
            // ELSE IF: if the down key is pressed
            else if (Keyboard.GetState().IsKeyDown(_input.Left))
            {
                //ASSIGNMENT: _velocity's X is set to -_speed
                _velocity.X = -_speed;
            }
            // ELSE IF: if the down key is pressed
            else if (Keyboard.GetState().IsKeyDown(_input.Right))
            {
                // ASSIGNMENT: _velocity's X is set to _speed
                _velocity.X = _speed;
            }
        }

        /// <summary>
        /// METHOD: SetAnimation
        /// </summary>
        protected virtual void SetAnimations()
        {
            // IF: _velocity's X value is greater than 0
            if (_velocity.X > 0)
            {
                // ASSIGNTMENT: sets the frame 
                _animationManager.Play(_animations["walkRight"]);
            }
            // ELSE IF: _velocity's X value is less than 0
            else if (_velocity.X < 0)
            {
                // ASSIGNTMENT: sets the frame
                _animationManager.Play(_animations["walkLeft"]);
            }
            // ELSE IF: _velocity's Y value is greater than 0
            else if (_velocity.Y > 0)
            {
                // ASSIGNTMENT: sets the frame
                _animationManager.Play(_animations["walkDown"]);
            }
            // ELSE IF: _velocity's Y value is less than 0
            else if (_velocity.Y < 0)
            {
                // ASSIGNTMENT: sets the frame
                _animationManager.Play(_animations["walkUp"]);
            }
        }


    }
}
