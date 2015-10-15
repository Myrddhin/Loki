using System;

using Loki.Commands;

namespace Loki.Core.Tests.Common
{
    public class NotifyCanExecuteChangedRaiser : INotifyCanExecuteChanged
    {
        public event EventHandler CanExecuteChanged;

        public void Raise()
        {
            if (CanExecuteChanged != null)
            {
                CanExecuteChanged(this, EventArgs.Empty);
            }
        }
    }
}