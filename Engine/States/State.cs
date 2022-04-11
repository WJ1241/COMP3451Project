using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;
using OrbitalEngine.States.Interfaces;

namespace OrbitalEngine.States
{
    /// <summary>
    /// Class which contains conditional information for entities to be modified by another class e.g. Behaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public class State : IState, ICommandSender, IInitialiseParam<IDictionary<string, EventArgs>>, IInitialiseParam<IDictionary<string, ICommand>>,
        IInitialiseParam<EventArgs>, IInitialiseParam<string, ICommand>, IName
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, ICommand>, name it ' _triggerDict':
        protected IDictionary<string, ICommand> _triggerDict;

        // DECLARE an IDictionary<string, EventArgs>, name it '_argsDict':
        protected IDictionary<string, EventArgs> _argsDict;

        // DECLARE an Action<ICommand>, name it '_scheduleCommand', used to schedule a command:
        protected Action<ICommand> _scheduleCommand;

        // DECLARE a string, name it '_stateName':
        protected string _stateName;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of State
        /// </summary>
        public State()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ICOMMANDSENDER

        /// <summary>
        /// Property which allows read and write access to a command scheduling method
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, EVENTARGS>>

        /// <summary>
        /// Initialises an object with an IDictionary<string, EventArgs> object
        /// </summary>
        /// <param name="pArgsDict"> IDictionary<string, EventArgs> object </param>
        public void Initialise(IDictionary<string, EventArgs> pArgsDict)
        {
            // IF pArgsDict DOES HAVE an active instance:
            if (pArgsDict != null)
            {
                // INITIALISE _argsDict with reference to pArgsDict:
                _argsDict = pArgsDict;
            }
            // IF pArgsDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pArgsDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, ICOMMAND>>

        /// <summary>
        /// Initialises an object with an IDictionary<string, ICommand> object
        /// </summary>
        /// <param name="pCommandDict"> IDictionary<string, ICommand> object </param>
        public void Initialise(IDictionary<string, ICommand> pCommandDict)
        {
            // IF pCommandDict DOES HAVE an active instance:
            if (pCommandDict != null)
            {
                // INITIALISE _triggerDict with reference to pCommandDict:
                _triggerDict = pCommandDict;
            }
            // IF pCommandDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommandDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<EVENTARGS>

        /// <summary>
        /// Initialises an object with an EventArgs object
        /// </summary>
        /// <param name="pArgs"> Reference to an EventArgs object </param>
        public void Initialise(EventArgs pArgs)
        {
            // IF pArgs DOES HAVE an active instance:
            if (pArgs != null)
            {
                // IF _argsDict DOES NOT contain pArgs.GetType().Name as a key:
                if (!_argsDict.ContainsKey(pArgs.GetType().Name))
                {
                    // ADD pArgs.GetType().Name as a key, and pArgs as a value to _argsDict:
                    _argsDict.Add(pArgs.GetType().Name, pArgs);
                }
                // IF _argsDict DOES contain value of pArgs.GetType().Name already:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pArgs.GetType().Name already stored in _argsDict!");
                }
            }
            // IF pArgs DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, ICOMMAND>

        /// <summary>
        /// Initialises an object with a string and an ICommand object
        /// </summary>
        /// <param name="pCommandName"> Name of Command </param>
        /// <param name="pCommand"> Reference to an ICommand object </param>
        public void Initialise(string pCommandName, ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // IF _triggerDict DOES NOT contain pCommandName as a key:
                if (!_triggerDict.ContainsKey(pCommandName))
                {
                    // ADD pCommandName as a key, and pCommand as a value to _triggerDict:
                    _triggerDict.Add(pCommandName, pCommand);
                }
                // IF _triggerDict DOES contain value of pCommandName already:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pCommandName already stored in _triggerDict!");
                }
            }
            // IF pCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF INAME

        /// <summary>
        /// Property which allows read and write access to the value of an object's specific name
        /// </summary>
        public string Name
        {
            get
            {
                // RETURN value of _stateName:
                return _stateName;
            }
            set
            {
                // SET value of _stateName to incoming value:
                _stateName = value;
            }
        }

        #endregion
    }
}
