using Loki.Common;

namespace Loki.Common.Messages
{
    public class SimpleMessageListener : IHandle<SimpleMessage>
    {
        public bool Received { get; set; }

        public void Handle(SimpleMessage message)
        {
            Received = true;
            ReceiveCount++;
        }

        public int ReceiveCount { get; set; }
    }
}