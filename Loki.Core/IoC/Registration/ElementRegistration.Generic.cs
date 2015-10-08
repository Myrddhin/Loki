using System;
using System.Collections.Generic;

namespace Loki.IoC.Registration
{
    /// <summary>
    /// IoC registration class.
    /// </summary>
    /// <typeparam name="TService">Component to configure.</typeparam>
    public class ElementRegistration<TService> where TService : class
    {
        public ElementRegistration()
        {
            Types = new List<Type>();
            Lifestyle = new Livestyle<TService>(this);
            Types.Add(typeof(TService));
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the defined types.
        /// </summary>
        /// <value>
        /// The types.
        /// </value>
        public IList<Type> Types { get; private set; }

        /// <summary>
        /// Gets the concrete type for the definitions.
        /// </summary>
        /// <value>
        /// The implementation.
        /// </value>
        public Type Implementation { get; private set; }

        /// <summary>
        /// Gets the lifestyle.
        /// </summary>
        /// <value>
        /// The lifestyle.
        /// </value>
        public Livestyle<TService> Lifestyle { get; private set; }

        private Dictionary<string, IProperty<TService>> configuredProperties = new Dictionary<string, IProperty<TService>>();

        /// <summary>
        /// Gets the configured properties.
        /// </summary>
        /// <value>
        /// The configured properties.
        /// </value>
        public IEnumerable<IProperty<TService>> ConfiguredProperties
        {
            get { return configuredProperties.Values; }
        }

        public ElementRegistration<TService> AsFactory()
        {
            IsFactory = true;
            return this;
        }

        public bool IsFactory
        {
            get;
            private set;
        }

        public bool IsDefault
        {
            get;
            private set;
        }

        public ElementRegistration<TService> AsDefault()
        {
            IsDefault = true;
            return this;
        }

        public bool IsFallback
        {
            get;
            private set;
        }

        public ElementRegistration<TService> AsFallback()
        {
            IsFallback = true;
            return this;
        }

        /// <summary>
        /// Nameds the specified service.
        /// </summary>
        /// <param name="serviceName">Name of the service.</param>
        /// <returns>Modified definition.</returns>
        public ElementRegistration<TService> Named(string serviceName)
        {
            Name = serviceName;
            return this;
        }

        /// <summary>
        /// Sets the concrete type for the definitions.
        /// </summary>
        /// <typeparam name="TConcrete">The concrete type.</typeparam>
        /// <returns>Modified definition.</returns>
        public ElementRegistration<TService> ImplementedBy<TConcrete>() where TConcrete : TService
        {
            Implementation = typeof(TConcrete);
            return this;
        }

        /// <summary>
        /// Define the concrete instance (singleton).
        /// </summary>
        /// <param name="serviceInstance">The service instance.</param>
        /// <returns>Modified definition.</returns>
        public ElementRegistration<TService> Instance(TService serviceInstance)
        {
            return Lifestyle.Instance(serviceInstance);
        }

        /// <summary>
        /// Sets the properties configuration.
        /// </summary>
        /// <param name="propertiesConfiguration">The properties configuration.</param>
        /// <returns>Modified definition.</returns>
        public ElementRegistration<TService> Properties(params IProperty<TService>[] propertiesConfiguration)
        {
            if (propertiesConfiguration != null)
            {
                foreach (var property in propertiesConfiguration)
                {
                    configuredProperties[property.Key.Name] = property;
                }
            }

            return this;
        }
    }
}