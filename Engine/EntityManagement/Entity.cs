using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.States.Interfaces;

namespace OrbitalEngine.EntityManagement
{
    /// <summary>
    /// Abstract class for more specific entities to inherit from
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
    /// </summary>
    public abstract class Entity : IEntity, IEntityInternal, IInitialiseParam<IState>, ICommandSender, ILayer, IContainBoundary, ITerminate
    {
        #region FIELD VARIABLES

        // DECLARE an IState, name it '_currentState', used for changing behaviour related to current state:
        protected IState _currentState;

        // DECLARE an ICommand, name it '_terminateMe', used to store termination command:
        protected ICommand _terminateMe;

        // DECLARE an ICommand, name it '_removeMe', used to store removal command:
        protected ICommand _removeMe;

        // DECLARE an Action<ICommand>, name it '_scheduleCommand', used to schedule a command:
        protected Action<ICommand> _scheduleCommand;

        // DECLARE a Vector2, name it '_initPosition', needed to store initial location, in the case of resetting game:
        protected Vector2 _initPosition;

        // DECLARE a Vector2, name it '_position', stores current location, needed to draw texture when location(x,y) is changed
        protected Vector2 _position;

        // DECLARE a Vector2, name it '_windowBorder', used for storing screen size:
        protected Vector2 _windowBorder;

        // DECLARE an int, name it '_layer', used to determine collision layer:
        protected int _layer;

        // DECLARE an int, name it '_uID', used to store unique ID:
        protected int _uID;

        // DECLARE an string, name it '_uName', used to store unique Name:
        protected string _uName;

        #endregion


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Property which can get and set value of an entity's position
        /// </summary>
        public virtual Vector2 Position
        {
            get
            {
                // RETURN value of current location(x,y):
                return _position;
            }
            set
            {
                // ASSIGNMENT give location(x,y) value of external class modified value:
                _position = value;
            }
        }

        /// <summary>
        /// Property which can get and set value of an entity's unique ID
        /// </summary>
        public int UID 
        {
            get
            {
                // RETURN value of current _uID:
                return _uID;
            }
            set 
            {
                // ASSIGNMENT give _uID value of external class modified value:
                _uID = value;
            }
        }

        /// <summary>
        /// Property which can get and set value of an entity's unique Name
        /// </summary>
        public string UName 
        {
            get 
            {
                // RETURN value of current _uName:
                return _uName;
            }
            set 
            {
                // ASSIGNMENT give _uName value of external class modified value:
                _uName = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IENTITYINTERNAL

        /// <summary>
        /// Sets the active state of an Entity
        /// </summary>
        /// <param name="pState"> IState instance </param>
        public void SetState(IState pState)
        {
            // SET _currentState to pState:
            _currentState = pState;

            // PRINT State Update message to console:
            Console.WriteLine("STATE OF " + _uName + " CHANGED, IT IS NOW: " + (pState as IName).Name);
        }

        /// <summary>
        /// Property which allows write access to a Termination command
        /// </summary>
        public ICommand TerminateMe
        {
            get
            {
                // RETURN value of _terminateMe:
                return _terminateMe;
            }
            set
            {
                // SET value of _terminateMe to incoming value:
                _terminateMe = value;
            } 
        }

        /// <summary>
        /// Property which allows write access to a Removal command
        /// </summary>
        public ICommand RemoveMe
        {
            get
            {
                // RETURN value of _removeMe:
                return _removeMe;
            }
            set
            {
                // SET value of _removeMe to incoming value:
                _removeMe = value;
            } 
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<ISTATE>

        /// <summary>
        /// Initialises an object with a reference to an IState instance
        /// </summary>
        /// <param name="pState"> IState instance </param>
        public virtual void Initialise(IState pState)
        {
            // IF pState DOES HAVE an active instance:
            if (pState != null)
            {
                // INITIALISE _currentState with instance of pState:
                _currentState = pState;
            }
            // IF pState DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pState does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF ICOMMANDSENDER

        /// <summary>
        /// Property which allows write access to a command scheduling method
        /// </summary>
        public Action<ICommand> ScheduleCommand
        {
            get
            {
                // RETURN value of _scheduleCommand:
                return _scheduleCommand;
            }
            set
            {
                // SET value of _scheduleCommand to incoming value:
                _scheduleCommand = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ILAYER

        /// <summary>
        /// Property which can get and set layer value
        /// Layer 1: Floors
        /// Layer 2: Walls
        /// Layer 3: Static Obstacles
        /// Layer 4: Items
        /// Layer 5: Level Change
        /// Layer 6: Player/NPC
        /// Layer 7: GUI
        /// </summary>
        public int Layer
        {
            get
            {
                // RETURN value of _layer:
                return _layer;
            }
            set
            {
                // SET value of _layer to incoming value:
                _layer = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ICONTAINBOUNDARY

        /// <summary>
        /// Property which has read and write access to the value of screen window borders
        /// </summary>
        public Vector2 WindowBorder
        {
            get
            {
                // RETURN _windowBorder:
                return _windowBorder;
            }
            set
            {
                // ASSIGNMENT give _windowBorder value of external class modified value:
                _windowBorder = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF ITERMINATE

        /// <summary>
        /// Disposes resources to the garbage collector
        /// </summary>
        public virtual void Terminate()
        {
            // SCHEDULE _removeMe to be executed:
            //_scheduleCommand(_removeMe);

            _removeMe.ExecuteMethod();

            // SCHEDULE TerminateMe to be executed:
            //_scheduleCommand(_terminateMe);

            _terminateMe.ExecuteMethod();
        }

        #endregion
    }
}
