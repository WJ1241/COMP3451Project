using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for NPC entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 11/04/22
    /// </summary>
    public class NPCBehaviour : RIRRBehaviour, IEventListener<CollisionEventArgs>
    {
        #region FIELD VARIABLES

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
            // EMPTY CONSTRUCTOR
        }

        #endregion

        /*

        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        { 
            
            //// ASSIGN value of _entity's Velocity to _currentVel:
            //_velocity = (_entity as IVelocity).Velocity;

            //// IF at top screen edge or bottom screen edge:
            //if (_entity.Position.Y <= (_entity.Position.Y + 48) || _entity.Position.Y >= (_entity as IContainBoundary).WindowBorder.Y - (_entity as ITexture).TextureSize.Y)
            //{
            //    // MULTIPLY _currentVel.Y by '-1':
            //    _velocity.Y *= -1;

            //    // APPLY new Velocity to _entity.Velocity:
            //    (_entity as IVelocity).Velocity = _velocity;

            //    // SET Data Property value of _sfxCommand to "WallHit":
            //    //(_sfxCommand as ICommandOneParam<string>).Data = "WallHit";

            //    // SCHEDULE _sfxCommand to be executed:
            //    (_entity as ICommandSender).ScheduleCommand(_sfxCommand);
            //}
            //// IF at left screen edge or right screen edge:
            //else if (_entity.Position.X <= 0 || _entity.Position.X >= (_entity as IContainBoundary).WindowBorder.X - (_entity as ITexture).TextureSize.X)
            //{
            //    // IF at left screen edge:
            //    if (_entity.Position.X <= 0)
            //    {
            //        // SET Data Property value of _scoreGoal to '2':
            //        //(_scoreGoal as ICommandOneParam<int>).Data = 2;
            //    }
            //    // IF at right screen edge:
            //    else if (_entity.Position.X >= (_entity as IContainBoundary).WindowBorder.X - (_entity as ITexture).TextureSize.X)
            //    {
            //        // SET Data Property value of _scoreGoal to '1':
            //        //(_scoreGoal as ICommandOneParam<int>).Data = 1;
            //    }

            //    // SCHEDULE _scoreGoal to be executed:
            //    //(_entity as ICommandSender).ScheduleCommand(_scoreGoal);

            //    // SET Data Property value of _sfxCommand to "Score":
            //    //(_sfxCommand as ICommandOneParam<string>).Data = "Score";

            //    // SCHEDULE _sfxCommand to be executed:
            //    //(_entity as ICommandSender).ScheduleCommand(_sfxCommand);

            //    // SCHEDULE RemoveMe to be executed:
            //    (_entity as ICommandSender).ScheduleCommand((_entity as IEntityInternal).RemoveMe);

            //    // SCHEDULE TerminateMe to be executed:
            //    (_entity as ICommandSender).ScheduleCommand((_entity as IEntityInternal).TerminateMe);
            //}
        }

        #endregion
        
        */

        #region IMPLEMENTATION OF IEVENTLISTENER<COLLISIONEVENTARGS>

        /// <summary>
        /// Method which is called after an object that requires output after colliding with another object
        /// </summary>
        /// <param name="pSource"> Object that requires output from colliding with another object </param>
        /// <param name="pArgs"> CollisionEventArgs object </param>
        public void OnEvent(object pSource, CollisionEventArgs pArgs)
        {


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
        }

        #endregion
    }
}
