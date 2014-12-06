using System;
using System.Collections.Generic;

namespace Loki.IoC
{
    /// <summary>
    /// Interface for object creators.
    /// </summary>
    public interface IObjectCreator
    {
        /// <summary>
        /// Gets an instance of type <typeparamref name="T"/> as defined in the configuration.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="objectName">Name of the object.</param>
        /// <returns>The object.</returns>
        T Get<T>(string objectName) where T : class;

        /// <summary>
        /// Gets an instance of type <typeparamref name="T"/> as defined in the configuration.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>The object.</returns>
        T Get<T>() where T : class;

        /// <summary>
        /// Gets an instance of type <paramref name="type"/> as defined in the configuration.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The object.</returns>
        object Get(Type type);

        /// <summary>
        /// Gets an instance of type <paramref name="type" /> as defined in the configuration.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="objectName">Name of the object.</param>
        /// <returns>
        /// The object.
        /// </returns>
        object Get(Type type, string objectName);

        /// <summary>
        /// Gets all instances of type <paramref name="type" /> as defined in the configuration.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The configured instances.</returns>
        IEnumerable<object> GetAll(Type type);

        /// <summary>
        /// Gets all instances of type <typeparamref name="T"/> as defined in the configuration.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <returns>The configured instances.</returns>
        IEnumerable<T> GetAll<T>();
    }
}