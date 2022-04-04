﻿using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;

namespace OrbitalEngine.Behaviours
{
    /// <summary>
    /// Class which contains basic variables and methods needed for ALL behaviour classes
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/02/22
    /// </summary>
    public abstract class Behaviour : IUpdateEventListener, IInitialiseParam<IEntity>, IName
    {
        #region FIELD VARIABLES

        // DECLARE an IEntity, name it '_entity':
        protected IEntity _entity;

        // DECLARE a string, name it '_name':
        protected string _name;

        #endregion


        #region IMPLEMENTATION OF IUPDATEEVENTLISTENER

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public abstract void OnUpdateEvent(object pSource, UpdateEventArgs pArgs);

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IENTITY>

        /// <summary>
        /// Initialises an object with an IEntity object
        /// </summary>
        /// <param name="pEntity"> IEntity object </param>
        public virtual void Initialise(IEntity pEntity)
        {
            // IF pEntity DOES HAVE an active instance:
            if (pEntity != null)
            {
                // INITIALISE _entity with reference to instance of pEntity:
                _entity = pEntity;
            }
            // IF pEntity DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pEntity does not have an active instance!");
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
                // RETURN value of _name:
                return _name;
            }
            set
            {
                // SET value of _name to incoming value:
                _name = value;
            }
        }

        #endregion
    }
}