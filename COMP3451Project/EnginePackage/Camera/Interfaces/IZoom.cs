

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface that allows implementations to be modify values depending on zoom value needed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public interface IZoom
    {
        #region PROPERTIES

        /// <summary>
        /// Property which can set view zoom value
        /// </summary>
        float Zoom { set; }

        #endregion
    }
}
