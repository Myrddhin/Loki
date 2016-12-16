using System;
using Loki.IoC.Registration;

namespace Loki.IoC
{
    /// <summary>
    /// Common interface for object contexts.
    /// </summary>
    public interface IObjectContext : IObjectCreator, IDisposable
    {
        /// <summary>
        /// Initializes the context with the specified installers.
        /// </summary>
        /// <param name="installers">The installers.</param>
        void Initialize(params IContextInstaller[] installers);

        /// <summary>
        /// Defines the type for creation.
        /// </summary>
        /// <typeparam name="T">Concrete type.</typeparam>
        /// <param name="definition">The type definition.</param>
        void Register<T>(ElementRegistration<T> definition)
            where T : class;
    }
}