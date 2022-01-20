using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COMP3451Project.EnginePackage.Services.Factories
{
    /// <summary>
    /// Interface which initialises an object with an IFactory<T> object
    /// Author: William Smith & Declan Kerby-Collins
    /// Date: 20/01/22
    /// </summary>
    /// <typeparam name="A"> Any Class to make a factory for </typeparam>
    public interface IInitialiseIFactory<A>
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with an IFactory<A> object
        /// </summary>
        /// <param name="pFactory"> IFactory<A> object </param>
        void Initialise(IFactory<A> pFactory);

        #endregion
    }
}
