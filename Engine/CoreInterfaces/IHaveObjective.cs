

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a boolean relative to their main in-game objective
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public interface IHaveObjective
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to an objective boolean
        /// </summary>
        bool ObjectiveComplete { get; set; }

        #endregion
    }
}
