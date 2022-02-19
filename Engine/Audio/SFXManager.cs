using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using OrbitalEngine.Audio.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.Audio
{
    /// <summary>
    /// Class which stores sound effects and plays a requested sound effect when needed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public class SFXManager : IInitialiseParam<string, SoundEffect>, IPlayAudio
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, SoundEffect>, name it '_sfxDict':
        private IDictionary<string, SoundEffect> _sfxDict;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SFXManager
        /// </summary>
        public SFXManager()
        {
            // INSTANTIATE _sfxDict as a new Dictionary<string, SoundEffect>():
            _sfxDict = new Dictionary<string, SoundEffect>();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, SOUNDEFFECT>

        /// <summary>
        /// Initialises an object with a string and a SoundEffect
        /// </summary>
        /// <param name="pSFXName"> Name of Sound Effect </param>
        /// <param name="pSFXFile"> Sound Effect File </param>
        public void Initialise(string pSFXName, SoundEffect pSFXFile)
        {
            // IF pSFXFile DOES NOT HAVE an active instance:
            if (pSFXFile != null)
            {
                // IF _sfxDict DOES NOT contain pSFXName as a key:
                if (!_sfxDict.ContainsKey(pSFXName))
                {
                    // ADD pSFXName as a key, and pSFXFile as a value to _sfxDict:
                    _sfxDict.Add(pSFXName, pSFXFile);
                }
                // IF _sfxDict DOES contain value of pSFXName already:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pSFXName already stored in _sfxDict!");
                }
            }
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pSFXFile does not have an active instance!");
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
            // PLAY sound effect addressed as value of pSoundFile in _sfxDict:
            _sfxDict[pSoundFile].Play();
        }

        #endregion
    }
}