using COMP3451Project.EnginePackage.Exceptions;
using COMP3451Project.EnginePackage.Services.Factories.Interfaces;
using COMP3451Project.EnginePackage.Services.Interfaces;

namespace COMP3451Project.EnginePackage.Services.Factories
{
    /// <summary>
    /// Class which creates and returns an object of any type specified as replacement of generics
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 19/12/21
    /// </summary>
    public class Factory<A> : IFactory<A>, IService
    {
        #region IMPLEMENTATION OF IFACTORY<A>

        /// <summary>
        /// Creation method which returns an object of type 'A'
        /// </summary>
        /// <typeparam name="C"> Generic Type which implements generic 'A' </typeparam>
        /// <returns> New instance of type 'C' </returns>
        public A Create<C>() where C : A, new()
        {
            // DECLARE an object of type 'A' and instantiate an instance of 'C', name it '_tempObj':
            A _tempObj = new C();

            // IF _tempObj is an instance of type 'A':
            if (_tempObj is A)
            {
                // RETURN _tempObj to method caller:
                return _tempObj;
            }
            else
            {
                // THROW a new ClassDoesNotExistException(), with corresponding message:
                throw new ClassDoesNotExistException("ERROR: Class or Interface passed in placement for generic does not exist in program");
            }
        }

        #endregion
    }
}
