using System;
using System.Collections.Specialized;
using System.ComponentModel;
using Loki.Commands;

namespace Loki.Common
{
    /// <summary>
    /// Interface for change manager service.
    /// </summary>
    public interface IEventComponent
    {
        /// <summary>
        /// Gets the changing manager.
        /// </summary>
        /// <value>
        /// The changing manager.
        /// </value>
        IWeakEventManager<INotifyPropertyChanging, PropertyChangingEventArgs> Changing { get; }

        IWeakEventManager<INotifyCanExecuteChanged, EventArgs> CanExecuteChanged { get; }

        IWeakEventManager<INotifyCollectionChanged, NotifyCollectionChangedEventArgs> CollectionChanged { get; }

        IWeakEventManager<ICentralizedChangeTracking, EventArgs> StateChanged { get; }

        IWeakEventManager<INotifyPropertyChanged, PropertyChangedEventArgs> Changed { get; }

        IWeakEventPropertyManager<INotifyPropertyChanged, PropertyChangedEventArgs> PropertyChanged { get; }

        IWeakEventPropertyManager<INotifyPropertyChanging, PropertyChangingEventArgs> PropertyChanging { get; }

        /// <summary>
        /// Removes the collected entries.
        /// </summary>
        void RemoveCollectedEntries();
    }
}