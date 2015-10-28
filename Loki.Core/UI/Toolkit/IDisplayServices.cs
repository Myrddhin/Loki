using Loki.Common;

namespace Loki.UI
{
    public interface IDisplayServices
    {
        /// <summary>
        /// Gets the core services.
        /// </summary>
        ICoreServices Core { get; }

        /// <summary>
        /// Gets the UI services.
        /// </summary>
        IUIServices UI { get; }
    }
}