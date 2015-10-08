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
        /// <param name="innerException">The inner exception.</param>
        /// <returns>The build error.</returns>
        T BuildError<T>(string message, Exception innerException) where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        T BuildError<T>(string message) where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        T BuildErrorFormat<T>(Exception innerException, string message, params object[] parameters) where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        T BuildErrorFormat<T>(string message, params object[] parameters) where T : Exception;

        /// <summary>
        /// Logs the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="exception">The exception.</param>
        /// <param name="parameters">The parameters.</param>
        void LogError(string message, Exception exception, params object[] parameters);
    }
}