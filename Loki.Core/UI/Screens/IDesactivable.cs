using System;

namespace Loki.UI
{
    /// <summary>
    /// Denotes an instance which requires deactivation.
    /// </summary>
    public interface IDesactivable
    {
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