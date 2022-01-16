using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Interface that allows implementations to be spawned on screen
    /// </summary>
    public interface ISpawn
    {
        #region METHODS

        /// <summary>
        /// Spawns Entity on screen and adds to a list/dictionary
        /// </summary>
        /// <param name="entity">Reference to an instance of IEntity</param>
        /// <param name="position">Positional values used to place entity</param>
        void Spawn(IEntity entity, Vector2 position);

        #endregion
    }
}
