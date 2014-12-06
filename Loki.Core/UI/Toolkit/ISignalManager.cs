using System;

namespace Loki.UI
{
    public interface ISignalManager
    {
        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="returnCode">The return code.</param>
        void ApplicationExit(int returnCode);

        /// <summary>
        /// Handles the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="imperative">If set to <c>true</c> the error should be raise immediatly and close the application.</param>
        void Error(Exception exception, bool imperative);

        /// <summary>
        /// Handle the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        void Message(string message);

        /// <summary>
        /// Handles the specified warning.
        /// </summary>
        /// <param name="warning">The warning.</param>
        void Warning(string warning);
    }
}