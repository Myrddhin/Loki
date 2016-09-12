using Loki.Common.Configuration;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    public interface IInfrastructure
    {
        IMessageBus MessageBus { get; }

        IDiagnostics Diagnostics { get; }

        IConfiguration Configuration { get; }
    }
}