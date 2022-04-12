using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for displaying health points
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public class HPBarShroudBehaviour : UpdatableBehaviour, IInitialiseParam<ICommand>
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_updateHPBarPosCommand':
        private ICommand _updateHPBarPosCommand;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of HPBarShroudBehaviour
        /// </summary>
        public HPBarShroudBehaviour()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnEvent(object pSource, UpdateEventArgs pArgs)
        {
            // INITIALISE FirstParam Property of _updateHPBarPosCommand with value of _entity.Position:
            (_updateHPBarPosCommand as ICommandOneParam<Vector2>).FirstParam = _entity.Position;

            // SCHEDULE _updateHPBarPosCommand to be executed:
            (_entity as ICommandSender).ScheduleCommand(_updateHPBarPosCommand);
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
                _updateHPBarPosCommand = pCommand;
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
