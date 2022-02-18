using System.Collections.Generic;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement.Interfaces;
using COMP3451Project.EnginePackage.Exceptions;
using COMP3451Project.EnginePackage.InputManagement.Interfaces;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.Services.Commands.Interfaces;
using COMP3451Project.EnginePackage.Services.Factories.Interfaces;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Class which Stores and Initialises required commands to Entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public class EntityManager : IEntityManager, IInitialiseParam<ICommandScheduler>, IInitialiseParam<IKeyboardPublisher>, IInitialiseParam<IFactory<IEntity>>
    {
        #region FIELD VARIABLES

        // DECLARE an IFactory<IEntity>, name it '_entityFactory':
        private IFactory<IEntity> _entityFactory;

        // DECLARE an ICommandScheduler, name it '_commandScheduler':
        private ICommandScheduler _commandScheduler;

        // DECLARE an IKeyboardPublisher, name it '_kbManager':
        private IKeyboardPublisher _kBManager;

        // DECLARE an IDictionary, name it '_entityDict':
        private IDictionary<string, IEntity> _entityDict;

        // DECLARE an int, name it 'uIDCount', used to set unique IDs:
        private int _uIDCount;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of EntityManager
        /// </summary>
        public EntityManager()
        {
            // INSTANTIATE _entityDict as a new Dictionary<string, IEntity>:
            _entityDict = new Dictionary<string, IEntity>();

            // ASSIGNMENT, set value of _uIDCount to 0:
            _uIDCount = 0;
        }

        #endregion


        #region IMPLEMENTATION OF IENTITYMANAGER

        /// <summary>
        /// Creates an object of IEntity, using <T> as a generic type
        /// </summary>
        /// <param name="pUName">Reference to object using unique name</param>
        public IEntity Create<T>(string pUName) where T : IEntity, new()
        {
            #region CREATION

            // INCREMENT iDCount by 1:
            _uIDCount++;

            // DECLARE & INSTANTIATE an IEntity as a new T, name it '_entity':
            IEntity _entity = _entityFactory.Create<T>();

            // CALL Generate() to initialise uID and pUName:
            Generate(_entity, _uIDCount, pUName);

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


            #region ADDING TO DICTIONARY & RETURN

            // ADD pUName as a key and _entity as a value to _entityDict:
            _entityDict.Add(pUName, _entity);

            // RETURN instance of _entityDict[pUName]:
            return _entityDict[pUName];

            #endregion
        }

        /// <summary>
        /// Returns an instance of an IDictionary<string, IEntity>
        /// </summary>
        public IDictionary<string, IEntity> GetDictionary()
        {
            // RETURN instance of _entityDict:
            return _entityDict;
        }

        /// <summary>
        /// Terminates an entity to be removed from memory
        /// </summary>
        /// <param name="pUName">Reference to object using unique name</param>
        public void Terminate(string pUName)
        {
            // CALL Terminate(), on ITerminate to dispose of resources:
            (_entityDict[pUName] as ITerminate).Terminate();

            // IF _entityDict[pUName] implements IKeyboardListener:
            if (_entityDict[pUName] is IKeyboardListener)
            {
                // CALL Unsubscribe on KeyboardManager, passing uName as a parameter:
                _kBManager.Unsubscribe(pUName);
            }

            // CALL Remove() on _entityDict to remove value addressed by 'pUName':
            _entityDict.Remove(pUName);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMANDSCHEDULER>

        /// <summary>
        /// Initialises an object with an ICommandScheduler object
        /// </summary>
        /// <param name="pCommandScheduler"> ICommandScheduler object </param>
        public void Initialise(ICommandScheduler pCommandScheduler)
        {
            // IF pCommandScheduler DOES HAVE an active instance:
            if (pCommandScheduler != null)
            {
                // INITIALISE _commandScheduler with reference to pCommandScheduler:
                _commandScheduler = pCommandScheduler;
            }
            // IF pCommandScheduler DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommandScheduler does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IKEYBOARDPUBLISHER>

        /// <summary>
        /// Initialises an object with an IKeyboardPublisher object
        /// </summary>
        /// <param name="pKBManager"> IKeyboardPublisher object </param>
        public void Initialise(IKeyboardPublisher pKBManager)
        {
            // IF pKBManager DOES HAVE an active instance:
            if (pKBManager != null)
            {
                // INITIALISE _kBManager with reference to pKBManager:
                _kBManager = pKBManager;
            }
            // IF pKBManager DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pKBManager does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFACTORY<IENTITY>>

        /// <summary>
        /// Initialises an object with an IFactory<IEntity> object
        /// </summary>
        /// <param name="pFactory"> IFactory<IEntity> object </param>
        public void Initialise(IFactory<IEntity> pFactory)
        {
            // IF pFactory DOES HAVE an active instance:
            if (pFactory != null)
            {
                // INITIALISE _entityFactory with reference to pFactory:
                _entityFactory = pFactory;
            }
            // IF pFactory DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pFactory does not have an active instance!");
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