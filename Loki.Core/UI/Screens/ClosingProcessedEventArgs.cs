using System;

namespace Loki.UI
{
    /// <summary>
    /// Contains details about the success or failure of an item's activation through an <see cref="IConductor"/>.
    /// </summary>
    public class ClosingProcessedEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Gets or sets the item whose activation was processed.
        /// </summary>
        /// <value>
        /// The item.
        /// </value>
        public T Item { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activation was a success.
        /// </summary>
        /// <value>Is <c>true</c> if success; otherwise, <c>false</c>.</value>
        public bool Success { get; set; }
    }
}