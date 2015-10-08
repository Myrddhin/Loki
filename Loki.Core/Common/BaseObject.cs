using System;

namespace Loki.Common
{
    /// <summary>
    /// Base object.
    /// </summary>
    public class BaseObject : LoggableObject
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObject"/> class. Use the static callback for error manager.
        /// </summary>
        public BaseObject()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseObject"/> class. Use dependency injection.
        /// </summary>
        /// <param name="logManager">
        /// Log manager.
        /// </param>
        /// <param name="errorManager">
        /// Error manager.
        /// </param>
        public BaseObject(ILoggerComponent logManager, IErrorComponent errorManager)
            : base(logManager)
        {
            errorComponent = errorManager;
        }

        #region Exception handling methods

        private readonly IErrorComponent errorComponent;

        internal virtual IErrorComponent ErrorManager
        {
            get
            {
                return this.errorComponent ?? Toolkit.Common.ErrorManager;
            }
        }

        /// <summary>
        /// Builds the error.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the exception to create.
        /// </typeparam>
        /// <param name="message">
        /// The message.
        /// </param>
        protected T BuildError<T>(string message) where T : Exception
        {
            return ErrorManager.BuildError<T>(message);
        }

        /// <summary>
        /// Builds the error.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the exception to create.
        /// </typeparam>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        protected T BuildError<T>(string message, Exception innerException) where T : Exception
        {
            return ErrorManager.BuildError<T>(message, innerException);
        }

        /// <summary>
        /// Builds the error with the specified format parameters.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the exception to create.
        /// </typeparam>
        /// <param name="message">The message.</param>
        /// <param name="parameters">The parameters.</param>
        protected T BuildErrorFormat<T>(string message, params object[] parameters) where T : Exception
        {
            return ErrorManager.BuildErrorFormat<T>(message, parameters);
        }

        /// <summary>
        /// Builds the error with the specified format parameters.
        /// </summary>
        ///  <typeparam name="T">
        /// The type of the exception to create.
        /// </typeparam>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        /// <param name="parameters">The parameters.</param>
        protected T BuildErrorFormat<T>(string message, Exception innerException, params object[] parameters) where T : Exception
        {
            return ErrorManager.BuildErrorFormat<T>(innerException, message, parameters);
        }

        #endregion Exception handling methods
    }
}