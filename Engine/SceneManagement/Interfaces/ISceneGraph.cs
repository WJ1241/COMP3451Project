using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using System.Collections.Generic;

namespace OrbitalEngine.SceneManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to remove an Entity from current scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public interface ISceneGraph
    {
        #region METHODS

        /// <summary>
        /// Removes and terminates any references to entities from the scene
        /// </summary>
        void ClearScene();

        /// <summary>
        /// Clears all references to entities in current scene and signals command to load next scene
        /// </summary>
        /// <param name="pNextScene"> Name of next scene </param>
        void GoToNextScene(string pNextScene);

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="pUName"> Used for passing unique name </param>
        void RemoveEntity(string pUName);

        /// <summary>
        /// Returns Scene Dictionary, used for testing
        /// </summary>
        /// <returns> IDictionary<string, IEntity> object </returns>
        IDictionary<string, IEntity> ReturnSceneDict();

        #endregion
    }
}
