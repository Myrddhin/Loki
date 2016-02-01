using System.Threading;

namespace Loki.Common
{
    /// <summary>
    /// Base object (with only log).
    /// </summary>
    public class LoggableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggableObject"/> class with dependency injection.
        /// </summary>
        /// <param name="logManager">
        /// Log component.
        /// </param>
        public LoggableObject(ILoggerComponent logManager)
        {
            this.logger = logManager;
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
        protected virtual string LoggerName
        {
            get
            {
                return this.loggerName ?? (this.loggerName = this.GetType().FullName);
            }
        }

        private readonly ILoggerComponent logger;

        internal virtual ILog GetLog()
        {
            return logger.GetLog(LoggerName);
        }

        #endregion Log
    }
}