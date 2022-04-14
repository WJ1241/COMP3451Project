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
    /// Date: 14/04/22
    /// </summary>
    public interface ISceneManager : IService
    {
        #region METHODS

        /// <summary>
        /// Creates a Scene which stores entities and their positions
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pCommandDict"> Dictionary to store commands in </param>
        /// <param name="pLoadSceneCommand"> Command to Load Scene when needed </param>
        void CreateScene(string pSceneName, IDictionary<string, ICommand> pCommandDict, ICommand pLoadSceneCommand);

        /// <summary>
        /// Creates a Cutscene which stores entities and their positions
        /// </summary>
        /// <param name="pCutsceneName"> Name of Cutscene </param>
        /// <param name="pCommandDict"> Dictionary to store commands in </param>
        /// <param name="pLoadSceneCommand"> Command to Load Scene when needed </param>
        void CreateCutscene(string pCutsceneName, IDictionary<string, ICommand> pCommandDict, ICommand pLoadSceneCommand);

        /// <summary>
        /// Uploads the next scene so there can be scene transitioning happening from one scene
        /// </summary>
        /// <param name="pNextSceneName"> Name of Next Scene </param>
        /// <param name="pLoadNextSceneCommand"> Command to keep ready for next scene </param>
        void UploadNextScene(string pNextSceneName, ICommand pLoadNextSceneCommand);

        /// <summary>
        /// Removes current scene references and loads the next scene for the user
        /// </summary>
        /// <param name="pPrevScene"> Name of previous scene </param>
        /// <param name="pNextScene"> Name of next scene </param>
        void LoadNextScene(string pPrevScene, string pNextScene);

        /// <summary>
        /// Removes current scene references and reloads the current scene again for the user
        /// </summary>
        /// <param name="pCurrentScene"> Name of current level </param>
        void ResetScene(string pCurrentScene);

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
