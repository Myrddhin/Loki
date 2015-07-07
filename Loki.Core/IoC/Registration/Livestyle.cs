namespace Loki.IoC.Registration
{
    /// <summary>
    /// Class for lifestype configuration.
    /// </summary>
    /// <typeparam name="TService">Service type.</typeparam>
    public class Livestyle<TService> where TService : class
    {
        private ElementRegistration<TService> parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="Livestyle{TService}"/> class.
        /// </summary>
        /// <param name="parentConfiguration">The parent configuration.</param>
        public Livestyle(ElementRegistration<TService> parentConfiguration)
        {
            parent = parentConfiguration;
            Type = LifestyleType.Singleton;
        }

        /// <summary>
        /// Gets a no tracking lifestyle.
        /// </summary>
        /// <value>
        /// Modified configuration.
        /// </value>
        public ElementRegistration<TService> NoTracking
        {
            get
            {
                Type = LifestyleType.NoTracking;
                return parent;
            }
        }

        /// <summary>
        /// Gets the pool size maximum.
        /// </summary>
        /// <value>
        /// The pool size maximum.
        /// </value>
        public int? PoolSizeMax { get; private set; }

        /// <summary>
        /// Gets the pool size minimum.
        /// </summary>
        /// <value>
        /// The pool size minimum.
        /// </value>
        public int? PoolSizeMin { get; private set; }

        /// <summary>
        /// Gets a singleton lifestyle.
        /// </summary>
        /// <value>
        /// Modified configuration.
        /// </value>
        public ElementRegistration<TService> Singleton
        {
            get
            {
                Type = LifestyleType.Singleton;
                return parent;
            }
        }

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public TService SingletonInstance { get; private set; }

        /// <summary>
        /// Gets a transient lifestyle.
        /// </summary>
        /// <value>
        /// Modified configuration.
        /// </value>
        public ElementRegistration<TService> Transient
        {
            get
            {
                Type = LifestyleType.Transient;
                return parent;
            }
        }

        /// <summary>
        /// Gets a per request lifestyle.
        /// </summary>
        /// <value>
        /// Modified configuration.
        /// </value>
        public ElementRegistration<TService> PerRequest
        {
            get
            {
                Type = LifestyleType.PerRequest;
                return parent;
            }
        }

        /// <summary>
        /// Gets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public LifestyleType Type { get; private set; }

        /// <summary>
        ///  Gets a singleton lifestyle and sets the singleton value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Modified configuration.</returns>
        public ElementRegistration<TService> Instance(TService instance)
        {
            Type = LifestyleType.Singleton;
            SingletonInstance = instance;
            return parent;
        }

        /// <summary>
        /// Gets a pooled lifestyle and sets the pool values.
        /// </summary>
        /// <param name="poolMinSize">Minimum size of the pool.</param>
        /// <param name="poolMaxSize">Maximum size of the pool.</param>
        /// <returns>Modified configuration.</returns>
        public ElementRegistration<TService> PooledWithSize(int? poolMinSize, int? poolMaxSize)
        {
            Type = LifestyleType.Pooled;
            PoolSizeMax = poolMaxSize;
            PoolSizeMin = poolMinSize;
            return parent;
        }
    }
}