using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Behaviours;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.CustomEventArgs;
using COMP3451Project.EnginePackage.EntityManagement;
using COMP3451Project.EnginePackage.Services.Commands;
using COMP3451Project.EnginePackage.Services.Commands.Interfaces;

namespace COMP3451Project.PongPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for Ball entities
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 21/01/22
    /// </summary>
    public class BallBehaviour : PongBehaviour, ICollisionEventListener
    {
        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {
            // ASSIGN value of _entity's Velocity to _currentVel:
            _velocity = (_entity as IVelocity).Velocity;

            // IF at top screen edge or bottom screen edge:
            if (_entity.Position.Y <= 0 || _entity.Position.Y >= (_entity as IContainBoundary).WindowBorder.Y - (_entity as ITexture).TextureSize.Y) 
            {
                // MULTIPLY _currentVel.Y by '-1':
                _velocity.Y *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _velocity;
            }
            // IF at left screen edge or right screen edge:
            else if (_entity.Position.X <= 0 || _entity.Position.X >= (_entity as IContainBoundary).WindowBorder.X - (_entity as ITexture).TextureSize.X)
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
            // IF moving left:
            if ((_entity as IVelocity).Velocity.X < 0) 
            {
                // MINUS 0.2 multiplied by _RequiredArg's Velocity, from _velocity:
                _velocity.X = _velocity.X - 0.2f * (pArgs.RequiredArg as IVelocity).Velocity.Length();
            }
            // IF moving right:
            else if ((_entity as IVelocity).Velocity.X > 0)  
            {
                // ADD 0.2 multiplied by _RequiredArg's Velocity, to _velocity:
                _velocity.X = _velocity.X + 0.2f * (pArgs.RequiredArg as IVelocity).Velocity.Length();
            }

            // MULTIPLY _currentVel.X by '-1':
            _velocity.X *= -1;

            // APPLY new Velocity to _entity.Velocity:
            (_entity as IVelocity).Velocity = _velocity;
        }

        #endregion
    }
}
