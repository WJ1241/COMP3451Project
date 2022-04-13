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
    /// Date: 12/04/22
    /// </summary>
    public class HealthBehaviour : UpdatableBehaviour, IEventListener<CollisionEventArgs>, IInitialiseParam<ICommand>
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_updateHealthDisplayCommand':
        private ICommand _updateHealthDisplayCommand;

        // DECLARE a static bool, name it '_damaged':
        private bool _damaged;

        // DECLARE a static int, name it '_dmgTimer':
        private int _dmgTimer;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of HealthBehaviour 
        /// </summary>
        public HealthBehaviour()
        {
            // SET _damaged to false:
            _damaged = false;

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
            // IF pArgs.RequiredArg is on layer 6 and _damaged is FALSE:
            if ((pArgs.RequiredArg as ILayer).Layer == 6 && !_damaged)
            {
                // CALL Damage(), passing pArgs.RequiredArg as a parameter:
                Damage(pArgs.RequiredArg);
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


        #region IMPLEMENTATION OF IINITIALISEPARAM<ICOMMAND>

        /// <summary>
        /// Initialises an object with an ICommand object
        /// </summary>
        /// <param name="pCommand"> ICommand object </param>
        public void Initialise(ICommand pCommand)
        {
            // IF pCommand DOES HAVE an active instance:
            if (pCommand != null)
            {
                // INITIALISE _updateHealthDisplayCommand with reference to pCommand:
                _updateHealthDisplayCommand = pCommand;
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
            // IF _damaged is TRUE:
            if (_damaged)
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

                    // SET _damaged to false:
                    _damaged = false;
                }
            }
            // IF _damaged is FALSE:
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
                // RESTART LEVEL STUFF HERE
                /*
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 * 
                 */
            }
        }

        /// <summary>
        /// Damages the Player and reduces HP
        /// </summary>
        /// <param name="pScndCollidable"> Value to impact Player's health points with </param>
        private void Damage(ICollidable pScndCollidable)
        {
            // IF bottom half of _entity is in contact with pScndCollidable:
            // USING HALF OF HITBOX FOR ANGLE EFFECT
            if ((_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y <= pScndCollidable.HitBox.Bottom)
            {
                // SET _damaged to true:
                _damaged = true;

                // IF _dmgTimer HAS NOT started yet:
                if (_dmgTimer <= 0)
                {
                    // DECREMENT _entity's HealthPoints Property value by '1':
                    (_entity as IHaveHealth).HealthPoints--;

                    // INITIALISE _updateHealthDisplayCommand's FirstParam Property with value of _entity.HealthPoints:
                    (_updateHealthDisplayCommand as ICommandOneParam<int>).FirstParam = (_entity as IHaveHealth).HealthPoints;

                    // SCHEDULE _updateHealthDisplayCommand to be executed:
                    (_entity as ICommandSender).ScheduleCommand(_updateHealthDisplayCommand);
                }
            }
        }

        #endregion
    }
}
