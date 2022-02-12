using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Camera;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services.Commands;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Class which holds reference to list in Scene Manager, Draws and Updates entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    /// <REFERENCE> Abuhakmeh, K. (2009) XNA 2D Camera Engine That Follows Sprite. Available at: https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite. (Accessed: 20 April 2021). </REFERENCE>
    public class SceneGraph : ISceneGraph, IDraw, ISpawn, IDrawCamera, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IEntity>, name it '_sceneEntDict':
        private IDictionary<string, IEntity> _sceneEntDict;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SceneGraph
        /// </summary>
        public SceneGraph()
        {
            // INSTANTIATE _sceneEntDict as a new Dictionary<string, IEntity>():
            _sceneEntDict = new Dictionary<string, IEntity>();
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEGRAPH

        /// <summary>
        /// Initialises an object with an ICollisionManager object
        /// </summary>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        public void Initialise(ICollisionManager pCollisionManager)
        {
            // INITIALISE pCollisionManager with _sceneEntDict:
            pCollisionManager.Initialise(_sceneEntDict as IReadOnlyDictionary<string, IEntity>);
        }

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="pUName"> Used for passing unique name </param>
        public void RemoveEntity(string pUName)
        {
            // REMOVE IEntity object addressed by pUName from _sceneEntDict:
            _sceneEntDict.Remove(pUName);
        }

        #endregion


        #region IMPLEMENTATION OF ISPAWN

        /// <summary>
        /// Spawns specified Entity and initialises its position
        /// </summary>
        /// <param name="pEntity"> IEntity object </param>
        /// <param name="pPosition"> Positional values used to place entity </param>
        public void Spawn(IEntity pEntity, Vector2 pPosition)
        {
            #region ADD TO DICTIONARY

            // ADD pEntity.UName as a key, and pEntity as a value to _sceneEntDict:
            _sceneEntDict.Add(pEntity.UName, pEntity);

            #endregion


            #region REMOVE COMMAND

            // DECLARE an ICommandOneParam<string>, name it '_removeMe':
            ICommandOneParam<string> _removeMe = new CommandOneParam<string>();

            // SET MethodRef of _removeMe with RemoveEntity method:
            _removeMe.MethodRef = RemoveEntity;

            // SET Data of _removeMe with pEntity's UName Property:
            _removeMe.Data = pEntity.UName;

            // SET RemoveMe property of pEntity with _removeMe Command:
            (pEntity as IEntityInternal).RemoveMe = _removeMe;

            #endregion


            #region SPAWN LOCATION

            // INITIALISE pEntity.Position with value of pPosition:
            pEntity.Position = pPosition;

            // WRITE to console, alerting when object has been added to the scene:
            Console.WriteLine(pEntity.UName + " ID:" + pEntity.UID + " has been Spawned on Scene!");
            
            #endregion
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // BEGIN creation of displayable objects:
            spriteBatch.Begin();

            // FOREACH any entity implementing IDraw:
            foreach (IDraw entity in _sceneEntDict.Values)
            {
                // CALL Draw method on all entities in _entityDictionary:
                entity.Draw(spriteBatch);
            }

            // END creation of displayable objects:
            spriteBatch.End();
        }

        #endregion


        #region IMPLEMENTATION OF IDRAWCAMERA

        /// <summary>
        /// When called, draws entity's texture on screen, as well as reposition a a camera object
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        /// <param name="camera">Needed to move camera position on screen</param>
        /// <CITATION> (Abuhakmeh, 2009) </CITATION>
        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            // BEGIN creation of displayable objects:
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ChngCamPos());

            // FOREACH any entity implementing IDraw:
            foreach (IDraw entity in _sceneEntDict.Values)
            {
                // CALL Draw method on all entities in _entityDictionary:
                entity.Draw(spriteBatch);
            }

            // END creation of displayable objects:
            spriteBatch.End();
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // FOREACH any entity implementing IUpdatable:
            foreach (IEntity entity in _sceneEntDict.Values)
            {
                if (entity is IUpdatable) // IF entity implements IUpdatable
                {
                    // CALL Update() on pEntity, passing pGameTime as a parameter:
                    (entity as IUpdatable).Update(pGameTime);
                }
            }
        }

        #endregion
    }
}
