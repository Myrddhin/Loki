using System.ComponentModel;

using Loki.Common;

namespace Loki.UI
{
    public class NotifyPropertyChangedEx : INotifyPropertyChangedEx
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Tracking { get; set; }

        public void NotifyChanged(PropertyChangedEventArgs propertyArgs)
        {
            var handler = PropertyChanged;
            if (Tracking && handler != null)
            {
                handler(this, propertyArgs);
            }
        }

        public void Refresh()
        {
            NotifyChanged(RefreshEventArgs);
        }

        private static readonly PropertyChangedEventArgs RefreshEventArgs = new PropertyChangedEventArgs(string.Empty);
    }
}