namespace Loki.Common.Messages
{
    public class StartMessage
    {
        public string[] Parameters { get; private set; }

        public StartMessage(string[] startParameters)
        {
            Parameters = startParameters;
        }
    }
}