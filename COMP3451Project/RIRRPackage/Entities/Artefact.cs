using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Exceptions;

namespace COMP3451Project.RIRRPackage.Entities
{
    public class Artefact : PongEntity, ICollidable, ICollisionListener
    {
        /// <summary>
        /// CONSTRUCTOR for artefact class
        /// </summary>
        public Artefact()
        {
            //empty contructor
        }


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public override void Update(GameTime pGameTime)
        {
            // CALL Update() on _currentState, passing pGameTime as a parameter:
            (_currentState as IUpdatable).Update(pGameTime);
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
                // RETURN a rectangle, object current X axis location, object current Y axis location, texture width size, texture height size:
                return new Rectangle((int)_position.X, (int)_position.Y, _textureSize.X, _textureSize.Y);
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
