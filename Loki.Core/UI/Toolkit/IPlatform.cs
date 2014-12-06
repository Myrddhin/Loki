using Loki.IoC;

namespace Loki.UI
{
    /// <summary>
    /// Common interface for platforms. Used by bootstrapper.
    /// </summary>
    public interface IPlatform
    {
        /// <summary>
        /// Gets the signal manager.
        /// </summary>
        /// <value>
        /// The signals manager.
        /// </value>
        ISignalManager Signals { get; }

        /// <summary>
        /// Gets the templating engine.
        /// </summary>
        /// <value>
        /// The templating engine.
        /// </value>
        ITemplatingEngine Templates { get; }

        /// <summary>
        /// Gets the threading wrapper.
        /// </summary>
        /// <value>
        /// The threading wrapper.
        /// </value>
        IThreadingContext Threading { get; }

        /// <summary>
        /// Gets the windows manager.
        /// </summary>
        /// <value>
        /// The windows manager.
        /// </value>
        IWindowManager Windows { get; }

        /// <summary>
        /// Gets or sets the main object.
        /// </summary>
        /// <value>
        /// The main object.
        /// </value>
        object EntryPoint { get; set; }
    }
}