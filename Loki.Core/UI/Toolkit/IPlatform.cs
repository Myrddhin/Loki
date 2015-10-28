using Loki.IoC;

namespace Loki.UI
{
    /// <summary>
    /// Common interface for platforms. Used by bootstrapper.
    /// </summary>
    public interface IPlatform : IDisplayServices
    {
        /// <summary>
        /// Gets the main object.
        /// </summary>
        object EntryPoint { get; }

        /// <summary>
        /// Gets the IoC context.
        /// </summary>
        IObjectContext Context { get; }
    }
}