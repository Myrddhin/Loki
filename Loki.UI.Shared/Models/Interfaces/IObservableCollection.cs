using System;
using System.Collections.Generic;

namespace Loki.UI.Models
{
    /// <summary>
    /// Represents a collection that is observable.
    /// </summary>
    /// <typeparam name = "T">The type of elements contained in the collection.</typeparam>
    public interface IObservableCollection<T> : IList<T>, IObservableEnumerable
    {
        /// <summary>
        /// Occurs when an item is changed.
        /// </summary>
        event EventHandler<ItemChangedEventArgs<T>> ItemChanged;

        /// <summary>
        ///   Adds the range to the collection.
        /// </summary>
        /// <param name = "items">The items.</param>
        void AddRange(IEnumerable<T> items);

        /// <summary>
        ///   Removes the range from the collection.
        /// </summary>
        /// <param name = "items">The items.</param>
        void RemoveRange(IEnumerable<T> items);
    }
}