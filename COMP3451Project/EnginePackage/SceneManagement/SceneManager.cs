using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Camera;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.Services.Factories;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Class which manages all entities in the scene
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public class SceneManager : ISceneManager, IInitialiseParam<IFactory<ISceneGraph>>, IService, IUpdatable, IDraw, ISpawn, IDrawCamera
    {
        #region FIELD VARIABLES

        // DECLARE an IFactory<ISceneGraph>, name it '_sGFactory':
        private IFactory<ISceneGraph> _sGFactory;

        // DECLARE an ISceneGraph, name it 'sceneGraph':
        private ISceneGraph _sceneGraph;

        // DECLARE an IDictionary<string, IEntity>, name it '_sceneDictionary':
        private IDictionary<string, IEntity> _sceneDictionary;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SceneManager
        /// </summary>
        public SceneManager()
        {
            // INSTANTIATE _sceneDictionary as new Dictionary<string, IEntity>:
            _sceneDictionary = new Dictionary<string, IEntity>();
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEMANAGER

        /// <summary>
        /// Initialises an object with a reference to an ISceneGraph
        /// </summary>
        /// <param name="sceneGraph">Holds References to an ISceneGraph</param>
        public void Initialise(ISceneGraph sceneGraph) 
        {
            // ASSIGNMENT, set instance of _sceneGraph as the same as sceneGraph:
            _sceneGraph = sceneGraph;

            // INITIALISE _sceneGraph, passing _sceneDictionary as a parameter:
            _sceneGraph.Initialise(_sceneDictionary);
        }

        /// <summary>
        /// Initialises an object with a reference to an ICollisionManager
        /// </summary>
        /// <param name="collisionManager">Holds References to an ICollisionManager</param>
        public void Initialise(ICollisionManager collisionManager) 
        {
            // INITIALISE _collisionManager, passing _sceneDictionary cast as an IReadOnlyDictionary<string, IEntity>, as a parameter:
            collisionManager.Initialise((IReadOnlyDictionary<string, IEntity>) _sceneDictionary);
        }

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="uName">Used for passing unique name</param>
        public void RemoveInstance(string uName)
        {
            // CALL Remove(), on Dictionary to remove 'value' of key 'uName':
            _sceneDictionary.Remove(uName);

            // WRITE to console, alerting when object has been removed from scene:
            Console.WriteLine(uName + " has been Removed from Scene!");
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFACTORY<ISCENEGRAPH>>

        /// <summary>
        /// Initialises an object with an IFactory<ISceneGraph> object
        /// </summary>
        /// <param name="pFactory"> IFactory<ISceneGraph> object </param>
        public void Initialise(IFactory<ISceneGraph> pFactory)
        {
            // INITIALISE _sGFactory with reference to pFactory:
            _sGFactory = pFactory;
        }

        #endregion


        #region IMPLEMENTATION OF ISPAWN

        /// <summary>
        /// Spawns Entity on screen and adds to a list/dictionary
        /// </summary>
        /// <param name="entity">Reference to an instance of IEntity</param>
        /// <param name="position">Positional values used to place entity</param>
        public void Spawn(IEntity pEntity, Vector2 pPosition)
        {
            #region ADD TO DICTIONARY

            // ADD entity to Dictionary<string, IEntity>:
            _sceneDictionary.Add(pEntity.UName, pEntity);

            #endregion


            #region REMOVE COMMAND

            // DECLARE an ICommandOneParam<string>, name it '_removeMe':
            ICommandOneParam<string> _removeMe = new CommandOneParam<string>();

            // SET MethodRef of _removeMe with RemoveInstance method:
            _removeMe.MethodRef = RemoveInstance;

            // SET Data of _removeMe with pEntity's UName Property:
            _removeMe.Data = pEntity.UName;

            // SET RemoveMe property of pEntity with _removeMe Command:
            (pEntity as IEntityInternal).RemoveMe = _removeMe;

            #endregion


            #region SPAWN LOCATION

            // CALL Spawn() on _sceneGraph, passing pEntity and pPosition as parameters:
            (_sceneGraph as ISpawn).Spawn(pEntity, pPosition);

            // WRITE to console, alerting when object has been added to the scene:
            Console.WriteLine(pEntity.UName + " ID:" + pEntity.UID + " has been Spawned on Scene!");

            #endregion
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="spritebatch">Needed to draw entity's texture on screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // CALL Draw() on _sceneGraph, passing spriteBatch as a parameter:
            (_sceneGraph as IDraw).Draw(spriteBatch);
        }

        /// <summary>
        /// When called, draws entity's texture on screen, as well as reposition a a camera object
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        /// <param name="camera">Needed to move camera position on screen</param>
        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            // CALL Draw() on _sceneGraph, passing spriteBatch and camera as parameters:
            (_sceneGraph as IDrawCamera).Draw(spriteBatch, camera);
        }

        #endregion
        

        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        public void Update(GameTime gameTime)
        {
            // CALL Update() on _sceneGraph, passing gameTime as a parameter:
            (_sceneGraph as IUpdatable).Update(gameTime);
        }

        #endregion
    }
}