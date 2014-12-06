using System;

namespace Loki.Common
{
    public class WeakNotifyPropertyManager<TEventInterface, TEventArgs> : IWeakEventPropertyManager<TEventInterface, TEventArgs>
        where TEventInterface : class
        where TEventArgs : EventArgs
    {
        private Func<TEventArgs, string> nameGetter;
        private Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> subscribeCallback;
        private Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> unsubscribeCallback;

        public WeakNotifyPropertyManager(
            Func<TEventArgs, string> propertyNameGetter,
            Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> subscribeMapper,
            Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> unsubscribeMapper)
        {
            nameGetter = propertyNameGetter;
            unsubscribeCallback = unsubscribeMapper;
            subscribeCallback = subscribeMapper;

            sourceToBridgeTable = new WeakDictionary<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>>();
        }

        private WeakDictionary<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> sourceToBridgeTable;

        public void Register<TListener>(TEventInterface source, string propertyName, TListener listener, Action<TListener, object, TEventArgs> propertyChangedCallback)
        {
            WeakNotifyPropertyBridge<TEventInterface, TEventArgs> bridge;
            bridge = GetBridgeForSource(source);

            bridge.AddListener(propertyName, listener, propertyChangedCallback);
        }

        private WeakNotifyPropertyBridge<TEventInterface, TEventArgs> GetBridgeForSource(TEventInterface source)
        {
            WeakNotifyPropertyBridge<TEventInterface, TEventArgs> bridge;

            if (!sourceToBridgeTable.TryGetValue(source, out bridge))
            {
                bridge = AddNewPropertyBridgeToTable(source);
            }

            // Can happen if the GC does it's magic
            if (bridge == null)
            {
                bridge = AddNewPropertyBridgeToTable(source);
            }

            return bridge;
        }

        private WeakNotifyPropertyBridge<TEventInterface, TEventArgs> AddNewPropertyBridgeToTable(
            TEventInterface source)
        {
            WeakNotifyPropertyBridge<TEventInterface, TEventArgs> bridge = new WeakNotifyPropertyBridge<TEventInterface, TEventArgs>(this, source, nameGetter, subscribeCallback, unsubscribeCallback);
            sourceToBridgeTable[source] = bridge;
            return bridge;
        }

        public void Unregister(TEventInterface source, string propertyName, object listener)
        {
            WeakNotifyPropertyBridge<TEventInterface, TEventArgs> bridge;

            if (!sourceToBridgeTable.TryGetValue(source, out bridge))
            {
                return;
            }

            if (bridge == null)
            {
                sourceToBridgeTable.Remove(source);
                return;
            }

            bridge.RemoveListener(listener, propertyName);
        }

        public void UnregisterSource(TEventInterface source)
        {
            sourceToBridgeTable.Remove(source);
        }

        public void RemoveCollectedEntries()
        {
            sourceToBridgeTable.RemoveCollectedEntries();
        }
    }
}