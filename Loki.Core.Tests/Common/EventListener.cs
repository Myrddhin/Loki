using System;
using System.ComponentModel;

namespace Loki.Core.Tests.Common
{
    public class EventListener
    {
        public EventArgs ListenCanExecuteChangedArgs
        {
            get;
            private set;
        }

        public PropertyChangedEventArgs ListenPropertyChangedEventArgs
        {
            get;
            private set;
        }

        public void NotifyCanExecuteChanged_Listen(object sender, EventArgs e)
        {
            ListenCanExecuteChangedArgs = e;
        }

        public void NotifyPropertyChanged_Listen(object sender, PropertyChangedEventArgs e)
        {
            ListenPropertyChangedEventArgs = e;
        }
    }
}