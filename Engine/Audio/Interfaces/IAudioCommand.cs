using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.Audio.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to have a reference to an audio playing command
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
    /// </summary>
    public interface IAudioCommand
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows only write access to an ICommand which plays audio
        /// </summary>
        ICommand AudioCommand { set; }

        #endregion
    }
}
