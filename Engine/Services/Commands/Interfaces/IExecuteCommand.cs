

namespace OrbitalEngine.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to execute commands
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface IExecuteCommand
    {
        #region METHODS

        /// <summary>
        /// Executes a specified Command via a parameter
        /// </summary>
        /// <param name="pCommand"> Requested command to be executed </param>
        void ExecuteCommand(ICommand pCommand);

        #endregion
    }
}
