using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using COMP3451Project.EnginePackage.CoreInterfaces;

namespace COMP3451Project.EnginePackage.Audio
{
    /// <summary>
    /// Class which stores sound effects and plays a requested sound effect when needed
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 04/02/22
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
            // ADD pSFXName as a key, and pSFXFile as a value to _sfxDict:
            _sfxDict.Add(pSFXName, pSFXFile);
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
