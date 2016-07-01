using System;
using System.Globalization;

using log4net;

namespace Loki.Common.Diagnostics
{
    /// <summary>
    /// Logger proxy class.
    /// </summary>
    internal class Log4NetLog : ILog
    {
        private readonly log4net.ILog logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="Log4NetLog"/> class.
        /// </summary>
        /// <param name="loggerName">
        /// The logger name.
        /// </param>
        public Log4NetLog(string loggerName)
        {
            logger = LogManager.GetLogger(loggerName);
        }

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        public void Debug(string message)
        {
            logger.Debug(message);
        }

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        public void DebugFormat(string message, params object[] args)
        {
            logger.DebugFormat(CultureInfo.InvariantCulture, message, args);
        }

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        public void Error(string message)
        {
            logger.Error(message);
        }

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="exception">
        /// The Exception.
        /// </param>
        public void Error(string message, Exception exception)
        {
            logger.Error(message, exception);
        }

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        public void ErrorFormat(string message, params object[] args)
        {
            logger.ErrorFormat(CultureInfo.InvariantCulture, message, args);
        }

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">
        /// The message.
        /// </param>
        public void Info(string message)
        {
            logger.Info(message);
        }

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        public void InfoFormat(string message, params object[] args)
        {
            logger.InfoFormat(CultureInfo.InvariantCulture, message, args);
        }

        /// <summary>
        /// Logs the specified warning.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        public void Warn(string message)
        {
            logger.Warn(message);
        }

        /// <summary>
        /// Logs the specified warning.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        public void WarnFormat(string message, params object[] args)
        {
            logger.WarnFormat(CultureInfo.InvariantCulture, message, args);
        }
    }
}