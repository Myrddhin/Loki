using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

using Loki.Common.Configuration;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common.Installers
{
    public class InfrastructureInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(Component.For<IDiagnostics>().ImplementedBy<Diagnostics.Diagnostics>().IsFallback());
            container.Register(Component.For<IMessageBus>().ImplementedBy<MessageBus>().IsFallback());
            container.Register(Component.For<IConfiguration>().ImplementedBy<AppSettingsConfiguration>().IsFallback());
            container.Register(Component.For<IInfrastructure>().ImplementedBy<Infrastructure>().IsFallback());
        }
    }
}