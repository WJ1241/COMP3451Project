

namespace COMP3451Project.RIRRPackage.Entities.Interfaces
{
    /// <summary>
    /// Interface which allows implementations 
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 12/04/22
    /// </summary>
    public interface IHPBar
    {
        #region METHODS

        /// <summary>
        /// Changes Health Points to display
        /// </summary>
        /// <param name="pHealth"> Value of Health Points to display </param>
        void ChangeHealth(int pHealth);

        #endregion
    }
}
