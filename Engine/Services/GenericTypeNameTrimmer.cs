using System;

namespace OrbitalEngine.Services
{
    /// <summary>
    /// Static Class which trims Generic Arity information to allow tidier naming and easier addressing
    /// Author: William Smith, Declan Kerby-Collins & 'LukeH'
    /// Date: 07/02/22
    /// </summary>
    /// <REFERENCE> LukeH (2010) C# Get Generic Type Name. Available at: https://stackoverflow.com/questions/4185521/c-sharp-get-generic-type-name. (Accessed: 7 February 2022) </REFERENCE>
    public static class GenericTypeNameTrimmer
    {
        #region METHODS

        /// <summary>
        /// Trims ONE generic from a type name so that addressing becomes easier
        /// </summary>
        /// <param name="pType"> Type including Generic parameters </param>
        /// <returns> Trimmed string name </returns>
        /// <CITATION> LukeH (2010) </CITATION>
        public static string TrimOneGeneric(Type pType)
        {
            // RETURN a trimmed string to remove generic info for easier addressing:
            return pType.Name.Remove(pType.Name.IndexOf("`")) + "<" + pType.GetGenericArguments()[0].Name + ">";
        }

        /// <summary>
        /// Trims TWO generics from a type name so that addressing becomes easier
        /// </summary>
        /// <param name="pType"> Type including Generic parameters </param>
        /// <returns> Trimmed string name </returns>
        /// <CITATION> LukeH (2010) </CITATION>
        public static string TrimTwoGeneric(Type pType)
        {
            // RETURN a trimmed string to remove generic info for easier addressing:
            return pType.Name.Remove(pType.Name.IndexOf("`")) + "<" + pType.GetGenericArguments()[0].Name + ", " + pType.GetGenericArguments()[1].Name + ">";
        }

        /// <summary>
        /// Trims ONE generic from use of TWO generic types so that addressing becomes easier
        /// </summary>
        /// <param name="pType"> Type including Generic parameters </param>
        /// <returns> Trimmed string name </returns>
        /// <CITATION> LukeH (2010) </CITATION>
        public static string TrimOneDoubleGeneric(Type pType)
        {
            // RETURN a trimmed string to remove generic info for easier addressing:
            return pType.Name.Remove(pType.Name.IndexOf("`")) + "<" + pType.GetGenericArguments()[0].Name.Remove(pType.GetGenericArguments()[0].Name.IndexOf("`")) + "<" + pType.GetGenericArguments()[0].GetGenericArguments()[0].Name + ">>";
        }

        #endregion
    }
}
