using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Loki.Common
{
    public static class EventExtensions
    {
        public static void WatchCollectionChange<TObserved, TObserver>(this IEventComponent events, TObserver listener, TObserved source, Func<TObserver, EventHandler<NotifyCollectionChangedEventArgs>> handler) where TObserved : INotifyCollectionChanged
        {
            events.CollectionChanged.Register(source, listener, (v, o, e) => handler(v)(o, e));
        }

        public static void WatchPropertyChanged<TObserved, TObserver>(this IEventComponent events, TObserver listener, TObserved source, Expression<Func<TObserved, object>> property, Func<TObserver, EventHandler<PropertyChangedEventArgs>> handler) where TObserved : INotifyPropertyChanged
        {
            events.PropertyChanged.Register(source, ExpressionHelper.GetProperty(property).Name, listener, (v, o, e) => handler(v)(o, e));
        }

        public static void UnWatchPropertyChanged<TObserved, TObserver>(this IEventComponent events, TObserver listener, TObserved source, Expression<Func<TObserved, object>> property) where TObserved : INotifyPropertyChanged
        {
            events.PropertyChanged.Unregister(source, ExpressionHelper.GetProperty(property).Name, listener);
        }

        public static void WatchPropertyChanging<TObserved, TObserver>(this IEventComponent events, TObserver listener, TObserved source, Expression<Func<TObserved, object>> property, Func<TObserver, EventHandler<PropertyChangingEventArgs>> handler) where TObserved : INotifyPropertyChanging
        {
            events.PropertyChanging.Register(source, ExpressionHelper.GetProperty(property).Name, listener, (v, o, e) => handler(v)(o, e));
        }

        public static void WatchStateChanged<TObserved, TObserver>(this IEventComponent events, TObserver listener, TObserved source, Func<TObserver, EventHandler> handler) where TObserved : ICentralizedChangeTracking
        {
            events.StateChanged.Register(source, listener, (v, o, e) => handler(v)(o, e));
        }
    }
}