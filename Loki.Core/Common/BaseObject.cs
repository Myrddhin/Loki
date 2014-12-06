using System;
using System.Threading;

namespace Loki.Common
{
    /// <summary>
    /// Base object.
    /// </summary>
    public class BaseObject : LoggableObject
    {
        #region Exception handling methods

        internal virtual IErrorComponent ErrorManager
        {
            get
            {
                return Toolkit.Common.ErrorManager;
            }
        }

        /// <summary>
        /// Builds the error.
        /// </summary>
        /// <param name="message">The message.</param>
        protected T BuildError<T>(string message) where T : Exception
        {
            return ErrorManager.BuildError<T>(message, Log);
        }

        /// <summary>
        /// Builds the error.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        protected T BuildError<T>(string message, Exception innerException) where T : Exception
        {
            return ErrorManager.BuildError<T>(message, Log, innerException);
        }

        /// <summary>
        /// Builds the error with the specified format paramters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The params.</param>
        protected T BuildErrorFormat<T>(string message, params object[] parameters) where T : Exception
        {
            return ErrorManager.BuildErrorFormat<T>(message, Log, parameters);
        }

        /// <summary>
        /// Builds the error with the specified format paramters.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The params.</param>
        protected T BuildErrorFormat<T>(string message, Exception innerException, params object[] parameters) where T : Exception
        {
            return ErrorManager.BuildErrorFormat<T>(message, Log, innerException, parameters);
        }

        #endregion Exception handling methods
    }
}