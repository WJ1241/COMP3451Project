using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to contain Source Rectangle to choose which part of an texture to draw
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
    /// </summary>
    public interface IDrawSourceRectangle
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to a Draw Position Rectangle
        /// </summary>
        Rectangle SourceRectangle { get; set; }

        #endregion
    }
}
