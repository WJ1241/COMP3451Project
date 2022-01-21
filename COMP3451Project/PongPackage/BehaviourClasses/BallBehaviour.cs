using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services.Commands;

namespace COMP3451Project.PongPackage.BehaviourClasses
{
    /// <summary>
    /// Class which defines the behaviour for Ball entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
    /// </summary>
    public class BallBehaviour : PongBehaviour, ICollisionEventListener
    {
        #region FIELD VARIABLES

        // DECLARE a Vector2, name it '_currentVel':
        private Vector2 _currentVel;

        #endregion


        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {
            // ASSIGN value of _entity's Velocity to _currentVel:
            _currentVel = (_entity as IVelocity).Velocity;

            if (_entity.Position.Y <= 0 || _entity.Position.Y >= (_entity as IContainBoundary).WindowBorder.Y - (_entity as ITexture).Texture.Height) // IF at top screen edge or bottom screen edge
            {
                // MULTIPLY _currentVel.Y by '-1':
                _currentVel.Y *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _currentVel;
            }
            else if (_entity.Position.X <= 0 || _entity.Position.X >= ((_entity as IContainBoundary).WindowBorder.X - (_entity as ITexture).Texture.Width)) // IF at left screen edge or right screen edge
            {
                // CALL ScheduleCommand Property, passing RemoveMe Property as a parameter:
                (_entity as ICommandSender).ScheduleCommand((_entity as IEntityInternal).RemoveMe);

                // CALL ScheduleCommand Property, passing TerminateMe Property as a parameter:
                (_entity as ICommandSender).ScheduleCommand((_entity as IEntityInternal).TerminateMe);
            }
        }

        #endregion


        #region IMPLEMENTATION OF ICOLLISIONEVENTLISTENER

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public void OnCollisionEvent(object pSource, CollisionEventArgs pArgs)
        {
            if ((_entity as IVelocity).Velocity.X < 0) // IF moving left
            {
                // MINUS 0.2 multiplied by _RequiredArg's Velocity, from _entity.Velocity:
                (_entity as IVelocity).Velocity = new Vector2((_entity as IVelocity).Velocity.X - 0.2f * (pArgs.RequiredArg as IVelocity).Velocity.Length(), (_entity as IVelocity).Velocity.Y);
            }
            else if ((_entity as IVelocity).Velocity.X > 0)  // IF moving right
            {
                // ADD 0.2 multiplied by _RequiredArg's Velocity, to _entity.Velocity:
                (_entity as IVelocity).Velocity = new Vector2((_entity as IVelocity).Velocity.X + 0.2f * (pArgs.RequiredArg as IVelocity).Velocity.Length(), (_entity as IVelocity).Velocity.Y);
            }

            // MULTIPLY _currentVel.X by '-1':
            _currentVel.X *= -1;

            // APPLY new Velocity to _entity.Velocity:
            (_entity as IVelocity).Velocity = _currentVel;
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEIENTITY

        /// <summary>
        /// Initialises an object with an IEntity object
        /// </summary>
        /// <param name="pEntity"> IEntity object </param>
        public override void Initialise(IEntity pEntity)
        {
            // INITIALISE _entity with reference to instance of pEntity:
            _entity = pEntity;

            // ASSIGN value of _entity's Velocity to _currentVel:
            _currentVel = (_entity as IVelocity).Velocity;
        }

        #endregion
    }
}
