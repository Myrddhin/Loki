using System;

namespace Loki.Common.Messages
{
    public class NavigationMessage
    {
        public Uri Target { get; set; }

        public object Parameter { get; set; }
    }
}