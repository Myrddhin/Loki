using Loki.Common.Configuration;
using Loki.Common.Diagnostics;
using Loki.Common.Messages;

namespace Loki.Common
{
    public class TestInfrastructure : IInfrastructure
    {
        public TestInfrastructure()
        {
            MessageBus = new TestMessageBus();
            Diagnostics = new TestDiagnostics();
            Configuration = new TestConfiguration();
        }

        public TestMessageBus MessageBus { get; }

       IMessageBus IInfrastructure.MessageBus => this.MessageBus;

        public IDiagnostics Diagnostics { get; }

        public IConfiguration Configuration { get; }
    }
}