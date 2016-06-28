using System;
using System.Threading;

using Loki.Common.Diagnostics;

namespace Loki.Common
{
    /// <summary>
    /// Base object (with only log).
    /// </summary>
    public class BaseObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObject"/> class with dependency injection.
        /// </summary>
        /// <param name="diagnosticsService">
        /// Log component.
        /// </param>
        public BaseObject(IDiagnostics diagnosticsService)
        {
            this.diagnostics = diagnosticsService;
        }

        #region Log

        private ILog log;

        private string loggerName;

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>The logger.</value>
        protected ILog Log
        {
            get
            {
                if (log == null)
                {
                    Interlocked.CompareExchange(ref log, GetLog(), null);
                }

                return log;
            }
        }

        /// <summary>
        /// Gets the logger name ; must be redefined in derived classes.
        /// </summary>
        protected virtual string LoggerName => this.loggerName ?? (this.loggerName = this.GetType().FullName);

        private readonly IDiagnostics diagnostics;

        internal virtual ILog GetLog()
        {
            return diagnostics.GetLog(LoggerName);
        }

        protected IActivityLog CreateActivityLog(string logName)
        {
            return diagnostics.GetActivityLog(logName);
        }

        #endregion Log

        #region Error Handling

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <returns>The build error.</returns>
        protected T BuildError<T>(string message, Exception innerException) where T : Exception
        {
            return diagnostics.BuildError<T>(message, innerException);
        }

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        protected T BuildError<T>(string message) where T : Exception
        {
            return diagnostics.BuildError<T>(message);
        }

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
       protected T BuildErrorFormat<T>(Exception innerException, string message, params object[] parameters) where T : Exception
        {
            return diagnostics.BuildErrorFormat<T>(innerException, message, parameters);
        }

        /// <summary>
        /// Builds a new error and log it.
        /// </summary>
        /// <typeparam name="T">The exception type to build.</typeparam>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        protected T BuildErrorFormat<T>(string message, params object[] parameters) where T : Exception
        {
            return diagnostics.BuildErrorFormat<T>(message, parameters);
        }
        #endregion  
    }
}