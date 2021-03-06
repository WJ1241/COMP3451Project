using System.Collections.Generic;
using Microsoft.Xna.Framework.Media;
using OrbitalEngine.Audio.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.Audio
{
    /// <summary>
    /// Class which stores songs and plays a requested song when needed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public class SongManager : ISongManager, IInitialiseParam<IDictionary<string, Song>>, IInitialiseParam<string, Song>, IService
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
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, SONG>>

        /// <summary>
        /// Initialises an object with an IDictionary<string, Song> instance
        /// </summary>
        /// <param name="pSongDict"> IDictionary<string, Song> instance </param>
        public void Initialise(IDictionary<string, Song> pSongDict)
        {
            // IF pSongDict DOES NOT HAVE an active instance:
            if (pSongDict != null)
            {
                // INITIALISE _songDict with reference to pSongDict:
                _songDict = pSongDict;
            }
            // IF pSongDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pSongDict does not have an active instance!");
            }
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
            // IF pSongFile DOES HAVE an active instance:
            if (pSongFile != null)
            {
                // IF _songDict DOES NOT contain pSongName as a key:
                if (!_songDict.ContainsKey(pSongName))
                {
                    // ADD pSongName as a key, and pSongFile as a value to _songDict:
                    _songDict.Add(pSongName, pSongFile);
                }
                // IF _songDict DOES contain value of pSongName already:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pSongName already stored in _songDict!");
                }
            }
            // IF pSongFile DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pSongFile does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IPLAYAUDIO

        /// <summary>
        /// Plays an audio clip related to the string parameter, either an effect or a song 
        /// </summary>
        /// <param name="pSoundFile"> Name of the Audio File to play </param>
        public void PlayAudio(string pSoundFile)
        {
            // SET volume of audio to 0.5f, as it starts VERY loud:
            MediaPlayer.Volume = 0.5f;

            // SET MediaPlayer to put track on loop:
            MediaPlayer.IsRepeating = true;

            // PLAY song addressed as pSoundFile in _songDict using MediaPlayer:
            MediaPlayer.Play(_songDict[pSoundFile]);
        }

        /// <summary>
        /// Stops an audio track from looping or playing over a scene it does not belong in
        /// </summary>
        public void StopAudio()
        {
            // CALL Stop() on MediaPlayer so that song ends when not needing to play:
            MediaPlayer.Stop();
        }

        #endregion
    }
}