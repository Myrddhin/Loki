using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI.Commands
{
    class TestCommandManager : ICommandManager
    {
        public ICommandBind CreateBind<T>(ICommand command, T handler, Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction, Func<T, Action<object, CommandEventArgs>> executeFunction) where T : class
        {
            throw new NotImplementedException();
        }

        public ICommandBind CreateBind<T>(
            ICommand command,
            T handler,
            Func<T, Action<object, CanExecuteCommandEventArgs>> canExecuteFunction,
            Func<T, Action<object, CommandEventArgs>> executeFunction,
            Func<T, Func<CommandEventArgs, bool>> confirmDelegate) where T : class
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
