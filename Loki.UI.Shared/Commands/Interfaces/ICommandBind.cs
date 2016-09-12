using System;

namespace Loki.UI.Commands
{
    public interface ICommandBind : IDisposable
    {
        bool Alive { get; }

        bool Active { get; }

        Action<object, CanExecuteCommandEventArgs> CanExecute { get; }

        Action<object, CommandEventArgs> Execute { get; }

        Func<CommandEventArgs, bool> ConfirmDelegate { get; }
    }
}