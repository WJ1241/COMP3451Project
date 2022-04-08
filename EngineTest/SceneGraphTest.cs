using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.SceneManagement;
using OrbitalEngine.SceneManagement.Interfaces;
using OrbitalEngine.Services.Commands;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using System.Collections.Generic;

namespace EngineTest
{
    /// <summary>
    /// Test Class to test if SceneGraph performs all required behaviours successfully
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 08/04/22
    /// </summary>
    [TestClass]
    public class SceneGraphTest
    {
        #region SPAWNING ENTITY

        /// <summary>
        /// Checks if ISpawn spawns a specified entity
        /// </summary>
        [TestMethod]
        public void Check_If_SceneGraph_Spawns_Specified_Entity()
        {
            #region ARRANGE

            // DECLARE & INSTANTIATE an ISceneGraph as a new SceneGraph(), name it 'sG':
            ISceneGraph sG = new SceneGraph();

            // DECLARE & INSTANTIATE an IEntity as a new DrawableRectangleEntity():
            IEntity entity = new DrawableRectangleEntity();

            // SET UName Property value of _entity to "Example":
            entity.UName = "Example";

            // INITIALISE sG with a new Dictionary<string, IEntity>():
            (sG as IInitialiseParam<IDictionary<string, IEntity>>).Initialise(new Dictionary<string, IEntity>());

            #region CREATE COMMAND

            // DECLARE & INSTANTIATE an IFactory<ICommand> as a new Factory<ICommand>(), name it 'cmdFactory':
            IFactory<ICommand> cmdFactory = new Factory<ICommand>();

            // DECLARE & INSTANTIATE an IFuncCommand<ICommand> as a new FuncCommandZeroParam<ICommand>(), name it 'createCommand':
            IFuncCommand<ICommand> createCommand = new FuncCommandZeroParam<ICommand>();

            // INITIALISE MethodRef Property of createCommand with reference to cmdFactory.Create<CommandOneParam<string>>:
            (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = cmdFactory.Create<CommandOneParam<string>>;

            // INITIALISE eM with reference to createCommand:
            (sG as IInitialiseParam<IFuncCommand<ICommand>>).Initialise(createCommand);

            #endregion

            #endregion


            #region ACT

            // SPAWN entity at 0, 0:
            (sG as ISpawn).Spawn(entity, new Vector2(0));

            #endregion


            #region ASSERT

            // IF sG DOES NOT contain a key named "Example":
            if (!sG.ReturnSceneDict().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' does not exist in sG Dictionary!");
            }

            #endregion
        }

        #endregion


        #region ENTITY REMOVAL

        /// <summary>
        /// Checks if ISceneGraph removes a specified entity
        /// </summary>
        [TestMethod]
        public void Check_If_SceneGraph_Removes_Specified_Entity()
        {
            #region ARRANGE

            // DECLARE & INSTANTIATE an ISceneGraph as a new SceneGraph(), name it 'sG':
            ISceneGraph sG = new SceneGraph();

            // DECLARE & INSTANTIATE an IEntity as a new DrawableRectangleEntity():
            IEntity entity = new DrawableRectangleEntity();

            // SET UName Property value of _entity to "Example":
            entity.UName = "Example";

            // INITIALISE sG with a new Dictionary<string, IEntity>():
            (sG as IInitialiseParam<IDictionary<string, IEntity>>).Initialise(new Dictionary<string, IEntity>());

            #region CREATE COMMAND

            // DECLARE & INSTANTIATE an IFactory<ICommand> as a new Factory<ICommand>(), name it 'cmdFactory':
            IFactory<ICommand> cmdFactory = new Factory<ICommand>();

            // DECLARE & INSTANTIATE an IFuncCommand<ICommand> as a new FuncCommandZeroParam<ICommand>(), name it 'createCommand':
            IFuncCommand<ICommand> createCommand = new FuncCommandZeroParam<ICommand>();

            // INITIALISE MethodRef Property of createCommand with reference to cmdFactory.Create<CommandOneParam<string>>:
            (createCommand as IFuncCommandZeroParam<ICommand>).MethodRef = cmdFactory.Create<CommandOneParam<string>>;

            // INITIALISE eM with reference to createCommand:
            (sG as IInitialiseParam<IFuncCommand<ICommand>>).Initialise(createCommand);

            #endregion

            #endregion


            #region ACT

            // SPAWN entity at 0, 0:
            (sG as ISpawn).Spawn(entity, new Vector2(0));

            // REMOVE Entity named "Example":
            sG.RemoveEntity("Example");

            #endregion


            #region ASSERT

            // IF sG DOES contain a key named "Example":
            if (sG.ReturnSceneDict().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' still exists in sG Dictionary!");
            }

            #endregion
        }

        #endregion
    }
}
