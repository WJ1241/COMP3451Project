using COMP3451Project.EnginePackage.Behaviours;

namespace COMP3451Project.EnginePackage.Behaviours
{
    /// <summary>
    /// Interface which allows implementations to hold a reference to an IUpdateEventListener object
    /// Author(s): William Smith & Declan Kerby-Collins
    /// Date: 24/01/22
    /// </summary>
    public interface IInitialiseIUpdateEventListener
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        void Initialise(IUpdateEventListener pUpdateEventListener);

        #endregion
    }
}
