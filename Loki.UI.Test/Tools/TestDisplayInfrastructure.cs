using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.Common;
using Loki.UI.Commands;

namespace Loki.UI.Wpf.Test.Tools
{
    public class TestDisplayInfrastructure : TestInfrastructure, IDisplayInfrastructure
    {
        public TestDisplayInfrastructure()
        {
            CommandsManager = new TestCommandManager();
            
        }

        public ICommandManager CommandsManager { get; }
    }
}
