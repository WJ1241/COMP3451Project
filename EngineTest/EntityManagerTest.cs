using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Services.Commands;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;

namespace EngineTest
{
    /// <summary>
    /// Test Class to test if EntityManager performs all required behaviours successfully
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/02/22
    /// </summary>
    [TestClass]
    public class EntityManagerTest
    {
        #region ENTITY STORING

        /// <summary>
        /// Checks if IEntityManager stores a specified entity
        /// </summary>
        [TestMethod]
        public void Check_If_EntityManager_Contains_Specified_Entity()
        {
            #region ARRANGE

            // DECLARE & INSTANTIATE an IEntityManager as a new EntityManager(), name it '_eM':
            IEntityManager _eM = new EntityManager();

            // INITIALISE _eM with a new Factory<IEntity>():
            (_eM as IInitialiseParam<IFactory<IEntity>>).Initialise(new Factory<IEntity>());

            // INITIALISE _eM with a new CommandScheduler():
            (_eM as IInitialiseParam<ICommandScheduler>).Initialise(new CommandScheduler());

            #endregion


            #region ACT

            // INSTANTIATE a DrawableRectangleEntity() named "Example":
            _eM.Create<DrawableRectangleEntity>("Example");

            #endregion


            #region ASSERT

            // IF _eM DOES NOT contain a key named "Example":
            if (!_eM.GetDictionary().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' does not exist in _eM Dictionary!");
            }

            #endregion
        }

        /// <summary>
        /// Checks if IEntityManager terminates a specified entity
        /// </summary>
        [TestMethod]
        public void Check_If_EntityManager_Terminates_Specified_Entity()
        {
            #region ARRANGE

            // DECLARE & INSTANTIATE an IEntityManager as a new EntityManager(), name it '_eM':
            IEntityManager _eM = new EntityManager();

            // INITIALISE _eM with a new Factory<IEntity>():
            (_eM as IInitialiseParam<IFactory<IEntity>>).Initialise(new Factory<IEntity>());

            // INITIALISE _eM with a new CommandScheduler():
            (_eM as IInitialiseParam<ICommandScheduler>).Initialise(new CommandScheduler());

            #endregion


            #region ACT

            // INSTANTIATE a DrawableRectangleEntity() named "Example":
            _eM.Create<DrawableRectangleEntity>("Example");

            // TERMINATE "Example" from _eM:
            _eM.Terminate("Example");

            #endregion


            #region ASSERT

            // IF _eM DOES contain a key named "Example":
            if (_eM.GetDictionary().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' still exists in _eM Dictionary!");
            }

            #endregion
        }


        #endregion
    }
}
