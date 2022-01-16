

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


        #region PROPERTIES

        /// <summary>
        /// Property which allows access to get boolean value to test if object should be terminated
        /// </summary>
        bool SelfDestruct {get;}

        #endregion
    }
}
