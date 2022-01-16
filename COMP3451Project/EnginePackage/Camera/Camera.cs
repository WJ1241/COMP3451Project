using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;


namespace COMP3451Project.EnginePackage.Camera
{
    /// <summary>
    /// Class which creates a camera to be used by a user based entity
    /// </summary>
    /// <REFERENCE> axlemke (2014) XNA 2D Camera, zoom into player. Available at: https://gamedev.stackexchange.com/questions/68978/xna-2d-camera-zoom-into-player. (Accessed: 20 April 2021). </REFERENCE>
    public class Camera : IEntity, ICamera, ISetBoundary, IZoom
    {
        #region FIELD VARIABLES

        // DECLARE a Matrix, name it '_camTransform':
        private Matrix _camTransform;

        // DECLARE a Vector2, name it '_camPos':
        private Vector2 _position;

        // DECLARE an int, name it '_uID':
        private int _uID;

        // DECLARE a string, name it '_uName':
        private string _uName;

        // DECLARE a Vector2, name it '_windowSize':
        private Vector2 _windowSize;

        // DECLARE a float, name it '_zoom':
        private float _zoom;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of Camera
        /// </summary>
        public Camera()
        {
        }

        #endregion


        #region IMPLEMENTATION OF IENTITY

        /// <summary>
        /// Initialises entity variable values
        /// </summary>
        public void Initialise()
        {
            // ASSIGNMENT, set value of _position to '0' on both X and Y axis:
            _position = Vector2.Zero;
        }

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


        #region IMPLEMENTATION OF ICAMERA

        /// <summary>
        /// Changes position of camera object when used as a parameter in a Draw Method
        /// </summary>
        /// <returns>A Matrix object to be used within Draw Methods</returns>
        /// <CITATION> (axlemke, 2014) </CITATION>
        public Matrix ChngCamPos()
        {
                                                                                                                                                                                                               
            // ASSIGNMENT, set value of _camTransform to zoom in using _zoom value, and position in centre of screen. Scale is changed first before Translation, does not work other way around, matrix rule ISROT applies:
            _camTransform = // SET scale using zoom, on X,Y axes:
                            Matrix.CreateScale(_zoom, _zoom, 1f)

                            // SET value of _camTransform to a new Vector3, passing parameters of the camera user's X,Y position, excluding Z as game operates in 2D:
                          * Matrix.CreateTranslation(new Vector3(-_position.X + (_windowSize.X / 2), -_position.Y + (_windowSize.Y / 2), 0));

            // RETURN value of _camTransform:
            return _camTransform;
        }

        #endregion


        #region IMPLEMENTATION OF ISETBOUNDARY

        /// <summary>
        /// Property which can set value of screen window borders
        /// </summary>
        public Vector2 WindowBorder
        {
            set
            {
                // SET value of _windowSize to incoming value:
                _windowSize = value;
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
