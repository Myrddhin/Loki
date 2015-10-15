using System;

namespace Loki.Common
{
    public class WeakNotifyPropertyManager<TEventInterface, TEventArgs> : BaseObject, IWeakEventPropertyManager<TEventInterface, TEventArgs>
        where TEventInterface : class
        where TEventArgs : EventArgs
    {
        private readonly Func<TEventArgs, string> nameGetter;
        private readonly Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> subscribeCallback;
        private readonly Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> unsubscribeCallback;
        private readonly WeakDictionary<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> sourceToBridgeTable;

        public WeakNotifyPropertyManager(
            ILoggerComponent loggerComponent,
            IErrorComponent errorComponent,
            Func<TEventArgs, string> propertyNameGetter,
            Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> subscribeMapper,
            Action<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>> unsubscribeMapper)
            : base(loggerComponent, errorComponent)
        {
            nameGetter = propertyNameGetter;
            unsubscribeCallback = unsubscribeMapper;
            subscribeCallback = subscribeMapper;

            sourceToBridgeTable = new WeakDictionary<TEventInterface, WeakNotifyPropertyBridge<TEventInterface, TEventArgs>>(loggerComponent, errorComponent);
        }

        public void Register<TListener>(TEventInterface source, string propertyName, TListener listener, Action<TListener, object, TEventArgs> propertyChangedCallback)
        {
            var bridge = this.GetBridgeForSource(source);

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
            return bridge ?? this.AddNewPropertyBridgeToTable(source);
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