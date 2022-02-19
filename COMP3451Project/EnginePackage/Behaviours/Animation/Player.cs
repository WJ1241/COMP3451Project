using Microsoft.Xna.Framework;
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

        // DECLARE: IAnimation name it '_playerAnimation'
        private IAnimation[] _playerAnime;
        
        private Input _input;

        

        public Player(Texture2D pAnime)
        {
            _playerAnime = new IAnimation[3];

            _playerAnimation = pAnime;

            _playerAnime[0] = new Animation(_playerAnimation, 0, 32, 32);
            _playerAnime[1] = new Animation(_playerAnimation, 0, 32, 32);
            _playerAnime[2] = new Animation(_playerAnimation, 0, 32, 32);


        }

        public void Update()
        {
            
        }


        public void PlayerDraw(SpriteBatch pSpriteBatch, Vector2 pMove, GameTime pGameTime)
        {
            _playerAnime[0].Draw(pSpriteBatch, Input.Update(pMove), pGameTime, 300);
        }
    }
}
