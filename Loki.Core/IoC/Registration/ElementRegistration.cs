using System;

namespace Loki.IoC.Registration
{
    /// <summary>
    /// General element configuration class.
    /// </summary>
    public class ElementRegistration : ElementRegistration<object>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementRegistration"/> class.
        /// </summary>
        /// <param name="serviceType">Type of the  service.</param>
        public ElementRegistration(Type serviceType)
        {
            Types.Clear();
            this.Types.Add(serviceType);
        }
    }
}