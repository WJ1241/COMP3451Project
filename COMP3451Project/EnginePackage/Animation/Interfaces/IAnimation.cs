using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.Animation.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to run animations
    /// Authors: Declan Kerby-Collins & William Smith
    /// Date: 17/02/22
    /// </summary>
    public interface IAnimation
    {
        #region METHODS

        /// <summary>
        /// METHOD: Input gives access to the Input class and passes in pString
        /// </summary>
        /// <param name="pInput"> Key Name </param>
        void Input(string pInput);

        #endregion


        #region PROPERTIES

        /// <summary>
        /// PROPERTY: int Row
        /// </summary>
        int Row { get; set; }

        /// <summary>
        /// PROPERTY: Vector2 Destination
        /// </summary>
        Vector2 Destination { get; set; }

        /// <summary>
        /// PROPERTY: Point SpriteSize
        /// </summary>
        Point SpriteSize { get; set; }

        #endregion
    }
}
