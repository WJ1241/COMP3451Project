using System.Collections.Generic;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Factories.Interfaces;

namespace OrbitalEngine.EntityManagement
{
    /// <summary>
    /// Class which Stores and Initialises required commands to Entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public class EntityManager : IEntityManager,  IInitialiseParam<ICommandScheduler>, IInitialiseParam<IDictionary<string, IEntity>>, IInitialiseParam<IFactory<IEntity>>,
        IInitialiseParam<IFuncCommand<ICommand>>, IInitialiseParam<IKeyboardPublisher>
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary, name it '_entityDict':
        private IDictionary<string, IEntity> _entityDict;

        // DECLARE an IFactory<IEntity>, name it '_entityFactory':
        private IFactory<IEntity> _entityFactory;

        // DECLARE an ICommandScheduler, name it '_commandScheduler':
        private ICommandScheduler _commandScheduler;

        // DECLARE an IKeyboardPublisher, name it '_kbManager':
        private IKeyboardPublisher _kBManager;

        // DECLARE an IFuncCommand<ICommand>, name it '_createCommand':
        private IFuncCommand<ICommand> _createCommand;

        // DECLARE an int, name it 'uIDCount', used to set unique IDs:
        private int _uIDCount;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of EntityManager
        /// </summary>
        public EntityManager()
        {
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

            // INCREMENT _uIDCount by 1:
            _uIDCount++;

            // DECLARE & INSTANTIATE an IEntity as a new T, name it 'entity':
            IEntity entity = _entityFactory.Create<T>();

            // CALL Generate() to initialise uID and pUName:
            Generate(entity, _uIDCount, pUName);

            #endregion


            #region TERMINATE COMMAND

            // DECLARE & INSTANTIATE an ICommandOneParam<string> as a new CommandOneParam<string>, name it 'terminate':
            ICommandOneParam<string> terminate = _createCommand.ExecuteMethod() as ICommandOneParam<string>;

            // SET MethodRef of terminate with Terminate method:
            terminate.MethodRef = Terminate;

            // SET Data of terminate with entity's UName Property:
            terminate.Data = entity.UName;

            // SET SchedulerCommand property of entity with _commandScheduler's ScheduleCommand method:
            (entity as ICommandSender).ScheduleCommand = _commandScheduler.ScheduleCommand;

            // SET TerminateMe property of entity with terminate Command:
            (entity as IEntityInternal).TerminateMe = terminate;

            #endregion


            #region ADDING TO DICTIONARY & RETURN

            // ADD pUName as a key and entity as a value to _entityDict:
            _entityDict.Add(pUName, entity);

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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, IENTITY>>

        /// <summary>
        /// Initialises an object with an IDictionary<string, IEntity> instance
        /// </summary>
        /// <param name="pEntityDict"> IDictionary<string, IEntity> object </param>
        public void Initialise(IDictionary<string, IEntity> pEntityDict)
        {
            // IF pEntityDict DOES HAVE an active instance:
            if (pEntityDict != null)
            {
                // INITIALISE _entityDict with reference to pEntityDict:
                _entityDict = pEntityDict;
            }
            // IF pEntityDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pEntityDict does not have an active instance!");
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFUNCCOMMAND<ICOMMAND>>

        /// <summary>
        /// Initialises an object with an IFuncCommand<ICommand> object
        /// </summary>
        /// <param name="pFuncCommand"> IFuncCommand<ICommand> object </param>
        public void Initialise(IFuncCommand<ICommand> pFuncCommand)
        {
            // IF pFuncCommand DOES HAVE an active instance:
            if (pFuncCommand != null)
            {
                // INITIALISE _createCommand with reference to pFuncCommand:
                _createCommand = pFuncCommand;
            }
            // IF pFuncCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pFuncCommand does not have an active instance!");
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


        #region PRIVATE METHODS

        /// <summary>
        /// Assigns unique IDs and Names to IEntity objects
        /// </summary>
        /// <typeparam name="T">Generic type substituted by a class implementing IEntity</typeparam>
        /// <param name="pRqdObject">Reference to an object of IEntity</param>
        /// <param name="pUID">Used to assign unique ID to entity</param>
        /// <param name="pUName">Used to assign unique Name to entity</param>
        private void Generate<T>(T pRqdObject, int pUID, string pUName) where T : IEntity
        {
            // ASSIGNMENT, set value of IEntity's UID as pID:
            pRqdObject.UID = pUID;

            // ASSIGNMENT, set value of IEntity's UName as pUName:
            pRqdObject.UName = pUName;
        }

        #endregion
    }
}