using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.Services.Commands
{
    /// <summary>
    /// Class which schedules commands at a specific point in a runtime loop
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 16/01/22
    /// </summary>
    public class CommandScheduler : IService, ICommandScheduler, IExecuteCommand, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IList<ICommand>, name it '_commandList':
        private IList<ICommand> _commandList;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of CommandScheduler
        /// </summary>
        public CommandScheduler()
        {
            // INSTANTIATE _commandList as a new List<ICommand>(), made on construction to store initial commands:
            _commandList = new List<ICommand>();
        }

        #endregion


        #region IMPLEMENTATION OF ICOMMANDSCHEDULER

        /// <summary>
        /// Method which schedules a command by adding it to a list containing ICommand objects
        /// </summary>
        public void ScheduleCommand(ICommand pCommand)
        {
            // ADD pCommand to _commandList:
            _commandList.Add(pCommand);
        }

        #endregion


        #region IMPLEMENTATION OF IEXECUTECOMMAND

        /// <summary>
        /// Executes a specified Command via a parameter
        /// </summary>
        /// <param name="pCommand"> Requested command to be executed </param>
        public void ExecuteCommand(ICommand pCommand)
        {
            // CALL ExecuteMethod() on pCommand:
            pCommand.ExecuteMethod();
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> Holds reference to GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // IF _commandList contains 1 or more ICommand objects:
            if (_commandList.Count >= 1)
            {
                // FOREACH ICommand object stored in _commandList:
                foreach (ICommand pCommand in _commandList)
                {
                    // CALL ExecuteCommand, passing pCommand as a parameter:
                    ExecuteCommand(pCommand);
                }
            }

            // CALL Clear() on _commandList to remove all previous commands in queue:
            _commandList.Clear();
        }

        #endregion
    }
}
