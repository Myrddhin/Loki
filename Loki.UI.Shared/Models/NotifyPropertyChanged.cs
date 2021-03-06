﻿using System.ComponentModel;

namespace Loki.UI.Models
{
    public class NotifyPropertyChanged : INotifyPropertyChangedEx
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public bool Tracking { get; set; } = true;

        public virtual void NotifyChanged(PropertyChangedEventArgs propertyArgs)
        {
            OnPropertyChanged(propertyArgs);
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = PropertyChanged;
            if (Tracking)
            {
                handler?.Invoke(this, e);
            }
        }

        private static readonly PropertyChangedEventArgs refreshArgs = new PropertyChangedEventArgs(string.Empty);

        public void Refresh()
        {
            OnPropertyChanged(refreshArgs);
        }
    }
}