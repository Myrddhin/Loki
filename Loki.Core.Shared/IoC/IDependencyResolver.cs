using System;
using System.Reflection;

namespace Loki.Common.IoC
{
    public interface IDependencyResolver : IDisposable
    {
        /// <summary>
        /// Overrides an infrastructure singleton.
        /// </summary>
        /// <typeparam name="T">
        /// Infrasctucure interface.
        /// </typeparam>
        /// <param name="type">
        /// Concrete implementation of infrastucture type.
        /// </param>
        void OverrideInfrastructureService<T>(Type type)
            where T : class;

        /// <summary>
        /// Registers an infrastructure singleton.
        /// </summary>
        /// <typeparam name="T">
        /// Infrasctucure interface.
        /// </typeparam>
        /// <param name="instance">
        /// Service instance.
        /// </param>
        void OverrideInfrastructureInstance<T>(T instance)
            where T : class;

        /// <summary>
        /// Resolves the required type.
        /// </summary>
        /// <typeparam name="T">Required type.</typeparam>
        /// <returns>Returns a configured instance for the specified type.</returns>
        T Resolve<T>();

        /// <summary>
        /// Resolves all registrations for the required type.
        /// </summary>
        /// <typeparam name="T">Required type.</typeparam>
        /// <returns>An array of configured instances of the specified type.</returns>
        T[] ResolveAll<T>();

        /// <summary>
        /// Resolves the required type.
        /// </summary>
        /// <typeparam name="T">
        /// Required type.
        /// </typeparam>
        /// <param name="name">
        /// A specific name.
        /// </param>
        /// <returns>
        /// Returns a configured instance of the specified type.
        /// </returns>
        T Resolve<T>(string name);

        /// <summary>
        /// Registers the installers included in an assembly.
        /// </summary>
        /// <param name="assembly">
        /// The specified assembly to scan for installers.
        /// </param>
        void RegisterAssembly(Assembly assembly);

        /// <summary>
        /// Registers an installer.
        /// </summary>
        /// <param name="installer">
        /// The installer.
        /// </param>
        void RegisterInstaller(object installer);

        /// <summary>
        /// Untrack the referenced resolved object.
        /// </summary>
        /// <param name="reference">
        /// The resolved object.
        /// </param>
        void Release(object reference);
    }
}