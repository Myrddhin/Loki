﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace Loki.Common
{
    /// <summary>
    /// Extensions for <see cref="IMessageComponent"/>.
    /// </summary>
    public static class EventAggregatorExtensions
    {
        /// <summary>
        /// Publishes a message on the current thread (synchronized).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void PublishOnCurrentThread(this IMessageComponent eventAggregator, object message)
        {
            eventAggregator.Publish(message, action => action());
        }

        /// <summary>
        /// Publishes a message on a background thread (async).
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void PublishOnBackgroundThread(this IMessageComponent eventAggregator, object message)
        {
            eventAggregator.Publish(message, action => Task.Factory.StartNew(action));
        }

        /// <summary>
        /// Publishes a message on the UI thread.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void PublishOnUIThread(this IMessageComponent eventAggregator, object message)
        {
            eventAggregator.Publish(message, DelegateExtensions.OnUIThread);
        }

        /// <summary>
        /// Publishes a message on the UI thread async.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name = "message">The message instance.</param>
        public static void BeginPublishOnUIThread(this IMessageComponent eventAggregator, object message)
        {
            eventAggregator.Publish(message, DelegateExtensions.BeginOnUIThread);
        }

        /// <summary>
        /// Publishes a message on the UI thread async.
        /// </summary>
        /// <param name="eventAggregator">The event aggregator.</param>
        /// <param name="message">The message instance.</param>
        public static Task PublishOnUIThreadAsync(this IMessageComponent eventAggregator, object message)
        {
            Task task = null;
            eventAggregator.Publish(message, action => task = action.OnUIThreadAsync());
            return task;
        }
    }
}