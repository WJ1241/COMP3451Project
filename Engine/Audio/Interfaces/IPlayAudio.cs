

namespace COMP3451Project.EnginePackage.Audio.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to play audio when given a specified string
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 04/02/22
    /// </summary>
    public interface IPlayAudio
    {
        #region METHODS

        /// <summary>
        /// Plays an audio clip related to the string parameter, either an effect or a song 
        /// </summary>
        /// <param name="pSoundFile"> Name of the Audio File to play </param>
        void PlayAudio(string pSoundFile);

        #endregion
    }
}
