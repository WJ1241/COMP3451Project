﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Abstract class for Pong Entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/02/22
    /// </summary>
    public abstract class PongEntity : UpdatableEntity, IRotation, IVelocity
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2, name it '_drawOrigin':
        protected Vector2 _drawOrigin;

        // DECLARE a Vector2, name it '_velocity':
        protected Vector2 _velocity;

        // DECLARE a float, name it '_rotAngle':
        protected float _rotAngle;

        // DECLARE a float, name it 'speed':
        protected float _speed;

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _position, null, Color.AntiqueWhite, _rotAngle, _drawOrigin, 1f, SpriteEffects.None, 1f);


            //(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color, float rotation, Vector2 origin, float scale, SpriteEffects effects, float layerDepth);
        }

        #endregion


        #region IMPLEMENTATION OF IROTATION

        /// <summary>
        /// Property which allows only read access to the point a texture is drawn
        /// </summary>
        public Vector2 DrawOrigin
        {
            get
            {
                // RETURN value of _drawOrigin:
                return _drawOrigin;
            }
        }

        /// <summary>
        /// Property which allows read and write access to how much a texture is rotated
        /// </summary>
        public float Angle
        {
            get
            {
                // RETURN value of _rotAngle:
                return _rotAngle;
            }
            set
            {
                // SET value of _rotAngle to incoming value:
                _rotAngle = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows access to get or set value of 'texture'
        /// </summary>
        public override Texture2D Texture
        {
            get
            {
                // RETURN value of current texture:
                return _texture;
            }
            set
            {
                // INITIALISE _texture with incoming value:
                _texture = value;

                // INSTANTIATE _textureSize as a new Point, using _texture's dimensions as parameters:
                _textureSize = new Point(_texture.Width, _texture.Height);

                // INSTANTIATE _drawOrigin as a new Vector2, with value of centre of _texture:
                _drawOrigin = new Vector2(_texture.Width / 2, _texture.Height / 2);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IVELOCITY

        /// <summary>
        /// Property which allows access to get value of an entity's velocity
        /// </summary>
        public Vector2 Velocity
        {
            get
            {
                // RETURN value of _velocity(x,y)
                return _velocity;
            }
            set
            {
                // SET value of _velocity to incoming value:
                _velocity = value;
            }
        }

        #endregion
    }
}
