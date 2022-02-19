

namespace OrbitalEngine.CoreInterfaces
{
    #region ABSTRACT ONE PARAM INITIALISE

    /// <summary>
    /// Interface that allows implementations to store a Value/Object of type 'T'
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for First Param </typeparam>
    public interface IInitialiseParam<T>
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with a value/object of type 'T'
        /// </summary>
        /// <param name="pT"> Value/Object of type 'T' </param>
        void Initialise(T pT);

        #endregion
    }

    #endregion


    #region ABSTRACT TWO PARAM INITIALISE

    /// <summary>
    /// Interface that allows implementations to store Values/Objects of types 'T' and 'U'
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for First Param </typeparam>
    /// <typeparam name="U"> Any Type, 'U' for Second Param </typeparam>
    public interface IInitialiseParam<T, U>
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with Values/Objects of types 'T' and 'U'
        /// </summary>
        /// <param name="pT"> Value/Object of type 'T' </param>
        /// <param name="pU"> Value/Object of type 'U' </param>
        void Initialise(T pT, U pU);

        #endregion
    }

    #endregion


    #region ABSTRACT THREE PARAM INITIALISE

    /// <summary>
    /// Interface that allows implementations to store Values/Objects of type 'T', 'U' and 'V'
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 30/01/22
    /// </summary>
    /// <typeparam name="T"> Any Type, 'T' for First Param </typeparam>
    /// <typeparam name="U"> Any Type, 'U' for Second Param </typeparam>
    /// <typeparam name="V"> Any Type, 'V' for Third Param </typeparam>
    public interface IInitialiseParam<T, U, V>
    {
        #region METHODS

        /// <summary>
        /// Initialises an object with Values/Objects of types 'T', 'U' and 'V'
        /// </summary>
        /// <param name="pT"> Value/Object of type 'T' </param>
        /// <param name="pU"> Value/Object of type 'U' </param>
        /// <param name="pV"> Value/Object of type 'V' </param>
        void Initialise(T pT, U pU, V pV);

        #endregion
    }

    #endregion
}
