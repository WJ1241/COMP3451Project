using Microsoft.Xna.Framework;

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to have their draw colour changed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public interface IChangeTexColour
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to a texture colour
        /// </summary>
        Color TexColour { get; set; }

        #endregion
    }
}
