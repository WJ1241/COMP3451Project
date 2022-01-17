

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface which allows implementations to hold a reference to another entity for modification
    /// Author(s): William Smith & Declan Kerby-Collins
    /// Date: 17/01/22
    /// </summary>
    interface IInitialiseIEntity
    {
        #region IMPLEMENTATION OF IINITIALISEIENTITY

        /// <summary>
        /// Initialises an object with an IEntity object
        /// </summary>
        /// <param name="entity"> IEntity object </param>
        void Initialise(IEntity pEntity);

        #endregion
    }
}
