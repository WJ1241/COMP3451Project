using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for NPC entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 13/04/22
    /// </summary>
    public class NPCBehaviour : RIRRBehaviour, IInitialiseParam<IDictionary<string, EventHandler<UpdateEventArgs>>>, IEventListener<CollisionEventArgs>
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, EventHandler<UpdateEventArgs>>, name it '_animationEventDict':
        private IDictionary<string, EventHandler<UpdateEventArgs>> _animationEventDict;

        // DECLARE an ICommand, name it '_sfxCommand':
        private ICommand _sfxCommand;

        // DECLARE an int, name it '_tileSize':
        int _tileSize = 16;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of NPCBehaviour
        /// </summary>
        public NPCBehaviour()
        {
        }

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public void OnEvent(object pSource, CollisionEventArgs pArgs)
        {
            #region OBSTACLE COLLISION

            // IF pArgs.RequiredArg is on Layer 2/3/5:
            if ((pArgs.RequiredArg as ILayer).Layer == 2 || (pArgs.RequiredArg as ILayer).Layer == 3 || (pArgs.RequiredArg as ILayer).Layer == 5)
            {
                // DECLARE & INITIALISE a Vector2 with value of _direction, name it 'tempVel':
                Vector2 tempDir = _direction;

                // IF _entity has collided with the bottom of the other collidable and is moving UPWARDS:
                if ((_entity as ICollidable).HitBox.Top + (_entity as IRotation).DrawOrigin.Y <= pArgs.RequiredArg.HitBox.Bottom
                 && (_entity as ICollidable).HitBox.Bottom >= pArgs.RequiredArg.HitBox.Bottom && _direction.Y < 0)
                {

                    _entity.Position = new Vector2(_entity.Position.X, pArgs.RequiredArg.HitBox.Bottom);

                    // INVERSE tempDir.Y:
                    tempDir.Y *= -1;
                }

                // IF _entity has collided with the top of the other collidable and is moving DOWNWARDS:
                if ((_entity as ICollidable).HitBox.Bottom >= pArgs.RequiredArg.HitBox.Top
                    && (_entity as ICollidable).HitBox.Top <= pArgs.RequiredArg.HitBox.Top && _direction.Y > 0)
                {
                    // INITIALISE _entity.Position Property to be positioned 
                    _entity.Position = new Vector2(_entity.Position.X, pArgs.RequiredArg.HitBox.Top - (_entity as IRotation).DrawOrigin.Y);

                    // INVERSE tempDir.Y:
                    tempDir.Y *= -1;
                }

                // IF _entity has collided with the right of the other collidable and is moving to the LEFT:
                if ((_entity as ICollidable).HitBox.Left <= pArgs.RequiredArg.HitBox.Right
                 && (_entity as ICollidable).HitBox.Right >= pArgs.RequiredArg.HitBox.Right && _direction.X < 0)
                {

                    _entity.Position = new Vector2(pArgs.RequiredArg.HitBox.Right + (_entity as IRotation).DrawOrigin.X, _entity.Position.Y);

                    // INVERSE tempDir.X:
                    tempDir.X *= -1;
                }

                // IF _entity has collided with the left of the other collidable and is moving to the RIGHT:
                if ((_entity as ICollidable).HitBox.Right >= pArgs.RequiredArg.HitBox.Left
                 && (_entity as ICollidable).HitBox.Left <= pArgs.RequiredArg.HitBox.Left && _direction.X > 0)
                {

                    _entity.Position = new Vector2(pArgs.RequiredArg.HitBox.Left - (_entity as IRotation).DrawOrigin.X, _entity.Position.Y);

                    // INVERSE tempDir.X:
                    tempDir.X *= -1;
                }

                // INITIALISE _direction with modified tempDir value:
                _direction = tempDir;
            }

            #endregion


            #region PLAYER COLLISION

            // IF pArgs.RequiredArg implements IPlayer:
            if ((pArgs.RequiredArg is IPlayer) && pArgs.RequiredArg.HitBox.Top + (pArgs.RequiredArg as IRotation).DrawOrigin.Y < (_entity as ICollidable).HitBox.Bottom)
            {
                // INITIALISE FirstParam Property of _sfxCommand with value of "Attack":
                (_sfxCommand as ICommandOneParam<string>).FirstParam = "Attack";

                // SCHEDULER _sfxCommand to play:
                (_entity as ICommandSender).ScheduleCommand(_sfxCommand);
            }

            #endregion


            #region OLD TRACKING CODE

            // ASIGNMENT _tileSize has its value multiplied by 3, this way we have the value of 3 tiles
            // and can be used for the tile height or width:
            _tileSize = _tileSize * 3;

            //------------------------------------ Entity Collision, for colliding with objects -------------------
            // IF moving left:
            if ((_entity as IVelocity).Velocity.X < 0)
            {
                
                // IF _entity(NPC) velocity.X is greater than or equal to pArgs(Player) textureSize.X minus 1:
                if ((_entity as IVelocity).Velocity.X >= (pArgs.RequiredArg as ITexture).TextureSize.X - 1)
                {
                    // ASSIGNMENT _velocitcy.X is set to 0:
                    _velocity.X -= _velocity.X;
                }

                // MULTIPLY _currentVel.X by '-1':
                _velocity.X *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _velocity;
            }
            // IF moving right:
            else if ((_entity as IVelocity).Velocity.X > 0)
            {
                // IF _entity(NPC) velocity.X is less than or equal to pArgs(Player) textureSize.X pluss 1:
                if ((_entity as IVelocity).Velocity.X <= (pArgs.RequiredArg as ITexture).TextureSize.X + 1)
                {
                    // ASSIGNMENT _velocitcy.X is set to 0:
                    _velocity.X -= _velocity.X;
                }

                // MULTIPLY _currentVel.X by '-1':
                _velocity.X *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _velocity;
            }
            // IF moving top:
            else if ((_entity as IVelocity).Velocity.Y < 0)
            {
                // IF _entity(NPC) velocity.Y is greater than or equal to pArgs(Player) textureSize.X  minus 1:
                if ((_entity as IVelocity).Velocity.Y >= (pArgs.RequiredArg as ITexture).TextureSize.Y - 1)
                {
                    // ASSIGNMENT _velocity.Y is set to 0:
                    _velocity.Y -= _velocity.Y;
                }

                // MULTIPLY _currentVel.Y by '-1':
                _velocity.Y *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _velocity;
            }
            // IF moving bottom:
            else if ((_entity as IVelocity).Velocity.Y > 0)
            {
                if ((_entity as IVelocity).Velocity.Y <= (pArgs.RequiredArg as ITexture).TextureSize.Y + 1)
                {
                    // ASSIGNMENT _velocity.Y is set to 0:
                    _velocity.Y -= _velocity.Y;
                }

                // MULTIPLY _currentVel.Y by '-1':
                _velocity.Y *= -1;

                // APPLY new Velocity to _entity.Velocity:
                (_entity as IVelocity).Velocity = _velocity;
            }
            // SET Data Property value of _sfxCommand to "PaddleHit":
            //(_sfxCommand as ICommandOneParam<string>).Data = "PaddleHit";

            // SCHEDULE _sfxCommand SFX to be executed:
            //(_entity as ICommandSender).ScheduleCommand(_sfxCommand);



            // ---------------------- Detection and interception Collision, for detecting and intercepting the player ----------

            // ok this bit is for the detection collision, so its the box around the npc and when
            // the player enters this box the npc becomes 'aware' and starts hunting the player down... theoretically

            // DECLARE local Vector2, name it 'tempPosition' and ASSIGN it the value of _entity.Position:
            Vector2 tempPosition = _entity.Position;

            // IF moving left:
            if ((_entity as IVelocity).Velocity.X < 0)
            {
                // IF NPC velocity.X - 3 tiles is greater than or equel to Player texture size X minus 1 pxl 
                if (((_entity as IVelocity).Velocity.X - _tileSize) >= (pArgs.RequiredArg as ITexture).TextureSize.X - 1)
                {
                    // this is how to get the position of the player
                    //(pArgs.RequiredArg as IEntity).Position 

                    // ASSIGNMENT tempPosition.X has the value of the entities x velocity added to it, this is to make it move towards the player:
                    tempPosition.X += (_entity as IVelocity).Velocity.X;

                    //     player cast as IEntity                      NPC 
                    // ((pArgs.RequiredArg as IEntity).Position.X >= _entity.Position.X)
                   

                    // ASSIGNMENT _entity.Position is set to the value of tempPosition
                    _entity.Position = tempPosition;

                }
            }
            // IF moving right:
            else if ((_entity as IVelocity).Velocity.X > 0)
            {
                // IF NPC velocity.X + 3 tiles is greater than or equel to Player texture size X minus 1 pxl 
                if (((_entity as IVelocity).Velocity.X + _tileSize) <= (pArgs.RequiredArg as ITexture).TextureSize.X + 1)
                {
                    // ASSIGNMENT tempPosition.X has the value of the entities x velocity subtracted from it:
                    tempPosition.X -= (_entity as IVelocity).Velocity.X;

                    // ASSIGNMENT _entity.Position is set to the value of tempPosition
                    _entity.Position = tempPosition;
                }
            }
            // IF moving up:
            if ((_entity as IVelocity).Velocity.Y < 0)
            {
                // IF NPC velocity - 3 tiles is greater than or equel to Player texture size X minus 1 pxl 

                if (((_entity as IVelocity).Velocity.Y + _tileSize) >= (pArgs.RequiredArg as ITexture).TextureSize.Y - 1)
                {
                    // ASSIGNMENT tempPosition.Y has the value of the entities Y velocity added to it:
                    tempPosition.Y += (_entity as IVelocity).Velocity.Y;

                    // ASSIGNMENT _entity.Position is set to the value of tempPosition
                    _entity.Position = tempPosition;
                }
            }
            // IF moving down:
            else if ((_entity as IVelocity).Velocity.Y > 0)
            {
                if (((_entity as IVelocity).Velocity.Y - _tileSize) <= (pArgs.RequiredArg as ITexture).TextureSize.Y + 1)
                {
                    // ASSIGNMENT tempPosition.Y has the value of the entities Y velocity subtracted from it:
                    tempPosition.Y -= (_entity as IVelocity).Velocity.Y;


                    // ASSIGNMENT _entity.Position is set to the value of tempPosition
                    _entity.Position = tempPosition;
                }
            }

            #endregion
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IDICTIONARY<STRING, EVENTHANDLER<UPDATEEVENTARGS>>>

        /// <summary>
        /// Initialises an object with a reference to an IDictionary<string, EventHandler<UpdateEventArgs> instance
        /// </summary>
        /// <param name="pUpdateEventDict"> IDictionary<string, EventHandler<UpdateEventArgs> instance </param>
        public void Initialise(IDictionary<string, EventHandler<UpdateEventArgs>> pUpdateEventDict)
        {
            // IF pUpdateEventDict DOES HAVE an active instance:
            if (pUpdateEventDict != null)
            {
                // INITIALISE _animationEventDict with reference to pUpdateEventDict:
                _animationEventDict = pUpdateEventDict;
            }
            // IF pUpdateEventDict DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message():
                throw new NullInstanceException("ERROR: pUpdateEventDict does not have an active instance!");
            }
        }

        #endregion
    }
}
