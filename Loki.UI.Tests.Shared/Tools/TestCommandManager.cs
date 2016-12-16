using System;
using System.Collections.Generic;

#if WPF
using System.Windows.Input;
#endif

namespace Loki.UI.Commands
{
    internal class TestCommandManager : ICommandManager
    {
        public ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction)
            where T : class
        {
            throw new NotImplementedException();
        }

        public ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction,
            Func<T, Func<CommandEventArgs, bool>> confirmDelegate)
            where T : class
        {
            throw new NotImplementedException();
        }

        public ICommand GetCommand(string commandName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ICommandBind> GetHandlers(ICommand command)
        {
            throw new NotImplementedException();
        }
    }
}