using OrbitalEngine.Behaviours;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using COMP3451Project.RIRRPackage.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Exceptions;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Artefact entities
    /// Authors: Declan Kerby-Collins & William Smith 
    /// Date: 12/04/22
    /// </summary>
    public class ArtefactBehaviour : CollidableBehaviour, IInitialiseParam<ICommand>
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_sfxCommand':
        private ICommand _sfxCommand;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of ArtefactBehaviour
        /// </summary>
        public ArtefactBehaviour()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public override void OnEvent(object pSource, CollisionEventArgs pArgs)
        {
            // IF ICollidable is of type IPlayer:
            if(pArgs.RequiredArg is IPlayer)
            {
                // SET ObjectiveComplete property of the Player to true:
                (pArgs.RequiredArg as IHaveObjective).ObjectiveComplete = true;

                // INITIALISE FirstParam Property of _sfxCommand with value of "ObjectiveComplete":
                //(_sfxCommand as ICommandOneParam<string>).FirstParam = "ObjectiveComplete";

                // SCHEDULER _sfxCommand to play:
                // (_entity as ICommandSender).ScheduleCommand(_sfxCommand);

                // CALL Terminate method in Artefact:
                (_entity as ITerminate).Terminate();
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMAND>

        /// <summary>
        /// Initialises an object with an ICommand object
        /// </summary>
        /// <param name="pCommand"> ICommand object </param>
        public void Initialise(ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // INITIALISE _sfxCommand with reference to pCommand:
                _sfxCommand = pCommand;
            }
            // IF pCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
            }
        }

        #endregion
    }
}
