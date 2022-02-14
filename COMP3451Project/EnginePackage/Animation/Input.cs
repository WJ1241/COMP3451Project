using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Animation
{
    /// <summary>
    /// public class Input
    /// contains the input properties
    /// Authors: Declan Kerby-Collins & William Smith
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    public class Input
    {
        // PROPERTY: type Keys name it 'up' 
        public Keys Up { get; set; }

        // PROPERTY: type Keys name it 'down' 
        public Keys Down { get; set; }

        // PROPERTY: type Keys name it 'right'  
        public Keys Right { get; set; }

        // PROPERTY: type Keys name it 'left'
        public Keys Left {get; set;}

        
    }
}
