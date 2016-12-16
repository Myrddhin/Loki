using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    public interface ICoreServices
    {
        IDiagnostics Diagnostics { get; }

        IMessageBus Messages { get; }
    }
}