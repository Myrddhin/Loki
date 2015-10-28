namespace Loki.Common
{
    /// <summary>
    /// Base object.
    /// </summary>
    public class BaseObject : LoggableObject
    {
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
            ErrorManager = errorManager;
        }

        protected virtual IErrorComponent ErrorManager { get; private set; }
    }
}