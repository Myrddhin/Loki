using System;
using System.Linq;
using System.Threading.Tasks;

using Loki.UI.Commands;

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
        public void IgnoreNotInitialized()
        {
            var command1 = Component.GetCommand("Name1");
            var state = new UIInitializableObject();
            Component.CreateHandler(command1, state.CanExecute, state.Execute, state);
            var handlers = Component.GetHandlers(command1).ToArray();
            Assert.Empty(handlers);

            state.Initialize();

            handlers = Component.GetHandlers(command1).ToArray();
            Assert.NotEmpty(handlers);
        }

        [Fact]
        public void IgnoreNotActivated()
        {
            var command1 = Component.GetCommand("Name1");
            var state = new UIActivableObject();
            Component.CreateHandler(command1, state.CanExecute, state.Execute, state);
            var handlers = Component.GetHandlers(command1).ToArray();
            Assert.Empty(handlers);

            state.Activate();

            handlers = Component.GetHandlers(command1).ToArray();
            Assert.NotEmpty(handlers);
        }

        [Fact]
        public void RemoveHandler()
        {
            var command1 = Component.GetCommand("Name1");
            var state = new UIObject();
            var handler = Component.CreateHandler(command1, state.CanExecute, state.Execute, state);
            var handlers = Component.GetHandlers(command1).ToArray();
            Assert.NotEmpty(handlers);

            Component.RemoveHandler(command1, handler);
            handlers = Component.GetHandlers(command1).ToArray();
            Assert.Empty(handlers);
        }

        [Fact]
        public void HandlerWeakReference()
        {
            var command1 = Component.GetCommand("Name1");
            var runner = Task.Run(
               () =>
               {
                   var state = new UIObject();
                   Component.CreateHandler(command1, state.CanExecute, state.Execute, state);
                   var handlers = Component.GetHandlers(command1);
                   Assert.NotEmpty(handlers);
               });

            runner.Wait();
            GC.Collect();

            Assert.Empty(Component.GetHandlers(command1));
        }
    }
}