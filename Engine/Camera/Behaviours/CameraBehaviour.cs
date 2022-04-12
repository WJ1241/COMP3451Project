using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Commands.Interfaces;

namespace OrbitalEngine.Camera.Behaviours
{
    /// <summary>
    /// Class which defines the behaviour for a Camera
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 11/04/22
    /// </summary>
    public class CameraBehaviour : UpdatableBehaviour, IInitialiseParam<EventHandler<MatrixEventArgs>>, IInitialiseParam<ICommand>, IInitialiseParam<MatrixEventArgs>
    {
        #region FIELD VARIABLES

        // DECLARE an ICommand, name it '_updateFollowEntityCommand':
        private ICommand _updateFollowEntityCommand;

        // DECLARE an EventHandler<MatrixEventArgs>, name it '_camPosChangeEvent':
        private EventHandler<MatrixEventArgs> _camPosChangeEvent;

        // DECLARE a MatrixEventArgs, name it '_matrixArgs':
        private MatrixEventArgs _matrixArgs;

        // DECLARE a Matrix, name it '_camTransform':
        private Matrix _camTransform;

        #endregion


        #region IMPLEMENTATION OF IEVENTLISTENER<UPDATEEVENTARGS>

        /// <summary>
        /// Method called when needing to update Behaviour
        /// </summary>
        /// <param name="pSource"> Object that is to be updated </param>
        /// <param name="pArgs"> EventArgs for an Update object </param>
        public override void OnEvent(object pSource, UpdateEventArgs pArgs)
        {
            // ASSIGNMENT, set value of _camTransform to zoom in using _zoom value, and position in centre of screen. Scale is changed first before Translation, does not work other way around, matrix rule ISROT applies:
            _camTransform =     // SET scale using zoom, on X,Y axes:
                                Matrix.CreateScale((_entity as IZoom).Zoom, (_entity as IZoom).Zoom, 1f)

                                // SET value of _camTransform to a new Vector3, passing parameters of the camera user's X,Y position, excluding Z as game operates in 2D:
                              * Matrix.CreateTranslation(new Vector3(-_entity.Position.X + (_entity as IContainBoundary).WindowBorder.X / 2, -_entity.Position.Y + (_entity as IContainBoundary).WindowBorder.Y / 2, 0));

            // INITIALISE _matrixArgs with value of _camTransform:
            _matrixArgs.RequiredArg = _camTransform;

            // INVOKE _camPosChangeEvent(), passing this class and _matrixArgs as parameters:
            _camPosChangeEvent.Invoke(this, _matrixArgs);

            // IF _updateFollowEntityCommand HAS NOT been initialised:
            if (_updateFollowEntityCommand != null)
            {
                // INITIALISE FirstParam Property of _updateFollowEntityCommand with value of _entity.Position:
                (_updateFollowEntityCommand as ICommandOneParam<Vector2>).FirstParam = _entity.Position;

                // SCHEDULE _updateFollowEntityCommand to be executed:
                (_entity as ICommandSender).ScheduleCommand(_updateFollowEntityCommand);
            }
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
                // INITIALISE _sfxCommand with reference to pCommand:
                _updateFollowEntityCommand = pCommand;
            }
            // IF pCommand DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pCommand does not have an active instance!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<EVENTHANDLER<MATRIXEVENTARGS>>

        /// <summary>
        /// Initialises an object with an EventHandler<MatrixEventArgs> reference
        /// </summary>
        /// <param name="pEventHandler"> EventHandler<MatrixEventArgs> reference </param>
        public void Initialise(EventHandler<MatrixEventArgs> pEventHandler)
        {
            // IF pEventHandler DOES HAVE a valid method reference:
            if (pEventHandler != null)
            {
                // INITIALISE _camPosChangeEvent with reference to pEventHandler:
                _camPosChangeEvent = pEventHandler;
            }
            // IF pEventHandler DOES NOT HAVE a valid method reference:
            else
            {
                // THROW a new NullReferenceException(), with corresponding message:
                throw new NullReferenceException("ERROR: pEventHandler does not have a valid method reference!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<MATRIXEVENTARGS>

        /// <summary>
        /// Initialises an object with a MatrixEventArgs instance
        /// </summary>
        /// <param name="pArgs"> MatrixEventArgs instance </param>
        public void Initialise(MatrixEventArgs pArgs)
        {
            // IF pArgs DOES HAVE an active instance:
            if (pArgs != null)
            {
                // INITIALISE _matrixArgs with reference to pArgs:
                _matrixArgs = pArgs;
            }
            // IF pArgs DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullReferenceException(), with corresponding message:
                throw new NullReferenceException("ERROR: pArgs does not have a valid method reference!");
            }
        }

        #endregion
    }
}
