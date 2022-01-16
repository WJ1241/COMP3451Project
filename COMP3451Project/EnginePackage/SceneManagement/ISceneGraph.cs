using System.Collections.Generic;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Interface that allows implementations to store a reference to the Dictionary in Scene Manager
    /// </summary>
    public interface ISceneGraph
    {
        #region METHODS

        /// <summary>
        /// Initialises object with a reference to an IDictionary<string, IEntity>
        /// </summary>
        /// <param name="sceneDictionary">Holds reference to 'IDictionary<string, IEntity>'</param>
        void Initialise(IDictionary<string, IEntity> sceneDictionary);

        #endregion
    }
}
