using System;

namespace Loki.UI
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