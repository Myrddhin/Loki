using System;
using System.Threading.Tasks;

using Loki.Common.Tests;

using Xunit;

namespace Loki.Common.Messages
{
    [Trait("Category", "Message bus")]
    public class MessageBusTest : CommonTest
    {
        public MessageBusTest()
        {
            Component = Context.Resolve<IMessageBus>();
        }

        public IMessageBus Component { get; private set; }

        [Fact(DisplayName = "Multi registration : only one effective")]
        public void SubscribeWithNoHandlers()
        {
            var multi = new SimpleMessageListener();
            Component.Subscribe(multi);
            Component.Subscribe(multi);

            Component.PublishOnCurrentThread(new SimpleMessage());

            Assert.Equal(1, multi.ReceiveCount);
        }

        [Fact(DisplayName = "Dead handlers subscribing")]
        public void DeadReceiver()
        {
            WeakReference<SimpleMessageListener> reference = null;
            var task = Task.Factory.StartNew(
                () =>
                    {
                        var receiver = new SimpleMessageListener();
                        reference = new WeakReference<SimpleMessageListener>(receiver);
                        Component.Subscribe(receiver);
                    });

            Task.WaitAll(task);
            GC.Collect();
            SimpleMessageListener buffer;
            Assert.False(reference.TryGetTarget(out buffer));

            Component.Publish(new object(), a => a());
        }

        [Fact(DisplayName = "Publish with argument null")]
        public void PublishNullProtecttion()
        {
            Assert.Throws<ArgumentNullException>(() => Component.Publish(new object(), null));
            Assert.Throws<ArgumentNullException>(() => Component.Publish(null, a => { }));
        }

        [Fact(DisplayName = "Subscribe with argument null")]
        public void SubscribeNullProtection()
        {
            Assert.Throws<ArgumentNullException>(() => Component.Subscribe(null));
        }

        [Fact(DisplayName = "UnSubscribe with argument null")]
        public void UnsubscribeNullProtection()
        {
            Assert.Throws<ArgumentNullException>(() => Component.Unsubscribe(null));
        }

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