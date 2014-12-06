using System;

namespace Loki.Commands
{
    public interface INotifyCanExecuteChanged
    {
        event EventHandler CanExecuteChanged;
    }
}
