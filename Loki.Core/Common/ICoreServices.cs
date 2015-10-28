namespace Loki.Common
{
    public interface ICoreServices
    {
        ILoggerComponent Logger { get; }

        IErrorComponent Error { get; }

        IMessageComponent Messages { get; }

        IEventComponent Events { get; }
    }
}