namespace Loki.Common.Messages
{
    public class WarningMessage
    {
        public WarningMessage(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}