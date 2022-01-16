using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to have access to Screen Size
    /// </summary>
    interface ISetBoundary
    {
        #region PROPERTIES

        /// <summary>
        /// Property which can set value of screen window borders
        /// </summary>
        Vector2 WindowBorder { set; }

        #endregion
    }
}
