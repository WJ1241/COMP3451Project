using OrbitalEngine.Services.Commands.Interfaces;

namespace COMP3451Project.RIRRPackage.Behaviours.Interfaces
{
    /// <summary>
    /// Interface which allows implementations to contain a command related to a scoring method
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 04/04/22
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
