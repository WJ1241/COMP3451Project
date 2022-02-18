

namespace COMP3451Project.EnginePackage.EntityManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to be terminated
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public interface ITerminate
    {
        #region METHODS

        /// <summary>
        /// Disposes resources to the garbage collector
        /// </summary>
        void Terminate();

        #endregion
    }
}
