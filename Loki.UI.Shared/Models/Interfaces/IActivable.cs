using System;
using System.ComponentModel;

namespace Loki.UI.Models
{
    /// <summary>
    /// Denotes an instance which requires activation.
    /// </summary>
    public interface IActivable : INotifyPropertyChanged
    {
        /// <summary>
        /// Raised before activation occurs.
        /// </summary>
        event EventHandler Activating;

        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        event EventHandler Activated;

        /// <summary>
        /// Gets a value indicating whether whether or not this instance is active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        void Activate();

        /// <summary>
        /// Raised before deactivation.
        /// </summary>
        event EventHandler Desactivating;

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        void Desactivate();

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        event EventHandler Desactivated;
    }
}