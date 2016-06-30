using System;
using System.Diagnostics;
using System.Reflection;

using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;

namespace Loki.Common.IoC
{
    public sealed class CastleWindsorDependencyResolver : IDependencyResolver
    {
        private readonly WindsorContainer internalContainer;

        public CastleWindsorDependencyResolver()
        {
            internalContainer = new WindsorContainer();
        }

        private bool disposed;

        public void Dispose()
        {
            if (!disposed)
            {
                internalContainer.Dispose();
            }

            disposed = true;
        }

        public void Release(object reference)
        {
            internalContainer.Release(reference);
        }

        public void OverrideInfrastructureService<T>(Type type) where T : class
        {
            internalContainer.Register(Component.For<T>().ImplementedBy(type).LifestyleSingleton().IsDefault());
        }

        public void OverrideInfrastructureInstance<T>(T instance) where T : class
        {
            internalContainer.Register(Component.For<T>().Instance(instance).LifestyleSingleton().IsDefault());
        }

        public T Resolve<T>()
        {
            return internalContainer.Resolve<T>();
        }

        public T Resolve<T>(string name)
        {
            return internalContainer.Resolve<T>(name);
        }

        public void RegisterAssembly(Assembly assembly)
        {
            internalContainer.Install(FromAssembly.Instance(assembly));
        }

        public void RegisterInstaller(object installer)
        {
            var windInstaller = installer as IWindsorInstaller;
            Debug.Assert(windInstaller != null, "Wrong installer type");

            internalContainer.Install(windInstaller);
        }
    }
}