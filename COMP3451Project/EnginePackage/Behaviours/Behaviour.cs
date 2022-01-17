﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.Behaviours
{
    /// <summary>
    /// Class which contains basic variables and methods needed for ALL behaviour classes
    /// Author(s): William Smith & Declan Kerby-Collins
    /// Date: 17/01/22
    /// </summary>
    public class Behaviour : IBehaviour, IInitialiseIEntity
    {
        #region FIELD VARIABLES

        // DECLARE an IEntity, name it '_entity':
        private IEntity _entity;

        #endregion


        #region IMPLEMENTATION OF IBEHAVIOUR

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public void OnUpdate(object pSource, UpdateEventArgs pArgs)
        {

        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEIENTITY

        /// <summary>
        /// Initialises an object with an IEntity object
        /// </summary>
        /// <param name="entity"> IEntity object </param>
        public void Initialise(IEntity pEntity)
        {
            // INITIALISE _entity with reference to instance of pEntity:
            _entity = pEntity;
        }

        #endregion
    }
}