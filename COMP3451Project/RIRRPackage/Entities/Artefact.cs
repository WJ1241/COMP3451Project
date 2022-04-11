using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.Exceptions;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds a Artefact entity on screen
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 10/04/22
    /// </summary>
    public class Artefact : SimpleCollidableEntity, ICollidable, ICollisionListener
    {

        #region CONSTRUCTOR

        /// <summary>
        /// CONSTRUCTOR for artefact class
        /// </summary>
        public Artefact()
        {
            //empty constructor
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
