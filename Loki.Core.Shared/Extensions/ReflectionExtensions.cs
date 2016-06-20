using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Loki.Common
{
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Get's the name of the assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly.
        /// </param>
        /// <returns>
        /// The assembly's name.
        /// </returns>
        public static string GetAssemblyName(this Assembly assembly)
        {
            return assembly.FullName.Remove(assembly.FullName.IndexOf(','));
        }

        /// <summary>
        /// Gets all the attributes of a particular type.
        /// </summary>
        /// <typeparam name="T">
        /// The type of attributes to get.
        /// </typeparam>
        /// <param name="member">
        /// The member to inspect for attributes.
        /// </param>
        /// <param name="inherit">
        /// Whether or not to search for inherited attributes.
        /// </param>
        /// <returns>
        /// The list of attributes found.
        /// </returns>
        public static IEnumerable<T> GetAttributes<T>(this Type member, bool inherit)
        {
            return member.GetTypeInfo().GetCustomAttributes(inherit).OfType<T>();
        }
    }
}