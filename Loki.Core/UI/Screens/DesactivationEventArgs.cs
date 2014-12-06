using System;

namespace Loki.UI
{
    /// <summary>
    /// EventArgs sent during deactivation.
    /// </summary>
    public class DesactivationEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets a value indicating whether the sender was closed in addition to being deactivated.
        /// </summary>
        public bool WasClosed { get; set; }
    }
}