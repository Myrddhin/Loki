using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    internal class Infrastructure : IInfrastrucure
    {
        public Infrastructure(
            IMessageBus bus,
            IDiagnostics diagnostics)
        {
            MessageBus = bus;
            Diagnostics = diagnostics;
        }

        public IMessageBus MessageBus { get; }

        public IDiagnostics Diagnostics { get; }
    }
}