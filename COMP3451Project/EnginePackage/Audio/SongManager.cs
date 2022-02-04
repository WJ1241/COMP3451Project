using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using COMP3451Project.EnginePackage.CoreInterfaces;

namespace COMP3451Project.EnginePackage.Audio
{
    /// <summary>
    /// Class which stores songs and plays a requested song when needed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 04/02/22
    /// </summary>
    public class SongManager : IInitialiseParam<string, Song>, IPlayAudio
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, Song>, name it '_songDict':
        private IDictionary<string, Song> _songDict;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SongManager
        /// </summary>
        public SongManager()
        {
            // INSTANTIATE _songDict as a new Dictionary<string, Song>():
            _songDict = new Dictionary<string, Song>();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, SONG>

        /// <summary>
        /// Initialises an object with a string and a Song
        /// </summary>
        /// <param name="pSongName"> Name of Song </param>
        /// <param name="pSongFile"> Song File </param>
        public void Initialise(string pSongName, Song pSongFile)
        {
            // ADD pSongName as a key, and pSongFile as a value to _sfxDict:
            _songDict.Add(pSongName, pSongFile);
        }

        #endregion


        #region IMPLEMENTATION OF IPLAYAUDIO

        /// <summary>
        /// Plays an audio clip related to the string parameter, either an effect or a song 
        /// </summary>
        /// <param name="pSoundFile"> Name of the Audio File to play </param>
        public void PlayAudio(string pSoundFile)
        {
            // PLAY song addressed as pSoundFile in _songDict using MediaPlayer:
            MediaPlayer.Play(_songDict[pSoundFile]);
        }

        #endregion
    }
}
