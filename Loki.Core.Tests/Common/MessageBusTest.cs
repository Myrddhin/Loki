using Loki.Common;

using Xunit;

namespace Loki.Common.Tests
{
    [Trait("Category", "Message bus")]
    public class MessageBusTest : CommonTest
    {
        public MessageBusTest()
        {
            Component = Context.Resolve<IMessageBus>();
        }

        public IMessageBus Component { get; private set; }

        [Fact(DisplayName = "Simple message reception")]
        public void TestSimpleMessage()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.True(receiver.Received);
        }

        [Fact(DisplayName = "Multiple receivers")]
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

        [Fact(DisplayName = "No reception before subscribing")]
        public void NoReceptionBeforeSubscribe()
        {
            var receiver = new SimpleMessageListener();

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.False(receiver.Received);
        }

        [Fact(DisplayName = "No reception after unsubscribing")]
        public void NoReceptionAfterUnsubscribe()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);
            Component.Unsubscribe(receiver);

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.False(receiver.Received);
        }

        [Fact(DisplayName = "Handler detection")]
        public void HandlerCheck()
        {
            var receiver = new SimpleMessageListener();
            Component.Subscribe(receiver);
            Assert.True(Component.HandlerExistsFor(typeof(SimpleMessage)));
        }
    }
}