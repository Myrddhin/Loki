using System;

using Loki.UI.Commands;

namespace Loki.Core.Tests.Common
{
    public class NotifyCanExecuteChangedRaiser //: ICommand
    {
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Raise()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }

        public string Name { get; private set; }

        public void RefreshState()
        {
            throw new NotImplementedException();
        }
    }
}