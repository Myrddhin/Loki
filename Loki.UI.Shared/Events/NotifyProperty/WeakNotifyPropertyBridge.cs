using System;
using System.Collections.Concurrent;

namespace Loki.Common
{
    public class WeakNotifyPropertyBridge<TEventInterface, TEventArgs>
        where TEventInterface : class
        where TEventArgs : EventArgs
    {
        private TEventInterface eventSource;
        private Func<TEventArgs, string> nameGetter;
        private ConcurrentDictionary<string, ConcurrentCollection<IWeakCallback>> propertyNameToCallbacks;
        private Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> unsubscribeCallback;
        private IWeakEventPropertyManager<TEventInterface, TEventArgs> eventManager;

        public WeakNotifyPropertyBridge(
            IWeakEventPropertyManager<TEventInterface, TEventArgs> manager,
            TEventInterface source,
            Func<TEventArgs, string> propertyNameGetter,
            Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> subscribeMapper,
            Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> unsubscribeMapper)
        {
            eventManager = manager;
            propertyNameToCallbacks = new ConcurrentDictionary<string, ConcurrentCollection<IWeakCallback>>();
            nameGetter = propertyNameGetter;
            eventSource = source;
            subscribeMapper(source, this);
            unsubscribeCallback = unsubscribeMapper;
        }

        /// <summary>
        /// Gets a value indicating whether the event source has active listeners.
        /// </summary>
        /// <value>
        ///     Is <c>true</c> if the event source has active listeners; otherwise, <c>false</c>.
        /// </value>
        public bool HasActiveListeners
        {
            get { return propertyNameToCallbacks.Count > 0; }
        }

        private ConcurrentCollection<IWeakCallback> LookupOrCreateCallbacksForProperty(string propertyName)
        {
            var callbacksForProperty = LookupCallbacksForProperty(propertyName);

            if (callbacksForProperty == null)
            {
                callbacksForProperty = new ConcurrentCollection<IWeakCallback>();
                propertyNameToCallbacks.TryAdd(propertyName, callbacksForProperty);
            }

            return callbacksForProperty;
        }

        private ConcurrentCollection<IWeakCallback> LookupCallbacksForProperty(string propertyName)
        {
            ConcurrentCollection<IWeakCallback> callbacksForProperty = null;

            if (propertyNameToCallbacks.ContainsKey(propertyName))
            {
                propertyNameToCallbacks.TryGetValue(propertyName, out callbacksForProperty);
            }

            return callbacksForProperty;
        }

        /// <summary>
        /// Removes the listener.
        /// </summary>
        /// <param name="listener">The listener.</param>
        /// <param name="propertyName">Name of the property.</param>
        public void RemoveListener(object listener, string propertyName)
        {
            var callbacksForProperty = LookupCallbacksForProperty(propertyName);
            if (callbacksForProperty != null)
            {
                foreach (var node in callbacksForProperty)
                {
                    IWeakEventCallback<TEventArgs> callback = (IWeakEventCallback<TEventArgs>)node.Value;
                    object listenerForCallback = callback.Listener;

                    if (listenerForCallback == null)
                    {
                        callbacksForProperty.Remove(node);
                    }
                    else if (listenerForCallback == listener)
                    {
                        callbacksForProperty.Remove(node);
                        break;
                    }
                }
            }

            CheckForUnsubscribe(LookupCallbacksForProperty(propertyName), propertyName);
        }

        public void OnProperty(object sender, TEventArgs e)
        {
            var callbacksForProperty = LookupCallbacksForProperty(nameGetter(e));
            if (callbacksForProperty != null)
            {
                foreach (var node in callbacksForProperty)
                {
                    IWeakEventCallback<TEventArgs> callback = (IWeakEventCallback<TEventArgs>)node.Value;

                    if (!callback.Invoke(sender, e))
                    {
                        callbacksForProperty.Remove(node);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the listener.
        /// </summary>
        /// <typeparam name="TListener">The type of the listener.</typeparam>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="propertyChangedCallback">The property changed callback.</param>
        public void AddListener<TListener>(
            string propertyName,
            TListener listener,
            Action<TListener, object, TEventArgs> propertyChangedCallback)
        {
            var callbacksForProperty = LookupOrCreateCallbacksForProperty(propertyName);
            var callback = new WeakEventCallback<TListener, TEventArgs>(listener, propertyChangedCallback);
            callbacksForProperty.Add(callback);
        }

        private void CheckForUnsubscribe(ConcurrentCollection<IWeakCallback> callbacksForProperty, string propertyName)
        {
            if (callbacksForProperty.IsEmpty)
            {
                ConcurrentCollection<IWeakCallback> oldValue = null;
                propertyNameToCallbacks.TryRemove(propertyName, out oldValue);
            }

            UnsubscribeIfNoMoreListeners();
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