using Microsoft.Xna.Framework;

namespace COMP3451Project.EnginePackage.EntityManagement
{
    /// <summary>
    /// Interface that allows implementations to be an on-screen Entity
    /// </summary>
    public interface IEntity
    {
        #region METHODS

        /// <summary>
        /// Initialises entity variable values
        /// </summary>
        void Initialise();

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which can get and set value of an entity's position
        /// </summary>
        Vector2 Position { get; set; }

        /// <summary>
        /// Property which can get and set value of an entity's unique ID
        /// </summary>
        int UID { get; set; }

        /// <summary>
        /// Property which can get and set value of an entity's unique Name
        /// </summary>
        string UName { get; set; }

        #endregion
    }
}
