using System;

namespace OrbitalEngine.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations send commands to another class
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface ICommandSender
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to a command scheduling method
        /// </summary>
        Action<ICommand> ScheduleCommand { get; set; }

        #endregion
    }
}
