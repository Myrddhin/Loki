using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Loki.Core.Tests.IoC
{
    public class DummyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<DummyClass>());
            container.Register(Component.For<DummyClass>().Named("Instance"));
            container.Register(Component.For<DummyTransient>().LifestyleTransient());
            container.Register(Component.For<DummyDisposable>().LifestyleTransient());
            container.Register(Component.For<DummyDependant>().LifestyleTransient());
            container.Register(Component.For<DummyInitializable>());
            container.Register(Component.For<DummyInitializable>().Named("Instance2"));
        }
    }
}