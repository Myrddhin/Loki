using System;
using System.Reflection;

namespace Loki.Common.IoC
{
    /// <summary>
    /// IoC object container.
    /// </summary>
    public partial class IoCContainer : IDisposable
    {
        private readonly IDependencyResolver resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="IoCContainer"/> class.
        /// </summary>
        /// <param name="naked">
        /// True if the container should not be initialized with infrascture services.
        /// </param>
        public IoCContainer(bool naked = false)
        {
            resolver = DependencyResolverFactory();

            if (!naked)
            {
                this.RegisterInfrastructure();
            }
        }

        /// <summary>
        /// Disposes the container.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed;

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }

            resolver.Dispose();

            disposed = true;
        }

        /// <summary>
        /// Resolves the required type.
        /// </summary>
        /// <typeparam name="T">Required type.</typeparam>
        /// <returns>Returns a configured instance for the specified type.</returns>
        public T Resolve<T>()
        {
            return resolver.Resolve<T>();
        }

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
        public T Resolve<T>(string name)
        {
            return resolver.Resolve<T>(name);
        }

        /// <summary>
        /// Registers the installers included in an assembly.
        /// </summary>
        /// <param name="assembly">
        /// The specified assembly to scan for installers.
        /// </param>
        public void RegisterAssembly(Assembly assembly)
        {
            resolver.RegisterAssembly(assembly);
        }

        /// <summary>
        /// Registers an installer.
        /// </summary>
        /// <param name="installer">
        /// The installer.
        /// </param>
        public void RegisterInstaller(object installer)
        {
            resolver.RegisterInstaller(installer);
        }

        /// <summary>
        /// Untrack the referenced resolved object.
        /// </summary>
        /// <param name="reference">
        /// The resolved object.
        /// </param>
        public void Release(object reference)
        {
            resolver.Release(reference);
        }
    }
}