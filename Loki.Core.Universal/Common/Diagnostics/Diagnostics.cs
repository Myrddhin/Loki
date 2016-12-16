using System;

namespace Loki.Common.Diagnostics
{
    internal partial class Diagnostics
    {
        public static Func<ILogFactory> LogFactory { get; set; } =
           () => new UwpLogFactory();
    }
}