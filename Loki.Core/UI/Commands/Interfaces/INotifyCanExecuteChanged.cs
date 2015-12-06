using System;

namespace Loki.UI.Commands
{
    public interface INotifyCanExecuteChanged
    {
        event EventHandler CanExecuteChanged;
    }
}