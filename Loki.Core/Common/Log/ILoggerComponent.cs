using System.IO;

namespace Loki.Common
{
    /// <summary>
    /// Logger manager abstraction.
    /// </summary>
    public interface ILoggerComponent
    {
        /// <summary>
        /// Configure logger from framework configuration.
        /// </summary>
        void Configure();

        /// <summary>
        /// Configure logger from specified configuration file.
        /// </summary>
        /// <param name="configurationFile">Configuration file path.</param>
        void Configure(string configurationFile);

        /// <summary>
        /// Configure logger from specified stream.
        /// </summary>
        /// <param name="configurationStream">Configuration file stream.</param>
        void Configure(Stream configurationStream);

        /// <summary>
        /// Gets a logger.
        /// </summary>
        /// <param name="logName">The logger name.</param>
        /// <returns>Configured logger.</returns>
        ILog GetLog(string logName);

        /// <summary>
        /// Gets the current log file (if available).
        /// </summary>
        string LogFileName { get; }
    }
}