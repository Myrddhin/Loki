using Loki.Common;

namespace Loki.Common.Tests
{
    public class SimpleMessageListener : IHandle<SimpleMessage>
    {
        public bool Received { get; set; }

        public void Handle(SimpleMessage message)
        {
            Received = true;
        }
    }
}