using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    internal class CoreServices : ICoreServices
    {
        public CoreServices(IDiagnostics logger,  IMessageBus message, IEventComponent events)
        {
            Diagnostics = logger;
            Messages = message;
            Events = events;
        }

        public IDiagnostics Diagnostics { get; }

        public IMessageBus Messages { get; }

        public IEventComponent Events { get; }
    }
}