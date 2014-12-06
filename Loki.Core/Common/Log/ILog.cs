using System;

namespace Loki.Common
{
    /// <summary>
    /// Interface for loggers.
    /// </summary>
    public interface ILog
    {
        #region Debug

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">The Message.</param>
        void Debug(string message);

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="exception">The Exception.</param>
        void Debug(string message, Exception exception);

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="args">The Args.</param>
        void DebugFormat(string message, params object[] args);

        #endregion Debug

        #region Error

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">The Message.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "Log standard pattern")]
        void Error(string message);

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="exception">The Exception.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1716:IdentifiersShouldNotMatchKeywords", MessageId = "Error", Justification = "Log standard pattern")]
        void Error(string message, Exception exception);

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="args">The Args.</param>
        void ErrorFormat(string message, params object[] args);

        #endregion Error

        #region Fatal

        /// <summary>
        /// Logs the specified fatal error.
        /// </summary>
        /// <param name="message">The Message.</param>
        void Fatal(string message);

        /// <summary>
        /// Logs the specified fatal error.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="exception">The Exception.</param>
        void Fatal(string message, Exception exception);

        /// <summary>
        /// Logs the specified fatal error.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="args">The Args.</param>
        void FatalFormat(string message, params object[] args);

        #endregion Fatal

        #region Info

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">The Message.</param>
        void Info(string message);

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="exception">The Exception.</param>
        void Info(string message, Exception exception);

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="args">The Args.</param>
        void InfoFormat(string message, params object[] args);

        #endregion Info

        #region Warn

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">The Message.</param>
        void Warn(string message);

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="exception">The Exception.</param>
        void Warn(string message, Exception exception);

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">The Message.</param>
        /// <param name="args">The Format parameters.</param>
        void WarnFormat(string message, params object[] args);

        #endregion Warn
    }
}