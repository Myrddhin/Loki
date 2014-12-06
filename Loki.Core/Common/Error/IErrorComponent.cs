using System;

namespace Loki.Common
{
    /// <summary>
    /// Loki error service.
    /// </summary>
    public interface IErrorComponent
    {
        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="log">The logger.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>The builded error.</returns>
        T BuildError<T>(string message, ILog log, Exception innerException) where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="log">The logger.</param>
        T BuildError<T>(string message, ILog log) where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="log">The logger.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        T BuildErrorFormat<T>(string message, ILog log, Exception innerException, params object[] parameters) where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="log">The logger.</param>
        /// <param name="parameters">The parameters.</param>
        T BuildErrorFormat<T>(string message, ILog log, params object[] parameters) where T : Exception;

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="log">The logger.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        void LogError(string message, ILog log, Exception exception, params object[] parameters);
    }
}