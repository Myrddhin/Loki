using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    public interface IInfrastrucure
    {
        IMessageBus MessageBus { get; }

        IDiagnostics Diagnostics { get; }
    }
}