using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.CollisionManagement;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Interface that allows implementations to manage entities in the scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public interface ISceneManager : IService
    {
        #region METHODS

        /// <summary>
        /// Creates a Scene which stores entities and their positions
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        void CreateScene(string pSceneName);

        /// <summary>
        /// Initialises a specified scene with an ICollisionManager object
        /// </summary>
        /// <param name="pSceneName"> Name of Scene </param>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        void Initialise(string pSceneName, ICollisionManager pCollisionManager);

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
