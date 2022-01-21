using COMP3451Project.EnginePackage.Services.Commands;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface which gives further implementations to IEntity objects
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface IEntityInternal
    {
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
