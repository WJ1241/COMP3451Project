using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.PongPackage.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to allow goals to be scored
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 20/02/22
    /// </summary>
    public interface IScoreGoal
    {
        #region PROPERTIES

        /// <summary>
        /// Property which allows only write access to ScoreGoal ICommand
        /// </summary>
        ICommand ScoreGoal { set; }

        #endregion
    }
}
