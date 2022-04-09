

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows displayable entities to be seperated on to different layers to identify specific collisions
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public interface ILayer
    {
        #region PROPERTIES

        /// <summary>
        /// Property which can get and set layer value
        /// Layer 1: Floors
        /// Layer 2: Walls
        /// Layer 3: Static Obstacles
        /// Layer 4: Items
        /// Layer 5: Level Change
        /// Layer 6: Player/NPC
        /// Layer 7: GUI
        /// </summary>
        int Layer { get; set; }

        #endregion
    }
}
