using COMP3451Project.EnginePackage.Services.Commands.Interfaces;
using COMP3451Project.EnginePackage.States.Interfaces;

namespace COMP3451Project.EnginePackage.EntityManagement.Interfaces
{
    /// <summary>
    /// Interface which gives further implementations to IEntity objects
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/02/22
    /// </summary>
    public interface IEntityInternal
    {
        #region METHODS

        /// <summary>
        /// Sets the active state of an Entity
        /// </summary>
        /// <param name="pState"> IState instance </param>
        void SetState(IState pState);

        #endregion

        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to a Termination command
        /// </summary>
        ICommand TerminateMe { get; set; }

        /// <summary>
        /// Property which allows read and write access to a Removal command
        /// </summary>
        ICommand RemoveMe { get; set; }

        #endregion
    }
}
