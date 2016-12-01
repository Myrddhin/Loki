using System;

namespace Loki.UI.Models
{
    /// <summary>
    /// Denotes an object that can be closed.
    /// </summary>
    public interface ICloseable
    {
        /// <summary>
        /// Tries to close this instance.
        /// Also provides an opportunity to pass a dialog result to it's corresponding view.
        /// </summary>
        /// <param name="dialogResult">The dialog result.</param>
        void TryClose(bool? dialogResult = null);

        /// <summary>
        /// Occurs when this instance is closing.
        /// </summary>
        event EventHandler Closing;

        /// <summary>
        /// Occurs when this instance is closed (just before dialog result is set).
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Sets the dialog result.
        /// </summary>
        Action<bool?> DialogResultSetter { get; set; }
    }
}