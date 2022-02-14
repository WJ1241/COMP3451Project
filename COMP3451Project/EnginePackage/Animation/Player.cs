using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Animation
{
    public class Player
    {
        // DECLARE: Texture 2d name it '_playerAnimation'
        private Texture2D _playerAnimation;




        public Player(Texture2D pAnime)
        {
            _playerAnimation = pAnime;
        }
    }
}
