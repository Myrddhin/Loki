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
        event EventHandler<DesactivationEventArgs> AttemptingDesactivation;

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        /// <param name="close">Indicates whether or not this instance is being closed.</param>
        void Desactivate(bool close);

        /// <summary>
        /// Raised after deactivation.
        /// </summary>
        event EventHandler<DesactivationEventArgs> Desactivated;
    }
}