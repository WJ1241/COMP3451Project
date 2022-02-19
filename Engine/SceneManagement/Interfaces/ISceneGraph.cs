using OrbitalEngine.CollisionManagement.Interfaces;

namespace OrbitalEngine.SceneManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to remove an Entity from current scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public interface ISceneGraph
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with an ICollisionManager object
        /// </summary>
        /// <param name="pCollisionManager"> ICollisionManager object </param>
        void Initialise(ICollisionManager pCollisionManager);

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="pUName"> Used for passing unique name </param>
        void RemoveEntity(string pUName);

        #endregion
    }
}
