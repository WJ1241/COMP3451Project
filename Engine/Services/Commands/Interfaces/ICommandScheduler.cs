

namespace OrbitalEngine.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to schedule commands
    /// Authors: William Smith & Declan Kerby-Collins
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
