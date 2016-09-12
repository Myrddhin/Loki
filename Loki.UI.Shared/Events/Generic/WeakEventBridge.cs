using System;

namespace Loki.Common
{
    internal class WeakEventBridge<TEventInterface, TEventArgs>
        where TEventInterface : class
        where TEventArgs : EventArgs
    {
        private readonly TEventInterface eventSource;

        private readonly IWeakEventManager<TEventInterface, TEventArgs> eventManager;

        private readonly ConcurrentCollection<IWeakEventCallback<TEventArgs>> eventCallbacks;

        private readonly Action<TEventInterface, WeakEventBridge<TEventInterface, TEventArgs>> unsubscribeCallback;

        /// <summary>
        /// Gets a value indicating whether the event source has active listeners.
        /// </summary>
        /// <value>
        ///     Is <c>true</c> if the event source has active listeners; otherwise, <c>false</c>.
        /// </value>
        public bool HasActiveListeners
        {
            get { return !eventCallbacks.IsEmpty; }
        }

        public WeakEventBridge(
            IWeakEventManager<TEventInterface, TEventArgs> manager,
            TEventInterface source,
            Action<TEventInterface, WeakEventBridge<TEventInterface, TEventArgs>> subscribeMapper,
            Action<TEventInterface, WeakEventBridge<TEventInterface, TEventArgs>> unsubscribeMapper)
        {
            eventManager = manager;
            eventSource = source;
            eventCallbacks = new ConcurrentCollection<IWeakEventCallback<TEventArgs>>();
            subscribeMapper(eventSource, this);

            unsubscribeCallback = unsubscribeMapper;
        }

        /// <summary>
        /// Adds the listener.
        /// </summary>
        /// <typeparam name="TListener">The type of the listener.</typeparam>
        /// <param name="listener">The listener.</param>
        /// <param name="eventCallback">The state changed callback.</param>
        public void AddListener<TListener>(
            TListener listener,
            Action<TListener, object, TEventArgs> eventCallback)
        {
            var callback = new WeakEventCallback<TListener, TEventArgs>(listener, eventCallback);
            eventCallbacks.Add(callback);
        }

        /// <summary>
        /// Removes the listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        public void RemoveListener(object listener)
        {
            foreach (var node in eventCallbacks)
            {
                IWeakEventCallback<TEventArgs> callback = node.Value;
                object listenerFoCallback = callback.Listener;

                if (listenerFoCallback == null)
                {
                    eventCallbacks.Remove(node);
                }
                else if (listenerFoCallback == listener)
                {
                    eventCallbacks.Remove(node);
                    break;
                }
            }

            UnsubscribeIfNoMoreListeners();
        }

        /// <summary>
        /// Called when the specified event is raised.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The event parameters.</param>
        public void OnEvent(object sender, TEventArgs e)
        {
            foreach (var node in eventCallbacks)
            {
                IWeakEventCallback<TEventArgs> callback = node.Value;

                if (!callback.Invoke(sender, e))
                {
                    eventCallbacks.Remove(node);
                }
            }
        }

        private void UnsubscribeIfNoMoreListeners()
        {
            if (!this.HasActiveListeners)
            {
                unsubscribeCallback(eventSource, this);
                eventManager.UnregisterSource(eventSource);
            }
        }
    }
}