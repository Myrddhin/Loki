using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI.Wpf.Test.Tools
{
    public class TestDisplayInfrastructure : TestInfrastructure, IDisplayInfrastructure
    {
        public ICommandManager CommandsManager { get; set; }
    }
}