namespace Loki.Common
{
    public interface ICoreServices
    {
        ILoggerComponent Logger { get; }

        IErrorComponent Error { get; }

        IMessageBus Messages { get; }

        IEventComponent Events { get; }
    }
}