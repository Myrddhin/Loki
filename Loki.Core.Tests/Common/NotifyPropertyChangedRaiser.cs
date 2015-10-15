using System.ComponentModel;

namespace Loki.Core.Tests.Common
{
    internal class NotifyPropertyChangedRaiser : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public void Raise()
        {
            this.OnPropertyChanged(string.Empty);
        }
    }
}