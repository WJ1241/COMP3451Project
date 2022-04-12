

namespace OrbitalEngine.Audio.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to manage song tracks in a game loop
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public interface ISongManager : IPlayAudio
    {
        #region METHODS

        /// <summary>
        /// Stops an audio track from looping or playing over a scene it does not belong in
        /// </summary>
        void StopAudio();

        #endregion
    }
}
