using Microsoft.Xna.Framework;

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to contain Tile properties to work with TiledSharp
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
    /// </summary>
    public interface IDrawDestinationRectangle
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to an on screen Position Rectangle
        /// </summary>
        Rectangle DestinationRectangle { get; set; }

        #endregion
    }
}
