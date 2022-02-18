using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Camera.Interfaces;
using COMP3451Project.EnginePackage.CollisionManagement.Interfaces;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement.Interfaces;
using COMP3451Project.EnginePackage.SceneManagement.Interfaces;
using COMP3451Project.EnginePackage.Services.Interfaces;
using COMP3451Project.EnginePackage.Services.Factories.Interfaces;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Class which manages all entities in the scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public class SceneManager : ISceneManager, IDraw, IDrawCamera, IInitialiseParam<IFactory<ISceneGraph>>, IService, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, ISceneGraph>, name it '_sGDict':
        private IDictionary<string, ISceneGraph> _sGDict;

        // DECLARE an IFactory<ISceneGraph>, name it '_sGFactory':
        private IFactory<ISceneGraph> _sGFactory;

        // DECLARE a string, name it '_currentScene':
        private string _currentScene;

        // DECLARE an IMap, name it _map:
        //private Map _map;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SceneManager
        /// </summary>
        public SceneManager()
        {
            // INSTANTIATE _sGDict as a new Dictionary<string, ISceneGraph>():
            _sGDict = new Dictionary<string, ISceneGraph>();
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEMANAGER

        /// <summary>
        /// Creates a Scene which stores entities and their positions
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        public void CreateScene(string pSceneName)
        {
            // INITIALISE _currentScene with value of pSceneName:
            _currentScene = pSceneName;

            // ADD _currentScene as a key, and a new SceneGraph() to _sGDict:
            _sGDict.Add(_currentScene, _sGFactory.Create<SceneGraph>());
        }

        /// <summary>
        /// Initialises a specified scene with an ICollisionManager object
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        public void Initialise(string pSceneName, ICollisionManager pCollisionManager) 
        {
            // INITIALISE _sGDict[pSceneName] with pCollisionManager:
            _sGDict[pSceneName].Initialise(pCollisionManager);
        }

        /// <summary>
        /// Spawns Entity in specified scene and adds to a list/dictionary
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pEntity"> IEntity object </param>
        /// <param name="pPosition"> Positional values used to place entity </param>
        public void Spawn(string pSceneName, IEntity pEntity, Vector2 pPosition)
        {
            // CALL Spawn() on _sgDict[pSceneName], passing pEntity and pPosition as parameters:
            (_sGDict[pSceneName] as ISpawn).Spawn(pEntity, pPosition);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFACTORY<ISCENEGRAPH>>

        /// <summary>
        /// Initialises an object with an IFactory<ISceneGraph> object
        /// </summary>
        /// <param name="pFactory"> IFactory<ISsceneGraph> object </param>
        public void Initialise(IFactory<ISceneGraph> pFactory)
        {
            // INITIALISE _sGFactory with reference to pFactory:
            _sGFactory = pFactory;
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // CALL Draw() on _sGDict[_currentScene], passing spriteBatch as a parameter:
            (_sGDict[_currentScene] as IDraw).Draw(pSpriteBatch);

            // CALL Draw() on _map, passing spriteBatch as a parameter:
            //(_map as IDraw).Draw(spriteBatch);
        }

        #endregion


        #region IMPLEMENTATION OF IDRAWCAMERA

        /// <summary>
        /// When called, draws entity's texture on screen, as well as reposition a a camera object
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        /// <param name="camera">Needed to move camera position on screen</param>
        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            // CALL Draw() on _sGDict[_currentScene], passing spriteBatch and camera as parameters:
            (_sGDict[_currentScene] as IDrawCamera).Draw(spriteBatch, camera);
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // CALL Update() on _sGDict[_currentScene], passing pGameTime as a parameter:
            (_sGDict[_currentScene] as IUpdatable).Update(pGameTime);
        }

        #endregion
    }
}