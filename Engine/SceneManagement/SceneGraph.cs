using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.Camera.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.SceneManagement
{
    /// <summary>
    /// Class which holds reference to list in Scene Manager, Draws and Updates entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    /// <REFERENCE> Abuhakmeh, K. (2009) XNA 2D Camera Engine That Follows Sprite. Available at: https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite. (Accessed: 20 April 2021). </REFERENCE>
    public class SceneGraph : ISceneGraph, IDraw, IDrawCamera, IInitialiseParam<IDictionary<string, IEntity>>, IInitialiseParam<IFuncCommand<ICommand>>, ISpawn, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IEntity>, name it '_sceneEntDict':
        private IDictionary<string, IEntity> _sceneEntDict;

        // DECLARE an IFuncCommand<ICommand>, name it '_createCommand':
        private IFuncCommand<ICommand> _createCommand;

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
            // BEGIN creation of displayable objects:
            pSpriteBatch.Begin();

            // FOREACH IDraw in _sceneEntDict.Values:
            foreach (IDraw entity in _sceneEntDict.Values)
            {
                // CALL Draw method on all entities in _entityDictionary:
                entity.Draw(pSpriteBatch);
            }

            // END creation of displayable objects:
            pSpriteBatch.End();
        }

        #endregion


        #region IMPLEMENTATION OF IDRAWCAMERA

        /// <summary>
        /// When called, draws entity's texture on screen, as well as reposition of a camera object
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        /// <param name="pCamera"> Needed to move camera position on screen </param>
        /// <CITATION> (Abuhakmeh, 2009) </CITATION>
        public void Draw(SpriteBatch pSpriteBatch, ICamera pCamera)
        {
            // BEGIN creation of displayable objects:
            //pSpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, pCamera.ChngCamPos());

            // FOREACH IDraw in _sceneEntDict.Values:
            foreach (IDraw pEntity in _sceneEntDict.Values)
            {
                // CALL Draw method on all entities in _entityDictionary:
                pEntity.Draw(pSpriteBatch);
            }

            // END creation of displayable objects:
            pSpriteBatch.End();
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

            // DECLARE & INSTANTIATE an ICommandOneParam<string> as a new CommandOneParam<string>(), name it 'removeMe':
            ICommandOneParam<string> removeMe = _createCommand.ExecuteMethod() as ICommandOneParam<string>;

            // SET MethodRef of removeMe with RemoveEntity method:
            removeMe.MethodRef = RemoveEntity;

            // SET Data of removeMe with pEntity's UName Property:
            removeMe.Data = pEntity.UName;

            // SET RemoveMe property of pEntity with removeMe Command:
            (pEntity as IEntityInternal).RemoveMe = removeMe;

            #endregion


            #region SPAWN LOCATION

            // INITIALISE pEntity.Position with value of pPosition:
            pEntity.Position = pPosition;

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
                // IF entity implements IUpdatable:
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
