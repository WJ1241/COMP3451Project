using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.SceneManagement
{
    /// <summary>
    /// Class which is used to hold all entities relative to a Cutscene situation, and to update and draw them depending on their case
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    public class CutsceneGraph : ISceneGraph, IDraw, IInitialiseParam<IDictionary<string, ICommand>>, IInitialiseParam<IDictionary<string, IDictionary<int, Texture2D>>>, IInitialiseParam<IDictionary<int, string>>, 
        IInitialiseParam<IFuncCommand<ICommand>>, IInitialiseParam<SpriteFont>, IInitialiseParam<string>, IInitialiseParam<string, ICommand>, IInitialiseParam<IDictionary<string, IEntity>>,
        IInitialiseParam<string, IDictionary<int, Texture2D>>, IInitialiseParam<Vector2>, IName, ISpawn, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IDictionary<int, Texture2D>>, name it '_entityTextureDict':
        private IDictionary<string, IDictionary<int, Texture2D>> _entityTextureDict;

        // DECLARE an IDictionary<string, ICommand>, name it '_commandDict':
        private IDictionary<string, ICommand> _commandDict;

        // DECLARE an IDictionary<string, IEntity>, name it '_cutsceneEntDict':
        private IDictionary<string, IEntity> _cutsceneEntDict;

        // DECLARE an IDictionary<int, string>, name it '_quoteDict':
        private IDictionary<int, string> _quoteDict;

        // DECLARE an IFuncCommand<ICommand>, name it '_createCommand':
        private IFuncCommand<ICommand> _createCommand;

        // DECLARE a SpriteFont, name it '_font':
        private SpriteFont _font;

        // DECLARE a Vector2, name it '_textPos':
        private Vector2 _textPos;

        // DECLARE a bool, name it '_frameBreak':
        private bool _frameBreak;

        // DECLARE a string, name it '_currentScene':
        private string _currentScene;

        // DECLARE a string, name it '_nextScene':
        private string _nextScene;  /**/

        // DECLARE an int, name it '_cutsceneCase':
        private int _cutsceneCase;

        // DECLARE an int, name it '_frameTimer':
        private int _frameTimer;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of CutsceneGraph
        /// </summary>
        public CutsceneGraph()
        {
            // INITIALISE _cutsceneCase with a value of '0':
            _cutsceneCase = 0;
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEGRAPH

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="pUName"> Used for passing unique name </param>
        public void RemoveEntity(string pUName)
        {
            // REMOVE IEntity object addressed by pUName from _cutsceneEntDict:
            _cutsceneEntDict.Remove(pUName);
        }

        /// <summary>
        /// Removes and terminates any references to entities from the scene
        /// </summary>
        public void ClearScene()
        {
            // FOREACH IEntity in _cutsceneEntDict.Values:
            foreach (IEntity pEntity in _cutsceneEntDict.Values.ToList())
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

            // INITIALISE _commandDict["NextScene"]'s FirstParam Property with value of _currentScene:
            (_commandDict["NextScene"] as ICommandTwoParam<string, string>).FirstParam = _currentScene;

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
            // RETURN instance of _cutsceneEntDict:
            return _cutsceneEntDict;
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // BEGIN creation of displayable objects:
            pSpriteBatch.Begin();

            // FOREACH IEntity in _cutsceneEntDict.Values:
            foreach (IEntity pEntity in _cutsceneEntDict.Values)
            {
                // IF pEntity implements IDraw:
                if (pEntity is IDraw)
                {
                    // CALL Draw() on pEntity, passing pSpriteBatch as a parameter:
                    (pEntity as IDraw).Draw(pSpriteBatch);
                }
            }

            // DRAW string value of _quoteDict[_cuteceneCase] at _textPos in _font coloured White:
            pSpriteBatch.DrawString(_font, _quoteDict[_cutsceneCase], _textPos, Color.White);

            // END creation of displayable objects:
            pSpriteBatch.End();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<INT, STRING>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<int, string> instance
        /// </summary>
        /// <param name="pQuoteDict"> IDictionary<int, string> instance </param>
        public void Initialise(IDictionary<int, string> pQuoteDict)
        {
            // IF pQuoteDict DOES HAVE an active instance:
            if (pQuoteDict != null)
            {
                // INITIALISE _quoteDict with reference to pQuoteDict:
                _quoteDict = pQuoteDict;
            }
            // IF pQuoteDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pQuoteDict does not have an active instance!");
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, IDICTIONARY<INT, TEXTURE2D>>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, IDictionary<int, Texture2D>> instance
        /// </summary>
        /// <param name="pEntTexDict"> IDictionary<string, IDictionary<int, Texture2D>> instance </param>
        public void Initialise(IDictionary<string, IDictionary<int, Texture2D>> pEntTexDict)
        {
            // IF pEntTexDict DOES HAVE an active instance:
            if (pEntTexDict != null)
            {
                // INITIALISE _entityTextureDict with reference to pEntTexDict:
                _entityTextureDict = pEntTexDict;
            }
            // IF pEntTexDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pEntTexDict does not have an active instance!");
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
                // INITIALISE _cutsceneEntDict with reference to pSceneEntDict:
                _cutsceneEntDict = pSceneEntDict;
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING>

        /// <summary>
        /// Initialises an object with a string
        /// </summary>
        /// <param name="pNextSceneName"> Name of next level/scene to transition to </param>
        public void Initialise(string pNextSceneName)
        {
            // IF pNextSceneName DOES HAVE a valid string:
            if (pNextSceneName != null && pNextSceneName != "")
            {
                // INITIALISE _nextScene with value of pNextSceneName:
                _nextScene = pNextSceneName;
            }
            // IF pNextLevelName DOES NOT HAVE a valid string:
            else
            {
                // THROW a new NullValueException(), with corresponding message:
                throw new NullValueException("ERROR: pNextSceneName does not have a valid string value!");
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, IDICTIONARY<INT, TEXTURE2D>>

        /// <summary>
        /// Initialises an object with a string and an IDictionary<int, Texture2D> object
        /// </summary>
        /// <param name="pEntityName"> Name of Entity </param>
        /// <param name="pTexDict"> IDictionary<int, Texture2D> object </param>
        public void Initialise(string pEntityName, IDictionary<int, Texture2D> pTexDict)
        {
            // IF pTexDict DOES HAVE an active instance:
            if (pTexDict != null)
            {
                // IF _entityTextureDict DOES NOT already contain pEntityName as a key:
                if (!_entityTextureDict.ContainsKey(pEntityName))
                {
                    // ADD pEntityName as a key, and pTexDict as a value to _commandDict:
                    _entityTextureDict.Add(pEntityName, pTexDict);
                }
                // IF _entityTextureDict DOES already contain pEntityName as a key:
                else
                {
                    // THROW a new NullInstanceException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pEntityName is already stored in _entityTextureDict!");
                }
            }
            // IF pTexDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pTexDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<SPRITEFONT>

        /// <summary>
        /// Initialises an object with a SpriteFont object
        /// </summary>
        /// <param name="pFont"> SpriteFont object </param>
        public void Initialise(SpriteFont pFont)
        {
            // IF pFont DOES HAVE an active instance:
            if (pFont != null)
            {
                // INITIALISE _font with reference to pFont:
                _font = pFont;
            }
            // IF pFont DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pFont does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<VECTOR2>

        /// <summary>
        /// Initialises an object with a Vector2 struct
        /// </summary>
        /// <param name="pTextPos"> Vector2 struct for positioning text </param>
        public void Initialise(Vector2 pTextPos)
        {
            // IF pTextPos DOES HAVE an active instance:
            if (pTextPos != null)
            {
                // INITIALISE _textPos with reference to pTextPos:
                _textPos = pTextPos;
            }
            // IF pTextPos DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pTextPos does not have an active instance!");
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
                // RETURN value of _currentScene:
                return _currentScene;
            }
            set
            {
                // SET value of _currentScene to incoming value:
                _currentScene = value;
            }
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

            // ADD pEntity.UName as a key, and pEntity as a value to _cutsceneEntDict:
            _cutsceneEntDict.Add(pEntity.UName, pEntity);

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
            // IF Enter key is pressed and _frameBreak is false:
            if (Keyboard.GetState().IsKeyDown(Keys.Enter) && !_frameBreak)
            {
                // INCREMENT _cutsceneCase:
                _cutsceneCase++;

                // SET _frameBreak to true:
                _frameBreak = true;
            }

            // IF _frameTimer is less than to '0' frames passed:
            if (_frameTimer < 30)
            {
                // INCREMENT _frameTimer by '1':
                _frameTimer++;
            }

            // IF _frameTimer is greater or equal to '30' frames passed:
            if (_frameTimer >= 30)
            {
                // SET _frameTimer back to '0':
                _frameTimer = 0;

                // SET _frameBreak to false:
                _frameBreak = false;
            }

            // IF _cutsceneCase HAS NOT exceeded the number of strings in _quoteDict:
            // USING QUOTES DUE TO EACH CASE INCLUDING A QUOTE
            if (_cutsceneCase < _quoteDict.Values.Count)
            {
                // FOREACH IEntity in _cutsceneEntDict:
                foreach (IEntity pEntity in _cutsceneEntDict.Values)
                {
                    // IF pEntity implements ITexture:
                    if (pEntity is ITexture)
                    {
                        // IF _entityTextureDict contains pEntity.UName:
                        // USED DUE TO ALL TEXTURES NOT NEEDING TO CHANGE DEPENDING ON CURRENT SCENE CASE
                        if (_entityTextureDict.ContainsKey(pEntity.UName))
                        {
                            // IF _entityTextureDict[pEntity.UName] does contain _cutsceneCase's value as a key:
                            if (_entityTextureDict[pEntity.UName].ContainsKey(_cutsceneCase))
                            {
                                // IF pEntity.Texture Property HAS NOT been updated to current Texture of current cutscene case:
                                if ((pEntity as ITexture).Texture != _entityTextureDict[pEntity.UName][_cutsceneCase])
                                {
                                    // INITIALISE pEntity.Texture Property with Texture of _entityTextureDict[pEntity.UName] using _cutsceneCase to address the exact image required:
                                    (pEntity as ITexture).Texture = _entityTextureDict[pEntity.UName][_cutsceneCase];
                                }
                            }
                        }
                    }
                }
            }
            // IF _cutsceneCase HAS exceeded the number of strings in _quoteDict:
            else
            {
                // CALL GoToNextScene(), passing _nextScene as a parameter:
                GoToNextScene(_nextScene);
            }
        }

        #endregion
    }
}