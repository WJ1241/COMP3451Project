using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Animation
{
    interface IAnimation
    {
        int Height { get; set; }

        int Width { get; set; }

        Texture2D SpriteSheet { get; set; }

        void Input(string pString);

        void Update(GameTime pGameTime);

        void Draw(SpriteBatch pSpriteBatch);



    }
}
