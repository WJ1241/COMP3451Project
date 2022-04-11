

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to be modify values depending on zoom value needed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public interface IZoom
    {
        #region PROPERTIES

        /// <summary>
        /// Property which can get and set view zoom value
        /// </summary>
        float Zoom { get; set; }

        #endregion
    }
}
