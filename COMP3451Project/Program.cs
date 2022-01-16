using System;
using Microsoft.Xna.Framework;
using COMP3451Project.EnginePackage.Services;
using COMP3451Project.EnginePackage.Services.Factories;

namespace COMP3451Project
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// Author: William Smith
        /// Date: 19/12/21
        /// </summary>
        [STAThread]
        static void Main()
        {
            #region FIELD VARIABLES & INSTANTIATIONS

            // DECLARE & INSTANTIATE an IFactory<IService> as a new Factory<IService>(), name it '_serviceFactory':
            IFactory<IService> _serviceFactory = new Factory<IService>();

            // DECLARE & INSTANTIATE an IService as a new EngineManager(), name it '_engineManager':
            IService _engineManager = _serviceFactory.Create<EngineManager>();

            // DECLARE & INSTANTIATE a Game as a new Kernel(), name it '_game':
            Game _game = new Kernel();

            #endregion


            #region OBJECT INITIALISATION

            // INITIALISE _engineManager with _serviceFactory:
            (_engineManager as IInitialiseIService).Initialise(_serviceFactory as IService);

            // INITIALISE _game with _engineManager:
            (_game as IInitialiseIService).Initialise(_engineManager);

            #endregion

            // CALL Run() on _game:
            _game.Run();
        }
    }
#endif
}
