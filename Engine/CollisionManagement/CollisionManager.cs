using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.CollisionManagement
{
    /// <summary>
    /// Class which stores references to entities that can collide with other entities
    /// Authors: William Smith, Declan Kerby-Collins & Marc Price
    /// Date: 07/04/22
    /// </summary>
    /// <REFERENCE> Price, M. (2021) ‘Session 16 - Collision Management’, Games Design & Engineering: Sessions. Available at: https://worcesterbb.blackboard.com. (Accessed: 17 February 2021). </REFERENCE>
    public class CollisionManager : ICollisionManager, IInitialiseParam<IList<ICollidable>>, IUpdatable, IService
    {
        #region FIELD VARIABLES

        // DECLARE an IReadOnlyDictionary, name it '_entityDictionary', used as CollisionManager should not modify entity Dictionary:
        private IReadOnlyDictionary<string, IEntity> _entityDictionary;

        // DECLARE an IList<ICollidable>, name it '_collidableList', used to store objects implementing ICollidable:
        private IList<ICollidable> _collidableList;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of CollisionManager
        /// </summary>
        public CollisionManager() 
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ILIST<ICOLLIDABLE>>

        /// <summary>
        /// Initialises an object with an IList<ICollidable> instance
        /// </summary>
        /// <param name="pCollidableList"> IList<ICollidable> instance </param>
        public void Initialise(IList<ICollidable> pCollidableList)
        {
            // IF pCollidableList DOES HAVE an active instance:
            if (pCollidableList != null)
            {
                // INITIALISE _collidableList with reference to pCollidableList:
                _collidableList = pCollidableList;
            }
            // IF pCollidableList DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCollidableList does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONMANAGER

        /// <summary>
        /// Initialises object with an IReadOnlyDictionary<string, IEntity>
        /// </summary>
        /// <param name="pEntityDictionary">holds reference to 'IReadOnlyDictionary<string, IEntity>'</param>
        public void Initialise(IReadOnlyDictionary<string, IEntity> pEntityDictionary)
        {
            // IF pEntityDictionary DOES HAVE an active instance:
            if (pEntityDictionary != null)
            {
                // ASSIGNMENT, set value of '_entityDictionary' as the same instance as 'entityDictionary'
                _entityDictionary = pEntityDictionary;
            }
            // IF pEntityDictionary DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pEntity does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        /// <CITATION> (Price, 2021) </CITATION>
        public void Update(GameTime pGameTime)
        {
            // CALL Clear() on _collidableList to remove old references
            _collidableList.Clear();

            // FOREACH IEntity in _entityDictionary.values:
            foreach (IEntity pEntity in _entityDictionary.Values)
            {
                // IF pEntity implements ICollidable:
                if (pEntity is ICollidable)
                {
                    // ADD pEntity to _collidableList:
                    _collidableList.Add(pEntity as ICollidable);
                }
            }

            // FORLOOP, _collidableList.Count - 1, so that object cannot collide with itself:
            for (int i = 0; i < (_collidableList.Count - 1); i++)
            {
                // FORLOOP, j = i + 1, so that object cannot collide with itself:
                for (int j = i + 1; j < _collidableList.Count; j++)
                {
                    // CALL 'CollideResponse()' passing two ICollidables as parameters, used to determine which ICollidable objects change on Collision:
                    CollideResponse(_collidableList[i], _collidableList[j]);
                }
            }
        }

        #endregion


        #region PRIVATE METHODS

        /// <summary>
        /// Called to check when two or more ICollidable objects have collided
        /// </summary>
        /// <param name="pFrstEntity">Reference to the first ICollidable in the collision</param>
        /// <param name="pScndEntity">Reference to the second ICollidable in the collision</param>
        /// <CITATION> (Price, 2021) </CITATION>
        private void CollideResponse(ICollidable pFrstEntity, ICollidable pScndEntity)
        {
            // IF both hitboxes have collided:
            if (pFrstEntity.HitBox.Intersects(pScndEntity.HitBox))
            {
                // IF pFrstEntity implements ICollisionListener:
                if (pFrstEntity is ICollisionListener)
                {
                    // CALL 'OnCollision()' passing an ICollidable as a parameter, used to modify entity depending on what type of collision:
                    (pFrstEntity as ICollisionListener).OnCollision(pScndEntity);
                }
                // IF pScndEntity implements ICollisionListener:
                else if (pScndEntity is ICollisionListener)
                {
                    // CALL 'OnCollision()' passing an ICollidable as a parameter, used to modify entity depending on what type of collision:
                    (pScndEntity as ICollisionListener).OnCollision(pFrstEntity);
                }
            }
        }

        #endregion

    }
}
