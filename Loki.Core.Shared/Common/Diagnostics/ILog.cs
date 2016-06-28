using System;

namespace Loki.Common.Diagnostics
{
    public interface ILog
    {
        #region Debug

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        void Debug(string message);

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        void DebugFormat(string message, params object[] args);

        #endregion Debug

        #region Error

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        void Error(string message);

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="exception">
        /// The Exception.
        /// </param>
        void Error(string message, Exception exception);

        /// <summary>
        /// Logs the specified error.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        void ErrorFormat(string message, params object[] args);

        #endregion Error

        #region Info

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        void Info(string message);

        /// <summary>
        /// Logs the specified info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Args.
        /// </param>
        void InfoFormat(string message, params object[] args);

        #endregion Info

        #region Warn

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        void Warn(string message);

        /// <summary>
        /// Logs the specified debug info.
        /// </summary>
        /// <param name="message">
        /// The Message.
        /// </param>
        /// <param name="args">
        /// The Format parameters.
        /// </param>
        void WarnFormat(string message, params object[] args);

        #endregion Warn
    }
}