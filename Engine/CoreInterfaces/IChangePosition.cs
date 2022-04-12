using Microsoft.Xna.Framework;

namespace OrbitalEngine.CoreInterfaces
{
    /// <summary>
    /// Interface which allows implementations to have their positional values changed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public interface IChangePosition
    {
        #region METHODS

        /// <summary>
        /// Changes positional values of an implementation, used with commands
        /// </summary>
        /// <param name="pPosition"> XY Positional Values </param>
        void ChangePosition(Vector2 pPosition);

        #endregion
    }
}
