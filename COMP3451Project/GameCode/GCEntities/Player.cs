using COMP3451Project.EnginePackage.EntityManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.GameCode.GCEntities
{
    /// <summary>
    /// Player class
    /// Date Created: 17/01/2022
    /// authors Declan Kerby Collins & Will Smith
    /// </summary>
    class Player: Entity
    {


        Dictionary<int, string> sceneDict;

        /// <summary>
        /// Initialises variables
        /// </summary>
        public override void Initialise()
        {

        }

        public void test()
        {
            if ((sceneDict.ContainsValue("Bob")))
            {
            }
        }

        /// <summary>
        /// Terminates class
        /// </summary>
        public override void Terminate()
        {

        }
    }
}
