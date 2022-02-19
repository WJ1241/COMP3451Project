using Microsoft.Xna.Framework;
using OrbitalEngine.EntityManagement.Interfaces;

namespace OrbitalEngine.SceneManagement.Interfaces
{
    /// <summary>
    /// Interface that allows implementations to be spawned on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public interface ISpawn
    {
        #region METHODS

        /// <summary>
        /// Spawns specified Entity and initialises its position
        /// </summary>
        /// <param name="pEntity"> IEntity object </param>
        /// <param name="pPosition"> Positional values used to place entity </param>
        void Spawn(IEntity pEntity, Vector2 pPosition);

        #endregion
    }
}
