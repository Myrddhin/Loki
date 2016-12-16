using System;

using Xunit.Sdk;

namespace Loki.UI
{
    [AttributeUsage(AttributeTargets.Method)]
    [XunitTestCaseDiscoverer("Loki.UI.WpfFactDiscoverer", "Loki.UI.Wpf.Test")]
    public sealed class WpfFactAttribute : Attribute
    {
        public string DisplayName { get; set; }
    }
}