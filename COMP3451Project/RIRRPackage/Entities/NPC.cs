using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds an NPC entity on screen
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 13/04/22
    /// </summary>
    public class NPC : RIRREntity, ICollidable, ICollisionListener, IDrawSourceRectangle
    {
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of NPC
        /// </summary>
        public NPC()
        {
            // INITIALISE _speed with a value of '1':
            _speed = 1;
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLIDABLE

        /// <summary>
        /// Used to Return a rectangle object to caller of property
        /// </summary>
        public Rectangle HitBox
        {
            get
            {
                // RETURN new Rectangle() using _position and _textureSize as parameters:
                return new Rectangle((int)_position.X - (int)_drawOrigin.X, (int)_position.Y - (int)_drawOrigin.Y, _textureSize.X, _textureSize.Y);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given location, _sourceRectangle, colour, rotation angle, origin point, scale, effects and draw layer:
            pSpriteBatch.Draw(_texture, _position, _sourceRectangle, Color.White, _rotAngle, _drawOrigin, 1f, SpriteEffects.None, 1f);
        }

        #endregion


        #region IMPLEMENTATION OF IDRAWSOURCERECTANGLE

        /// <summary>
        /// Property which allows read and write access to a Draw Position Rectangle
        /// </summary>
        public Rectangle SourceRectangle
        {
            get
            {
                // RETURN value of _sourceRectangle:
                return _sourceRectangle;
            }
            set
            {
                // SET value of _sourceRectangle to incoming value:
                _sourceRectangle = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONLISTENER

        /// <summary>
        /// Called by Collision Manager when two entities collide
        /// </summary>
        /// <param name="pScndCollidable"> Other entity implementing ICollidable </param>
        public void OnCollision(ICollidable pScndCollidable)
        {
            // CALL OnCollision() on _currentState, passing pScndCollidable as a parameter:
            (_currentState as ICollisionListener).OnCollision(pScndCollidable);
            
        }

        #endregion
    }
}
