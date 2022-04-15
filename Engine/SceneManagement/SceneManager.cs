using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.SceneManagement
{
    /// <summary>
    /// Class which manages all entities in the scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    public class SceneManager : ISceneManager, IDraw, IInitialiseParam<IDictionary<string, ICommand>>, IInitialiseParam<IDictionary<string, ISceneGraph>>, IInitialiseParam<IFactory<ISceneGraph>>, IInitialiseParam<IFuncCommand<ICommand>>,
        IService, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, ISceneGraph>, name it '_sGDict':
        private IDictionary<string, ISceneGraph> _sGDict;

        // DECLARE an IDictionary<string, ICommand>, name it '_sceneLoadDict':
        private IDictionary<string, ICommand> _sceneLoadDict;

        // DECLARE an IFactory<ISceneGraph>, name it '_sGFactory':
        private IFactory<ISceneGraph> _sGFactory;

        // DECLARE an IFuncCommand<ICommand>, name it '_createCommand':
        private IFuncCommand<ICommand> _createCommand;

        // DECLARE a string, name it '_currentScene':
        private string _currentScene;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SceneManager
        /// </summary>
        public SceneManager()
        {
            // INITIALISE _currentScene with a blank string, so that the scene can be loaded empty without any scene rather than crashing:
            _currentScene = "";
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEMANAGER

        /// <summary>
        /// Creates a Scene which stores entities and their positions
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pCommandDict"> Dictionary to store commands in </param>
        /// <param name="pLoadSceneCommand"> Command to Load Scene when needed </param>
        public void CreateScene(string pSceneName, IDictionary<string, ICommand> pCommandDict, ICommand pLoadSceneCommand)
        {
            // IF pLoadSceneCommand DOES HAVE an active instance:
            if (pLoadSceneCommand != null)
            {
                // INITIALISE _currentScene with value of pSceneName:
                _currentScene = pSceneName;

                // ADD _currentScene as a key, and a new LevelSceneGraph() to _sGDict:
                _sGDict.Add(_currentScene, _sGFactory.Create<LevelSceneGraph>());

                // IF _sceneLoadDict DOES NOT already contain pSceneName:
                if (!_sceneLoadDict.ContainsKey(pSceneName))
                {
                    // ADD pSceneName as a key, and a reference to pLoadSceneCommand to _sceneLoadDict:
                    _sceneLoadDict.Add(pSceneName, pLoadSceneCommand);
                }

                // INITIALISE _sGDict[pSceneName] with a reference to pCommandDict:
                (_sGDict[pSceneName] as IInitialiseParam<IDictionary<string, ICommand>>).Initialise(pCommandDict);

                // DECLARE & INSTANTIATE an ICommandOneParam<string> as a new CommandOneParam<string>(), name it 'resetSceneCommand':
                ICommandOneParam<string> resetSceneCommand = _createCommand.ExecuteMethod() as ICommandOneParam<string>;

                // INITIALISE resetSceneCommand with reference to ResetScene():
                resetSceneCommand.MethodRef = ResetScene;

                // INITIALISE _sGDict[pSceneName] with "ResetScene" and a reference to resetSceneCommand as parameters:
                (_sGDict[pSceneName] as IInitialiseParam<string, ICommand>).Initialise("ResetScene", resetSceneCommand);
            }
            // IF pLoadSceneCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pLoadSceneCommand does not have an active instance!");
            }
        }

        /// <summary>
        /// Creates a Cutscene which stores entities and their positions
        /// </summary>
        /// <param name="pCutsceneName"> Name of Cutscene </param>
        /// <param name="pCommandDict"> Dictionary to store commands in </param>
        /// <param name="pLoadSceneCommand"> Command to Load Scene when needed </param>
        public void CreateCutscene(string pCutsceneName, IDictionary<string, ICommand> pCommandDict, ICommand pLoadSceneCommand)
        {
            // IF pLoadSceneCommand DOES HAVE an active instance:
            if (pLoadSceneCommand != null)
            {
                // INITIALISE _currentScene with value of pCutsceneName:
                _currentScene = pCutsceneName;

                // ADD _currentScene as a key, and a new CutsceneGraph() to _sGDict:
                _sGDict.Add(_currentScene, _sGFactory.Create<CutsceneGraph>());

                // IF _sceneLoadDict DOES NOT already contain pCutsceneName:
                if (!_sceneLoadDict.ContainsKey(pCutsceneName))
                {
                    // ADD pCutsceneName as a key, and a reference to pLoadSceneCommand to _sceneLoadDict:
                    _sceneLoadDict.Add(pCutsceneName, pLoadSceneCommand);
                }

                // INITIALISE _sGDict[pCutsceneName] with a reference to pCommandDict:
                (_sGDict[pCutsceneName] as IInitialiseParam<IDictionary<string, ICommand>>).Initialise(pCommandDict);
            }
            // IF pLoadSceneCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pLoadSceneCommand does not have an active instance!");
            }
        }

        /// <summary>
        /// Uploads the next scene so there can be scene transitioning happening from one scene
        /// </summary>
        /// <param name="pNextSceneName"> Name of Next Scene </param>
        /// <param name="pLoadNextSceneCommand"> Command to keep ready for next scene </param>
        public void UploadNextScene(string pNextSceneName, ICommand pLoadNextSceneCommand)
        {
            // IF pLoadNextSceneCommand DOES HAVE an active instance:
            if (pLoadNextSceneCommand != null)
            {
                // IF _sceneLoadDict DOES NOT already contain pNextSceneName:
                if (!_sceneLoadDict.ContainsKey(pNextSceneName))
                {
                    // ADD pNextSceneName as a key, and a reference to pLoadNextSceneCommand to _sceneLoadDict:
                    _sceneLoadDict.Add(pNextSceneName, pLoadNextSceneCommand);
                }
            }
            // IF pLoadNextSceneCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pLoadNextSceneCommand does not have an active instance!");
            }
        }

        /// <summary>
        /// Removes current scene references and loads the next scene for the user
        /// </summary>
        /// <param name="pPrevScene"> Name of previous scene </param>
        /// <param name="pNextScene"> Name of next scene </param>
        public void LoadNextScene(string pPrevScene, string pNextScene)
        {
            // CALL RemoveScene(), passing pPrevScene as a parameter:
            RemoveScene(pPrevScene);

            // IF _sceneLoadDict DOES contain pNextScene as a key:
            if (_sceneLoadDict.ContainsKey(pNextScene))
            {
                // CALL ExecuteMethod() on _sceneLoadDict[pNextScene]:
                _sceneLoadDict[pNextScene].ExecuteMethod();
            }
        }

        /// <summary>
        /// Removes current scene references and reloads the current scene for the user
        /// </summary>
        /// <param name="pCurrentScene"> Name of current scene </param>
        public void ResetScene(string pCurrentScene)
        {
            // REMOVE ISceneGraph instance stored using pCurrentScene from _sGDict:
            _sGDict.Remove(pCurrentScene);

            // CALL ExecuteMethod() on _sceneLoadDict[pCurrentScene]:
            _sceneLoadDict[pCurrentScene].ExecuteMethod();
        }

        /// <summary>
        /// Removes a Scene/Cutscene specified by its Name
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        public void RemoveScene(string pSceneName)
        {
            // REMOVE ISceneGraph instance stored using pSceneName from _sGDict:
            _sGDict.Remove(pSceneName);

            // REMOVE ICommand instance stored using pSceneName from _sceneLoadDict:
            _sceneLoadDict.Remove(pSceneName);
        }

        /// <summary>
        /// Returns Current loaded scene
        /// </summary>
        /// <returns> Current Loaded Scene </returns>
        public ISceneGraph ReturnCurrentScene()
        {
            // RETURN instance of _sgDict[_currentScene]:
            return _sGDict[_currentScene];
        }

        /// <summary>
        /// Initialises a specified scene with a string, an IDictionary<string, IEntity> object and an IFuncCommand<ICommand> object
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pEntDict"> IDictionary<string, IEntity> object </param>
        /// <param name="pCreateCommand"> IFuncCommand<ICommand> object </param>
        public void Initialise(string pSceneName, IDictionary<string, IEntity> pEntDict, IFuncCommand<ICommand> pCreateCommand)
        {
            // TRY checking if Initialise() throws a NullInstanceException:
            try
            {
                // IF _sGDict DOES contain pSceneName as a key already:
                if (_sGDict.ContainsKey(pSceneName))
                {
                    // SET Name Property of _sGDict[pSceneName] to equal pSceneName:
                    (_sGDict[pSceneName] as IName).Name = pSceneName;

                    // INITIALISE _sGDict[pSceneName] with reference to pEntDict:
                    (_sGDict[pSceneName] as IInitialiseParam<IDictionary<string, IEntity>>).Initialise(pEntDict);

                    // INITIALISE _sGDict[pSceneName] with reference to pCreateCommand:
                    (_sGDict[pSceneName] as IInitialiseParam<IFuncCommand<ICommand>>).Initialise(pCreateCommand);
                }
                // IF _sGDict DOES NOT contain pSceneName as a key:
                else
                {
                    // THROW a new NullValueException(), with corresponding message:
                    throw new NullValueException("ERROR: _sGDict does not contain" + pSceneName + "as a key!");
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
        /// Initialises a specified scene with an ICollisionManager object, an IDictionary<string, IEntity> object and an IFuncCommand<ICommand> object
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        /// <param name="pEntDict"> IDictionary<string, IEntity> object </param>
        /// <param name="pCreateCommand"> IFuncCommand<ICommand> object </param>
        public void Initialise(string pSceneName, ICollisionManager pCollisionManager, IDictionary<string, IEntity> pEntDict, IFuncCommand<ICommand> pCreateCommand)
        {
            // TRY checking if Initialise() throws a NullInstanceException:
            try
            {
                // IF _sGDict DOES contain pSceneName as a key already:
                if (_sGDict.ContainsKey(pSceneName))
                {
                    // CALL Initialise(), passing pSceneName, pEntDict and pCreateCommand as parameters:
                    Initialise(pSceneName, pEntDict, pCreateCommand);

                    // INITIALISE _sGDict[pSceneName] with reference to pCollisionManager:
                    (_sGDict[pSceneName] as IInitialiseParam<ICollisionManager>).Initialise(pCollisionManager);
                }
                // IF _sGDict DOES NOT contain pSceneName as a key:
                else
                {
                    // THROW a new NullValueException(), with corresponding message:
                    throw new NullValueException("ERROR: _sGDict does not contain" + pSceneName + "as a key!");
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


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // IF _sgDict DOES contain _currentScene as a key:
            if (_sGDict.ContainsKey(_currentScene))
            {
                // CALL Draw() on _sGDict[_currentScene], passing spriteBatch as a parameter:
                (_sGDict[_currentScene] as IDraw).Draw(pSpriteBatch);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, ICOMMAND>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, ICommand> instance
        /// </summary>
        /// <param name="pCommandDict"> IDictionary<string, ICommand> instance </param>
        public void Initialise(IDictionary<string, ICommand> pCommandDict)
        {
            // IF pCommandDict DOES HAVE an active instance:
            if (pCommandDict != null)
            {
                // INITIALISE _sceneLoadDict with reference to pCommandDict:
                _sceneLoadDict = pCommandDict;
            }
            // IF pCommandDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pCommandDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, ISCENEGRAPH>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, ISceneGraph> instance
        /// </summary>
        /// <param name="pSGDict"> IDictionary<string, ISceneGraph> instance </param>
        public void Initialise(IDictionary<string, ISceneGraph> pSGDict)
        {
            // IF pSGDict DOES HAVE an active instance:
            if (pSGDict != null)
            {
                // INITIALISE _sGDict with reference to pSGDict:
                _sGDict = pSGDict;
            }
            // IF pSGDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pSGDict does not have an active instance!");
            }
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFUNCCOMMAND<ICOMMAND>>

        /// <summary>
        /// Initialises an object with an IFuncCommand<ICommand> object
        /// </summary>
        /// <param name="pFuncCommand"> IFuncCommand<ICommand> object </param>
        public void Initialise(IFuncCommand<ICommand> pFuncCommand)
        {
            // IF pFuncCommand DOES HAVE an active instance:
            if (pFuncCommand != null)
            {
                // INITIALISE _createCommand with reference to pFuncCommand:
                _createCommand = pFuncCommand;
            }
            // IF pFuncCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pFuncCommand does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // IF _sgDict DOES contain _currentScene as a key:
            if (_sGDict.ContainsKey(_currentScene))
            {
                // CALL Update() on _sGDict[_currentScene], passing pGameTime as a parameter:
                (_sGDict[_currentScene] as IUpdatable).Update(pGameTime);
            }
        }

        #endregion
    }
}