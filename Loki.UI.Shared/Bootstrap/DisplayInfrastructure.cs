using Loki.Common.Configuration;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.UI
{
    internal class DisplayInfrastructure : IDisplayInfrastructure
    {
        public DisplayInfrastructure(
            IMessageBus bus,
            IDiagnostics diagnostics,
            IConfiguration configuration)
        {
            MessageBus = bus;
            Diagnostics = diagnostics;
            Configuration = configuration;
        }

        public IMessageBus MessageBus { get; }

        public IDiagnostics Diagnostics { get; }

        public IConfiguration Configuration { get; }
    }
}