

namespace OrbitalEngine.Services.Commands.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to execute a void return method
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface ICommand
    {
        #region METHODS

        /// <summary>
        /// Executes specified method
        /// </summary>
        void ExecuteMethod();

        #endregion
    }
}
