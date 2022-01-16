using System.Collections.Generic;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.SceneManagement;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to store Entities
    /// </summary>
    public interface IEntityManager
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with a reference to an ISceneManager
        /// </summary>
        /// <param name="sceneManager">Reference to ISceneManager object</param>
        void Initialise(ISceneManager sceneManager);

        /// <summary>
        /// Initialises an object with a reference to an IKeyboardPublisher
        /// </summary>
        /// <param name="kBManager">Reference to IKeyboardPublisher object</param>
        void Initialise(IKeyboardPublisher kBManager);

        /// <summary>
        /// Creates an object of IEntity, using <T> as a generic type
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        IEntity Create<T>(string uName) where T : IEntity, new();

        /// <summary>
        /// Terminates an object from entity manager and other managers
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        void Terminate(string uName);

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which can get a reference to an IDictionary<string, IEntity>
        /// </summary>
        IDictionary<string, IEntity> GetDictionary {get;}

        #endregion
    }
}
