using System.Collections.Generic;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.InputManagement;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.Services.Factories;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Class which Stores and Initialises required commands to Entities
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public class EntityManager : IEntityManager, IInitialiseParam<ICommandScheduler>, IInitialiseParam<IKeyboardPublisher>, IInitialiseParam<IFactory<IEntity>>
    {
        #region FIELD VARIABLES

        // DECLARE an IFactory<IEntity>, name it '_entityFactory':
        private IFactory<IEntity> _entityFactory;

        // DECLARE an ICommandScheduler, name it '_commandScheduler':
        private ICommandScheduler _commandScheduler;

        // DECLARE an IKeyboardPublisher, call it '_kbManager':
        private IKeyboardPublisher _kBManager;

        // DECLARE an IDictionary, call it '_entityDictionary':
        private IDictionary<string, IEntity> _entityDictionary;

        // DECLARE an int, call it 'uIDCount', used to set unique IDs:
        private int _uIDCount;


        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of EntityManager
        /// </summary>
        public EntityManager()
        {
            // INSTANTIATE _entityDictionary as new Dictionary<string, IEntity>:
            _entityDictionary = new Dictionary<string, IEntity>();

            // ASSIGNMENT, set value of _uIDCount to 0:
            _uIDCount = 0;
        }

        #endregion


        #region IMPLEMENTATION OF IENTITYMANAGER

        /// <summary>
        /// Creates an object of IEntity, using <T> as a generic type
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        public IEntity Create<T>(string uName) where T : IEntity, new()
        {
            #region CREATION

            // INCREMENT iDCount by 1:
            _uIDCount++;

            // DECLARE & INSTANTIATE an IEntity as a new T, name it '_entity':
            IEntity _entity = _entityFactory.Create<T>();

            // CALL Generate() to initialise uID and uName:
            Generate(_entity, _uIDCount, uName);

            #endregion


            #region TERMINATE COMMAND

            // DECLARE an ICommandOneParam<string>, name it '_terminate':
            ICommandOneParam<string> _terminate = new CommandOneParam<string>();

            // SET MethodRef of _terminate with Terminate method:
            _terminate.MethodRef = Terminate;

            // SET Data of _terminate with _entity's UName Property:
            _terminate.Data = _entity.UName;

            // SET SchedulerCommand property of _entity with _commandScheduler's ScheduleCommand method:
            (_entity as ICommandSender).ScheduleCommand = _commandScheduler.ScheduleCommand;

            // SET TerminateMe property of _entity with _terminate Command:
            (_entity as IEntityInternal).TerminateMe = _terminate;

            #endregion


            #region ENTITY RETURN

            // ADD _entity to Dictionary<string, IEntity>:
            _entityDictionary.Add(uName, _entity);

            // RETURN newly created entity:
            return _entityDictionary[uName];

            #endregion
        }

        /// <summary>
        /// Returns an instance of an IDictionary<string, IEntity>
        /// </summary>
        public IDictionary<string, IEntity> GetDictionary()
        {
            // RETURN instance of _entityDictionary:
            return _entityDictionary;
        }

        /// <summary>
        /// Terminates an object from entity manager and other managers
        /// </summary>
        /// <param name="uName">Reference to object using unique name</param>
        public void Terminate(string uName)
        {
            // CALL Terminate(), on ITerminate to dispose of resources:
            (_entityDictionary[uName] as ITerminate).Terminate();

            // IF _entityDictionary[uName] implements IKeyboardListener:
            if (_entityDictionary[uName] is IKeyboardListener)
            {
                // CALL Unsubscribe on KeyboardManager, passing uName as a parameter
                _kBManager.Unsubscribe(uName);
            }

            // CALL Remove(), on Dictionary to remove 'value' of key 'uName':
            _entityDictionary.Remove(uName);
        }

        

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMANDSCHEDULER>

        /// <summary>
        /// Initialises an object with an ICommandScheduler object
        /// </summary>
        /// <param name="pCommandScheduler"> ICommandScheduler object </param>
        public void Initialise(ICommandScheduler pCommandScheduler)
        {
            // INITIALISE _commandScheduler with reference to pCommandScheduler:
            _commandScheduler = pCommandScheduler;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMANDSCHEDULER>

        /// <summary>
        /// Initialises an object with an IKeyboardPublisher object
        /// </summary>
        /// <param name="kBManager"> IKeyboardPublisher object </param>
        public void Initialise(IKeyboardPublisher pKBManager)
        {
            // INITIALISE _kBManager with reference to pKBManager:
            _kBManager = pKBManager;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFACTORY<IENTITY>>

        /// <summary>
        /// Initialises an object with an IFactory<IEntity> object
        /// </summary>
        /// <param name="pFactory"> IFactory<IEntity> object </param>
        public void Initialise(IFactory<IEntity> pFactory)
        {
            // INITIALISE _entityFactory with reference to pFactory:
            _entityFactory = pFactory;
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