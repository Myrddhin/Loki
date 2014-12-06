using System;

namespace Loki.UI
{
    /// <summary>
    /// Denotes an instance which requires activation.
    /// </summary>
    public interface IActivable
    {
        /// <summary>
        /// Raised after activation occurs.
        /// </summary>
        event EventHandler<ActivationEventArgs> Activated;

        /// <summary>
        /// Gets a value indicating whether whether or not this instance is active.
        /// </summary>
        bool IsActive { get; }

        /// <summary>
        /// Activates this instance.
        /// </summary>
        void Activate();
    }
}