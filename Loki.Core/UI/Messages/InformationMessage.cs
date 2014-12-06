namespace Loki.UI
{
    public class InformationMessage
    {
        public InformationMessage(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}