using Loki.Common.Configuration;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;
using Loki.UI.Commands;

namespace Loki.UI
{
    internal class DisplayInfrastructure : IDisplayInfrastructure
    {
        public DisplayInfrastructure(
            IMessageBus bus,
            IDiagnostics diagnostics,
            IConfiguration configuration,
            ICommandManager commands)
        {
            MessageBus = bus;
            Diagnostics = diagnostics;
            Configuration = configuration;
        }

        public IMessageBus MessageBus { get; }

        public IDiagnostics Diagnostics { get; }

        public IConfiguration Configuration { get; }

        public ICommandManager CommandsManager { get; }
    }
}