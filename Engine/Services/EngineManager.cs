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
    /// Date: 18/02/22
    /// </summary>
    public class EngineManager : IService, IInitialiseParam<IFactory<IService>>, IRtnService
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
            // INSTANTIATE _serviceDict as a new Dictionary<string, IService>():
            _serviceDict = new Dictionary<string, IService>();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IFACTORY<SERVICE>>

        /// <summary>
        /// Initialises an object with a reference to an IService instance
        /// </summary>
        /// <param name="pServiceFactory"> IFactory<IService> instance </param>
        public void Initialise(IFactory<IService> pServiceFactory)
        {
            // IF pServiceFactory DOES HAVE an active instance:
            if (pServiceFactory != null)
            {
                // DECLARE & INITIALISE a string, name it '_serviceName', give value of a trimmed incoming class' type:
                string _serviceName = GenericTypeNameTrimmer.TrimOneGeneric(pServiceFactory.GetType());

                // ADD new service to _serviceDict, with type of pServiceFactory as key, and pServiceFactory as value:
                _serviceDict.Add(_serviceName, pServiceFactory as IService);
            }
            // IF pServiceFactory DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pServiceFactory does not have an active instance!");
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
            // DECLARE a string, name it '_serviceName':
            // "" PREVENTS ADDRESSIING ISSUES:
            string _serviceName = "";

            // IF typeof(C) DOES HAVE one or more generic arguments:
            if (typeof(C).GetGenericArguments().Length >= 1)
            {
                // INITIALISE _serviceName, give value of incoming class' type which is trimmed:
                _serviceName = GenericTypeNameTrimmer.TrimOneGeneric(typeof(C));
            }
            // IF typeof(C) DOES HAVE one or more generic arguments:
            else if (typeof(C).GetGenericArguments().Length == 0)
            {
                // INITIALISE _serviceName, give value of incoming class/interface:
                _serviceName = typeof(C).ToString();
            }

            // IF _serviceDict DOES NOT contain a key of name of class/interface to be created:
            if (!_serviceDict.ContainsKey(_serviceName))
            {
                // ADD new service to _serviceDict, with type 'C' name as key, and instance of type 'C' as value:
                _serviceDict.Add(_serviceName, (_serviceDict["Factory<IService>"] as IFactory<IService>).Create<C>());
            }

            // RETURN instance of current _serviceName in _serviceDict:
            return _serviceDict[_serviceName];
        }

        #endregion
    }
}