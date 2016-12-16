using System;

namespace Loki.IoC.Registration
{
    /// <summary>
    /// Element registration hook.
    /// </summary>
    public static class Element
    {
        /// <summary>
        /// Registers a new service type.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <returns>Returns the registration object.</returns>
        public static ElementRegistration<TService> For<TService>()
            where TService : class
        {
            return new ElementRegistration<TService>();
        }

        /// <summary>
        /// Registers a new service type with two contracts.
        /// </summary>
        /// <typeparam name="TService">
        /// The first type of the service.
        /// </typeparam>
        /// <typeparam name="TService2">
        /// The second type of the service.
        /// </typeparam>
        /// <returns>
        /// Returns the registration object.
        /// </returns>
        public static ElementRegistration<TService> For<TService, TService2>()
            where TService : class
        {
            var registration = new ElementRegistration<TService>();
            registration.Types.Add(typeof(TService2));

            return registration;
        }

        /// <summary>
        /// Registers a new type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>Returns the registration object.</returns>
        public static ElementRegistration For(Type type)
        {
            return new ElementRegistration(type);
        }

        /// <summary>
        /// Registers a new service instance.
        /// </summary>
        /// <typeparam name="TService">The type of the service.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        /// <returns>Returns the registration object.</returns>
        public static ElementRegistration<TService> Service<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
        {
            return For<TService>().ImplementedBy<TImplementation>().Lifestyle.Singleton;
        }
    }
}