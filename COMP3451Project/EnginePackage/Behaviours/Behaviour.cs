using COMP3451Project.EnginePackage.Behaviours.Interfaces;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.EntityManagement.Interfaces;
using COMP3451Project.EnginePackage.Exceptions;

namespace COMP3451Project.EnginePackage.Behaviours
{
    /// <summary>
    /// Class which contains basic variables and methods needed for ALL behaviour classes
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 18/02/22
    /// </summary>
    public abstract class Behaviour : IUpdateEventListener, IInitialiseParam<IEntity>
    {
        #region FIELD VARIABLES

        // DECLARE an IEntity, name it '_entity':
        protected IEntity _entity;

        // DECLARE a string, name it '_behaviourName':
        private string _behaviourName;

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
    }
}
