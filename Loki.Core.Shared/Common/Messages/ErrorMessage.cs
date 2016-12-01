using System;

namespace Loki.Common.Messages
{
    public class ErrorMessage
    {
        public ErrorMessage(Exception error)
        {
            Error = error;
        }

        public Exception Error { get; private set; }
    }
}