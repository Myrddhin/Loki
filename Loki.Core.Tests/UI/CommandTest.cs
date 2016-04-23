using Loki.UI.Commands;

using Xunit;

namespace Loki.Core.Tests.UI
{
    public abstract class CommandTest
    {
        protected CommandTest()
        {
            State = new UIObject();
        }

        public ICommand Command { get; protected set; }

        public UIObject State { get; private set; }

        public ICommandHandler Handler { get; protected set; }

        [Fact]
        public void NoHandlerCanExecuteFalse()
        {
        }

        [Fact]
        public void CheckName()
        {
            Assert.NotNull(Command.Name);
        }

        [Fact]
        public void CheckCanExecute()
        {
            Command.CanExecute(null);
            Assert.Equal(State.CanExecuteCount, 1);
        }

        [Fact]
        public void CheckNoExecuteWhenCanExecuteFalse()
        {
            Command.Execute(null);
            Assert.Equal(1, State.CanExecuteCount);
            Assert.Equal(0, State.ExecuteCount);
        }

        [Fact]
        public void CheckExecuteWhenCanExecuteTrue()
        {
            State.CanExecuteReturn = true;
            Command.Execute(null);
            Assert.Equal(1, State.CanExecuteCount);
            Assert.Equal(1, State.ExecuteCount);
        }

        [Fact]
        public void CheckEvent()
        {
            State.CanExecuteReturn = true;

            Command.CanExecuteChanged += State.CanExecuteChanged;

            Command.CanExecute(null);
            Assert.Equal(1, State.EventCount);
        }

        [Fact]
        public void CheckEventRebounce()
        {
            State.CanExecuteReturn = true;
            Command.CanExecuteChanged += State.CanExecuteChanged;

            Command.CanExecute(null);
            Assert.Equal(1, State.EventCount);

            Command.CanExecute(null);
            Assert.Equal(1, State.EventCount);
        }

        [Fact]
        public void CheckRefresh()
        {
            State.CanExecuteReturn = true;
            Command.CanExecuteChanged += State.CanExecuteChanged;

            Command.CanExecute(null);
            Assert.Equal(1, State.EventCount);

            Command.RefreshState();
            Assert.Equal(2, State.EventCount);
        }
    }
}