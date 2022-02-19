

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to have Speed when displayed on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public interface IGetSpeed
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the value of an entity's Speed
        /// </summary>
        float GetSpeed { get; }

        #endregion
    }
}
