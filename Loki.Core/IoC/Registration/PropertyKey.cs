using System.Reflection;

namespace Loki.IoC.Registration
{
    /// <summary>
    /// Class for property keys.
    /// </summary>
    /// <typeparam name="TService">Configured service.</typeparam>
    public class PropertyKey<TService>
    {
        internal PropertyKey(Property<TService> propertyParent)
        {
            parent = propertyParent;
        }

        private Property<TService> parent;

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>
        /// The key.
        /// </value>
        public PropertyInfo Key { get; internal set; }

        /// <summary>
        /// Sets the configured value for the property.
        /// </summary>
        /// <param name="propertyValue">The property value.</param>
        /// <returns>Modified configuration.</returns>
        public Property<TService> Value(object propertyValue)
        {
            parent.Value = propertyValue;
            return parent;
        }

        /// <summary>
        /// Ignores the auto mapping for this property.
        /// </summary>
        /// <returns>Modified configuration.</returns>
        public Property<TService> Ignore()
        {
            parent.Ignore = true;
            return parent;
        }

        public Property<TService> Named(string name)
        {
            parent.Name = name;
            return parent;
        }
    }
}