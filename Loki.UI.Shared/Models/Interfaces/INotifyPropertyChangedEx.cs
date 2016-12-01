using System.ComponentModel;

namespace Loki.UI.Models
{
    /// <summary>
    /// Extends <see cref = "INotifyPropertyChanged" /> such that the change event can be raised by external parties.
    /// </summary>
    public interface INotifyPropertyChangedEx : INotifyPropertyChanged
    {
        /// <summary>
        /// Gets or sets a value indicating whether the tracking is active.
        /// </summary>
        bool Tracking { get; set; }

        /// <summary>
        /// Notifies subscribers of the property change.
        /// </summary>
        /// <param name = "propertyArgs">Name of the property.</param>
        void NotifyChanged(PropertyChangedEventArgs propertyArgs);

        /// <summary>
        /// Raises a change notification indicating that all bindings should be refreshed.
        /// </summary>
        void Refresh();
    }
}