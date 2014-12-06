namespace Loki.Common
{
    /// <summary>
    ///   Denotes a class which can handle a particular type of message.
    /// </summary>
    /// <typeparam name = "TMessage">The type of message to handle.</typeparam>
    public interface IHandle<TMessage> : IHandle
    {
        /// <summary>
        ///   Handles the message.
        /// </summary>
        /// <param name = "message">The message.</param>
        void Handle(TMessage message);
    }
}