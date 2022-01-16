﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Commands
{
    /// <summary>
    /// Interface which allows implementations send commands to another class
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 06/01/22
    /// </summary>
    public interface ICommandSender
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows write access to a command scheduling method
        /// </summary>
        Action<ICommand> ScheduleCommand { set; }

        #endregion
    }
}
