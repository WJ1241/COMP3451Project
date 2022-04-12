using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.EntityManagement.Interfaces;
using OrbitalEngine.SceneManagement.Interfaces;
using System.Collections.Generic;

namespace OrbitalEngine.SceneManagement
{
    /// <summary>
    /// Class which is used to hold all entities relative to a Cutscene situation, and to update and draw them depending on their case
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 11/04/22
    /// </summary>
    public class CutsceneGraph : ISceneGraph, IDraw//, IInitialiseParam<IDictionary<string, IDictionary<int, Texture2D>>>, IInitialiseParam<SpriteFont>, IInitialiseParam<IDictionary<int, string>>, IName, ISpawn, IUpdatable
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary<string, IDictionary<int, Texture2D>>, name it '_entityTextureDict':
        private IDictionary<string, IDictionary<int, Texture2D>> _entityTextureDict;

        // DECLARE an IDictionary<string, IEntity>, name it '_cutsceneEntDict':
        private IDictionary<string, IEntity> _cutsceneEntDict;

        // DECLARE an IDictionary<int, string>, name it '_quoteDict':
        private IDictionary<int, string> _quoteDict;

        // DECLARE a SpriteFont, name it '_font':
        private SpriteFont _font;

        // DECLARE a Vector2, name it '_textPos':
        private Vector2 _textPos;

        // DECLARE an int, name it '_cutsceneCase':
        private int _cutsceneCase;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of CutsceneGraph
        /// </summary>
        public CutsceneGraph()
        {
            // EMPTY CONSTRUCTOR
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEGRAPH

        /// <summary>
        /// Removes instance of object from list/dictionary using an entity's unique name
        /// </summary>
        /// <param name="pUName"> Used for passing unique name </param>
        public void RemoveEntity(string pUName)
        {
            // REMOVE IEntity object addressed by pUName from _cutsceneEntDict:
            _cutsceneEntDict.Remove(pUName);
        }

        /// <summary>
        /// Removes and terminates any references to entities from the scene
        /// </summary>
        public void ClearScene()
        {
            // FOREACH IEntity in _cutsceneEntDict.Values:
            foreach (IEntity pEntity in _cutsceneEntDict.Values)
            {
                // CALL Terminate() on pEntity:
                (pEntity as ITerminate).Terminate();
            }
        }

        /// <summary>
        /// Returns Scene Dictionary, used for testing
        /// </summary>
        /// <returns> IDictionary<string, IEntity> object </returns>
        public IDictionary<string, IEntity> ReturnSceneDict()
        {
            // RETURN instance of _cutsceneEntDict:
            return _cutsceneEntDict;
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="pSpriteBatch"> Needed to draw entity's texture on screen </param>
        public void Draw(SpriteBatch pSpriteBatch)
        {
            // FOREACH IEntity in _cutsceneEntDict.Values:
            foreach (IEntity pEntity in _cutsceneEntDict.Values)
            {
                // IF pEntity implements IDraw:
                if (pEntity is IDraw)
                {
                    // CALL Draw() on pEntity, passing pSpriteBatch as a parameter:
                    (pEntity as IDraw).Draw(pSpriteBatch);
                }
            }

            // DRAW string value of _quoteDict[_cuteceneCase] at _textPos in _font coloured White:
            pSpriteBatch.DrawString(_font, _quoteDict[_cutsceneCase], _textPos, Color.White);
        }

        #endregion


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="pGameTime"> GameTime object </param>
        public void Update(GameTime pGameTime)
        {
            // WHILE _cutsceneCase HAS NOT exceeded the number of strings in _quoteDict:
            // USING QUOTES DUE TO EACH CASE INCLUDING A QUOTE
            while (_cutsceneCase <= _quoteDict.Values.Count)
            {
                // IF Enter key is pressed:
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    // INCREMENT _cutsceneCase:
                    _cutsceneCase++;
                }

                // FOREACH IEntity in _cutsceneEntDict:
                foreach (IEntity pEntity in _cutsceneEntDict.Values)
                {
                    // IF pEntity implements ITexture:
                    if (pEntity is ITexture)
                    {
                        // IF pEntity.Texture Property HAS NOT been updated to current Texture of current cutscene case:
                        if ((pEntity as ITexture).Texture != _entityTextureDict[pEntity.UName][_cutsceneCase])
                        {
                            // INITIALISE pEntity.Texture Property with Texture of _entityTextureDict[pEntity.UName] using _cutsceneCase to address the exact image required:
                            (pEntity as ITexture).Texture = _entityTextureDict[pEntity.UName][_cutsceneCase];
                        }
                    }
                }
            }
        }

        #endregion
    }
}