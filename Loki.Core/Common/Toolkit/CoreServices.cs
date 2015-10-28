namespace Loki.Common
{
    internal class CoreServices : ICoreServices
    {
        public CoreServices(ILoggerComponent logger, IErrorComponent error, IMessageComponent message, IEventComponent events)
        {
            Logger = logger;
            Error = error;
            Messages = message;
            Events = events;
        }

        public ILoggerComponent Logger { get; private set; }

        public IErrorComponent Error { get; private set; }

        public IMessageComponent Messages { get; private set; }

        public IEventComponent Events { get; private set; }
    }
}