using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services;

namespace COMP3451Project.EnginePackage.InputManagement
{
    /// <summary>
    /// Class which manages all entities listening for Mouse input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/02/22
    /// </summary>
    public class MouseManager : IUpdatable, IMousePublisher, IService
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IMouseListener>, call it '_mouseListeners':
        private IDictionary<string, IMouseListener> _mouseListeners;

        // DECLARE a MouseState, call it '_mouseState':
        private MouseState _mouseState;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of MouseManager
        /// </summary>
        public MouseManager()
        {
            // INSTANTIATE _mouseListeners as new Dictionary<string, IMouseListener>:
            _mouseListeners = new Dictionary<string, IMouseListener>();
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDPUBLISHER

        /// <summary>
        /// Subscribes a Mouse listening object to be stored in a list/dictionary
        /// </summary>
        /// <param name="mouseListener">Reference to an object implementing IMouseListener</param>
        public void Subscribe(IMouseListener mouseListener)
        {
            // ADD KeyboardListener to Dictionary<string, IMouseListener>:
            _mouseListeners.Add((mouseListener as IEntity).UName, mouseListener);
        }

        /// <summary>
        /// Unsubscribes a Mouse listening object from list/dictionary using its unique name
        /// </summary>
        /// <param name="uName">Used for passing unique name</param>
        public void Unsubscribe(string uName)
        {
            // CALL Remove(), on Dictionary to remove 'value' of key 'uName':
            _mouseListeners.Remove(uName);
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
            _mouseState = Mouse.GetState();

            foreach (IMouseListener mouseListener in _mouseListeners.Values) // FOREACH IMouseListener objects in _mouseListeners
            {
                // CALL 'OnMouseInput()' passing an MouseState as a parameter, used to get Mouse input:
                mouseListener.OnMouseInput(_mouseState);
            }
        }

        #endregion
    }
}
