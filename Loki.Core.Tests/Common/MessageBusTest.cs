using Loki.Common;

using Xunit;

namespace Loki.Core.Tests.Common
{
    public class MessageBusTest : CommonTest
    {
        public MessageBusTest()
        {
            Component = Context.Resolve<IMessageBus>();
        }

        public IMessageBus Component { get; private set; }

        [Fact]
        public void TestSimpleMessage()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.True(receiver.Received);
        }

        [Fact]
        public void TestBroadcastMessage()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);

            var receiver2 = new SimpleMessageListener();
            Component.Subscribe(receiver2);

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.True(receiver.Received);
            Assert.True(receiver2.Received);
        }

        [Fact]
        public void NoReceptionBeforeSubscribe()
        {
            var receiver = new SimpleMessageListener();

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.False(receiver.Received);
        }

        [Fact]
        public void NoReceptionAfterUnsubscribe()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);
            Component.Unsubscribe(receiver);

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.False(receiver.Received);
        }

        [Fact]
        public void HandlerCheck()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);
            Assert.True(Component.HandlerExistsFor(typeof(SimpleMessage)));
        }
    }
}