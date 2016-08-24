using System;
using System.ComponentModel;

namespace Loki.Common
{
    /// <summary>
    /// Common interface for centralized state change.
    /// </summary>
    public interface ICentralizedChangeTracking : IChangeTracking
    {
        /// <summary>
        /// Occurs when internal state changed.
        /// </summary>
        event EventHandler StateChanged;
    }
}