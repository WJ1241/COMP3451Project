using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Animation
{
    /// <summary>
    /// Interface which allows implementations to run animations
    /// Authors:  Declan Kerby-Collins & William Smith
    /// Date: 17/02/22
    /// </summary>
    public interface IAnimation
    {
        // PROPERTY: int Row
        int Row { get; set; }

        // PROPERTY: Vector2 Position
        Vector2 Position { get; set; }

        // PROPERTY: Point SpriteSize
        Point SpriteSize { get; set; }

        /// <summary>
        /// METHOD: Initialise gives access to the Initialise class and passes in pSpriteSheet
        /// </summary>
        /// <param name="pSpriteSheet"></param>
        void Initialise(Texture2D pSpriteSheet);

        /// <summary>
        /// METHOD: Input gives access to the Input class and passes in pString
        /// </summary>
        /// <param name="pString"></param>
        void Input(string pString);

        /// <summary>
        /// METHOD: Draw gives access to the Draw class and passes in pSpriteBatch
        /// </summary>
        /// <param name="pSpriteBatch"></param>
        void Draw(SpriteBatch pSpriteBatch);

        /// <summary>
        /// METHOD: Update gives access to the Update class and passes in game time 
        /// </summary>
        /// <param name="pGameTime"></param>
        void Update(GameTime pGameTime);


    }
}
