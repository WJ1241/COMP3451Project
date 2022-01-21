using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.EntityManagement;

namespace COMP3451Project.PongPackage.BehaviourClasses
{
    /// <summary>
    /// Class which defines the behaviour for Paddle entities
    /// Authors: William Smith & Declan Kerby-Collins 
    /// Date: 21/01/22
    /// </summary>
    public class PaddleBehaviour : PongBehaviour
    {
        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {
            if (_entity.Position.Y <= 0) // IF paddle at top of screen
            {
                // ASSIGNMENT, set _position.Y to 0:
                _entity.Position = new Vector2(_entity.Position.X, 0); // Keeps at top of screen
            }
            else if (_entity.Position.Y >= (_entity as IContainBoundary).WindowBorder.Y - (_entity as IContainBoundary).WindowBorder.Y) // IF paddle at bottom of screen
            {
                // ASSIGNMENT, set _position.Y to _windowBorder.Y - _texture.Height:
                _entity.Position = new Vector2(_entity.Position.X, (_entity as IContainBoundary).WindowBorder.Y - (_entity as IContainBoundary).WindowBorder.Y); // Keeps at bottom of screen
            }
        }

        #endregion
    }
}
