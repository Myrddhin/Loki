using Loki.Common.Diagnostics;

namespace Loki.Common
{
    public interface ICoreServices
    {
        IDiagnostics Diagnostics { get; }

        IMessageBus Messages { get; }

        IEventComponent Events { get; }
    }
}