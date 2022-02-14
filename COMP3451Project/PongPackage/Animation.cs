using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.Models
{
    /// <summary>
    /// public class Animation
    /// contains code for Animation
    /// Authors: Declan Kerby-Collins & William Smith
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    public class Animation
    {
        #region properties
        // PROPERTY: type int name it 'CurrentFrame'
        public int CurrentFrame { get; set; }

        // PROPERTY: type int name it 'FrameCount'
        public int FrameCount { get; set; }

        // PROPERTY: type int name it 'FrameHeight'
        public int FrameHeight { get { return _texture.Height; } }

        // PROPERTY: type int name it 'FrameWidth'
        public int FrameWidth { get { return _texture.Width / FrameCount; } }

        // PROPERTY: type Keys name it 'FrameSpeed'
        public float FrameSpeed { get; set; }

        // PROPERTY: type bool name it 'IsLooping'
        public bool IsLooping { get; set; }

        // PROPERTY: type Texture2D name it '_texture'
        public Texture2D _texture { get; private set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pTexture"></param> sprite texture
        /// <param name="pFrameCount"></param> current FrameCount
        public Animation(Texture2D pTexture, int pFrameCount)
        {
            // ASSIGNMENT: _texture is set to the value of the parameter texture
            _texture = pTexture;

            // ASSIGNMENT: FrameCount is set to the value of the parameter FrameCount
            FrameCount = pFrameCount;

            // ASSIGNMENT: IsLooping is set to true
            IsLooping = true;

            // ASSIGNMENT: FrameSpeed is set to 0.2f
            FrameSpeed = 0.2f;

        }
    }
}
