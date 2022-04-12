using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement;
using COMP3451Project.RIRRPackage.Entities.Interfaces;

namespace COMP3451Project.RIRRPackage.Entities
{
    /// <summary>
    /// Class which adds a HealthBar entity on screen
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    /// <REFERENCE> Lewin, N. (2012). XNA Tutorial 37 - Advanced Health Bar. Available at: https://youtu.be/03Hq0qbAy8s?t=105. (Accessed: 24 April 2021). </REFERENCE>
    /// <REFERENCE> Smith, W. (2021) 'Post-Production Milestone'. Assignment for COMP2451 Game Design & Development, Computing BSc (Hons), University of Worcester. Unpublished. </REFERENCE>
    public class HPBar : DrawableRectangleEntity, IChangePosition, IChangeTexColour, IHaveHealth, IHPBar
    {
        #region FIELD VARIABLES

        // DECLARE a Color, name it '_texColour':
        private Color _texColour;

        // DECLARE a int, name it '_texPartition':
        private int _texPartition;

        // DECLARE an int, name it '_maxHP':
        private int _maxHP;

        // DECLARE an int, name it '_hP':
        private int _hP;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of HPBar
        /// </summary>
        public HPBar()
        {
            // INITIALISE _drawColour to be Red:
            _texColour = new Color(220, 0, 0);
        }

        #endregion


        #region IMPLEMENTATION OF ICHANGEPOSITION

        /// <summary>
        /// Changes positional values of an implementation, used with commands
        /// </summary>
        /// <param name="pPosition"> XY Positional Values </param>
        public void ChangePosition(Vector2 pPosition)
        {
            // ASSIGNMENT, change position to have an offset from given position:
            _position = new Vector2(pPosition.X + 9, pPosition.Y + 3);
        }

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows read and write access to a texture colour
        /// </summary>
        public Color TexColour
        {
            get
            {
                // RETURN value of _texColour:
                return _texColour;
            }
            set
            {
                // SET value of _texColour to incoming value:
                _texColour = value;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public override void Draw(SpriteBatch pSpriteBatch)
        {
            // DRAW given texture, given position, source rectangle (where to draw) and colour
            pSpriteBatch.Draw(_texture, _position, _sourceRect, _texColour);
        }

        #endregion


        #region IMPLEMENTATION OF IHAVEHEALTH

        /// <summary>
        /// Property which has read and write access to an implementation's health points
        /// </summary>
        /// <CITATION> (Smith, 2021) </CITATION>
        public int HealthPoints
        { 
            get 
            {
                // RETURN value of _hP:
                return _hP;
            }
            set
            {
                // SET value of _hP to incoming value:
                _hP = value;
            }
        }

        /// <summary>
        /// Property which has read and write access to an implementation's maximum health points
        /// </summary>
        public int MaxHealthPoints
        {
            get
            {
                // RETURN value of _maxHP:
                return _maxHP;
            }
            set
            {
                // SET value of _maxHP to incoming value:
                _maxHP = value;

                // SET value of _hP to same value as _maxHP:
                _hP = _maxHP;
            }
        }

        #endregion


        #region IMPLEMENTATION OF IHPBAR

        /// <summary>
        /// Changes Health Points to display to user
        /// </summary>
        /// <param name="pHealth"> Value of Health Points to display </param>
        public void ChangeHealth(int pHealth)
        {
            // SET value of _hP to value of pHealth:
            _hP = pHealth;

            // SET Width of _sourceRect to be _texPartition multiplied by the value of _hP:
            _sourceRect.Width = _texPartition * _hP;
        }

        #endregion


        #region IMPLEMENTATION OF ITEXTURE

        /// <summary>
        /// Property which allows access to get or set value of 'texture'
        /// </summary>
        /// <CITATION> (Lewin, 2012) </CITATION>
        public override Texture2D Texture
        {
            get
            {
                // RETURN value of _texture:
                return _texture;
            }
            set
            {
                // SET value of _texture to incoming value:
                _texture = value;

                // INSTANTIATE _textureSize as a new Point(), using _texture's dimensions:
                _textureSize = new Point(_texture.Width, _texture.Height);

                // INITIALISE _texPartition with the value of _textureSize.X divided by _maxHP:
                _texPartition = _textureSize.X / _maxHP;

                // INSTANTIATE _sourceRect as a new Rectangle(), using 0,0 for coordinates of a texture, and passing textureSize's dimensions as parameters:
                _sourceRect = new Rectangle(0, 0, _textureSize.X, _textureSize.Y);
            }
        }

        #endregion
    }
}
