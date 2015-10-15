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

        private readonly IErrorComponent errorComponent;

        protected virtual IErrorComponent ErrorManager
        {
            get
            {
                return this.errorComponent ?? Toolkit.Common.ErrorManager;
            }
        }
    }
}