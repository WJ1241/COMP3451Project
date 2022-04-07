using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace EngineTest
{
    /// <summary>
    /// Test Class to test if Factory<A> performs all required behaviours successfully
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    [TestClass]
    public class FactoryTest
    {
        #region MAKING OBJECT

        /// <summary>
        /// Checks if IFactory<A> creates a specified object and returns it successfully
        /// </summary>
        [TestMethod]
        public void Check_If_Factory_Creates_Specified_Object()
        {
            #region ARRANGE

            // DECLARE & INSTANTIATE an IFactory<IService> as a new Factory<IService>, name it 'serviceFactory':
            IFactory<IService> serviceFactory = new Factory<IService>();

            // DECLARE an IEntityManager, name it 'eM', set to null to prevent addressing error:
            IEntityManager eM = null;

            #endregion


            #region ACT

            // INSTANTIATE eM as a new EntityManager():
            eM = serviceFactory.Create<EntityManager>() as IEntityManager;

            #endregion


            #region ASSERT

            // IF eM DOES NOT HAVE an active instance:
            if (eM == null)
            {
                // FAIL test:
                Assert.Fail("eM is null, _serviceFactory has not created an EntityManager()!");
            }

            #endregion
        }

        #endregion
    }
}
