using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.SceneManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to manage entities in the scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public interface ISceneManager : IService
    {
        #region METHODS

        /// <summary>
        /// Creates a Scene which stores entities and their positions
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pResetSceneCommand"> Command to Reset Scene when needed </param>
        void CreateScene(string pSceneName, ICommand pResetSceneCommand);

        /// <summary>
        /// Creates a Cutscene which stores entities and their positions
        /// </summary>
        /// <param name="pCutsceneName"> Name of Cutscene </param>
        /// /// <param name="pResetSceneCommand"> Command to Reset Scene when needed </param>
        void CreateCutscene(string pCutsceneName, ICommand pResetSceneCommand);

        /// <summary>
        /// Removes current level references and loads the next level for the user
        /// </summary>
        /// <param name="pPrevLevel"> Name of previous level </param>
        /// <param name="pNextLevel"> Name of next level </param>
        void LoadNextLevel(string pPrevLevel, string pNextLevel);

        /// <summary>
        /// Removes a Scene/Cutscene specified by its Name
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        void RemoveScene(string pSceneName);

        /// <summary>
        /// Returns Current loaded scene
        /// </summary>
        /// <returns> Current Loaded Scene </returns>
        ISceneGraph ReturnCurrentScene();

        /// <summary>
        /// Initialises a specified scene with an IDictionary<string, IEntity> object and an IFuncCommand<ICommand> object
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pEntDict"> IDictionary<string, IEntity> object </param>
        /// <param name="pCreateCommand"> IFuncCommand<ICommand> object </param>
        void Initialise(string pSceneName, IDictionary<string, IEntity> pEntDict, IFuncCommand<ICommand> pCreateCommand);

        /// <summary>
        /// Initialises a specified scene with an ICollisionManager object, an IDictionary<string, IEntity> object and an IFuncCommand<ICommand> object
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        /// <param name="pEntDict"> IDictionary<string, IEntity> object </param>
        /// <param name="pCreateCommand"> IFuncCommand<ICommand> object </param>
        void Initialise(string pSceneName, ICollisionManager pCollisionManager, IDictionary<string, IEntity> pEntDict, IFuncCommand<ICommand> pCreateCommand);

        /// <summary>
        /// Spawns Entity in specified scene and adds to a list/dictionary
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pEntity"> IEntity object </param>
        /// <param name="pPosition"> Positional values used to place entity </param>
        void Spawn(string pSceneName, IEntity pEntity, Vector2 pPosition);

        #endregion
    }
}
