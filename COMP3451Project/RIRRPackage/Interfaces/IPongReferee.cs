using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to keep score of a Pong game
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
    /// </summary>
    public interface IPongReferee
    {
        #region METHODS

        /// <summary>
        /// Checks to see who scores a point
        /// </summary>
        /// <param name="pPlayerNum"> Player Number </param>
        void CheckWhoScored(int pPlayerNum);

        #endregion


        #region PROPERTIES

        /// <summary>
        /// Property which allows only write access to a RespawnBall ICommand
        /// </summary>
        ICommand RespawnBall { set; }

        #endregion
    }
}
