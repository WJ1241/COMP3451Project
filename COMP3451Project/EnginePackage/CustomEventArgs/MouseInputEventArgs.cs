using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace COMP3451Project.EnginePackage.CustomEventArgs
{
    /// <summary>
    /// Class which acts as an EventArgs object for an 'MouseInput' event
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    public class MouseInputEventArgs : EventArgs
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows get access to current mouse state
        /// </summary>
        public MouseState RequiredArg
        {
            get
            {
                // RETURN Mouse.GetState():
                return Mouse.GetState();
            }
        }

        #endregion
    }
}
