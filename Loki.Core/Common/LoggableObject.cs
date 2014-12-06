using System.Threading;

namespace Loki.Common
{
    /// <summary>
    /// Base object (with only log).
    /// </summary>
    public class LoggableObject
    {
        #region Log

        private ILog log = null;

        private string loggerName = null;

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
                if (loggerName == null)
                {
                    loggerName = this.GetType().FullName;
                }

                return loggerName;
            }
        }

        internal virtual ILog GetLog()
        {
            return Toolkit.Common.Logger.GetLog(LoggerName);
        }

        #endregion Log
    }
}