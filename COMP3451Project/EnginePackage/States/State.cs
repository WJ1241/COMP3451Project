using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.Services.Commands;

namespace COMP3451Project.EnginePackage.States
{
    /// <summary>
    /// Class which contains conditional information for entities to be modified by another class e.g. Behaviour
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    public abstract class State : IState, ICommandSender, IName, IUpdatable
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


        #region PROPERTIES

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
            // DECLARE & INITIALISE an UpdateEventArgs, name it '_tempUpdateEA':
            UpdateEventArgs _tempUpdateEA = new UpdateEventArgs();

            // SET RequiredArg Property's value to pGameTime:
            _tempUpdateEA.RequiredArg = pGameTime;

            // CALL Invoke on _behaviourEvent, passing this class and _tempUpdateEA as parameters:
            _behaviourEvent.Invoke(this, _tempUpdateEA);
        }

        #endregion
    }
}
