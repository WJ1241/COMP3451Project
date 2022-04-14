using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;
using COMP3451Project.RIRRPackage.Entities.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for entities with health characteristics
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 14/04/22
    /// </summary>
    public class HealthBehaviour : UpdatableBehaviour, IEventListener<CollisionEventArgs>, IInitialiseParam<IDictionary<string, ICommand>>, IInitialiseParam<string, ICommand>
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, ICommand>, name it '_commandDict':
        private IDictionary<string, ICommand> _commandDict;

        // DECLARE an int, name it '_dmgTimer':
        private int _dmgTimer;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of HealthBehaviour 
        /// </summary>
        public HealthBehaviour()
        {
            // INITIALISE _dmgTimer with a value of '0':
            _dmgTimer = 0;
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method called when needing to perform behaviour based on collision
        /// </summary>
        /// <param name="pSource"> Object that has collided with another object </param>
        /// <param name="pArgs"> EventArgs for an Collidable object </param>
        public void OnEvent(object pSource, CollisionEventArgs pArgs)
        {
            // IF pArgs.RequiredArg is on layer 4 OR
            // IF pArgs.RequiredArg is on Layer 6 and _entity's Damaged Property is FALSE:
            if ((pArgs.RequiredArg as ILayer).Layer == 4
            || ((pArgs.RequiredArg as ILayer).Layer == 6 && !(_entity as ITakeDamage).Damaged))
            {
                // CALL AffectHealth(), passing pArgs.RequiredArg as a parameter:
                AffectHealth(pArgs.RequiredArg);
            }
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnEvent(object pSource, UpdateEventArgs pArgs)
        {
            // CALL CheckHealth():
            CheckHealth();
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, ICOMMAND>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, ICommand> instance
        /// </summary>
        /// <param name="pCommandDict"> IDictionary<string, ICommand> instance </param>
        public void Initialise(IDictionary<string, ICommand> pCommandDict)
        {
            // IF pCommandDict DOES HAVE an active instance:
            if (pCommandDict != null)
            {
                // INITIALISE _commandDict with reference to pCommandDict:
                _commandDict = pCommandDict;
            }
            // IF pCommandDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: _commandDict does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<STRING, ICOMMAND>

        /// <summary>
        /// Initialises an object with a string and an ICommand object
        /// </summary>
        /// <param name="pCommandName"> Name of Command </param>
        /// <param name="pCommand"> ICommand object </param>
        public void Initialise(string pCommandName, ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // IF _commandDict DOES NOT already contain pCommandName as a key:
                if (!_commandDict.ContainsKey(pCommandName))
                {
                    // ADD pCommandName as a key, and pCommand as a value to _commandDict:
                    _commandDict.Add(pCommandName, pCommand);
                }
                // IF _commandDict DOES already contain pCommandName as a key:
                else
                {
                    // THROW a new NullInstanceException(), with corresponding message:
                    throw new ValueAlreadyStoredException("ERROR: pCommandName is already stored in _commandDict!");
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


        #region PRIVATE METHODS

        /// <summary>
        /// Called to see if Player has run out of HP and to check if they are damaged
        /// </summary>
        /// <CITATION> (Smith, 2021) </CITATION>
        private void CheckHealth()
        {
            // IF _entity's Damaged Property is TRUE:
            if ((_entity as ITakeDamage).Damaged)
            {
                // SET colour of _entity to Red:
                (_entity as IChangeTexColour).TexColour = Color.IndianRed;

                // INCREMENT _dmgTimer by '1':
                _dmgTimer++;

                // IF _dmgTimer is greater or equal to 60, used due to 60 FPS in Framework:
                if (_dmgTimer >= 60)
                {
                    // RESET _dmgTimer to '0':
                    _dmgTimer = 0;

                    // SET _entity's Damaged Property to false:
                    (_entity as ITakeDamage).Damaged = false;
                }
            }
            // IF _entity's Damaged Property is FALSE:
            else
            {
                // IF _entity's TexColour IS NOT White:
                if ((_entity as IChangeTexColour).TexColour != Color.White)
                {
                    // SET TexColour Property value of _entity to White:
                    (_entity as IChangeTexColour).TexColour = Color.White;
                }
            }

            // IF _entity has lost all of its health points:
            if ((_entity as IHaveHealth).HealthPoints <= 0)
            {
                // SCHEDULE _commandDict["ResetScene"] to be executed:
                (_entity as ICommandSender).ScheduleCommand(_commandDict["ResetScene"]);
            }
        }

        /// <summary>
        /// Changes Health Points either positively or negatively
        /// </summary>
        /// <param name="pScndCollidable"> Other collidable to see how health should be impacted </param>
        private void AffectHealth(ICollidable pScndCollidable)
        {
            // IF bottom half of _entity is in contact with pScndCollidable:
            // USING HALF OF HITBOX FOR ANGLE EFFECT
            if ((_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y <= pScndCollidable.HitBox.Bottom)
            {
                // IF pScndCollidable is on layer 4:
                if ((pScndCollidable as ILayer).Layer == 4)
                {
                    // IF _entity's health points are less than it's maximum health points:
                    if ((_entity as IHaveHealth).HealthPoints < (_entity as IHaveHealth).MaxHealthPoints)
                    {
                        // INCREMENT _entity's HealthPoints Property value by '1':
                        (_entity as IHaveHealth).HealthPoints++;
                    }
                }

                // IF pScndCollidable is on layer 6 and _entity's Damaged Property is FALSE:
                if ((pScndCollidable as ILayer).Layer == 6 && !(_entity as ITakeDamage).Damaged)
                {
                    // SET _entity's Damaged Property to true:
                    (_entity as ITakeDamage).Damaged = true;

                    // IF _dmgTimer HAS NOT started yet:
                    if (_dmgTimer <= 0)
                    {
                        // DECREMENT _entity's HealthPoints Property value by '1':
                        (_entity as IHaveHealth).HealthPoints--;
                    }
                }

                // INITIALISE _commandDict["UpdateHealthDisplay"]'s FirstParam Property with value of _entity.HealthPoints:
                (_commandDict["UpdateHealthDisplay"] as ICommandOneParam<int>).FirstParam = (_entity as IHaveHealth).HealthPoints;

                // SCHEDULE _updateHealthDisplayCommand to be executed:
                (_entity as ICommandSender).ScheduleCommand(_commandDict["UpdateHealthDisplay"]);
            }
        }

        #endregion
    }
}
