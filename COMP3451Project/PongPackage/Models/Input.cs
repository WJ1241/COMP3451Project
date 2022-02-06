using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.PongPackage.Models
{
    /// <summary>
    /// public class Input
    /// contains the input properties
    /// Authors: Declan Kerby-Collins & Oyyou
    /// date: 04/02/2022
    /// <Reference> Oyyou (2017) MonoGame Tutorial 011 - Sprite Animation Avaliable at: https://www.youtube.com/watch?v=OLsiWxgONeM&t=297s Accessed 04/02/2022 </Refference>
    /// </summary>
    public class Input
    {
        //PROPERTY: type Keys name it 'Up'
        public Keys Up { get; set; }

        //PROPERTY: type Keys name it 'Down'
        public Keys Down { get; set; }

        //PROPERTY: type Keys name it 'Left'
        public Keys Left { get; set; }

        //PROPERTY: type Keys name it 'Right'
        public Keys Right {get; set;}

        
    }
}
