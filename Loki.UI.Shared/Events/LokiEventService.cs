using System;
using System.Collections.Specialized;
using System.ComponentModel;

using Loki.Common.Diagnostics;
using Loki.UI.Commands;

namespace Loki.Common
{
    /// <summary>
    /// Loki change management service.
    /// </summary>
    public class LokiEventService : BaseObject, IEventComponent
    {
       // private readonly WeakEventManager<ICommand, EventArgs> canExecuteChangedManager;

        private readonly WeakEventManager<ICentralizedChangeTracking, EventArgs> centralizedChangeManager;

        private readonly WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> changedManager;

        private readonly WeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs> changingManager;

        private readonly WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs> collectionChangedManager;

        private readonly WeakNotifyPropertyManager<INotifyPropertyChanged, PropertyChangedEventArgs> notifyPropertyChangedManager;

        private readonly WeakNotifyPropertyManager<INotifyPropertyChanging, PropertyChangingEventArgs> notifyPropertyChangingManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="LokiEventService"/> class.
        /// </summary>
        /// <param name="loggerComponent">
        /// The logger Component.
        /// </param>
        /// <param name="errorComponent">
        /// The error Component.
        /// </param>
        public LokiEventService(IDiagnostics loggerComponent)
            : base(loggerComponent)
        {
            changingManager = new WeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs>(
                loggerComponent,
                (s, b) => s.PropertyChanging += b.OnEvent,
                (s, b) => s.PropertyChanging -= b.OnEvent);
            changedManager = new WeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs>(
                loggerComponent,
  
                (s, b) => s.PropertyChanged += b.OnEvent,
                (s, b) => s.PropertyChanged -= b.OnEvent);
            centralizedChangeManager = new WeakEventManager<ICentralizedChangeTracking, EventArgs>(
                loggerComponent,
       
                (s, b) => s.StateChanged += b.OnEvent,
                (s, b) => s.StateChanged -= b.OnEvent);
            collectionChangedManager =
                new WeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs>(
                    loggerComponent,
           
                    (s, b) => s.CollectionChanged += b.OnEvent,
                    (s, b) => s.CollectionChanged -= b.OnEvent);
            //canExecuteChangedManager = new WeakEventManager<ICommand, EventArgs>(
            //    loggerComponent,
          
            //    (s, b) => s.CanExecuteChanged += b.OnEvent,
            //    (s, b) => s.CanExecuteChanged -= b.OnEvent);

            notifyPropertyChangedManager =
                new WeakNotifyPropertyManager<INotifyPropertyChanged, PropertyChangedEventArgs>(
                    loggerComponent,
             
                    e => e.PropertyName,
                    (s, b) => s.PropertyChanged += b.OnProperty,
                    (s, b) => s.PropertyChanged -= b.OnProperty);
            notifyPropertyChangingManager =
                new WeakNotifyPropertyManager<INotifyPropertyChanging, PropertyChangingEventArgs>(
                    loggerComponent,
             
                    e => e.PropertyName,
                    (s, b) => s.PropertyChanging += b.OnProperty,
                    (s, b) => s.PropertyChanging -= b.OnProperty);
        }

        //public IWeakEventManager<ICommand, EventArgs> CanExecuteChanged
        //{
        //    get
        //    {
        //        return canExecuteChangedManager;
        //    }
        //}

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
            //canExecuteChangedManager.RemoveCollectedEntries();
        }
    }
}