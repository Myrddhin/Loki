using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace Loki.Common.Installers
{
    public class InfrastructureInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IMessageBus>().ImplementedBy<MessageBus>().IsFallback());
            container.Register(Component.For<IInfrastrucure>().ImplementedBy<Infrastructure>().IsFallback());
        }
    }
}