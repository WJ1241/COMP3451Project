using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.Camera.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement.Interfaces;

namespace OrbitalEngine.Camera
{
    /// <summary>
    /// Class which creates a camera to follow a user based entity
    /// Authors: William Smith, Declan Kerby-Collins & 'axlemke'
    /// Date: 07/04/22
    /// </summary>
    /// <REFERENCE> axlemke (2014) XNA 2D Camera, zoom into player. Available at: https://gamedev.stackexchange.com/questions/68978/xna-2d-camera-zoom-into-player. (Accessed: 20 April 2021). </REFERENCE>
    public class Camera : ICamera, IContainBoundary, IEntity, IZoom
    {
        #region FIELD VARIABLES

        // DECLARE a Matrix, name it '_camTransform':
        private Matrix _camTransform;

        // DECLARE a Vector2, name it '_camPos':
        private Vector2 _position;

        // DECLARE a Point, name it '_windowSize':
        private Point _windowSize;

        // DECLARE a string, name it '_uName':
        private string _uName;

        // DECLARE a float, name it '_zoom':
        private float _zoom;

        // DECLARE an int, name it '_uID':
        private int _uID;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Camera
        /// </summary>
        public Camera()
        {
            // ASSIGNMENT, set value of _position to '0' on both X and Y axis:
            _position = Vector2.Zero;
        }

        #endregion


        #region IMPLEMENTATION OF ICAMERA

        /// <summary>
        /// Changes position of camera object when used as a parameter in a Draw Method
        /// </summary>
        /// <returns> A Matrix object to be used within Draw Methods </returns>
        /// <CITATION> (axlemke, 2014) </CITATION>
        public Matrix DrawCam()
        {
            // ASSIGNMENT, set value of _camTransform to zoom in using _zoom value, and position in centre of screen. Scale is changed first before Translation, does not work other way around, matrix rule ISROT applies:
            _camTransform = // SET scale using zoom, on X,Y axes:
                            Matrix.CreateScale(_zoom, _zoom, 1f)

                          // SET value of _camTransform to a new Vector3, passing parameters of the camera user's X,Y position, excluding Z as game operates in 2D:
                          * Matrix.CreateTranslation(new Vector3(-_position.X + (_windowSize.X / 2), -_position.Y + (_windowSize.Y / 2), 0));

            // RETURN value of _camTransform:
            return _camTransform;
        }

        /// <summary>
        /// Changes Positional values so it is updated to stay with caller source's position
        /// </summary>
        /// <param name="pSource"> Object that is changing Position </param>
        /// <param name="pArgs"> EventArgs for a Positioned object </param>
        public void ChangeCamPos(object pSource, PositionEventArgs pArgs)
        {
            // IF pArgs DOES HAVE an active instance:
            if (pArgs != null)
            {
                // SET value of _position to value of pArgs.RequiredArg:
                _position = pArgs.RequiredArg;
            }
            // IF pArgs DOES NOT HAVE an active instance:
            else
            {
                // WRITE to console, explaining that pArgs cannot be used:
                Console.WriteLine("ERROR: pArgs does not have an active instance, therefore Camera position cannot be updated!");
            }
        }

        #endregion


        #region IMPLEMENTATION OF ICONTAINBOUNDARY

        /// <summary>
        /// Property which can set value of screen window borders
        /// </summary>
        public Point WindowBorder
        {
            get
            {
                // RETURN value of _windowSize:
                return _windowSize;
            }
            set
            {
                // SET value of _windowSize to incoming value:
                _windowSize = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Property which can get and set value of an entity's position
        /// </summary>
        public Vector2 Position
        {
            get
            {
                // RETURN value of current location(x,y):
                return _position;
            }
            set
            {
                // ASSIGNMENT give location(x,y) value of external class modified value:
                _position = value;
            }
        }

        /// <summary>
        /// Property which can get and set value of an entity's unique ID
        /// </summary>
        public int UID
        {
            get
            {
                // RETURN value of current _uID:
                return _uID;
            }
            set
            {
                // ASSIGNMENT give _uID value of external class modified value:
                _uID = value;
            }
        }

        /// <summary>
        /// Property which can get and set value of an entity's unique Name
        /// </summary>
        public string UName
        {
            get
            {
                // RETURN value of current _uName:
                return _uName;
            }
            set
            {
                // ASSIGNMENT give _uName value of external class modified value:
                _uName = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IZOOM

        /// <summary>
        /// Property which can set view zoom value
        /// </summary>
        public float Zoom
        {
            set
            {
                // SET value of _zoom to incoming value:
                _zoom = value;
            }
        }

        #endregion
    }
}
