using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.SceneManagement;
using COMP3451Project.EnginePackage.Services.Factories;

namespace COMP3451Project.EnginePackage.Services
{
    /// <summary>
    /// Class which manages the game engine, holding all references for any service used to create game entities, level structure etc.
    /// Author: William Smith
    /// Date: 19/12/21
    /// </summary>
    public class EngineManager : IService, IInitialiseParam<IService>, IRtnService
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IService>, name it '_serviceDict':
        private IDictionary<string, IService> _serviceDict;

        // DECLARE an IFactory<IService>, name it '_serviceFactory':
        private IFactory<IService> _servicefactory;

        #endregion

        
        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of EngineManager
        /// </summary>
        public EngineManager()
        {
            // INSTANTIATE _serviceDict as a new Dictionary<string, IService>():
            _serviceDict = new Dictionary<string, IService>();

            // INSTANTIATE _serviceFactory as a new Factory<IService>():
            _servicefactory = new Factory<IService>();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ISERVICE>

        /// <summary>
        /// Initialises an object with a reference to an IService instance
        /// </summary>
        /// <param name="pService"> IService instance </param>
        public void Initialise(IService pService)
        {
            // INITIALISE _serviceFactory with pService cast as IFactory<IService>:
            _servicefactory = pService as IFactory<IService>;
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
            // DECLARE & INITIALISE a string, name it '_serviceName', give value of incoming class/interface:
            string _serviceName = "" + typeof(C);
            
            // IF _serviceDict DOES NOT contain a key of name of class/interface to be created:
            if (!_serviceDict.ContainsKey(_serviceName))
            {
                // ADD new service to _serviceDict, with type 'C' name as key, and instance of a type 'C' factory as value:
                _serviceDict.Add(_serviceName, _servicefactory.Create<C>());
            }

            // RETURN instance of current _serviceName in _serviceDict:
            return _serviceDict[_serviceName];
        }

        #endregion
    }
}