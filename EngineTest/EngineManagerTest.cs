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
    /// Date: 03/04/22
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

            // DECLARE & INSTANTIATE an IRtnService as a new EngineManager(), name it 'engMgr':
            IRtnService engMgr = new EngineManager();

            // INITIALISE engMgr with a new Factory<IService>():
            (engMgr as IInitialiseParam<IFactory<IService>>).Initialise(new Factory<IService>());

            // DECLARE an IEntityManager, name it 'eM', set to null to prevent addressing issues:
            IEntityManager eM = null;

            #endregion


            #region ACT

            // RETURN an EntityManager() from engMgr:
            eM = engMgr.GetService<EntityManager>() as IEntityManager;

            #endregion


            #region ASSERT

            // IF eM is null:
            if (eM == null)
            {
                // FAIL test:
                Assert.Fail("eM is null, _engMgr has not returned an IEntityManager instance!");
            }

            #endregion
        }

        #endregion
    }
}
