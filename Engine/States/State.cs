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
    /// Date: 19/02/22
    /// </summary>
    public abstract class State : IState, ICommandSender, IInitialiseParam<string, ICommand>, IInitialiseParam<IUpdateEventListener>, IName, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, ICommand>, name it ' _triggerDict':
        protected IDictionary<string, ICommand> _triggerDict;

        // DECLARE an Action<ICommand>, name it '_scheduleCommand', used to schedule a command:
        protected Action<ICommand> _scheduleCommand;

        // DECLARE an EventHandler<UpdateEventArgs>, name it '_behaviourEvent':
        protected EventHandler<UpdateEventArgs> _behaviourEvent;

        // DECLARE a string, name it '_stateName':
        protected string _stateName;

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


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, ICOMMAND>

        /// <summary>
        /// Initialises an object with an ICommand object
        /// </summary>
        /// <param name="pCommandName"> String Value </param>
        /// <param name="pCommand"> Reference to an ICommand object </param>
        public void Initialise(string pCommandName, ICommand pCommand)
        {
            // IF pCommandName DOES NOT HAVE an active instance:
            if (pCommandName != null)
            {
                // IF _triggerDict DOES NOT contain pCommandName as a key:
                if (!_triggerDict.ContainsKey(pCommandName))
                {
                    // ADD pCommandName as a key, and pCommand as a value to _sfxDict:
                    _triggerDict.Add(pCommandName, pCommand);
                }
                // IF _triggerDict DOES contain value of pSFXName already:
                else
                {
                    // THROW a new ValueAlreadyStoredException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pCommandName already stored in _sfxDict!");
                }
            }
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IUPDATEEVENTLISTENER>

        /// <summary>
        /// Initialises an object with an IUpdateEventListener object
        /// </summary>
        /// <param name="pUpdateEventListener"> IUpdateEventListener object </param>
        public virtual void Initialise(IUpdateEventListener pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // SUBSCRIBE _behaviourEvent to pUpdateEventListener.OnUpdateEvent():
                _behaviourEvent += pUpdateEventListener.OnUpdateEvent;
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance!");
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


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime">holds reference to GameTime object</param>
        public virtual void Update(GameTime pGameTime)
        {
            // DECLARE & INITIALISE an UpdateEventArgs, name it 'tempUpdateEA':
            UpdateEventArgs tempUpdateEA = new UpdateEventArgs();

            // SET RequiredArg Property's value to pGameTime:
            tempUpdateEA.RequiredArg = pGameTime;

            // CALL Invoke on _behaviourEvent, passing this class and _tempUpdateEA as parameters:
            _behaviourEvent.Invoke(this, tempUpdateEA);
        }

        #endregion
    }
}
