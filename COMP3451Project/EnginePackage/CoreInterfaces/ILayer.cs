

namespace COMP3451Project.EnginePackage.CoreInterfaces
{
    /// <summary>
    /// Interface which allows displayable entities to be seperated on to different layers to identify specific collisions
    /// </summary>
    public interface ILayer
    {
        #region PROPERTIES

        /// <summary>
        /// Property which can get and set layer value
        /// Layer 1: Walls
        /// Layer 2: Floors
        /// Layer 3: Static Obstacles
        /// Layer 4: Items
        /// Layer 5: Player/Enemy
        /// Layer 6: GUI
        /// </summary>
        int Layer { get; set; }

        #endregion
    }
}
