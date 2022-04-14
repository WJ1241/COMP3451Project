

namespace COMP3451Project.RIRRPackage.Entities.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to be damaged in a game level
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 13/04/22
    /// </summary>
    public interface ITakeDamage
    {
        #region PROPERTIES

        /// <summary>
        /// Property which has read and write acess to an implementation's damage state
        /// </summary>
        bool Damaged { get; set; }

        #endregion
    }
}