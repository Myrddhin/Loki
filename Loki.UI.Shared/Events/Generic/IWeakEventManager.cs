﻿using System;

namespace Loki.Common
{
    public interface IWeakEventManager<TEventClass, TEventArgs>
        where TEventClass : class
        where TEventArgs : EventArgs
    {
        /// <summary>
        /// Registers the specified source.
        /// </summary>
        /// <typeparam name="TListener">The type of the listener.</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        /// <param name="callback">The callback.</param>
        void Register<TListener>(TEventClass source, TListener listener, Action<TListener, object, TEventArgs> callback);

        /// <summary>
        /// Unregisters the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="listener">The listener.</param>
        void Unregister(TEventClass source, object listener);

        /// <summary>
        /// Unregisters the source.
        /// </summary>
        /// <param name="source">The source.</param>
        void UnregisterSource(TEventClass source);
    }
}