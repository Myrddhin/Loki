using System;

namespace Loki.Core.Tests.Common
{
    public class NotifyCanExecuteChangedRaiser
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
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        public string Name { get; private set; }

        public void RefreshState()
        {
            throw new NotImplementedException();
        }
    }
}