using System.Collections.Generic;
using System.Linq;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.SceneManagement;
using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    class EntityManager : IEntityManager, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an int, call it 'uIDCount', used to set unique IDs:
        private int _uIDCount;

        // DECLARE an IDictionary, call it '_entityDictionary':
        private IDictionary<string, IEntity> _entityDictionary;

        // DECLARE an ISceneManager, call it '_sceneManager':
        private ISceneManager _sceneManager;

        // DECLARE an IKeyboardPublisher, call it '_kbManager':
        private IKeyboardPublisher _kBManager;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of EntityManager
        /// </summary>
        public EntityManager()
        {
            // ASSIGNMENT, set value of _uIDCount to 0:
            _uIDCount = 0;

            // INSTANTIATE _entityDictionary as new Dictionary<string, IEntity>:
            _entityDictionary = new Dictionary<string, IEntity>();
        }

        #endregion


        #region IMPLEMENTATION OF IENTITYMANAGER

        /// <summary>
        /// Initialises an object with a reference to an ISceneManager
        /// </summary>
        /// <param name="sceneManager">Reference to ISceneManager object</param>
        public void Initialise(ISceneManager sceneManager)
        {
            // ASSIGNMENT, set instance of _sceneManager as sceneManager:
            _sceneManager = sceneManager;
        }

        /// <summary>
        /// Initialises an object with a reference to an IKeyboardPublisher
        /// </summary>
        /// <param name="kBManager">Reference to IKeyboardPublisher object</param>
        public void Initialise(IKeyboardPublisher kBManager)
        {
            // ASSIGNMENT, set instance of _kBManager as kBManager:
            _kBManager = kBManager;
        }

        /// <summary>
        /// Creates an object of IEntity, using <T> as a generic type
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        public IEntity Create<T>(string uName) where T : IEntity, new()
        {
            // INCREMENT iDCount by 1:
            _uIDCount++;

            // DECLARE & INSTANTIATE _object as new T:
            T _object = new T();
            
            // CALL generate() to initialise uID and uName:
            Generate(_object, _uIDCount, uName);

            // ADD _object to Dictionary<string, IEntity>:
            _entityDictionary.Add(uName, _object);

            // RETURN newly created object:
            return _entityDictionary[uName];
        }

        /// <summary>
        /// Terminates an object from entity manager and other managers
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        public void Terminate(string uName)
        {
            // CALL Terminate(), on ITerminate to dispose of resources:
            (_entityDictionary[uName] as ITerminate).Terminate();

            // CALL RemoveInstance(), on SceneManager to remove 'value' of key 'uName':
            _sceneManager.RemoveInstance(uName);

            // IF _entityDictionary[uName] implements IKeyboardListener:
            if (_entityDictionary[uName] is IKeyboardListener)
            {
                // CALL Unsubscribe on KeyboardManager, passing uName as a parameter
                _kBManager.Unsubscribe(uName);
            }

            // CALL Remove(), on Dictionary to remove 'value' of key 'uName':
            _entityDictionary.Remove(uName);
        }

        /// <summary>
        /// Property which can get a reference to an IDictionary<string, IEntity>
        /// </summary>
        public IDictionary<string, IEntity> GetDictionary
        {
            get 
            {
                // RETURN value of current _entityDictionary:
                return _entityDictionary;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        public void Update(GameTime gameTime)
        {
            // FOREACH IEntity Value in _entityDictionary, convert to List, prevents modification of dictionary error:
            foreach (IEntity entity in _entityDictionary.Values.ToList())
            {
                // IF entity implements ITerminate:
                if (entity is ITerminate)
                {
                    // IF entity's (cast as ITerminate) SelfDestruct property returns true:
                    if ((entity as ITerminate).SelfDestruct == true)
                    {
                        // CALL Terminate(), passing entity's unique name as a parameter:
                        Terminate(entity.UName);
                    }
                }
            }
        }

        #endregion


        #region PRIVATE METHODS

        /// <summary>
        /// Assigns unique IDs and Names to IEntity objects
        /// </summary>
        /// <typeparam name="T">Generic type substituted by a class implementing IEntity</typeparam>
        /// <param name="rqdObject">Reference to an object of IEntity</param>
        /// <param name="uID">Used to assign unique ID to entity</param>
        /// <param name="uName">Used to assign unique Name to entity</param>
        private void Generate<T>(T rqdObject, int uID, string uName) where T : IEntity
        {
            // ASSIGNMENT, set value of IEntity's UID as pID:
            rqdObject.UID = uID;

            // ASSIGNMENT, set value of IEntity's UName as uName:
            rqdObject.UName = uName;
        }

        #endregion

    }
}
