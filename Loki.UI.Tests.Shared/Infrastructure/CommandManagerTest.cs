using System;
using System.Threading.Tasks;

using Loki.UI.Tests;
#if WPF
using System.Windows.Input;
#endif

using Xunit;

namespace Loki.UI.Commands
{
    [Trait("Category", "UI infrastructure")]
    public class CommandManagerTest : CommonTest
    {
        private readonly ICommand command;

        private readonly ICommandManager Component;

        private int confirmCalled;

        private int executeCalled;

        private int canExecuteCalled;

        public CommandManagerTest()
        {
            this.Component = Context.Resolve<ICommandManager>();
            this.command = this.Component.GetCommand("TestCommand");
        }

        [Fact(DisplayName = "Commands are singletons")]
        public void CommandsAreSingletons()
        {
            var secondCommand = this.Component.GetCommand("TestCommand");

            Assert.Same(command, secondCommand);
        }

        [Fact(DisplayName = "Standard command work")]
        public void Standard()
        {
            using (this.Component.CreateBind(
                this.command,
                this,
                c => c.CanExecuteTrue,
                c => c.Execute,
                c => c.ConfirmTrue))
            {
                this.command.Execute(null);
            }

            Assert.True(this.canExecuteCalled > 0);
            Assert.True(this.executeCalled > 0);
            Assert.True(this.confirmCalled > 0);
        }

        [Fact(DisplayName = "Two registrations for same handler do two calls")]
        public void MultiHanlder()
        {
            var handler = new TestCommandHandler(true, true);
            using (this.Component.CreateBind(
                    this.command,
                    handler,
                    c => c.CanExecute,
                    c => c.Execute,
                    c => c.Confirm))
            using (this.Component.CreateBind(
                   this.command,
                   handler,
                   c => c.CanExecute,
                   c => c.Execute,
                   c => c.Confirm))
            {
                this.command.Execute(null);
            }

            Assert.Equal(2, handler.CanExecuteCalled);
            Assert.Equal(2, handler.ExecuteCalled);
            Assert.Equal(2, handler.ConfirmCalled);
        }

        [Fact(DisplayName = "Property changed refresh command")]
        public void NotifyPropertyChangedRaiseCommandRefresh()
        {
            var handler = new TestCommandHandler(false, true);
            bool raised = false;
            this.command.CanExecuteChanged += (sender, args) => raised = true;
            using (this.Component.CreateBind(
                    this.command,
                    handler,
                    c => c.CanExecute,
                    c => c.Execute,
                    c => c.Confirm))
            using (this.Component.CreateBind(
                   this.command,
                   handler,
                   c => c.CanExecute,
                   c => c.Execute,
                   c => c.Confirm))
            {
                this.command.CanExecute(null);

                // the link is a weak reference.
                GC.Collect();
                Assert.True(raised);
                handler.SetReturnValues(true, true);
                handler.Refresh();

                this.command.CanExecute(null);
            }

            Assert.True(raised);
        }

        [Fact(DisplayName = "Can execute changed raised")]
        public void CanExecuteChangedTest()
        {
            bool raised = false;
            this.command.CanExecuteChanged += (sender, args) => raised = true;

            Task.Run(
                () =>
                {
                    var handler = new TestCommandHandler(true, true);

                    using (this.Component.CreateBind(
                        this.command,
                        handler,
                        c => c.CanExecute,
                        c => c.Execute,
                        c => c.Confirm))
                    {
                        this.command.CanExecute(null);
                    }
                }).Wait();

            GC.Collect();
            this.command.CanExecute(null);

            Assert.True(raised);
        }

        [Fact(DisplayName = "False confirm implies no execute")]
        public void FalseConfirm()
        {
            var handler = new TestCommandHandler(true, false);
            using (this.Component.CreateBind(
                this.command,
                handler,
                c => c.CanExecute,
                c => c.Execute,
                c => c.Confirm))
            {
                this.command.Execute(null);
            }

            Assert.True(handler.CanExecuteCalled > 0);
            Assert.Equal(0, handler.ExecuteCalled);
            Assert.True(handler.ConfirmCalled > 0);
        }

        [Fact(DisplayName = "False can execute implies no execute")]
        public void FalseCanExecute()
        {
            var handler = new TestCommandHandler(false, true);
            using (this.Component.CreateBind(
                this.command,
                handler,
                c => c.CanExecute,
                c => c.Execute))
            {
                this.command.Execute(null);
            }

            Assert.True(handler.CanExecuteCalled > 0);
            Assert.True(handler.ExecuteCalled == 0);
            Assert.True(handler.ConfirmCalled == 0);
        }

        [Fact(DisplayName = "Binds use weak references")]
        public void WeakLink()
        {
            WeakReference<TestCommandHandler> reference = null;
            Task.Run(
                () =>
                    {
                        var handler = new TestCommandHandler(true, true);
                        reference = new WeakReference<TestCommandHandler>(handler);
                        using (this.Component.CreateBind(
                            this.command,
                            handler,
                            c => c.CanExecute,
                            c => c.Execute,
                            c => c.Confirm))
                        {
                            this.command.Execute(null);
                        }
                    }).Wait();

            GC.Collect();
            this.command.Execute(null);
            TestCommandHandler buffer;
            Assert.False(reference.TryGetTarget(out buffer));
        }

        private void CanExecuteTrue(object sender, CanExecuteCommandEventArgs e)
        {
            e.CanExecute = true;
            this.canExecuteCalled++;
        }

        private void Execute(object sender, CommandEventArgs e)
        {
            this.executeCalled++;
        }

        private bool ConfirmTrue(CommandEventArgs e)
        {
            this.confirmCalled++;
            return true;
        }
    }
}