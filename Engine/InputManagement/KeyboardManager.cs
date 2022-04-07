using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.InputManagement.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.InputManagement
{
    /// <summary>
    /// Class which manages all entities listening for Keyboard input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public class KeyboardManager : IKeyboardPublisher, IInitialiseParam<IDictionary<string, IKeyboardListener>>, IService, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IKeyboardListener>, name it '_kBListeners':
        private IDictionary<string, IKeyboardListener> _kBListeners;

        // DECLARE a KeyboardState, name it '_keyboardState':
        private KeyboardState _keyboardState;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of KeyboardManager
        /// </summary>
        public KeyboardManager() 
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDPUBLISHER

        /// <summary>
        /// Subscribes a Keyboard listening object to be stored in a list/dictionary
        /// </summary>
        /// <param name="pKeyboardListener"> Reference to an object implementing IKeyboardListener </param>
        public void Subscribe(IKeyboardListener pKeyboardListener)
        {
            // ADD KeyboardListener to Dictionary<string, IKeyboardListener>:
            _kBListeners.Add((pKeyboardListener as IEntity).UName, pKeyboardListener);
        }

        /// <summary>
        /// Unsubscribes a Keyboard listening object from list/dictionary using its unique name
        /// </summary>
        /// <param name="pUName">Used for passing unique name</param>
        public void Unsubscribe(string pUName)
        {
            // CALL Remove(), on Dictionary to remove 'value' of key 'uName':
            _kBListeners.Remove(pUName);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, IKEYBOARDLISTENER>>

        /// <summary>
        /// Initialises an object with an IDictionary<string, IKeyboardListener> instance
        /// </summary>
        /// <param name="pKBListenerDict"> IDictionary<string, IKeyboardListener> object </param>
        public void Initialise(IDictionary<string, IKeyboardListener> pKBListenerDict)
        {
            // IF pKBListenerDict DOES HAVE an active instance:
            if (pKBListenerDict != null)
            {
                // INITIALISE _kBListeners with reference to pKBListenerDict:
                _kBListeners = pKBListenerDict;
            }
            // IF pKBListenerDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pKBListenerDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> Holds reference to GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // ASSIGNMENT, use GetState() to get what keys have been activated:
            _keyboardState = Keyboard.GetState();

            // FOREACH IKeyboardListener objects in _kBListeners:
            foreach (IKeyboardListener pKeyboardListener in _kBListeners.Values)
            {
                // CALL 'OnKBInput()' passing a KeyboardState as a parameter, used to get Keyboard input:
                pKeyboardListener.OnKBInput(_keyboardState);
            }
        }

        #endregion
    }
}
