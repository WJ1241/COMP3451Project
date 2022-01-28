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
    /// Date: 24/01/22
    /// </summary>
    public class PaddleBehaviour : PongBehaviour
    {
        #region INHERITED FROM PONGBEHAVIOUR

        /// <summary>
        /// Used when an object hits a boundary, possibly to change direction or stop
        /// </summary>
        protected override void Boundary()
        {
            // IF Paddle at top of screen
            if (_entity.Position.Y <= 0)
            {
                // ASSIGNMENT, set _position.Y to 0, keeps at top of screen:
                _entity.Position = new Vector2(_entity.Position.X, 0);
            }

            // ELSE IF Paddle at bottom of screen
            else if (_entity.Position.Y >= (_entity as IContainBoundary).WindowBorder.Y - (_entity as IContainBoundary).WindowBorder.Y)
            {
                // ASSIGNMENT, set _position.Y to _windowBorder.Y - _texture.Height, keeps at bottom of screen:
                _entity.Position = new Vector2(_entity.Position.X, (_entity as IContainBoundary).WindowBorder.Y - (_entity as IContainBoundary).WindowBorder.Y);
            }
        }

        #endregion
    }
}
