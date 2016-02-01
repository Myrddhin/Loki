using System.Threading.Tasks;

using Loki.UI;

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
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        /// <param name="message">
        /// The message instance.
        /// </param>
        public static void PublishOnCurrentThread(this IMessageComponent eventAggregator, object message)
        {
            eventAggregator.Publish(message, action => action());
        }

        /// <summary>
        /// Publishes a message on a background thread (async).
        /// </summary>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        /// <param name="message">
        /// The message instance.
        /// </param>
        public static void PublishOnBackgroundThread(this IMessageComponent eventAggregator, object message)
        {
            eventAggregator.Publish(message, action => Task.Factory.StartNew(action));
        }

        /// <summary>
        /// Publishes a message on the UI thread.
        /// </summary>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        /// <param name="thread">
        /// The threading component.
        /// </param>
        /// <param name="message">
        /// The message instance.
        /// </param>
        public static void PublishOnUIThread(this IMessageComponent eventAggregator, IThreadingContext thread, object message)
        {
            eventAggregator.Publish(message, thread.OnUIThread);
        }

        /// <summary>
        /// Publishes a message on the UI thread async.
        /// </summary>
        /// <param name="eventAggregator">
        /// The event aggregator.
        /// </param>
        /// <param name="thread">
        /// The threading context.
        /// </param>
        /// <param name="message">
        /// The message instance.
        /// </param>
        /// <returns>
        /// The created task.
        /// </returns>
        public static Task PublishOnUIThreadAsync(this IMessageComponent eventAggregator, IThreadingContext thread, object message)
        {
            Task task = null;
            eventAggregator.Publish(message, action => task = thread.OnUIThreadAsync(action));
            return task;
        }
    }
}