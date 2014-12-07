using System;

namespace Loki.UI
{
    public class BindingEventArgs : EventArgs
    {
        public object Bind { get; set; }

        public object View { get; set; }

        public object ViewModel { get; set; }
    }
}