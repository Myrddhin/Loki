using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Loki.Commands;

namespace Loki.Common
{
    /// <summary>
    /// Loki change management service.
    /// </summary>
    public class LokiEventService : BaseObject, IEventComponent
    {
        private WeakNotifyPropertyManager<INotifyPropertyChanging, PropertyChangingEventArgs> notifyPropertyChangingManager;
        private WeakNotifyPropertyManager<INotifyPropertyChanged, PropertyChangedEventArgs> notifyPropertyChangedManager;
        private WeakEventManager<INotifyCanExecuteChanged, EventArgs> canExecuteChangedManager;
        private WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs> collectionChangedManager;
        private WeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs> changingManager;
        private WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> changedManager;
        private WeakEventManager<ICentralizedChangeTracking, EventArgs> centralizedChangeManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiEventService"/> class.
        /// </summary>
        public LokiEventService()
        {
            changingManager = new WeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs>((s, b) => s.PropertyChanging += b.OnEvent, (s, b) => s.PropertyChanging -= b.OnEvent);
            changedManager = new WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>((s, b) => s.PropertyChanged += b.OnEvent, (s, b) => s.PropertyChanged -= b.OnEvent);
            centralizedChangeManager = new WeakEventManager<ICentralizedChangeTracking, EventArgs>((s, b) => s.StateChanged += b.OnEvent, (s, b) => s.StateChanged -= b.OnEvent);
            collectionChangedManager = new WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>((s, b) => s.CollectionChanged += b.OnEvent, (s, b) => s.CollectionChanged -= b.OnEvent);
            canExecuteChangedManager = new WeakEventManager<INotifyCanExecuteChanged, EventArgs>((s, b) => s.CanExecuteChanged += b.OnEvent, (s, b) => s.CanExecuteChanged -= b.OnEvent);

            notifyPropertyChangedManager = new WeakNotifyPropertyManager<INotifyPropertyChanged, PropertyChangedEventArgs>(e => e.PropertyName, (s, b) => s.PropertyChanged += b.OnProperty, (s, b) => s.PropertyChanged -= b.OnProperty);
            notifyPropertyChangingManager = new WeakNotifyPropertyManager<INotifyPropertyChanging, PropertyChangingEventArgs>(e => e.PropertyName, (s, b) => s.PropertyChanging += b.OnProperty, (s, b) => s.PropertyChanging -= b.OnProperty);
        }

        public IWeakEventManager<INotifyCanExecuteChanged, EventArgs> CanExecuteChanged
        {
            get
            {
                return canExecuteChangedManager;
            }
        }

        public IWeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs> CollectionChanged
        {
            get
            {
                return collectionChangedManager;
            }
        }

        public IWeakEventManager<ICentralizedChangeTracking, EventArgs> StateChanged
        {
            get
            {
                return centralizedChangeManager;
            }
        }

        public IWeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> Changed
        {
            get
            {
                return changedManager;
            }
        }

        public IWeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs> Changing
        {
            get
            {
                return changingManager;
            }
        }

        public IWeakEventPropertyManager<INotifyPropertyChanged, PropertyChangedEventArgs> PropertyChanged
        {
            get
            {
                return notifyPropertyChangedManager;
            }
        }

        public IWeakEventPropertyManager<INotifyPropertyChanging, PropertyChangingEventArgs> PropertyChanging
        {
            get
            {
                return notifyPropertyChangingManager;
            }
        }

        /// <summary>
        /// Removes the collected entries.
        /// </summary>
        public void RemoveCollectedEntries()
        {
            notifyPropertyChangedManager.RemoveCollectedEntries();
            notifyPropertyChangingManager.RemoveCollectedEntries();
            centralizedChangeManager.RemoveCollectedEntries();
            changingManager.RemoveCollectedEntries();
            changedManager.RemoveCollectedEntries();
            collectionChangedManager.RemoveCollectedEntries();
            canExecuteChangedManager.RemoveCollectedEntries();
        }
    }
}