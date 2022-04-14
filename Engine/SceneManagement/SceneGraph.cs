using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.SceneManagement
{
    /// <summary>
    /// Class which holds reference to list in Scene Manager, Draws and Updates entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    /// <REFERENCE> Abuhakmeh, K. (2009) XNA 2D Camera Engine That Follows Sprite. Available at: https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite. (Accessed: 20 April 2021). </REFERENCE>
    public class SceneGraph : ISceneGraph, IDraw, IEventListener<MatrixEventArgs>, IInitialiseParam<ICollisionManager>, IInitialiseParam<IDictionary<string, ICommand>>, IInitialiseParam<IDictionary<string, IEntity>>,
        IInitialiseParam<IFuncCommand<ICommand>>, IInitialiseParam<string, ICommand>, IName, IResetScene, ISpawn, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IEntity>, name it '_sceneEntDict':
        private IDictionary<string, IEntity> _sceneEntDict;

        // DECLARE an IDictionary<string, ICommand>, name it '_commandDict':
        private IDictionary<string, ICommand> _commandDict;

        // DECLARE an IFuncCommand<ICommand>, name it '_createCommand':
        private IFuncCommand<ICommand> _createCommand;

        // DECLARE a Matrix, name it '_zoomFollowMatrix':
        private Matrix _zoomFollowMatrix;

        // DECLARE a string, name it '_sceneName':
        private string _sceneName;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SceneGraph
        /// </summary>
        public SceneGraph()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEGRAPH

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="pUName"> Used for passing unique name </param>
        public void RemoveEntity(string pUName)
        {
            // REMOVE IEntity object addressed by pUName from _sceneEntDict:
            _sceneEntDict.Remove(pUName);
        }

        /// <summary>
        /// Removes and terminates any references to entities from the scene
        /// </summary>
        public void ClearScene()
        {
            // FOREACH IEntity in _sceneEntDict.Values:
            foreach (IEntity pEntity in _sceneEntDict.Values.ToList())
            {
                // CALL Terminate() on pEntity:
                (pEntity as ITerminate).Terminate();
            }

            // EXECUTE _commandDict["StopAudio"]:
            _commandDict["StopAudio"].ExecuteMethod();
        }

        /// <summary>
        /// Clears all references to entities in current scene and signals command to load next scene
        /// </summary>
        /// <param name="pNextScene"> Name of next scene </param>
        public void GoToNextScene(string pNextScene)
        {
            // CALL ClearScene():
            ClearScene();

            // INITIALISE _commandDict["NextScene"]'s FirstParam Property with value of _sceneName:
            (_commandDict["NextScene"] as ICommandTwoParam<string, string>).FirstParam = _sceneName;

            // INITIALISE _commandDict["NextScene"]'s SecondParam Property with value of pNextScene:
            (_commandDict["NextScene"] as ICommandTwoParam<string, string>).SecondParam = pNextScene;

            // EXECUTE _commandDict["NextScene"]:
            _commandDict["NextScene"].ExecuteMethod();
        }

        /// <summary>
        /// Returns Scene Dictionary, used for testing
        /// </summary>
        /// <returns> IDictionary<string, IEntity> object </returns>
        public IDictionary<string, IEntity> ReturnSceneDict()
        {
            // RETURN instance of _sceneEntDict:
            return _sceneEntDict;
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // IF "Camera" IS stored in _sceneEntDict:
            if (_sceneEntDict.ContainsKey("Camera"))
            {
                // BEGIN creation of displayable objects:
                pSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, _zoomFollowMatrix);
            }
            // IF "Camera" IS NOT stored in _sceneEntDict:
            else
            {
                // BEGIN creation of displayable objects:
                pSpriteBatch.Begin();
            }

            // FOREACH IEntity in _sceneEntDict.Values:
            foreach (IEntity pEntity in _sceneEntDict.Values)
            {
                // IF pEntity implements IDraw:
                if (pEntity is IDraw)
                {
                    // CALL Draw() on pEntity, passing pSpriteBatch as a parameter:
                    (pEntity as IDraw).Draw(pSpriteBatch);
                }
            }

            // END creation of displayable objects:
            pSpriteBatch.End();
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<MATRIXEVENTARGS>

        /// <summary>
        /// Event called when needing to update Matrix value for Draw() Method
        /// </summary>
        /// <param name="pSource"> Caller of Event </param>
        /// <param name="pArgs"> MatrixEventArgs object </param>
        public void OnEvent(object pSource, MatrixEventArgs pArgs)
        {
            // INITIALISE _zoomFollowMatrix with value of pArgs' RequiredArg Property:
            _zoomFollowMatrix = pArgs.RequiredArg;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOLLISIONMANAGER>

        /// <summary>
        /// Initialises an object with an ICollisionManager object
        /// </summary>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        public void Initialise(ICollisionManager pCollisionManager)
        {
            // IF pCollisionManager DOES HAVE an active instance:
            if (pCollisionManager != null)
            {
                // INITIALISE pCollisionManager with _sceneEntDict:
                pCollisionManager.Initialise(_sceneEntDict as IReadOnlyDictionary<string, IEntity>);
            }
            // IF pCollisionManager DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCollisionManager does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, ICOMMAND>

        /// <summary>
        /// Initialises an object with a string and an ICommand object
        /// </summary>
        /// <param name="pCommandName"> Name of Command </param>
        /// <param name="pCommand"> ICommand object </param>
        public void Initialise(string pCommandName, ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // IF _commandDict DOES NOT already contain pCommandName as a key:
                if (!_commandDict.ContainsKey(pCommandName))
                {
                    // ADD pCommandName as a key, and pCommand as a value to _commandDict:
                    _commandDict.Add(pCommandName, pCommand);
                }
                // IF _commandDict DOES already contain pCommandName as a key:
                else
                {
                    // THROW a new NullInstanceException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pCommandName is already stored in _commandDict!");
                }
            }
            // IF pCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
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
                // INITIALISE _commandDict with reference to pCommandDict:
                _commandDict = pCommandDict;
            }
            // IF pCommandDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: _commandDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, IENTITY>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, IEntity> instance
        /// </summary>
        /// <param name="pSceneEntDict"> IDictionary<string, IEntity> instance </param>
        public void Initialise(IDictionary<string, IEntity> pSceneEntDict)
        {
            // IF pSceneEntDict DOES HAVE an active instance:
            if (pSceneEntDict != null)
            {
                // INITIALISE _sceneEntDict with reference to pSceneEntDict:
                _sceneEntDict = pSceneEntDict;
            }
            // IF pSceneEntDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pSceneEntDict does not have an active instance!");
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


        #region IMPLEMENTATION OF INAME

        /// <summary>
        /// Property which allows read and write access to the value of an object's specific name
        /// </summary>
        public string Name
        {
            get
            {
                // RETURN value of _sceneName:
                return _sceneName;
            }
            set
            {
                // SET value of _sceneName to incoming value:
                _sceneName = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IRESETSCENE

        /// <summary>
        /// Clears all references to entities in current scene and signals command to reset current scene
        /// </summary>
        public void ResetScene()
        {
            // CALL ClearScene();
            ClearScene();

            // INITIALISE _commandDict["ResetScene"]'s FirstParam with value of _sceneName:
            (_commandDict["ResetScene"] as ICommandOneParam<string>).FirstParam = _sceneName;

            // EXECUTE _commandDict["ResetScene"]:
            _commandDict["ResetScene"].ExecuteMethod();
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

            // IF pEntity implements IEntityInternal:
            if (pEntity is IEntityInternal)
            {
                // DECLARE & INSTANTIATE an ICommandOneParam<string> as a new CommandOneParam<string>(), name it 'removeMe':
                ICommandOneParam<string> removeMe = _createCommand.ExecuteMethod() as ICommandOneParam<string>;

                // SET MethodRef of removeMe with RemoveEntity method:
                removeMe.MethodRef = RemoveEntity;

                // SET FirstParam of removeMe with pEntity's UName Property:
                removeMe.FirstParam = pEntity.UName;

                // SET RemoveMe property of pEntity with removeMe Command:
                (pEntity as IEntityInternal).RemoveMe = removeMe;
            }

            #endregion


            #region SPAWN LOCATION

            // IF pEntity's Position Property DOES NOT return the same value as pPosition:
            if (pEntity.Position != pPosition)
            {
                // INITIALISE pEntity.Position with value of pPosition:
                pEntity.Position = pPosition;
            }

            // WRITE to console, alerting when object has been added to the scene:
            Console.WriteLine(pEntity.UName + " ID:" + pEntity.UID + " has been Spawned on Scene!");
            
            #endregion
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // FOREACH IEntity in _sceneEntDict.Values:
            foreach (IEntity pEntity in _sceneEntDict.Values)
            {
                // IF pEntity implements IUpdatable:
                if (pEntity is IUpdatable)
                {
                    // CALL Update() on pEntity, passing pGameTime as a parameter:
                    (pEntity as IUpdatable).Update(pGameTime);
                }
            }
        }

        #endregion
    }
}
