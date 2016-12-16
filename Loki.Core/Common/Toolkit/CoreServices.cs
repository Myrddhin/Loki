using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    internal class CoreServices : ICoreServices
    {
        public CoreServices(IDiagnostics logger,  IMessageBus message)
        {
            Diagnostics = logger;
            Messages = message;
        }

        public IDiagnostics Diagnostics { get; }

        public IMessageBus Messages { get; }
    }
}