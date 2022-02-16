using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to schedule commands
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public interface ICommandScheduler
    {
        #region METHODS

        /// <summary>
        /// Method which schedules a command by adding it to a list containing ICommand objects
        /// </summary>
        void ScheduleCommand(ICommand pCommand);

        #endregion
    }
}
