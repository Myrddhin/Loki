using Loki.Common.Configuration;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    public class TestInfrastructure : IInfrastrucure
    {
        public TestInfrastructure()
        {
            MessageBus = new TestMessageBus();
            Diagnostics = new TestDiagnostics();
            Configuration = new TestConfiguration();
        }

        public TestMessageBus MessageBus { get; }

       IMessageBus IInfrastrucure.MessageBus => this.MessageBus;

        public IDiagnostics Diagnostics { get; }

        public IConfiguration Configuration { get; }
    }
}