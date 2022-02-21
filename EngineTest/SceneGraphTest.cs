using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Xna.Framework;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.SceneManagement;
using OrbitalEngine.SceneManagement.Interfaces;

namespace EngineTest
{
    /// <summary>
    /// Test Class to test if SceneGraph performs all required behaviours successfully
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/02/22
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

            // DECLARE & INSTANTIATE an ISceneGraph as a new SceneGraph(), name it '_sG':
            ISceneGraph _sG = new SceneGraph();

            // DECLARE & INSTANTIATE an IEntity as a new DrawableRectangleEntity():
            IEntity _entity = new DrawableRectangleEntity();

            // SET UName Property value of _entity to "Example":
            _entity.UName = "Example";

            #endregion


            #region ACT

            // SPAWN _entity at 0, 0:
            (_sG as ISpawn).Spawn(_entity, new Vector2(0));

            #endregion


            #region ASSERT

            // IF _sG DOES NOT contain a key named "Example":
            if (!_sG.ReturnSceneDict().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' does not exist in _sg Dictionary!");
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

            // DECLARE & INSTANTIATE an ISceneGraph as a new SceneGraph(), name it '_sG':
            ISceneGraph _sG = new SceneGraph();

            // DECLARE & INSTANTIATE an IEntity as a new DrawableRectangleEntity():
            IEntity _entity = new DrawableRectangleEntity();

            // SET UName Property value of _entity to "Example":
            _entity.UName = "Example";

            #endregion


            #region ACT

            // SPAWN _entity at 0, 0:
            (_sG as ISpawn).Spawn(_entity, new Vector2(0));

            // REMOVE Entity named "Example":
            _sG.RemoveEntity("Example");

            #endregion


            #region ASSERT

            // IF _sG DOES contain a key named "Example":
            if (_sG.ReturnSceneDict().ContainsKey("Example"))
            {
                // FAIL test:
                Assert.Fail("'Example' still exists in _sg Dictionary!");
            }

            #endregion
        }

        #endregion
    }
}
