using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Loki.UI.Tests;

using Xunit;

namespace Loki.UI.Commands
{
    [Trait("Category", "UI infrastructure")]
    public class CommandManagerTest : CommonTest
    {
        private ICommand command;

        private ICommandManager Component;

        public CommandManagerTest()
        {
            this.Component = Context.Resolve<ICommandManager>();
            this.command = this.Component.GetCommand("TestCommand");
        }

        [Fact(DisplayName ="Commands are singletons")]
        public void CommandsAreSingletons()
        {
            var secondCommand = this.Component.GetCommand("TestCommand");

            Assert.Same(command, secondCommand);
        }
    }
}
