using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using COMP3451Project.EnginePackage.Camera;
using COMP3451Project.EnginePackage.CoreInterfaces;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.EnginePackage.SceneManagement
{
    /// <summary>
    /// Class which holds reference to list in Scene Manager, Draws and Updates entities
    /// </summary>
    /// <REFERENCE> Abuhakmeh, K. (2009) XNA 2D Camera Engine That Follows Sprite. Available at: https://stackoverflow.com/questions/712296/xna-2d-camera-engine-that-follows-sprite. (Accessed: 20 April 2021). </REFERENCE>
    public class SceneGraph : ISceneGraph, IUpdatable, IDraw, ISpawn, IDrawCamera
    {
        #region FIELD VARIABLES

        // DECLARE an IDictionary, call it '_entityDictionary':
        private IDictionary<string, IEntity> _sceneDictionary;

        #endregion


        #region CONSTRUCTOR

        /// <summary>
        /// Constructor for objects of SceneGraph
        /// </summary>
        public SceneGraph()
        {
        }

        #endregion


        #region IMPLEMENTATION OF ISCENEGRAPH

        /// <summary>
        /// Initialises object with a reference to an IDictionary<string, IEntity>
        /// </summary>
        /// <param name="sceneDictionary">Holds reference to 'IDictionary<string, IEntity>'</param>
        public void Initialise(IDictionary<string, IEntity> sceneDictionary)
        {
            // ASSIGN _sceneDictionary as the same instance as sceneDictionary:
            _sceneDictionary = sceneDictionary;
        }

        #endregion


        #region IMPLEMENTATION OF ISPAWN

        /// <summary>
        /// Spawns Entity on screen and adds to a list/dictionary
        /// </summary>
        /// <param name="entity">Reference to an instance of IEntity</param>
        /// <param name="position">Positional values used to place entity</param>
        public void Spawn(IEntity entity, Vector2 position)
        {
            // ASSIGN entity.Position as the same value as position:
            entity.Position = position;
        }

        #endregion


        #region IMPLEMENTATION OF IDRAW

        /// <summary>
        /// When called, draws entity's texture on screen
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        public void Draw(SpriteBatch spriteBatch)
        {
            // BEGIN creation of displayable objects:
            spriteBatch.Begin();

            // FOREACH any entity implementing IDraw:
            foreach (IDraw entity in _sceneDictionary.Values)
            {
                // CALL Draw method on all entities in _entityDictionary:
                entity.Draw(spriteBatch);
            }

            // END creation of displayable objects:
            spriteBatch.End();
        }

        #endregion

        /// <summary>
        /// When called, draws entity's texture on screen, as well as reposition a a camera object
        /// </summary>
        /// <param name="spriteBatch">Needed to draw entity's texture on screen</param>
        /// <param name="camera">Needed to move camera position on screen</param>
        /// <CITATION> (Abuhakmeh, 2009) </CITATION>
        public void Draw(SpriteBatch spriteBatch, ICamera camera)
        {
            // BEGIN creation of displayable objects:
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, camera.ChngCamPos());

            // FOREACH any entity implementing IDraw:
            foreach (IDraw entity in _sceneDictionary.Values)
            {
                // CALL Draw method on all entities in _entityDictionary:
                entity.Draw(spriteBatch);
            }

            // END creation of displayable objects:
            spriteBatch.End();
        }


        #region IMPLEMENTATION OF IUPDATABLE

        /// <summary>
        /// Updates object when a frame has been rendered on screen
        /// </summary>
        /// <param name="gameTime">holds reference to GameTime object</param>
        public void Update(GameTime gameTime)
        {
            // FOREACH any entity implementing IUpdatable:
            foreach (IEntity entity in _sceneDictionary.Values)
            {
                if (entity is IUpdatable) // IF entity implements IUpdatable
                {
                    // CALL Update method on all updatable entities in _entityDictionary, casting as an IUpdatable:
                    (entity as IUpdatable).Update(gameTime);
                }
            }
        }

        #endregion
    }
}
