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
    /// Class which holds entity references for a Level situation
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 15/04/22
    /// </summary>
    /// <REFERENCE> Abuhakmeh, K. (2009) XNA 2D Camera Engine That Follows Sprite. Available at: https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite. (Accessed: 20 April 2021). </REFERENCE>
    public class LevelSceneGraph : SceneGraph, IEventListener<MatrixEventArgs>, IInitialiseParam<ICollisionManager>, IResetScene
    {
        #region FIELD VARIABLES

        // DECLARE a Matrix, name it '_zoomFollowMatrix':
        private Matrix _zoomFollowMatrix;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Level SceneGraph
        /// </summary>
        public LevelSceneGraph()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
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
    }
}
