using OrbitalEngine.Behaviours;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Services.Commands.Interfaces;
using COMP3451Project.RIRRPackage.Interfaces;
using OrbitalEngine.Exceptions;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Level Change entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public class LevelChangeBehaviour : CollidableBehaviour, IInitialiseParam<string>
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_changeLevelCommand':
        private ICommand _changeLevelCommand;

        // DECLARE a string, name it '_nextLevel':
        private string _nextLevel;


        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of LevelChangeBehaviour
        /// </summary>
        public LevelChangeBehaviour()
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
            // IF pArgs.RequiredArg implements IPlayer and has completed their objective:
            if (pArgs.RequiredArg is IPlayer && (pArgs.RequiredArg as IHaveObjective).ObjectiveComplete)
            {

                //(_changeLevelCommand as ICommandOneParam<string>)
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING>

        /// <summary>
        /// Initialises an object with a string
        /// </summary>
        /// <param name="pNextLevelName"> Name of next level/scene to transition to </param>
        public void Initialise(string pNextLevelName)
        {
            // IF pNextLevelName DOES HAVE a valid string:
            if (pNextLevelName != null && pNextLevelName != "")
            {
                // INITIALISE _nextLevel with value of pNextLevelName
                _nextLevel = pNextLevelName;
            }
            // IF pNextLevelName DOES NOT HAVE a valid string:
            else
            {
                // THROW a new NullValueException(), with corresponding message:
                throw new NullValueException("ERROR: pNextLevelName does not have a valid string value!");
            }
        }

        #endregion
    }
}
