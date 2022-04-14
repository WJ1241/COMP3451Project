

namespace COMP3451Project.RIRRPackage.Entities.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to have health points within a game level
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 13/04/22
    /// </summary>
    public interface IHaveHealth
    {
        #region PROPERTIES

        /// <summary>
        /// Property which has read and write access to an implementation's health points
        /// </summary>
        int HealthPoints { get; set; }

        /// <summary>
        /// Property which has read and write access to an implementation's maximum health points
        /// </summary>
        int MaxHealthPoints { get; set; }

        #endregion
    }
}
