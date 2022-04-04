using System;
using Microsoft.Xna.Framework;
using OrbitalEngine.CoreInterfaces;
using OrbitalEngine.Services;
using OrbitalEngine.Services.Factories;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;

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

            // DECLARE & INSTANTIATE an IFactory<IService> as a new Factory<IService>(), name it 'serviceFactory':
            IFactory<IService> serviceFactory = new Factory<IService>();

            // DECLARE & INSTANTIATE an IService as a new EngineManager(), name it 'engineManager':
            IService engineManager = serviceFactory.Create<EngineManager>();

            // DECLARE & INSTANTIATE a Game as a new Kernel(), name it 'kernel':
            Game kernel = new Kernel();

            #endregion


            #region OBJECT INITIALISATION

            // INITIALISE engineManager with serviceFactory:
            (engineManager as IInitialiseParam<IFactory<IService>>).Initialise(serviceFactory);

            // INITIALISE kernel with engineManager:
            (kernel as IInitialiseParam<IService>).Initialise(engineManager);

            #endregion

            // CALL Run() on kernel:
            kernel.Run();
        }
    }
#endif
}
