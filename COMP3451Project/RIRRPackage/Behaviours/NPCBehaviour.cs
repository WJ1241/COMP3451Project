using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for NPC entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 07/04/22
    /// </summary>
    public class NPCBehaviour : PongBehaviour, ICollisionEventListener
    {
        //behaviour class for NPC's 

        // DECLARE an ICommand, name it '_sfxCommand':
        private ICommand _sfxCommand;

        public NPCBehaviour()
        {

        }

        #region ROAMING
        // BEHAVIOUR for when npc is unaware of the player

        private void Roaming()
        {
            // decides if to move
        }

        #endregion


        #region HUNTING
        // BEHAVIOUR for when npc is aware of the player
        private void Hunting()
        {

        }


        #endregion



        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        { }

        #endregion

        /*
            // ASSIGN value of _entity's Velocity to _currentVel:
            _velocity = (_entity as IVelocity).Velocity;

            // IF at top screen edge or bottom screen edge:
            if (_entity.Position.Y <= 0 || _entity.Position.Y >= (_entity as IContainBoundary).WindowBorder.Y - (_entity as ITexture).TextureSize.Y)
            {
                // MULTIPLY _currentVel.Y by '-1':
                _velocity.Y *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _velocity;

                // SET Data Property value of _sfxCommand to "WallHit":
                (_sfxCommand as ICommandOneParam<string>).Data = "WallHit";

                // SCHEDULE _sfxCommand to be executed:
                (_entity as ICommandSender).ScheduleCommand(_sfxCommand);
            }
            // IF at left screen edge or right screen edge:
            else if (_entity.Position.X <= 0 || _entity.Position.X >= (_entity as IContainBoundary).WindowBorder.X - (_entity as ITexture).TextureSize.X)
            {
                // IF at left screen edge:
                if (_entity.Position.X <= 0)
                {
                    // SET Data Property value of _scoreGoal to '2':
                    (_scoreGoal as ICommandOneParam<int>).Data = 2;
                }
                // IF at right screen edge:
                else if (_entity.Position.X >= (_entity as IContainBoundary).WindowBorder.X - (_entity as ITexture).TextureSize.X)
                {
                    // SET Data Property value of _scoreGoal to '1':
                    (_scoreGoal as ICommandOneParam<int>).Data = 1;
                }

                // SCHEDULE _scoreGoal to be executed:
                (_entity as ICommandSender).ScheduleCommand(_scoreGoal);

                // SET Data Property value of _sfxCommand to "Score":
                (_sfxCommand as ICommandOneParam<string>).Data = "Score";

                // SCHEDULE _sfxCommand to be executed:
                (_entity as ICommandSender).ScheduleCommand(_sfxCommand);

                // SCHEDULE RemoveMe to be executed:
                (_entity as ICommandSender).ScheduleCommand((_entity as IEntityInternal).RemoveMe);

                // SCHEDULE TerminateMe to be executed:
                (_entity as ICommandSender).ScheduleCommand((_entity as IEntityInternal).TerminateMe);
            }
        }

        #endregion
        */

        #region IMPLEMENTATION OF ICOLLISIONEVENTLISTENER

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public void OnCollisionEvent(object pSource, CollisionEventArgs pArgs)
        {
            // IF moving left:
            if ((_entity as IVelocity).Velocity.X < 0)
            {
                // IF speed DOES NOT exceed lower bound of other ICollidable's TextureSize.X - 1 as a negative:
                // PREVENTS CLIPPING
                if ((_entity as IVelocity).Velocity.X >= ((pArgs.RequiredArg as ITexture).TextureSize.X - 1) * -1)
                {
                    // MINUS 0.2 multiplied by _RequiredArg's Velocity, from _velocity:
                    _velocity.X = _velocity.X - 0.2f * (pArgs.RequiredArg as IVelocity).Velocity.Length();
                }
            }
            // IF moving right:
            else if ((_entity as IVelocity).Velocity.X > 0)
            {
                // IF speed DOES NOT exceed higher bound of other ICollidable's TextureSize.X - 1:
                // PREVENTS CLIPPING
                if ((_entity as IVelocity).Velocity.X <= (pArgs.RequiredArg as ITexture).TextureSize.X - 1)
                {
                    // ADD 0.2 multiplied by _RequiredArg's Velocity, to _velocity:
                    _velocity.X = _velocity.X + 0.2f * (pArgs.RequiredArg as IVelocity).Velocity.Length();
                }
            }

            // SET Data Property value of _sfxCommand to "PaddleHit":
            (_sfxCommand as ICommandOneParam<string>).Data = "PaddleHit";

            // SCHEDULE _sfxCommand SFX to be executed:
            (_entity as ICommandSender).ScheduleCommand(_sfxCommand);

            // MULTIPLY _currentVel.X by '-1':
            _velocity.X *= -1;

            // APPLY new Velocity to _entity.Velocity:
            (_entity as IVelocity).Velocity = _velocity;
        }

        #endregion

    }
}
