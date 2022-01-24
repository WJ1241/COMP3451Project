using System;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.CoreInterfaces;


namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Abstract class for more specific entities to inherit from
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 24/01/22
    /// </summary>
    public abstract class Entity : IEntity, IEntityInternal, ICommandSender, ILayer, IContainBoundary, ITerminate
    {
        #region FIELD VARIABLES

        // DECLARE an int, name it '_layer', used to determine collision layer:
        protected int _layer;

        // DECLARE an int, name it '_uID', used to store unique ID:
        protected int _uID;

        // DECLARE an string, name it '_uName', used to store unique Name:
        protected string _uName;

        // DECLARE a Vector2, name it '_initPosition', needed to store initial location, in the case of resetting game:
        protected Vector2 _initPosition;

        // DECLARE a Vector2, name it '_position', stores current location, needed to draw texture when location(x,y) is changed
        protected Vector2 _position;

        // DECLARE a Vector2, name it '_windowBorder', used for storing screen size:
        protected Vector2 _windowBorder;

        // DECLARE an ICommand, name it '_terminateMe', used to store termination command:
        protected ICommand _terminateMe;

        // DECLARE an ICommand, name it '_removeMe', used to store removal command:
        protected ICommand _removeMe;

        // DECLARE an Action<ICommand>, name it '_scheduleCommand', used to schedule a command:
        protected Action<ICommand> _scheduleCommand;

        #endregion


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Initialises entity variable values
        /// </summary>
        public abstract void Initialise();

        /// <summary>
        /// Property which can get and set value of an entity's position
        /// </summary>
        public Vector2 Position
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
        /// Layer 1: Walls
        /// Layer 2: Floors
        /// Layer 3: Static Obstacles
        /// Layer 4: Items
        /// Layer 5: Player/Enemy
        /// Layer 6: GUI
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
        public abstract void Terminate();

        #endregion
    }
}
