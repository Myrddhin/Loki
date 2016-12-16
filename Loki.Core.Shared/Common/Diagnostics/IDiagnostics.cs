using System;
using System.Threading.Tasks;

namespace Loki.Common.Diagnostics
{
    public interface IDiagnostics : IInitializable
    {
        ILog GetLog(string logName);

        IActivityLog GetActivityLog(string logName);

        Task<string> GetLogDataAsync();

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>The build error.</returns>
        T BuildError<T>(string message, Exception innerException)
            where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <returns>The build error.</returns>
        T BuildError<T>(string message)
            where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The build error.</returns>
        T BuildErrorFormat<T>(Exception innerException, string message, params object[] parameters)
        where T : Exception;

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The build error.</returns>
        T BuildErrorFormat<T>(string message, params object[] parameters)
            where T : Exception;
    }
}