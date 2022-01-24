

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to be terminated
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
