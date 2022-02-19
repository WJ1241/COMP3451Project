

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface which allows displayable entities to be seperated on to different layers to identify specific collisions
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
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
        /// Layer 5: Player/Enemy
        /// Layer 6: GUI
        /// </summary>
        int Layer { get; set; }

        #endregion
    }
}
