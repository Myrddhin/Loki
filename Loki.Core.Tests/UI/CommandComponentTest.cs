using System.Linq;

using Loki.Commands;

using Xunit;

namespace Loki.Core.Tests.UI
{
    public class CommandComponentTest : UITest<ICommandComponent>
    {
        [Fact]
        public void CommandImmutability()
        {
            var command1 = Component.GetCommand("Name1");
            var command2 = Component.GetCommand("Name1");

            Assert.Equal(command1, command2);
        }

        [Fact]
        public void NoHandlerReturnEmpty()
        {
            var command1 = Component.GetCommand("Name1");
            Assert.Empty(Component.GetHandlers(command1));
        }

        [Fact]
        public void AddHandlerImmutable()
        {
            var command1 = Component.GetCommand("Name1");
            var state = new UIObject();
            var handler1 = Component.CreateHandler(command1, state.CanExecute, state.Execute, state);
            var handlers = Component.GetHandlers(command1).ToArray();
            Assert.NotEmpty(handlers);
            Assert.Equal(handler1, handlers.First());
        }

        [Fact]
        public void RemoveHandler()
        {
        }

        [Fact]
        public void HandlerWeakReference()
        {
        }
    }
}