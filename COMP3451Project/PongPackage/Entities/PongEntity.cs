using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.Entities
{
    /// <summary>
    /// Abstract class for Pong Entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
    /// </summary>
    public abstract class PongEntity : Entity, IInitialiseParam<IUpdateEventListener>, IDraw, IUpdatable, ITexture, IVelocity
    {
        #region FIELD VARIABLES

        // DECLARE a Texture2D, call it '_texture':
        protected Texture2D _texture;

        // DECLARE a Vector2, call it '_velocity':
        protected Vector2 _velocity;

        // DECLARE a float, call it 'speed':
        protected float _speed;

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public abstract void Initialise(IUpdateEventListener pUpdateEventListener);

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpritebatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, and colour
            pSpriteBatch.Draw(_texture, _position, Color.AntiqueWhite);
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public abstract void Update(GameTime pGameTime);

        #endregion


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows access to get or set value of 'texture'
        /// </summary>
        public Texture2D Texture
        {
            get
            {
                // RETURN value of current texture:
                return _texture;
            }
            set
            {
                // ASSIGNMENT give texture value of whichever class is modifying value:
                _texture = value;
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
