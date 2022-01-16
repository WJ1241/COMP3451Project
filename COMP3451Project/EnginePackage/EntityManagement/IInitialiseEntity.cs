

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface which allows implementations to hold a reference to another entity for modification
    /// </summary>
    interface IInitialiseEntity
    {
        #region IMPLEMENTATION OF IINITIALISEENTITY

        /// <summary>
        /// Initialises an object with an IEntity object
        /// </summary>
        /// <param name="entity"></param>
        void Initialise(IEntity entity);

        #endregion
    }
}
