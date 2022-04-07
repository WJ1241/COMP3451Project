using OrbitalEngine.Exceptions;
using OrbitalEngine.Services.Factories.Interfaces;
using OrbitalEngine.Services.Interfaces;

namespace OrbitalEngine.Services.Factories
{
    /// <summary>
    /// Class which creates and returns an object of any type specified as replacement of generics
    /// Authors: William Smith & Declan Kerby-Collins
    /// Date: 07/04/22
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
            // DECLARE an object of type 'A' and instantiate an instance of 'C', name it 'tempObj':
            A tempObj = new C();

            // IF tempObj is an instance of type 'A':
            if (tempObj is A)
            {
                // RETURN tempObj to method caller:
                return tempObj;
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
