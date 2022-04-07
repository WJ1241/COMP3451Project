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
    /// Class which manages all entities listening for Mouse input
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public class MouseManager : IMousePublisher, IInitialiseParam<IDictionary<string, IMouseListener>>, IService, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IMouseListener>, name it '_mouseListeners':
        private IDictionary<string, IMouseListener> _mouseListeners;

        // DECLARE a MouseState, name it '_mouseState':
        private MouseState _mouseState;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of MouseManager
        /// </summary>
        public MouseManager()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF IKEYBOARDPUBLISHER

        /// <summary>
        /// Subscribes a Mouse listening object to be stored in a list/dictionary
        /// </summary>
        /// <param name="pMouseListener">Reference to an object implementing IMouseListener</param>
        public void Subscribe(IMouseListener pMouseListener)
        {
            // ADD pMouseListener to Dictionary<string, IMouseListener>:
            _mouseListeners.Add((pMouseListener as IEntity).UName, pMouseListener);
        }

        /// <summary>
        /// Unsubscribes a Mouse listening object from list/dictionary using its unique name
        /// </summary>
        /// <param name="pUName">Used for passing unique name</param>
        public void Unsubscribe(string pUName)
        {
            // CALL Remove(), on Dictionary to remove 'value' of key pUName:
            _mouseListeners.Remove(pUName);
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, IMOUSELISTENER>>

        /// <summary>
        /// Initialises an object with an IDictionary<string, IMouseListener> instance
        /// </summary>
        /// <param name="pMouseListenerDict"> IDictionary<string, IMouseListener> object </param>
        public void Initialise(IDictionary<string, IMouseListener> pMouseListenerDict)
        {
            // IF pMouseListenerDict DOES HAVE an active instance:
            if (pMouseListenerDict != null)
            {
                // INITIALISE _mouseListeners with reference to pMouseListenerDict:
                _mouseListeners = pMouseListenerDict;
            }
            // IF pMouseListenerDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pMouseListenerDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public void Update(GameTime pGameTime)
        {
            // ASSIGNMENT, use GetState() to get what keys have been activated:
            _mouseState = Mouse.GetState();

            // FOREACH IMouseListener objects in _mouseListeners:
            foreach (IMouseListener pMouseListener in _mouseListeners.Values)
            {
                // CALL 'OnMouseInput()' passing an MouseState as a parameter, used to get Mouse input:
                pMouseListener.OnMouseInput(_mouseState);
            }
        }

        #endregion
    }
}
