

namespace OrbitalEngine.Services.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to return an instance of a service to be used within engine
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/12/21
    /// </summary>
    public interface IRtnService
    {
        #region METHODS

        /// <summary>
        /// Returns an instance of an IService object specified in place of the generic 'C'
        /// </summary>
        /// <typeparam name="C"> Generic for class needed </typeparam>
        /// <returns> Instance of IService </returns>
        IService GetService<C>() where C : IService, new();

        #endregion
    }
}
