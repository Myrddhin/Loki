using System;
using System.Collections.Generic;
using System.Text;

using Loki.Common.Messages;

namespace Loki.Common.Messages
{
    /// <summary>
    /// Extensions for <see cref="IMessageBus"/>.
    /// </summary>
    public static class MessageBusExtensions
    {
        /// <summary>
        /// Publishes a message on the current thread (synchronized).
        /// </summary>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        /// <param name="message">
        /// The message instance.
        /// </param>
        public static void PublishOnCurrentThread(this IMessageBus eventAggregator, object message)
        {
            eventAggregator.Publish(message, action => action());
        }
    }
}