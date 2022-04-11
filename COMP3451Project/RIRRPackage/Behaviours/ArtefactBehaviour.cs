using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CollisionManagement.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.RIRRPackage.Behaviours
{
    public class ArtefactBehaviour : PongBehaviour, IEventListener<CollisionEventArgs>
    {

        public ArtefactBehaviour()
        {
            //empty
        }




        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
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

            //------------------------------------ Entity Collision, for colliding with player -------------------

                //IF Player Top is greater than Bottom of Artefact:
            if (pArgs.RequiredArg.HitBox.Top > (_entity as ICollidable).HitBox.Bottom &&
                // AND Player Bottom is greater than Artefact Bottom:
                pArgs.RequiredArg.HitBox.Bottom < (_entity as ICollidable).HitBox.Bottom &&
                // AND Player Left is less than the artefact Right:
                pArgs.RequiredArg.HitBox.Left < ((_entity as ICollidable).HitBox.Right ) &&
                // AND Player Right is greater than Artefact Right:
                pArgs.RequiredArg.HitBox.Right > (_entity as ICollidable).HitBox.Left)
            {
                // CALL DeSpawn method
                DeSpawn();
            }


                // IF player Bottom is Less than Artefact Top:
            if (pArgs.RequiredArg.HitBox.Bottom < (_entity as ICollidable).HitBox.Top &&
                // AND Player Top is greater than Artefact Top:
                pArgs.RequiredArg.HitBox.Top > (_entity as ICollidable).HitBox.Top &&
                // AND Player Left is less than the artefact Right:
                pArgs.RequiredArg.HitBox.Left < ((_entity as ICollidable).HitBox.Right) &&
                // AND Player Right is greater than Artefact Left:
                pArgs.RequiredArg.HitBox.Right > (_entity as ICollidable).HitBox.Left) 
            {
                // CALL DeSpawn method
                DeSpawn();
            }

                // IF Player Right is greater than Artefact Left:
            if (pArgs.RequiredArg.HitBox.Right > (_entity as ICollidable).HitBox.Left &&
                // AND Player Left is les than Artefact Left:
                pArgs.RequiredArg.HitBox.Left < (_entity as ICollidable).HitBox.Left &&
                // AND Player Top is greater than Artefact Bottom:
                pArgs.RequiredArg.HitBox.Top > (_entity as ICollidable).HitBox.Bottom &&
                // AND Player Bottom is less than Artefact Top:
                pArgs.RequiredArg.HitBox.Bottom < (_entity as ICollidable).HitBox.Top)
            {
                // CALL DeSpawn method
                DeSpawn();
            }

                // IF Player Left is less than Artefact right:
            if (pArgs.RequiredArg.HitBox.Left < (_entity as ICollidable).HitBox.Right &&
                // AND Player Right is greater than Artefact Right:
                pArgs.RequiredArg.HitBox.Right > (_entity as ICollidable).HitBox.Right &&
                // AND Player Top is greater than Artefact Bottom:
                pArgs.RequiredArg.HitBox.Top > (_entity as ICollidable).HitBox.Bottom &&
                // AND Player Bottom is less than Artefact Top:
                pArgs.RequiredArg.HitBox.Bottom < (_entity as ICollidable).HitBox.Top)
            {
                // CALL DeSpawn method
                DeSpawn();
            }

        }

        #endregion

        #region PRIVATE METHODS

        /// <summary>
        /// METHOD PickUp, called when player has colided with the Artefact
        /// </summary>
        private void DeSpawn()
        {
            // CALL Terminate method in Artefact
            (_entity as ITerminate).Terminate();
        }


        #endregion

    }
}
