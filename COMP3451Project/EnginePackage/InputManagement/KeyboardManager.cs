using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.InputManagement
{
    /// <summary>
    /// Class which manages all entities listening for Keyboard input
    /// </summary>
    public class KeyboardManager : IUpdatable, IKeyboardPublisher
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IKeyboardListener>, call it '_kBListeners':
        private IDictionary<string, IKeyboardListener> _kBListeners;

        // DECLARE a KeyboardState, call it '_keyboardState':
        private KeyboardState _keyboardState;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of KeyboardManager
        /// </summary>
        public KeyboardManager() 
        {
            // INSTANTIATE _kBListeners as new Dictionary<string, IKeyboardListener>:
            _kBListeners = new Dictionary<string, IKeyboardListener>();
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDPUBLISHER

        /// <summary>
        /// Subscribes a Keyboard listening object to be stored in a list/dictionary
        /// </summary>
        /// <param name="KeyboardListener">Reference to an object implementing IKeyboardListener</param>
        public void Subscribe(IKeyboardListener keyboardListener)
        {
            // ADD KeyboardListener to Dictionary<string, IKeyboardListener>:
            _kBListeners.Add((keyboardListener as IEntity).UName, keyboardListener);
        }

        /// <summary>
        /// Unsubscribes a Keyboard listening object from list/dictionary using its unique name
        /// </summary>
        /// <param name="uName">Used for passing unique name</param>
        public void Unsubscribe(string uName)
        {
            // CALL Remove(), on Dictionary to remove 'value' of key 'uName':
            _kBListeners.Remove(uName);
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        public void Update(GameTime gameTime)
        {
            // ASSIGNMENT, use GetState() to get what keys have been activated:
            _keyboardState = Keyboard.GetState();

            foreach (IKeyboardListener keyboardListener in _kBListeners.Values) // FOREACH IKeyboardListener objects in _kBListeners
            {
                // CALL 'OnKBInput()' passing an KeyboardState as a parameter, used to get Keyboard input:
                keyboardListener.OnKBInput(_keyboardState);
            }
        }

        #endregion
    }
}
