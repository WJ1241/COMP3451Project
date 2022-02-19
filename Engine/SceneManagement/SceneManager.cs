using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.Camera.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.SceneManagement
{
    /// <summary>
    /// Class which manages all entities in the scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
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
            // TRY checking if Initialise() throws a NullInstanceException:
            try
            {
                // IF _sgDict DOES contain pSceneName as a key already:
                if (_sGDict.ContainsKey(pSceneName))
                {
                    // INITIALISE _sGDict[pSceneName] with pCollisionManager:
                    _sGDict[pSceneName].Initialise(pCollisionManager);
                }
                // IF _sgDict DOES NOT contain pSceneName as a key:
                else
                {
                    // THROW a new NullValueException(), with corresponding message:
                    throw new NullValueException("ERROR: _sgDict does not contain" + pSceneName + "as a key!");
                }
            }
            // CATCH NullInstanceException from Initialise():
            catch (NullInstanceException e)
            {
                // THROW a new NullInstanceException, with corrsponding message:
                throw new NullInstanceException(e.Message);
            }
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
            // IF pFactory DOES HAVE an active instance:
            if (pFactory != null)
            {
                // INITIALISE _sGFactory with reference to pFactory:
                _sGFactory = pFactory;
            }
            // IF pFactory DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pFactory does not have an active instance!");
            }
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