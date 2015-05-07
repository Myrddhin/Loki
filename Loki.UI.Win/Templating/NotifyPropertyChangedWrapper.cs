using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Loki.UI.Win
{
    public class NotifyPropertyChangedWrapper : INotifyPropertyChanged
    {
        private static ConcurrentDictionary<string, PropertyChangedEventArgs> eventArgs = new ConcurrentDictionary<string, PropertyChangedEventArgs>();

        private string propertyName;

        public NotifyPropertyChangedWrapper(PropertyInfo property)
        {
            propertyName = property.Name;
            eventArgs.TryAdd(propertyName, new PropertyChangedEventArgs(propertyName));
        }

        #region Event

        /// <summary>
        /// Event raised after the <see cref="Text" /> property value has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="e"><see cref="PropertyChangedEventArgs" /> object that provides the arguments for the event.</param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Event

        public void EventBridge(object sender, EventArgs e)
        {
            PropertyChangedEventArgs args = null;
            if (eventArgs.TryGetValue(propertyName, out args))
            {
                OnPropertyChanged(args);
            }
        }
    }
}