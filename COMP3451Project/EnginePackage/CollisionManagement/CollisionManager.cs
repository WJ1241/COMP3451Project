using System.Collections.Generic;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.CollisionManagement
{
    /// <summary>
    /// Class which stores references to entities that can collide with other entities
    /// </summary>
    /// <REFERENCE> Price, M. (2021) ‘Session 16 - Collision Management’, Games Design & Engineering: Sessions. Available at: https://worcesterbb.blackboard.com. (Accessed: 17 February 2021). </REFERENCE>
    public class CollisionManager : ICollisionManager, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IReadOnlyDictionary, call it '_entityDictionary', used as CollisionManager should not modify entity Dictionary:
        private IReadOnlyDictionary<string, IEntity> _entityDictionary;

        // DECLARE an IList<ICollidable>, call it '_collidableList', used to store objects implementing ICollidable:
        private IList<ICollidable> _collidableList;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of CollsionManager
        /// </summary>
        public CollisionManager() 
        {
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONMANAGER

        /// <summary>
        /// Initialises object with a IReadOnlyDictionary<string, IEntity>
        /// </summary>
        /// <param name="entityDictionary">holds reference to 'IReadOnlyDictionary<string, IEntity>'</param>
        public void Initialise(IReadOnlyDictionary<string, IEntity> entityDictionary)
        {
            // ASSIGNMENT, set value of '_entityDictionary' as the same instance as 'entityDictionary'
            _entityDictionary = entityDictionary;
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        /// <CITATION> (Price, 2021) </CITATION>
        public void Update(GameTime gameTime)
        {
            // INSTANTIATE a new List<ICollidable>, newly created instance on update, allows for changes from entity Dictionary:
            _collidableList = new List<ICollidable>();

            foreach (IEntity entity in _entityDictionary.Values) // FOREACH IEntity object in _entityDictionary
            {
                if (entity is ICollidable) // IF entity implements ICollidable
                {
                    // ADD entity to list of ICollidable:
                    _collidableList.Add(entity as ICollidable);
                }
            }

            for (int i = 0; i < (_collidableList.Count - 1); i++) // List.Count - 1, so that object cannot collide with itself
            {
                for (int j = i + 1; j < _collidableList.Count; j++) // j = i + 1, so that object cannot collide with itself
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
        /// <param name="frstEntity">Reference to the first ICollidable in the collision</param>
        /// <param name="scndEntity">Reference to the second ICollidable in the collision</param>
        /// <CITATION> (Price, 2021) </CITATION>
        private void CollideResponse(ICollidable frstEntity, ICollidable scndEntity)
        {
            if (frstEntity.HitBox.Intersects(scndEntity.HitBox)) // IF both hitboxes have collided
            {
                if (frstEntity is ICollisionListener) // IF frstEntity implements ICollisionListener
                {
                    // CALL 'OnCollision()' passing an ICollidable as a parameter, used to modify entity depending on what type of collision:
                    (frstEntity as ICollisionListener).OnCollision(scndEntity);
                }
                else if (scndEntity is ICollisionListener) // IF scndEntity implements ICollisionListener
                {
                    // CALL 'OnCollision()' passing an ICollidable as a parameter, used to modify entity depending on what type of collision:
                    (scndEntity as ICollisionListener).OnCollision(frstEntity);
                }
            }
        }

        #endregion

    }
}
