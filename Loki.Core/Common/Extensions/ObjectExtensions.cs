using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq.Expressions;
using Loki.Common;

namespace System
{
    public static class ObjectExtensions
    {
        #region Event Helpers

        public static void WatchCollectionChange<TObserved, TObserver>(this TObserver listener, TObserved source, Func<TObserver, EventHandler<NotifyCollectionChangedEventArgs>> handler) where TObserved : INotifyCollectionChanged
        {
            Toolkit.Common.Events.CollectionChanged.Register(source, listener, (v, o, e) => handler(v)(o, e));
        }

        public static void WatchPropertyChanged<TObserved, TObserver>(this TObserver listener, TObserved source, Expression<Func<TObserved, object>> property, Func<TObserver, EventHandler<PropertyChangedEventArgs>> handler) where TObserved : INotifyPropertyChanged
        {
            Toolkit.Common.Events.PropertyChanged.Register(source, ExpressionHelper.GetProperty(property).Name, listener, (v, o, e) => handler(v)(o, e));
        }

        public static void UnWatchPropertyChanged<TObserved, TObserver>(this TObserver listener, TObserved source, Expression<Func<TObserved, object>> property) where TObserved : INotifyPropertyChanged
        {
            Toolkit.Common.Events.PropertyChanged.Unregister(source, ExpressionHelper.GetProperty(property).Name, listener);
        }

        public static void WatchPropertyChanging<TObserved, TObserver>(this TObserver listener, TObserved source, Expression<Func<TObserved, object>> property, Func<TObserver, EventHandler<PropertyChangingEventArgs>> handler) where TObserved : INotifyPropertyChanging
        {
            Toolkit.Common.Events.PropertyChanging.Register(source, ExpressionHelper.GetProperty(property).Name, listener, (v, o, e) => handler(v)(o, e));
        }

        public static void WatchStateChanged<TObserved, TObserver>(this TObserver listener, TObserved source, Func<TObserver, EventHandler> handler) where TObserved : ICentralizedChangeTracking
        {
            Toolkit.Common.Events.StateChanged.Register(source, listener, (v, o, e) => handler(v)(o, e));
        }

        #endregion Event Helpers

        public static void SafeDispose(this object potentialDisposable)
        {
            var disposable = potentialDisposable as IDisposable;
            if (disposable != null)
            {
                disposable.Dispose();
            }
        }

        public static string SafeToString(this object potentialString)
        {
            if (potentialString == null)
            {
                return string.Empty;
            }
            else
            {
                return potentialString.ToString();
            }
        }
    }
}