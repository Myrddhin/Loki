using System;

using Loki.Common.Diagnostics;

namespace Loki.Common
{
    internal class WeakEventManager<TEventClass, TEventArgs> : BaseObject, IWeakEventManager<TEventClass, TEventArgs>
        where TEventClass : class
        where TEventArgs : EventArgs
    {
        private readonly WeakDictionary<TEventClass, WeakEventBridge<TEventClass, TEventArgs>> sourceToBridgeTable;

        private readonly Action<TEventClass, WeakEventBridge<TEventClass, TEventArgs>> eventMapper;

        private readonly Action<TEventClass, WeakEventBridge<TEventClass, TEventArgs>> eventUnmapper;

        public WeakEventManager(
            IDiagnostics loggerComponent,
            Action<TEventClass, WeakEventBridge<TEventClass, TEventArgs>> mapper,
            Action<TEventClass, WeakEventBridge<TEventClass, TEventArgs>> unmapper)
            : base(loggerComponent)
        {
            sourceToBridgeTable = new WeakDictionary<TEventClass, WeakEventBridge<TEventClass, TEventArgs>>(loggerComponent);
            eventMapper = mapper;
            eventUnmapper = unmapper;
        }

        /// <summary>
        /// Registers the specified source.
        /// </summary>
        /// <typeparam name="TListener">The type of the listener.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="callback">The callback.</param>
        public void Register<TListener>(TEventClass source, TListener listener, Action<TListener, object, TEventArgs> callback)
        {
            WeakEventBridge<TEventClass, TEventArgs> bridge = GetBridgeForSource(source);

            bridge.AddListener(listener, callback);
        }

        private WeakEventBridge<TEventClass, TEventArgs> GetBridgeForSource(TEventClass source)
        {
            WeakEventBridge<TEventClass, TEventArgs> bridge;

            if (!sourceToBridgeTable.TryGetValue(source, out bridge))
            {
                bridge = AddNewPropertyBridgeToTable(source);
            }

            // Can happen if the GC does it's magic
            return bridge ?? this.AddNewPropertyBridgeToTable(source);
        }

        private WeakEventBridge<TEventClass, TEventArgs> AddNewPropertyBridgeToTable(
            TEventClass source)
        {
            WeakEventBridge<TEventClass, TEventArgs> bridge = new WeakEventBridge<TEventClass, TEventArgs>(this, source, eventMapper, eventUnmapper);
            sourceToBridgeTable[source] = bridge;
            return bridge;
        }

        /// <summary>
        /// Unregisters the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        public void Unregister(TEventClass source, object listener)
        {
            WeakEventBridge<TEventClass, TEventArgs> bridge;

            if (!sourceToBridgeTable.TryGetValue(source, out bridge))
            {
                return;
            }

            if (bridge == null)
            {
                sourceToBridgeTable.Remove(source);
                return;
            }

            bridge.RemoveListener(listener);
        }

        /// <summary>
        /// Unregisters the source.
        /// </summary>
        /// <param name="source">The source.</param>
        public void UnregisterSource(TEventClass source)
        {
            WeakEventBridge<TEventClass, TEventArgs> bridge;

            if (!sourceToBridgeTable.TryGetValue(source, out bridge))
            {
                return;
            }

            if (bridge == null)
            {
                sourceToBridgeTable.Remove(source);
                return;
            }

            eventUnmapper(source, bridge);
        }

        /// <summary>
        /// Removes the collected entries.
        /// </summary>
        public void RemoveCollectedEntries()
        {
            sourceToBridgeTable.RemoveCollectedEntries();
        }
    }
}