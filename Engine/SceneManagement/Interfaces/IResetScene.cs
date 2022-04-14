
namespace OrbitalEngine.SceneManagement.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to Reset the current scene
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    public interface IResetScene
    {
        #region METHODS

        /// <summary>
        /// Clears all references to entities in current scene and signals command to reset current scene
        /// </summary>
        void ResetScene();

        #endregion
    }
}
