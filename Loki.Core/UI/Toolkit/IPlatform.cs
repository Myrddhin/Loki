using Loki.Common;
using Loki.IoC;

namespace Loki.UI
{
    /// <summary>
    /// Common interface for platforms. Used by bootstrapper.
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// Gets the main object.
        /// </summary>
        object EntryPoint { get; }

        /// <summary>
        /// Gets the core services.
        /// </summary>
        ICoreServices Services { get; }

        /// <summary>
        /// Gets the IoC context.
        /// </summary>
        IObjectContext Context { get; }

        /// <summary>
        /// Gets the UI services.
        /// </summary>
        IUIServices UI { get; }
    }
}