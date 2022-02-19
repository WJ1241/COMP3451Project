

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to be given specific names if there is a need for containing objects of the same type
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 24/01/22
    /// </summary>
    public interface IName
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to the value of an object's specific name
        /// </summary>
        string Name { get; set; }

        #endregion
    }
}
