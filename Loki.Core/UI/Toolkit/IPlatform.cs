using Loki.IoC;

namespace Loki.UI
{
    /// <summary>
    /// Common interface for platforms. Used by bootstrapper.
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// Gets or sets the main object.
        /// </summary>
        /// <value>
        /// The main object.
        /// </value>
        object EntryPoint { get; set; }
    }
}