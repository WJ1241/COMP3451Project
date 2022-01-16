using COMP3451Project.EnginePackage.CollisionManagement;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Interface that allows implementations to manage entities in the scene
    /// </summary>
    public interface ISceneManager
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with a reference to an ISceneGraph
        /// </summary>
        /// <param name="sceneGraph">Holds References to an ISceneGraph</param>
        void Initialise(ISceneGraph sceneGraph);

        /// <summary>
        /// Initialises an object with a reference to an ICollisionManager
        /// </summary>
        /// <param name="collisionManager">Holds References to an ICollisionManager</param>
        void Initialise(ICollisionManager collisionManager);

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="uName">Used for passing unique name</param>
        void RemoveInstance(string uName);

        #endregion
    }
}
