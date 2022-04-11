using OrbitalEngine.Behaviours;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Artefact entities
    /// Authors: Declan Kerby-Collins & William Smith 
    /// Date: 11/04/22
    /// </summary>
    public class ArtefactBehaviour : Behaviour, IEventListener<CollisionEventArgs>
    {

        public ArtefactBehaviour()
        {
            //empty
        }

        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public void OnEvent(object pSource, CollisionEventArgs pArgs)
        {
            // IF ICollidable is of type IPlayer:
            if(pArgs.RequiredArg is IPlayer)
            {
                // SET ObjectiveComplete property of the Player to true:
                (pArgs.RequiredArg as IHaveObjective).ObjectiveComplete = true;

                // CALL Terminate method in Artefact:
                (_entity as ITerminate).Terminate();
            }
        }

        #endregion
    }
}
