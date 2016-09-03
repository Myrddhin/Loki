using System;
using System.ComponentModel;

using Loki.Common;

namespace Loki.UI.Models
{
    public class StateUnit : NotifyPropertyChanged, ICentralizedChangeTracking, INotifyPropertyChanging
    {
        public void AcceptChanges()
        {
            IsChanged = false;
        }

        #region IsChanged
        private static readonly PropertyChangedEventArgs argsChanedChanged = new PropertyChangedEventArgs(nameof(IsChanged));

        private bool dirty;

        public bool IsChanged
        {
            get
            {
                return dirty;
            }
            private set
            {
                if (value == dirty)
                {
                    return;
                }

                dirty = value;
                NotifyChanged(argsChanedChanged);
                OnStateChanged(EventArgs.Empty);
            }
        } 
        #endregion

        public event EventHandler StateChanged;

        public event PropertyChangingEventHandler PropertyChanging;

        public void NotifyChanging(PropertyChangingEventArgs propertyArgs)
        {
            OnPropertyChanging(propertyArgs);
        }

        public void NotifyChanged(PropertyChangedEventArgs propertyArgs, bool changed = false)
        {
            base.NotifyChanged(propertyArgs);
            if (changed)
            {
                IsChanged = true;
            }
        }

        protected void OnStateChanged(EventArgs e)
        {
            var handler = StateChanged;
            if (Tracking)
            {
                handler?.Invoke(this, e);
            }
        }

        protected void OnPropertyChanging(PropertyChangingEventArgs e)
        {
            var handler = PropertyChanging;
            if (Tracking)
            {
                handler?.Invoke(this, e);
            }
        }
    }
}