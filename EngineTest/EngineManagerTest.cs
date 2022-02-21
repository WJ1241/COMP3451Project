using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Services;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace EngineTest
{
    /// <summary>
    /// Test Class to test if EngineManager performs all required behaviours successfully
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/02/22
    /// </summary>
    [TestClass]
    public class EngineManagerTest
    {
        #region RETURNING SERVICE

        /// <summary>
        /// Checks if IRtnService returns a specified service
        /// </summary>
        [TestMethod]
        public void Check_If_EngineManager_Returns_Specified_Service()
        {
            #region ARRANGE

            // DECLARE & INSTANTIATE an IRtnService as a new EngineManager(), name it '_engMgr':
            IRtnService _engMgr = new EngineManager();

            // INITIALISE _engMgr with a new Factory<IService>():
            (_engMgr as IInitialiseParam<IFactory<IService>>).Initialise(new Factory<IService>());

            // DECLARE an IEntityManager, name it '_eM', set to null to prevent addressing issues:
            IEntityManager _eM = null;

            #endregion


            #region ACT

            // RETURN an EntityManager() from _engMgr:
            _eM = _engMgr.GetService<EntityManager>() as IEntityManager;

            #endregion


            #region ASSERT

            // IF _eM is null:
            if (_eM == null)
            {
                // FAIL test:
                Assert.Fail("_eM is null, _engMgr has not returned an IEntityManager instance!");
            }

            #endregion
        }

        #endregion
    }
}
