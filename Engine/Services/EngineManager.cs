using System.Collections.Generic;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.Services
{
    /// <summary>
    /// Class which manages the game engine, holding all references for any service used to create game entities, level structure etc.
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public class EngineManager : IService, IInitialiseParam<IFactory<IService>>, IInitialiseParam<IDictionary<string, IService>>, IRtnService
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IService>, name it '_serviceDict':
        private IDictionary<string, IService> _serviceDict;

        #endregion

        
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of EngineManager
        /// </summary>
        public EngineManager()
        {
            _serviceDict = new Dictionary<string, IService>();

            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFACTORY<SERVICE>>

        /// <summary>
        /// Initialises an object with a reference to an IFactory<IService> instance
        /// </summary>
        /// <param name="pServiceFactory"> IFactory<IService> instance </param>
        public void Initialise(IFactory<IService> pServiceFactory)
        {
            // IF pServiceFactory DOES HAVE an active instance:
            if (pServiceFactory != null)
            {
                // DECLARE & INITIALISE a string, name it 'serviceName', give value of a trimmed incoming class' type:
                string serviceName = GenericTypeNameTrimmer.TrimOneGeneric(pServiceFactory.GetType());

                // ADD new service to _serviceDict, with type of serviceName as a key, and pServiceFactory as a value:
                _serviceDict.Add(serviceName, pServiceFactory as IService);
            }
            // IF pServiceFactory DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pServiceFactory does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, IService>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, IService> instance
        /// </summary>
        /// <param name="pServiceDict"> IDictionary<string, IService> instance </param>
        public void Initialise(IDictionary<string, IService> pServiceDict)
        {
            // IF pServiceDict DOES HAVE an active instance:
            if (pServiceDict != null)
            {
                // INITIALISE _serviceDict with reference to pServiceDict:
                //_serviceDict = pServiceDict;
            }
            // IF pServiceDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pServiceDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IRTNSERVICE

        /// <summary>
        /// Returns an instance of an IService object specified in place of the generic 'C'
        /// </summary>
        /// <typeparam name="C"> Generic for Class needed </typeparam>
        /// <returns> Instance of IService </returns>
        public IService GetService<C>() where C : IService, new()
        {
            // DECLARE a string, name it 'serviceName':
            // "" PREVENTS ADDRESSIING ISSUES:
            string serviceName = "";

            // IF typeof(C) DOES HAVE one or more generic arguments:
            if (typeof(C).GetGenericArguments().Length >= 1)
            {
                // INITIALISE serviceName, give value of incoming class' type which is trimmed:
                serviceName = GenericTypeNameTrimmer.TrimOneGeneric(typeof(C));
            }
            // IF typeof(C) DOES HAVE one or more generic arguments:
            else if (typeof(C).GetGenericArguments().Length == 0)
            {
                // INITIALISE serviceName, give value of incoming class/interface:
                serviceName = typeof(C).ToString();
            }

            // IF _serviceDict DOES NOT contain a key of name of class/interface to be created:
            if (!_serviceDict.ContainsKey(serviceName))
            {
                // ADD new service to _serviceDict, with serviceName as key, and instance of type 'C' as value:
                _serviceDict.Add(serviceName, (_serviceDict["Factory<IService>"] as IFactory<IService>).Create<C>());
            }

            // RETURN instance with address of serviceName's value in _serviceDict:
            return _serviceDict[serviceName];
        }

        #endregion
    }
}