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
    /// Date: 07/04/22
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

            // DECLARE & INSTANTIATE an IEntityManager as a new EntityManager(), name it 'eM':
            IEntityManager eM = new EntityManager();

            // INITIALISE eM with a new Factory<IEntity>():
            (eM as IInitialiseParam<IFactory<IEntity>>).Initialise(new Factory<IEntity>());

            // INITIALISE eM with a new CommandScheduler():
            (eM as IInitialiseParam<ICommandScheduler>).Initialise(new CommandScheduler());

            #endregion


            #region ACT

            // INSTANTIATE a DrawableRectangleEntity() named "Example":
            eM.Create<DrawableRectangleEntity>("Example");

            #endregion


            #region ASSERT

            // IF eM DOES NOT contain a key named "Example":
            if (!eM.GetDictionary().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' does not exist in eM Dictionary!");
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

            // DECLARE & INSTANTIATE an IEntityManager as a new EntityManager(), name it 'eM':
            IEntityManager eM = new EntityManager();

            // INITIALISE eM with a new Factory<IEntity>():
            (eM as IInitialiseParam<IFactory<IEntity>>).Initialise(new Factory<IEntity>());

            // INITIALISE eM with a new CommandScheduler():
            (eM as IInitialiseParam<ICommandScheduler>).Initialise(new CommandScheduler());

            #endregion


            #region ACT

            // INSTANTIATE a DrawableRectangleEntity() named "Example":
            eM.Create<DrawableRectangleEntity>("Example");

            // TERMINATE "Example" from eM:
            eM.Terminate("Example");

            #endregion


            #region ASSERT

            // IF eM DOES contain a key named "Example":
            if (eM.GetDictionary().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' still exists in eM Dictionary!");
            }

            #endregion
        }

        #endregion
    }
}
