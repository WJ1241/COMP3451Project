

namespace COMP3451Project.EnginePackage.Services.Factories.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to create a Factory of any type
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/12/21
    /// </summary>
    /// <typeparam name="A"> Any Class to make a factory for </typeparam>
    public interface IFactory<A>
    {
        #region METHODS

        /// <summary>
        /// Creation method which returns an object of type 'A'
        /// </summary>
        /// <typeparam name="C"> Generic Type which implements generic 'A' </typeparam>
        /// <returns> New instance of type 'C' </returns>
        A Create<C>() where C : A, new();

        #endregion
    }
}
