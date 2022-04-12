using Microsoft.Xna.Framework;
using OrbitalEngine.Behaviours.Interfaces;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.CustomEventArgs;
using OrbitalEngine.EntityManagement;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.Exceptions;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds a HealthBar entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    /// <REFERENCE> Smith, W. (2021) 'Post-Production Milestone'. Assignment for COMP2451 Game Design & Development, Computing BSc (Hons), University of Worcester. Unpublished. </REFERENCE>
    public class HPBarShroud : DrawableEntity, IChangePosition, IInitialiseParam<IEventListener<UpdateEventArgs>>, IUpdatable, IZoom
    {
        #region FIELD VARIABLES

        // DECLARE an float, name it '_zoom':
        private float _zoom;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of HPBarShroud
        /// </summary>
        public HPBarShroud()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ICHANGEPOSITION

        /// <summary>
        /// Changes positional values of an implementation, used with commands
        /// </summary>
        /// <param name="pPosition"> XY Positional Values </param>
        public void ChangePosition(Vector2 pPosition)
        {
            // INITIALISE _position
            // Requires _zoom * 2 as dividing by zoom finds centre of screen
            _position = (pPosition / _zoom) - (_windowBorder / (_zoom * 2));
        }

        #endregion


        #region IMPLEMENTATION OF IINITIALISEPARAM<IEVENTLISTENER<UPDATEVENTARGS>>

        /// <summary>
        /// Initialises an object with an IEventListener<UpdateEventArgs> object
        /// </summary>
        /// <param name="pUpdateEventListener"> IEventListener<UpdateEventArgs> object </param>
        public virtual void Initialise(IEventListener<UpdateEventArgs> pUpdateEventListener)
        {
            // IF pUpdateEventListener DOES HAVE an active instance:
            if (pUpdateEventListener != null)
            {
                // INITIALISE pUpdateEventListener with this class:
                (pUpdateEventListener as IInitialiseParam<IEntity>).Initialise(this);
            }
            // IF pUpdateEventListener DOES NOT HAVE an active instance:
            else
            {
                // THROW a new NullInstanceException(), with corresponding message:
                throw new NullInstanceException("ERROR: pUpdateEventListener does not have an active instance");
            }
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> Holds reference to GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // CALL Update() on _currentState, passing pGameTime as a parameter:
            (_currentState as IUpdatable).Update(pGameTime);
        }

        #endregion


        #region IMPLEMENTATION OF IZOOM

        /// <summary>
        /// Property which can set view zoom value
        /// </summary>
        public float Zoom
        {
            get
            {
                // RETURN value of _zoom:
                return _zoom;
            }
            set
            {
                // SET value of _zoom to incoming value:
                _zoom = value;
            }
        }

        #endregion
    }
}
